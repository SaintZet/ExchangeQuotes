using ExchangeQuotes.Core.Communication;
using System.Net;

namespace ExchangeQuotes.Client.Services
{
    internal class UdpMulticastReceiver : UdpClientWrapper
    {
        private Action<byte[]> _action;

        public UdpMulticastReceiver(int port, IPAddress multicastIPAddress, IPAddress? localIPAddress = null)
                    : base(port, multicastIPAddress, localIPAddress)
        {
        }

        public event EventHandler<UdpMessageReceivedEventArgs>? UdpMessageReceived;

        public void StartListeningIncomingData()
        {
            _udpclient.BeginReceive(new AsyncCallback(ReceivedCallback), null);
        }

        internal void AddMessageReceivedHandler(Action<byte[]> action)
        {
            _action = action;
        }

        private void ReceivedCallback(IAsyncResult asyncResult)
        {
            try
            {
                int managedThreadId = Environment.CurrentManagedThreadId;
                Console.WriteLine("Reciver: " + managedThreadId);

                IPEndPoint sender = new(0, 0);
                byte[] receivedBytes = _udpclient.EndReceive(asyncResult, ref sender!);

                _action(receivedBytes);

                //UdpMessageReceived?.Invoke(this, new UdpMessageReceivedEventArgs() { Data = receivedBytes });

                _udpclient.BeginReceive(new AsyncCallback(ReceivedCallback), null);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        internal class UdpMessageReceivedEventArgs : EventArgs
        {
            public byte[]? Data { get; set; }
        }
    }
}