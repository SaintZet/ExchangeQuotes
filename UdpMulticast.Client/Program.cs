using System.Collections.Concurrent;
using System.Timers;
using UdpMulticast.Client.Abstractions;
using UdpMulticast.Client.Services;

namespace UdpMulticast.Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var queue = new ConcurrentQueue<double>();
            var signal = new AutoResetEvent(true);

            IExchangeQuotesReceiver client = new UdpMulticastReceiver(1000);

            var groupAddress = "FF01::1";
            client.StartMulticastConversation(groupAddress);

            var thread = new Thread(new ThreadStart(() => client.ReciveData(ref queue, ref signal)));
            thread.Start();

            var consumer = new ExchangeQuoteCalc();
            var threadCalk = new Thread(new ThreadStart(() => consumer.DoSomething(ref queue, ref signal)));
            threadCalk.Start();

            //var timer = new System.Timers.Timer(1000);

            //timer.Elapsed += (sender, eventArgs) =>
            //{
            //    thread.Interrupt();
            //};

            //timer.Start();

            while (true)
            {
                var key = Console.ReadKey();

                if (key.Key == ConsoleKey.Enter)
                {
                    //Console.WriteLine(client.CountReceivedPackets.ToString());
                }
            }
        }
    }
}