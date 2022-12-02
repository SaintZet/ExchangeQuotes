namespace UdpMulticast
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Join the multicast group.

            if (ClientOriginator.ConnectOriginatorAndTarget())
            {
                // Perform a multicast conversation with the ClientTarget.
                string Ret = ClientOriginator.SendAndReceive();
                Console.WriteLine("\nThe ClientOriginator received: " + "\n\n" + Ret);
            }
            else
            {
                Console.WriteLine("Unable to Join the multicast group");
            }
        }
    }
}