using ExchangeQuotes.Client.Abstractions;
using static ExchangeQuotes.Client.Services.UdpMulticastReceiver;

namespace ExchangeQuotes.Client
{
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
            _exchangeQuotesReceiver!.DataReceived += DataReciveHandler;

            var threadRecive = new Thread(new ThreadStart(() => _exchangeQuotesReceiver.StartListeningIncomingData()));
            threadRecive.Start();

            _exchangeQuotesView.RequestedData += DataRequestedHandler;
            _exchangeQuotesView.StartWork();

            var timer = new Timer(ReciverDelay!, workDelay, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }

        private void ReciverDelay(object delay)
        {
            _exchangeQuotesReceiver!.DataReceived -= DataReciveHandler;

            Thread.Sleep((int)delay);

            _exchangeQuotesReceiver!.DataReceived += DataReciveHandler;
        }

        private void DataReciveHandler(object? sender, EventArgs e)
        {
            ReceivedEventArgs receivedEventArgs = (ReceivedEventArgs)e;
            _exchangeQuotesCalculateWorker!.CalculateValues(receivedEventArgs.Data!);
        }

        private void DataRequestedHandler(object? sender, EventArgs e)
        {
            var data = _exchangeQuotesCalculateWorker.GetCurrentValues();
            var packetLoss = _exchangeQuotesReceiver.PacketLoss;

            _exchangeQuotesView.DisplayData(data, packetLoss);
        }
    }
}