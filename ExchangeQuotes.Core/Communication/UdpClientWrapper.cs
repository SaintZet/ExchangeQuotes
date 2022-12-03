using System.Net;
using System.Net.Sockets;

namespace ExchangeQuotes.Core.Communication
{
    public class UdpClientWrapper
    {
        protected UdpClient _udpclient;

        protected IPEndPoint _localEndPoint;
        protected IPEndPoint _remoteEndPoint;

        public UdpClientWrapper(int port, IPAddress multicastIPAddress, IPAddress? localIPAddress = null)
        {
            _remoteEndPoint = new IPEndPoint(multicastIPAddress, port);

            localIPAddress ??= IPAddress.Any;
            _localEndPoint = new IPEndPoint(localIPAddress, port);

            _udpclient = CreateAndConfigureUdpClient();

            _udpclient.Client.Bind(_localEndPoint);
            _udpclient.JoinMulticastGroup(multicastIPAddress, localIPAddress);
        }

        private UdpClient CreateAndConfigureUdpClient()
        {
            var udpClient = new UdpClient();

            // The following three lines allow multiple clients on the same PC
            udpClient.ExclusiveAddressUse = false;
            udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            udpClient.ExclusiveAddressUse = false;

            return udpClient;
        }
    }
}