using System.Net;
using System.Net.Sockets;

namespace ExchangeQuotes.Core.Communication
{
    public class UdpClientWrapper : IDisposable
    {
        protected UdpClient _udpclient;

        protected IPEndPoint _localEndPoint;
        protected IPEndPoint _remoteEndPoint;

        public UdpClientWrapper(int port, string multicastIPAddress, string? localIPAddress = null)
        {
            IPAddress multicastIp = IPAddress.Parse(multicastIPAddress);
            IPAddress localIp = string.IsNullOrEmpty(localIPAddress) ? IPAddress.Any : IPAddress.Parse(localIPAddress);

            _remoteEndPoint = new IPEndPoint(multicastIp, port);
            _localEndPoint = new IPEndPoint(localIp, port);

            _udpclient = CreateAndConfigureUdpClient();

            _udpclient.Client.Bind(_localEndPoint);
            _udpclient.JoinMulticastGroup(multicastIp, localIp);
        }

        public void Dispose()
        {
            _udpclient.Close();
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