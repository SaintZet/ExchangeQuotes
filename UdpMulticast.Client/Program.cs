using ExchangeQuotes.Client.Abstractions;
using ExchangeQuotes.Client.Models;
using ExchangeQuotes.Client.Services;
using ExchangeQuotes.Core.Abstractions;
using ExchangeQuotes.Core.Сonfiguration;

namespace ExchangeQuotes.Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IConfigProvider<Config> configPrivider = new XmlConfigProvider<Config>("ClientConfig.xml");

            var config = configPrivider.GetOrCreateDefaultConfig();

            IExchangeQuotesReceiver client = new UdpMulticastReceiver(config.Port, config.MulticastIPAddress);

            IExchangeQuotesCalculateWorker calcWorker = new ExchangeQuotesCalculateStatistic();

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