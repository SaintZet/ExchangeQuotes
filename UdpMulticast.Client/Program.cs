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

            ExchangeQuotesCalculateStatistic calcWorker = new ExchangeQuotesCalculateStatistic();

            UdpMulticastReceiver client = new UdpMulticastReceiver(port, multicastIPAddress);

            client.AddMessageReceivedHandler(calcWorker.CalculateValues);

            client.UdpMessageReceived += calcWorker.OnUdpMessageReceived!;

            var threadRecive = new Thread(new ThreadStart(() => client.StartListeningIncomingData()));
            threadRecive.Start();

            //var threadCalc = new Thread(new ThreadStart(() => calcWorker.DoWork(ref data, ref signal)));
            //threadCalc.Start();

            while (true)
            {
                var key = Console.ReadKey();

                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine($"Average: {calcWorker.CurrentValues.Average}\n");
                }
            }
        }
    }
}