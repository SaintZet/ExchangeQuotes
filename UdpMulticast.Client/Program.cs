using System.Net;
using System.Net.Sockets;

namespace UdpMulticast.Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //TODO: Get this from xml:

            var groupAddress = IPAddress.Parse("FF01::1");
            //var serverPort = 2000;
            var udpClient = new UdpClient(1000, AddressFamily.InterNetworkV6);

            var client = new MyUdpClient(udpClient, groupAddress);

            client.StartMulticastConversation();

            Thread.Sleep(2000);

            Console.ReadLine();
        }
    }
}