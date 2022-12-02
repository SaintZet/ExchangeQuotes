using UdpMulticast.Client.Services;

namespace UdpMulticast.Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var client = new UdpMulticastReceiver(1000);

            var groupAddress = "FF01::1";
            client.StartMulticastConversation(groupAddress);

            var thread = new Thread(new ThreadStart(client.ReciveData));
            thread.Start();

            while (true)
            {
                var key = Console.ReadKey();

                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine(client.CountReceivedPackets.ToString());
                }
            }
        }
    }
}