using System.Net;
using System.Net.Sockets;

namespace UdpMulticast.Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var serverUdpClient = new UdpClient(2000, AddressFamily.InterNetworkV6);
            var groupAddress = IPAddress.Parse("FF01::1");
            var clientPort = 1000;

            var server = new ClientOriginator(serverUdpClient);

            if (server.ConnectServerAndClient(groupAddress, clientPort))
            {
                server.SendData(20.3);
                //server.CloseConnection();
            }
            else
            {
                Console.WriteLine("Unable to Join the multicast group");
            }

            Console.ReadLine();
        }
    }
}