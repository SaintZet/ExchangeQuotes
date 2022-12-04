namespace ExchangeQuotes.Server.Models
{
    public record Config
    {
        public int Port { get; set; } = 2222;
        public string MulticastIPAddress { get; set; } = "239.0.0.222";
        public double MinValue { get; set; } = 0;
        public double MaxValue { get; set; } = 38;
    }
}