namespace UdpMulticast.Client.Abstractions
{
    internal interface IExchangeQuotesReceiver<T> : IDisposable where T : struct
    {
        void ReciveData(T exchangeQuotes);

        bool StartMulticastConversation(params object[] dataForConnect);
    }
}