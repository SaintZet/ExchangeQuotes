using ExchangeQuotes.Client.Abstractions;
using ExchangeQuotes.Client.Services;
using System.Collections.Concurrent;

namespace ExchangeQuotes.Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var data = new ConcurrentQueue<double>();
            var signal = new AutoResetEvent(true);

            IExchangeQuotesReceiver client = new UdpMulticastReceiver(1000);

            var groupAddress = "FF01::1";
            client.StartConversation(groupAddress);

            var threadRecive = new Thread(new ThreadStart(() => client.ReciveData(ref data, ref signal)));
            threadRecive.Start();

            IExchangeQuotesCalculateWorker calcWorker = new ExchangeQuotesCalculateStatistic();

            var threadCalc = new Thread(new ThreadStart(() => calcWorker.DoWork(ref data, ref signal)));
            threadCalc.Start();

            while (true)
            {
                var key = Console.ReadKey();

                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine($"Average: {calcWorker.CurrentValues.Average}\n Packets: {client.CountReceivedPackets}");
                }
            }
        }
    }
}