using UdpMulticast.Server.Abstractions;
using UdpMulticast.Server.Services;

namespace UdpMulticast.Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int serverPort = 2000;
            using IExchangeQuotesSender<double> server = new UdpMulticastSender<double>(serverPort);

            double minRnd = 0.0001;
            double maxRnd = 1000;
            IExchangeQuotesProvider<double> exchangeQuotesProvider = new RandomExchangeQuotesGenerator<double>(minRnd, maxRnd);

            var groupAddress = "FF01::1";
            var clientPort = 1000;

            if (!server.StartMulticastConversation(groupAddress, clientPort))
            {
                throw new Exception("Unable to Join the multicast group");
            }

            Console.WriteLine($"The ClientOriginator sent:\n");

            for (int i = 0; i < 1000; i++)
            {
                double data = exchangeQuotesProvider.CurrentExchangeQuote();

                server.SendData(data);

                Console.WriteLine(data.ToString());
            }
        }
    }
}