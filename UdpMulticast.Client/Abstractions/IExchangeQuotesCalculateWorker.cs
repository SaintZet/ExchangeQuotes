using ExchangeQuotes.Client.Models;
using ExchangeQuotes.Client.Services;

namespace ExchangeQuotes.Client.Abstractions
{
    internal interface IExchangeQuotesCalculateWorker
    {
        ExchangeQuotesStatistic CurrentValues { get; }

        public void OnUdpMessageReceived(object sender, UdpMulticastReceiver.UdpMessageReceivedEventArgs e);

        public void CalculateValues(double exchangeQuote);
    }
}