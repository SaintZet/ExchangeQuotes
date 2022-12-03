using ExchangeQuotes.Server.Abstractions;
using ExchangeQuotes.Server.Services;
using System.Net;

namespace ExchangeQuotes.Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int port = 2222;
            IPAddress multicastIPAddress = IPAddress.Parse("239.0.0.222");

            IExchangeQuotesSender server = new UdpMulticastSender(port, multicastIPAddress);

            double minRnd = 0.0001;
            double maxRnd = 10000;

            IExchangeQuotesProvider exchangeQuotesProvider = new RandomExchangeQuotesGenerator(minRnd, maxRnd);

            while (true)
            {
                double data = exchangeQuotesProvider.CurrentExchangeQuote();

                server.SendData(data);
                Console.WriteLine(data.ToString()); Console.WriteLine(data.ToString()); Console.WriteLine(data.ToString()); Console.WriteLine(data.ToString()); Console.WriteLine(data.ToString()); Console.WriteLine(data.ToString()); Console.WriteLine(data.ToString()); Console.WriteLine(data.ToString()); Console.WriteLine(data.ToString()); Console.WriteLine(data.ToString()); Console.WriteLine(data.ToString());
                Console.WriteLine(data.ToString());
            }
        }
    }
}