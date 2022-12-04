using ExchangeQuotes.Core.Communication;
using ExchangeQuotes.Server.Abstractions;

namespace ExchangeQuotes.Server.Services
{
    internal class UdpMulticastSender : UdpClientWrapper, IExchangeQuotesSender
    {
        public UdpMulticastSender(int port, string multicastIPAddress, string? localIPAddress = null)
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