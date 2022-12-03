using System.Collections.Concurrent;
using static ExchangeQuotes.Client.Services.UdpMulticastReceiver;

namespace ExchangeQuotes.Client.Abstractions
{
    internal interface IExchangeQuotesReceiver : IDisposable
    {
        public event EventHandler<UdpMessageReceivedEventArgs> UdpMessageReceived;

        int CountReceivedPackets { get; }

        void ReciveData(ref ConcurrentQueue<double> numbers, ref AutoResetEvent signal);

        bool StartConversation(params object[] dataForConnect);
    }
}