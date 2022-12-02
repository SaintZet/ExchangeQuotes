using System.Net;
using System.Net.Sockets;
using UdpMulticast.Server.Abstractions;

namespace UdpMulticast.Server.Services
{
    internal class UdpMulticastSender<T> : IDisposable, IExchangeQuotesSender<T> where T : struct
    {
        private readonly UdpClient _udpClient;

        private IPEndPoint? _clientReceiver;

        public UdpMulticastSender(int udpPort)
        {
            _udpClient = new UdpClient(udpPort, AddressFamily.InterNetworkV6);
        }

        public void SendData(T exghange)
        {
            if (_clientReceiver is null)
            {
                throw new InvalidOperationException();
            }

            _udpClient.Send(BitConverter.GetBytes((dynamic)exghange), _clientReceiver!);
        }

        public bool StartMulticastConversation(params object[] dataForConnect)
        {
            if (dataForConnect[0] is null || dataForConnect[1] is null)
            {
                throw new Exception();
            }

            IPAddress ipGroup = IPAddress.Parse(dataForConnect[0].ToString()!);
            int receiverPort = (int)dataForConnect[1];

            try
            {
                _clientReceiver = new(ipGroup, receiverPort);

                // Display the multicast address used.
                Console.WriteLine("Multicast Address: [" + ipGroup.ToString() + "]");

                // Exercise the use of the IPv6MulticastOption.
                Console.WriteLine("Instantiate IPv6MulticastOption(IPAddress)");

                // Instantiate IPv6MulticastOption using one of the overloaded constructors.
                var ipv6MulticastOption = new IPv6MulticastOption(ipGroup);

                // Store the IPAdress multicast options.
                IPAddress group = ipv6MulticastOption.Group;
                long interfaceIndex = ipv6MulticastOption.InterfaceIndex;

                // Display IPv6MulticastOption properties.
                Console.WriteLine("IPv6MulticastOption.Group: [" + group + "]");
                Console.WriteLine("IPv6MulticastOption.InterfaceIndex: [" + interfaceIndex + "]");

                // Instantiate IPv6MulticastOption using another overloaded constructor.
                var ipv6MulticastOption2 = new IPv6MulticastOption(group, interfaceIndex);

                // Store the IPAdress multicast options.
                group = ipv6MulticastOption2.Group;
                interfaceIndex = ipv6MulticastOption2.InterfaceIndex;

                // Display the IPv6MulticastOption2 properties.
                Console.WriteLine("IPv6MulticastOption.Group: [" + group + "]");
                Console.WriteLine("IPv6MulticastOption.InterfaceIndex: [" + interfaceIndex + "]");

                // Join the specified multicast group using one of the JoinMulticastGroup overloaded methods.
                _udpClient.JoinMulticastGroup((int)interfaceIndex, group);

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("[ClientOriginator.ConnectClients] Exception: " + e.ToString());
                return false;
            }
        }

        public void Dispose()
        {
            _udpClient!.DropMulticastGroup(_clientReceiver!.Address);
        }
    }
}