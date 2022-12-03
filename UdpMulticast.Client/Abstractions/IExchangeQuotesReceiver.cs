using System.Collections.Concurrent;

namespace ExchangeQuotes.Client.Abstractions
{
    internal interface IExchangeQuotesReceiver : IDisposable
    {
        int CountReceivedPackets { get; }

        void ReciveData(ref ConcurrentQueue<double> numbers, ref AutoResetEvent signal);

        bool StartConversation(params object[] dataForConnect);
    }
}