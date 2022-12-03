namespace ExchangeQuotes.Client.Abstractions
{
    internal interface IExchangeQuotesReceiver // : IDisposable
    {
        int PacketLoss { get; set; }

        public void StartListeningIncomingData();

        public void SetReceiveHandler(Action<byte[]> action);
    }
}