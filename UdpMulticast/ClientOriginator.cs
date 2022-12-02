using System.Net;
using System.Net.Sockets;

namespace UdpMulticast
{
    // The following ClientOriginator class starts the multicast conversation with the ClientTarget class..
    // It performs the following main tasks:
    // 1)Creates a socket and binds it to the port on  which to communicate.
    // 2)Specifies that the connection must use an IPv6 address.
    // 3)Joins or create a multicast group. Note that the multicast address ranges to use are specified in the RFC#2375.
    // 4)Defines the endpoint to send the data to and starts the client target (ClientTarget) thread.
    public class ClientOriginator
    {
        // Transform the string address into the internal format.
        private static readonly IPAddress _grpAddr = IPAddress.Parse("FF01::1");

        // Bind and listen on port 2000.
        // This constructor creates a socket and binds it to the port on which to receive data.
        // The family parameter specifies that this connection uses an IPv6 address.
        private static readonly UdpClient _clientOriginator = new(2000, AddressFamily.InterNetworkV6);

        // Define the endpoint data port.
        // Note that this port number must match the ClientTarget UDP port number which is the port on which the ClientTarget is receiving data.
        private static readonly IPEndPoint? _clientTargetdest = new(_grpAddr, 1000);

        private static readonly Thread _thread = new(new ThreadStart(ClientTarget.StartMulticastConversation));

        // The ConnectOriginatorAndTarget method connects the ClientOriginator with the ClientTarget.
        // It performs the following main tasks:
        // 1)Creates a UDP client to receive data on a specific port using IPv6 addresses.
        // 2)Joins or create a multicast group at the specified address.
        // 3)Defines the endpoint port to send data to on the ClientTarget.
        // 4)Starts the ClientTarget thread that also creates the ClientTarget object.
        // Note this method is the counterpart of the ClientTarget.StartMulticastConversation().
        public static bool ConnectOriginatorAndTarget()
        {
            try
            {
                // Join or create a multicast group. The multicast address ranges to use are
                // specified in RFC#2375. You are free to use different addresses.

                // Display the multicast address used.
                Console.WriteLine("Multicast Address: [" + _grpAddr.ToString() + "]");

                // Exercise the use of the IPv6MulticastOption.
                Console.WriteLine("Instantiate IPv6MulticastOption(IPAddress)");

                // Instantiate IPv6MulticastOption using one of the overloaded constructors.
                var ipv6MulticastOption = new IPv6MulticastOption(_grpAddr);

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

                // Start the ClientTarget thread so it is ready to receive.
                _thread.Start();

                // Make sure that the thread has started.
                Thread.Sleep(2000);

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("[ClientOriginator.ConnectClients] Exception: " + e.ToString());
                return false;
            }
        }

        // The SendAndReceive performs the data exchange between the ClientOriginator and the ClientTarget classes.
        public static string SendAndReceive()
        {
            // Send data to ClientTarget.
            Console.WriteLine("\nThe ClientOriginator sent:\n");
            Send.OriginatorSendData(_clientOriginator!, _clientTargetdest!);

            // Receive data from ClientTarget
            string ret = Receive.ReceiveUntilStop(_clientOriginator!);

            // Stop the ClientTarget thread
            _thread!.Interrupt();

            // Abandon the multicast group.
            _clientOriginator!.DropMulticastGroup(_grpAddr!);

            return ret;
        }
    }
}