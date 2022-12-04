using ExchangeQuotes.Client.Abstractions;
using ExchangeQuotes.Core.Communication;
using System.Net;

namespace ExchangeQuotes.Client.Services
{
    internal class UdpMulticastReceiver : UdpClientWrapper, IExchangeQuotesReceiver
    {
        public UdpMulticastReceiver(int port, string multicastIPAddress, string? localIPAddress = null)
                    : base(port, multicastIPAddress, localIPAddress)
        {
        }

        public event EventHandler? DataReceived;

        public int PacketLoss { get; set; }

        public void StartListeningIncomingData()
        {
            _udpclient.BeginReceive(new AsyncCallback(ReceivedCallback), null);
        }

        private void ReceivedCallback(IAsyncResult asyncResult)
        {
            IPEndPoint sender = new(0, 0);

            byte[] receivedBytes = _udpclient.EndReceive(asyncResult, ref sender!);

            DataReceived?.Invoke(this, new ReceivedEventArgs() { Data = receivedBytes });

            _udpclient.BeginReceive(new AsyncCallback(ReceivedCallback), null);
        }

        internal class ReceivedEventArgs : EventArgs
        {
            public byte[]? Data { get; set; }
        }
    }
}