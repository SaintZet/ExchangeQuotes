using ExchangeQuotes.Core.Communication;
using ExchangeQuotes.Server.Abstractions;
using System.Net;

namespace ExchangeQuotes.Server.Services
{
    internal class UdpMulticastSender : UdpClientWrapper, IExchangeQuotesSender
    {
        public UdpMulticastSender(int port, IPAddress multicastIPAddress, IPAddress? localIPAddress = null)
            : base(port, multicastIPAddress, localIPAddress)
        {
        }

        public void SendData(double data)
        {
            byte[] dgram = BitConverter.GetBytes(data);

            _udpclient.Send(dgram, dgram.Length, _remoteEndPoint);
        }
    }
}