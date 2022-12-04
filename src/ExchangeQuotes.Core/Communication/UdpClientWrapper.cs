using ExchangeQuotes.Core.Abstractions;
using System.Net;
using System.Net.Sockets;

namespace ExchangeQuotes.Core.Communication
{
    public sealed class UdpClientWrapper : IDisposable, IExchangeQuotesReceiver, IExchangeQuotesSender
    {
        private readonly UdpClient _udpclient;
        private readonly IPEndPoint _remoteEndPoint;

        private IPEndPoint _emptySender = new(0, 0);
        private AsyncCallback? _callBack;

        private long _lastRecivedPacketNumber;
        private long _lastSendedPacketNumber;

        public UdpClientWrapper(int port, string multicastIPAddress, string? localIPAddress = null)
        {
            IPAddress multicastIp = IPAddress.Parse(multicastIPAddress);
            IPAddress localIp = string.IsNullOrEmpty(localIPAddress) ? IPAddress.Any : IPAddress.Parse(localIPAddress);

            _remoteEndPoint = new IPEndPoint(multicastIp, port);
            var localEndPoint = new IPEndPoint(localIp, port);

            _udpclient = CreateAndConfigureUdpClient();

            _udpclient.Client.Bind(localEndPoint);
            _udpclient.JoinMulticastGroup(multicastIp, localIp);
        }

        public event EventHandler? DataReceived;

        //Emulate packet delay
        public bool RecivePause { get; set; }

        public long PacketLoss { get; private set; }

        public void SendData(double data)
        {
            byte[] dgram = new byte[16];

            _lastSendedPacketNumber++;

            byte[] packetNumber = BitConverter.GetBytes(_lastSendedPacketNumber);
            byte[] dataBytes = BitConverter.GetBytes(data);

            Buffer.BlockCopy(packetNumber, 0, dgram, 0, packetNumber.Length);
            Buffer.BlockCopy(dataBytes, 0, dgram, 8, dataBytes.Length);

            _udpclient.Send(dgram, dgram.Length, _remoteEndPoint);
        }

        public void StartListeningIncomingData()
        {
            _callBack = new AsyncCallback(ReceivedCallback);
            _udpclient.BeginReceive(_callBack, null);
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

        private void ReceivedCallback(IAsyncResult asyncResult)
        {
            //Emulate packet delay
            if (RecivePause)
            {
                _udpclient.BeginReceive(_callBack, null);

                return;
            }

            byte[] receivedBytes = _udpclient.EndReceive(asyncResult, ref _emptySender!);

            long currentPacketNumber = BitConverter.ToInt64(receivedBytes.Take(8).ToArray(), 0);

            PacketLoss += currentPacketNumber - (_lastRecivedPacketNumber + 1);

            _lastRecivedPacketNumber = currentPacketNumber;

            byte[] data = receivedBytes.TakeLast(8).ToArray();

            DataReceived?.Invoke(this, new ReceivedEventArgs() { Data = data });

            _udpclient.BeginReceive(_callBack, null);
        }
    }

    public class ReceivedEventArgs : EventArgs
    {
        public byte[]? Data { get; set; }
    }
}