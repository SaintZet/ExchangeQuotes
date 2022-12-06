namespace ExchangeQuotes.Core.Communication.Udp;

public class ReceivedEventArgs : EventArgs
{
    public byte[]? Data { get; set; }
}