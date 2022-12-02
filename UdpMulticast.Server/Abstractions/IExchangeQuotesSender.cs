namespace UdpMulticast.Server.Abstractions
{
    internal interface IExchangeQuotesSender<T> : IDisposable where T : struct
    {
        void SendData(T exchangeQuotes);

        bool StartMulticastConversation(params object[] dataForConnect);
    }
}