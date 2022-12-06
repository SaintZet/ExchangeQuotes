namespace ExchangeQuotes.Server.Models;

public record Config
{
    public int Port { get; set; } = 2222;
    public string MulticastIPAddress { get; set; } = "239.0.0.222";
    public int MinValue { get; set; } = 1;
    public int MaxValue { get; set; } = 100;
}