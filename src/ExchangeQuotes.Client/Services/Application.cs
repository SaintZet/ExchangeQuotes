﻿using ExchangeQuotes.Client.Abstractions;
using ExchangeQuotes.Core.Abstractions;
using ExchangeQuotes.Core.Communication;

namespace ExchangeQuotes.Client.Services;

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

        var thread = new Thread(new ThreadStart(_exchangeQuotesReceiver.StartListeningIncomingData));
        thread.Start();

        var timer = new Timer(ReciverDelay!, workDelay, TimeSpan.Zero, TimeSpan.FromSeconds(1));

        _exchangeQuotesView.RequestedData += DataRequestedHandler;
        _exchangeQuotesView.StartDoWork();
    }

    //Emulate packet delay
    private void ReciverDelay(object delay)
    {
        int ms = (int)delay;

        if (delay == null)
        {
            return;
        }

        _exchangeQuotesReceiver.RecivePause = true;

        Thread.Sleep(ms);

        _exchangeQuotesReceiver.RecivePause = false;
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