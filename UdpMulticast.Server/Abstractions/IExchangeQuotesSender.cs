namespace UdpMulticast.Server.Abstractions
{
    internal interface IExchangeQuotesSender : IDisposable
    {
        void SendData(double exchangeQuotes);

        bool StartMulticastConversation(params object[] dataForConnect);
    }
}