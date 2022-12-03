using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using ExchangeQuotes.Client.Abstractions;

namespace ExchangeQuotes.Client.Services
{
    internal class UdpMulticastReceiver : IExchangeQuotesReceiver
    {
        private readonly UdpClient _udpClient;
        private IPAddress? _ipGroupAddress;

        public UdpMulticastReceiver(int udpPort)
        {
            _udpClient = new UdpClient(udpPort, AddressFamily.InterNetworkV6);
        }

        public int CountReceivedPackets { get; private set; }

        public bool StartConversation(params object[] dataForConnect)
        {
            if (dataForConnect[0] is null || dataForConnect is null)
            {
                throw new ArgumentException($"First element must be ip group address.");
            }

            try
            {
                _ipGroupAddress = IPAddress.Parse(dataForConnect[0].ToString()!);
                _udpClient.JoinMulticastGroup(_ipGroupAddress);
            }
            catch (Exception)
            {
                //TODO: Add handler
            }

            return true;
        }

        public void ReciveData(ref ConcurrentQueue<double> numbers, ref AutoResetEvent signal)
        {
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
            catch (Exception)
            {
                //TODO: Add handler
            }
            finally
            {
                Dispose();
            }
        }

        public void Dispose()
        {
            _udpClient.DropMulticastGroup(_ipGroupAddress!);
        }
    }
}