using System.Collections.Concurrent;

namespace UdpMulticast.Client.Services
{
    internal class ExchangeQuoteCalc
    {
        public void DoSomething(ref ConcurrentQueue<double> items, ref AutoResetEvent signal)
        {
            while (true)
            {
                signal.WaitOne();

                double item;
                while (items.TryDequeue(out item))
                {
                    Console.WriteLine(item);
                }
            }
        }
    }
}