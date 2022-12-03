using ExchangeQuotes.Client.Abstractions;
using ExchangeQuotes.Client.Services;
using System.Net;

namespace ExchangeQuotes.Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int port = 2222;
            IPAddress multicastIPAddress = IPAddress.Parse("239.0.0.222");

            IExchangeQuotesCalculateWorker calcWorker = new ExchangeQuotesCalculateStatistic();

            IExchangeQuotesReceiver client = new UdpMulticastReceiver(port, multicastIPAddress);

            client.SetReceiveHandler(calcWorker.CalculateValues);

            var threadRecive = new Thread(new ThreadStart(() => client.StartListeningIncomingData()));
            threadRecive.Start();

            while (true)
            {
                var key = Console.ReadKey();

                if (key.Key == ConsoleKey.Enter)
                {
                    var statistic = calcWorker.GetCurrentValues();
                    Console.WriteLine($"Average: {statistic.Average}\n");
                }
            }
        }
    }
}