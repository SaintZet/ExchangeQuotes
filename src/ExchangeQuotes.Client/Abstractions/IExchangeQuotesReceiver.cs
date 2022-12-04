namespace ExchangeQuotes.Client.Abstractions
{
    internal interface IExchangeQuotesReceiver : IDisposable
    {
        public event EventHandler? DataReceived;

        int PacketLoss { get; set; }

        public void StartListeningIncomingData();
    }
}