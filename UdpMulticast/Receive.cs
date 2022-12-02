using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UdpMulticast
{
    // The following Receive class is used by both the ClientOriginator and the ClientTarget class to receive data from one another..
    public class Receive
    {
        // The following static method performs the actual data exchange.
        // In particular, it performs the following tasks:
        // 1)Establishes a communication endpoint.
        // 2)Receive data through this end point on behalf of the caller.
        // 3) Returns the received data in ASCII format.
        public static string ReceiveUntilStop(UdpClient c)
        {
            string strData = "";
            string Ret = "";
            ASCIIEncoding ASCII = new();

            // Establish the communication endpoint.
            var endpoint = new IPEndPoint(IPAddress.IPv6Any, 50);

            while (!strData.Equals("Over"))
            {
                byte[] data = c.Receive(ref endpoint);
                strData = ASCII.GetString(data);
                Ret += strData + "\n";
            }
            return Ret;
        }
    }
}