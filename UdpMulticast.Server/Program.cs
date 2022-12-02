using UdpMulticast.Server.Abstractions;
using UdpMulticast.Server.Services;

namespace UdpMulticast.Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int serverPort = 2000;
            using IExchangeQuotesSender server = new UdpMulticastSender(serverPort);

            double minRnd = 0.0001;
            double maxRnd = 1000;
            IExchangeQuotesProvider exchangeQuotesProvider = new RandomExchangeQuotesGenerator(minRnd, maxRnd);

            var groupAddress = "FF01::1";
            var clientPort = 1000;

            if (!server.StartMulticastConversation(groupAddress, clientPort))
            {
                throw new Exception("Unable to Join the multicast group");
            }

            for (int i = 0; i < 200000; i++)
            {
                double data = exchangeQuotesProvider.CurrentExchangeQuote();

                server.SendData(data);

                Console.WriteLine(data.ToString());
            }
        }
    }
}