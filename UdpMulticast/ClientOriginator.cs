using System.Net;
using System.Net.Sockets;
using UdpMulticast.Domain;

namespace UdpMulticast.Server
{
    internal class ClientOriginator
    {
        private readonly UdpClient _clientOriginator;

        private IPEndPoint? _clientTargetdest;

        public ClientOriginator(UdpClient client)
        {
            _clientOriginator = client;
        }

        public void SendData(double exghange)
        {
            Thread.Sleep(100);
            if (_clientTargetdest is null)
            {
                throw new InvalidOperationException();
            }

            Console.WriteLine($"\nThe ClientOriginator sent:\n");

            Send.OriginatorSendData(_clientOriginator, _clientTargetdest);

            //Console.WriteLine($"\nThe ClientOriginator sent:{exghange}\n");

            //_clientOriginator.Send(BitConverter.GetBytes(exghange), _clientTargetdest!);
        }

        public bool ConnectServerAndClient(IPAddress groupAddress, int clientPort)
        {
            try
            {
                _clientTargetdest = new(groupAddress, clientPort);

                // Display the multicast address used.
                Console.WriteLine("Multicast Address: [" + groupAddress.ToString() + "]");

                // Exercise the use of the IPv6MulticastOption.
                Console.WriteLine("Instantiate IPv6MulticastOption(IPAddress)");

                // Instantiate IPv6MulticastOption using one of the overloaded constructors.
                var ipv6MulticastOption = new IPv6MulticastOption(groupAddress);

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
                _clientOriginator.JoinMulticastGroup((int)interfaceIndex, group);

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("[ClientOriginator.ConnectClients] Exception: " + e.ToString());
                return false;
            }
        }

        public void CloseConnection()
        {
            _clientOriginator!.DropMulticastGroup(_clientTargetdest!.Address);
        }
    }
}