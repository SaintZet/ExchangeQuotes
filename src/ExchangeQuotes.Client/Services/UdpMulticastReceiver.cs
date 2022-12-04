using ExchangeQuotes.Client.Abstractions;
using ExchangeQuotes.Core.Communication;
using System.Net;

namespace ExchangeQuotes.Client.Services
{
    internal class UdpMulticastReceiver : UdpClientWrapper, IExchangeQuotesReceiver
    {
        private Action<byte[]>? _receiveHandler;

        public UdpMulticastReceiver(int port, string multicastIPAddress, string? localIPAddress = null)
                    : base(port, multicastIPAddress, localIPAddress)
        {
        }

        public int PacketLoss { get; set; }

        public void StartListeningIncomingData()
        {
            _udpclient.BeginReceive(new AsyncCallback(ReceivedCallback), null);
        }

        public void SetReceiveHandler(Action<byte[]> action)
        {
            _receiveHandler = action;
        }

        private void ReceivedCallback(IAsyncResult asyncResult)
        {
            IPEndPoint sender = new(0, 0);

            byte[] receivedBytes = _udpclient.EndReceive(asyncResult, ref sender!);

            if (_receiveHandler is not null)
            {
                _receiveHandler(receivedBytes);
            }

            _udpclient.BeginReceive(new AsyncCallback(ReceivedCallback), null);
        }
    }
}