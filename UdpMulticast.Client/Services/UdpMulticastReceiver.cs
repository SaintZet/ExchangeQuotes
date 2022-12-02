using System.Net;
using System.Net.Sockets;

namespace UdpMulticast.Client.Services
{
    internal class UdpMulticastReceiver : IDisposable
    {
        private readonly UdpClient _udpClient;
        private IPAddress? _ipGroup;

        public UdpMulticastReceiver(int udpPort)
        {
            _udpClient = new UdpClient(udpPort, AddressFamily.InterNetworkV6);
        }

        public int CountReceivedPackets { get; private set; }

        public void StartMulticastConversation(params object[] dataForConnect)
        {
            if (dataForConnect[0] is null)
            {
                throw new Exception();
            }

            _ipGroup = IPAddress.Parse(dataForConnect[0].ToString()!);
            _udpClient.JoinMulticastGroup(_ipGroup);
        }

        public void ReciveData()
        {
            var endpoint = new IPEndPoint(IPAddress.IPv6Any, 50);

            try
            {
                while (true)
                {
                    byte[] bytes = _udpClient.Receive(ref endpoint);
                    double number = BitConverter.ToDouble(bytes, 0);

                    CountReceivedPackets++;

                    Console.WriteLine(number.ToString());

                    //raise event
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                Dispose();
                Console.WriteLine("Done listening for UDP broadcast");
            }
        }

        public void Dispose()
        {
            _udpClient.DropMulticastGroup(_ipGroup!);
        }
    }
}