using System.Collections.Concurrent;

namespace UdpMulticast.Client.Abstractions
{
    internal interface IExchangeQuotesReceiver : IDisposable
    {
        void ReciveData(ref ConcurrentQueue<double> numbers, ref AutoResetEvent signal);

        bool StartMulticastConversation(params object[] dataForConnect);
    }
}