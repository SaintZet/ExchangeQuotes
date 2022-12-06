using ExchangeQuotes.Client.Abstractions;
using ExchangeQuotes.Client.Constants;
using ExchangeQuotes.Core.Abstractions;
using ExchangeQuotes.Core.Communication.Udp;

namespace ExchangeQuotes.Client;

internal class Application
{
    private readonly IExchangeQuotesView _exchangeQuotesView;
    private readonly IExchangeQuotesReceiver _exchangeQuotesReceiver;
    private readonly IExchangeQuotesCalculateWorker _exchangeQuotesCalculateWorker;

    public Application(IExchangeQuotesReceiver exchangeQuotesReceiver, IExchangeQuotesCalculateWorker exchangeQuotesCalculateWorker, IExchangeQuotesView exchangeQuotesView)
    {
        _exchangeQuotesReceiver = exchangeQuotesReceiver;
        _exchangeQuotesCalculateWorker = exchangeQuotesCalculateWorker;
        _exchangeQuotesView = exchangeQuotesView;
    }

    internal void StartDoWork(int workDelay = 0)
    {
        _exchangeQuotesReceiver!.DataReceived += DataRecivedHandler;
        _exchangeQuotesReceiver.StartListeningIncomingData();

        var timer = new Timer(ReciverDelay!, workDelay, TimeSpan.Zero, TimeSpan.FromMilliseconds(ApplicationConstants.DuringForDelayInMilliseconds));

        _exchangeQuotesView.RequesteData += DataRequestHandler;
        _exchangeQuotesView.StartDoWork();
    }

    //Emulate packet delay
    private void ReciverDelay(object delay)
    {
        int ms = (int)delay;

        if (ms == 0)
        {
            return;
        }

        _exchangeQuotesReceiver.RecivePause = true;

        Thread.Sleep(ms);

        _exchangeQuotesReceiver.RecivePause = false;
    }

    private void DataRecivedHandler(object? sender, EventArgs e)
    {
        ReceivedEventArgs receivedEventArgs = (ReceivedEventArgs)e;
        _exchangeQuotesCalculateWorker!.CalculateValues(receivedEventArgs.Data!);
    }

    private void DataRequestHandler(object? sender, EventArgs e)
    {
        var data = _exchangeQuotesCalculateWorker.GetCurrentValues();
        var packetLoss = _exchangeQuotesReceiver.PacketLoss;

        _exchangeQuotesView.DisplayData(data, packetLoss);
    }
}