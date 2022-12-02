using System.Net;
using System.Net.Sockets;

namespace UdpMulticast
{
    // The ClientTarget class is the receiver of the ClientOriginator messages.
    // The StartMulticastConversation method contains the logic for exchanging data between the
    // ClientTarget and its counterpart ClientOriginator in a multicast operation.
    public class ClientTarget
    {
        private static readonly IPAddress _grpAddr = IPAddress.Parse("FF01::1");

        private static UdpClient? _clientTarget;

        // The following StartMulticastConversation method connects the UDP ClientTarget with the  ClientOriginator.
        // It performs the following main tasks:
        // 1)Creates a UDP client to receive data on a specific port and using IPv6 addresses.
        // The port is the same one used by the  ClientOriginator to define its communication endpoint.
        // 2)Joins or creates a multicast group at the specified address.
        // 3)Defines the endpoint port to send data to the ClientOriginator.
        // 4)Receives data from the ClientOriginator until the end of the communication.
        // 5)Sends data to the ClientOriginator. Note this method is the counterpart of the ClientOriginator.ConnectOriginatorAndTarget().
        public static void StartMulticastConversation()
        {
            string Ret;

            // Bind and listen on port 1000. Specify the IPv6 address family type.
            _clientTarget = new UdpClient(1000, AddressFamily.InterNetworkV6);

            // Join or create a multicast group

            // Use the overloaded JoinMulticastGroup method. Refer to the ClientOriginator method to
            // see how to use the other methods.
            _clientTarget.JoinMulticastGroup(_grpAddr);

            // Define the endpoint data port. Note that this port number must match the
            // ClientOriginator UDP port number which is the port on which the ClientOriginator is
            // receiving data.
            var ClientOriginatordest = new IPEndPoint(_grpAddr, 2000);

            // Receive data from the ClientOriginator.
            Ret = Receive.ReceiveUntilStop(_clientTarget);
            Console.WriteLine("\nThe ClientTarget received: " + "\n\n" + Ret + "\n");

            // Done receiving, now respond to the ClientOriginator.

            // Wait to make sure the ClientOriginator is ready to receive.
            Thread.Sleep(2000);

            Console.WriteLine("\nThe ClientTarget sent:\n");

            Send.TargetSendData(_clientTarget, ClientOriginatordest);

            // Exit the multicast conversation.
            _clientTarget.DropMulticastGroup(_grpAddr);
        }
    }
}