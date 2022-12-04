namespace ExchangeQuotes.Core.Abstractions
{
    public interface IExchangeQuotesReceiver
    {
        event EventHandler? DataReceived;

        //Emulate packet delay
        bool RecivePause { get; set; }

        long PacketLoss { get; }

        void StartListeningIncomingData();
    }
}