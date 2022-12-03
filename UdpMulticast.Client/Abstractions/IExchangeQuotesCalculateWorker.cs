using ExchangeQuotes.Client.Models;

namespace ExchangeQuotes.Client.Abstractions
{
    internal interface IExchangeQuotesCalculateWorker
    {
        public ExchangeQuotesStatistic GetCurrentValues();

        public void CalculateValues(byte[] bytes);
    }
}