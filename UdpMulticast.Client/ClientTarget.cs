using System;
using System.Net;
using System.Net.Sockets;
using UdpMulticast.Domain;

namespace UdpMulticast.Client
{
    internal class MyUdpClient
    {
        private readonly UdpClient _udpClient;
        private readonly IPAddress _groupAddress;

        public MyUdpClient(UdpClient udpClient, IPAddress groupAddress)
        {
            if (udpClient is null)
            {
                throw new Exception();
            }

            _groupAddress = groupAddress;
            _udpClient = udpClient;
        }

        public void StartMulticastConversation()
        {
            _udpClient.JoinMulticastGroup(_groupAddress);

            string ret = Receive.ReceiveUntilStop(_udpClient);

            Console.WriteLine("\nThe ClientTarget received: " + "\n\n" + ret + "\n");
        }

        public void CloseConection()
        {
            _udpClient.DropMulticastGroup(_groupAddress);
        }
    }
}