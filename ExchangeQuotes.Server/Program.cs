using ExchangeQuotes.Core.Abstractions;
using ExchangeQuotes.Core.Сonfiguration;
using ExchangeQuotes.Server.Abstractions;
using ExchangeQuotes.Server.Models;
using ExchangeQuotes.Server.Services;

namespace ExchangeQuotes.Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IConfigProvider<Config> configPrivider = new XmlConfigProvider<Config>("ServerConfig.xml");

            var config = configPrivider.GetOrCreateDefaultConfig();

            IExchangeQuotesSender server = new UdpMulticastSender(config.Port, config.MulticastIPAddress);

            IExchangeQuotesProvider exchangeQuotesProvider = new RandomExchangeQuotesGenerator(config.MinValue, config.MaxValue);

            while (true)
            {
                double data = exchangeQuotesProvider.CurrentExchangeQuote();

                server.SendData(data);

                Console.WriteLine(data.ToString());
            }
        }
    }
}