using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using UdpMulticast.Client.Abstractions;

namespace UdpMulticast.Client.Services
{
    internal class UdpMulticastReceiver : IExchangeQuotesReceiver
    {
        private readonly UdpClient _udpClient;
        private IPAddress? _ipGroup;

        public UdpMulticastReceiver(int udpPort)
        {
            _udpClient = new UdpClient(udpPort, AddressFamily.InterNetworkV6);
        }

        public int CountReceivedPackets { get; private set; }

        public bool StartMulticastConversation(params object[] dataForConnect)
        {
            if (dataForConnect[0] is null)
            {
                throw new Exception();
            }

            _ipGroup = IPAddress.Parse(dataForConnect[0].ToString()!);
            _udpClient.JoinMulticastGroup(_ipGroup);

            return true;
        }

        public void ReciveData(ref ConcurrentQueue<double> numbers, ref AutoResetEvent signal)
        {
            numbers = new ConcurrentQueue<double>();
            try
            {
                var endpoint = new IPEndPoint(IPAddress.IPv6Any, 50);

                while (true)
                {
                    byte[] bytes = _udpClient.Receive(ref endpoint);
                    double number = BitConverter.ToDouble(bytes, 0);

                    numbers.Enqueue(number);
                    signal.Set();

                    CountReceivedPackets++;
                }
            }
            catch (ThreadInterruptedException)
            {
                Thread.Sleep(1000);
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