using ExchangeQuotes.Client.Models;

namespace ExchangeQuotes.Client.Abstractions
{
    internal interface IExchangeQuotesCalculateWorker
    {
        ExchangeQuotesStatistic GetCurrentValues();

        void CalculateValues(byte[] bytes);
    }
}