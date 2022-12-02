using System.Net;
using System.Net.Sockets;

namespace UdpMulticast
{
    // The following Send class is used by both the ClientOriginator and ClientTarget classes to send data to one another.
    public class Send
    {
        private static readonly char[] _greetings = { 'H', 'e', 'l', 'l', 'o', ' ', 'T', 'a', 'r', 'g', 'e', 't', '.' };
        private static readonly char[] _nice = { 'H', 'a', 'v', 'e', ' ', 'a', ' ', 'n', 'i', 'c', 'e', ' ', 'd', 'a', 'y', '.' };
        private static readonly char[] _eom = { 'O', 'v', 'e', 'r' };

        private static readonly char[] _tGreetings = { 'H', 'e', 'l', 'l', 'o', ' ', 'O', 'r', 'i', 'g', 'i', 'n', 'a', 't', 'o', 'r', '!' };
        private static readonly char[] _tNice = { 'Y', 'o', 'u', ' ', 't', 'o', 'o', '.' };

        // The following static method sends data to the ClientTarget on behalf of the ClientOriginator.
        public static void OriginatorSendData(UdpClient udpClient, IPEndPoint ipEndPoint)
        {
            Console.WriteLine(new string(_greetings));
            udpClient.Send(GetByteArray(_greetings), _greetings.Length, ipEndPoint);
            Thread.Sleep(1000);

            Console.WriteLine(new string(_nice));
            udpClient.Send(GetByteArray(_nice), _nice.Length, ipEndPoint);

            Thread.Sleep(1000);
            Console.WriteLine(new string(_eom));
            udpClient.Send(GetByteArray(_eom), _eom.Length, ipEndPoint);
        }

        // The following static method sends data to the ClientOriginator on behalf of the ClientTarget.
        public static void TargetSendData(UdpClient c, IPEndPoint ep)
        {
            Console.WriteLine(new string(_tGreetings));
            c.Send(GetByteArray(_tGreetings), _tGreetings.Length, ep);
            Thread.Sleep(1000);

            Console.WriteLine(new string(_tNice));
            c.Send(GetByteArray(_tNice), _tNice.Length, ep);

            Thread.Sleep(1000);
            Console.WriteLine(new string(_eom));
            c.Send(GetByteArray(_eom), _eom.Length, ep);
        }

        // Internal utility
        private static byte[] GetByteArray(char[] ChArray)
        {
            byte[] Ret = new byte[ChArray.Length];
            for (int i = 0; i < ChArray.Length; i++)
            {
                Ret[i] = (byte)ChArray[i];
            }

            return Ret;
        }
    }
}