using ExchangeQuotes.Client.Models;
using System.Collections.Concurrent;

namespace ExchangeQuotes.Client.Abstractions
{
    internal interface IExchangeQuotesCalculateWorker
    {
        ExchangeQuotesStatistic CurrentValues { get; }

        void DoWork(ref ConcurrentQueue<double> exchangeQuotes, ref AutoResetEvent signal);
    }
}