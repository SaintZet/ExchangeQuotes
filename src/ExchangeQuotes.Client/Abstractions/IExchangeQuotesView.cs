using ExchangeQuotes.Client.Models;

namespace ExchangeQuotes.Client.Abstractions
{
    internal interface IExchangeQuotesView
    {
        public event EventHandler? RequestedData;

        void StartWork();

        void DisplayData(ExchangeQuotesStatistic data, int packetLoss);
    }
}