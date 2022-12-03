using System.Net;
using System.Net.Sockets;
using ExchangeQuotes.Server.Abstractions;

namespace ExchangeQuotes.Server.Services
{
    internal class UdpMulticastSender : IExchangeQuotesSender
    {
        private readonly UdpClient _udpClient;

        private IPEndPoint? _clientReceiver;

        public UdpMulticastSender(int udpPort)
        {
            _udpClient = new UdpClient(udpPort, AddressFamily.InterNetworkV6);
        }

        public void SendData(double exghange)
        {
            if (_clientReceiver is null)
            {
                throw new InvalidOperationException();
            }

            _udpClient.Send(BitConverter.GetBytes(exghange), _clientReceiver);
        }

        public bool StartMulticastConversation(params object[] dataForConnect)
        {
            if (dataForConnect[0] is null || dataForConnect[1] is null)
            {
                throw new Exception();
            }

            try
            {
                IPAddress ipGroup = IPAddress.Parse(dataForConnect[0].ToString()!);

                int receiverPort = (int)dataForConnect[1];

                _clientReceiver = new(ipGroup, receiverPort);

                _udpClient.JoinMulticastGroup(ipGroup);

                return true;
            }
            catch (Exception)
            {
                //TODO: exception handler

                return false;
            }
        }

        public void Dispose()
        {
            _udpClient!.DropMulticastGroup(_clientReceiver!.Address);
        }
    }
}