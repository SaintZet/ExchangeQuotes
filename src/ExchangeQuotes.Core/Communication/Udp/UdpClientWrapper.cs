using ExchangeQuotes.Core.Abstractions;
using System.Net;
using System.Net.Sockets;

namespace ExchangeQuotes.Core.Communication.Udp;

public sealed class UdpClientWrapper : IDisposable, IExchangeQuotesReceiver, IExchangeQuotesSender
{
    private readonly UdpClient _udpclient;
    private readonly IPEndPoint _remoteEndPoint;

    private readonly ReceivedEventArgs _receivedEventArgs = new();
    private readonly PacketCountControl _packetCountChecker = new();

    private IPEndPoint _emptySender = new(0, 0);
    private AsyncCallback? _callBack;

    public UdpClientWrapper(int port, string multicastIPAddress, string? localIPAddress = null)
    {
        IPAddress multicastIp = IPAddress.Parse(multicastIPAddress);
        IPAddress localIp = string.IsNullOrEmpty(localIPAddress) ? IPAddress.Any : IPAddress.Parse(localIPAddress);

        _remoteEndPoint = new IPEndPoint(multicastIp, port);

        _udpclient = new UdpClient
        {
            ExclusiveAddressUse = false
        };

        _udpclient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

        var localEndPoint = new IPEndPoint(localIp, port);
        _udpclient.Client.Bind(localEndPoint);

        _udpclient.JoinMulticastGroup(multicastIp, localIp);
    }

    public event EventHandler? DataReceived;

    //Emulate packet delay
    public bool RecivePause { get; set; }

    public long PacketLoss => _packetCountChecker.PacketLoss;

    public void SendData(int data)
    {
        byte[] dgram = new byte[12];

        byte[] packetNumber = BitConverter.GetBytes(_packetCountChecker.LastSendedPacketNumber);
        byte[] dataBytes = BitConverter.GetBytes(data);

        Buffer.BlockCopy(packetNumber, 0, dgram, 0, packetNumber.Length);
        Buffer.BlockCopy(dataBytes, 0, dgram, 8, dataBytes.Length);

        _udpclient.Send(dgram, dgram.Length, _remoteEndPoint);

        _packetCountChecker.LastSendedPacketNumber++;
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

    private void ReceivedCallback(IAsyncResult asyncResult)
    {
        byte[] receivedBytes = _udpclient.EndReceive(asyncResult, ref _emptySender!);

        InvokeHandlerAsync(receivedBytes);

        _udpclient.BeginReceive(_callBack, null);
    }

    /// <summary>
    /// Method take received data, parse and send to handler. Method don't need return some value.
    /// </summary>
    /// <param name="receivedBytes">Recived bytes.</param>
    private async void InvokeHandlerAsync(byte[] receivedBytes)
    {
        //Emulate packet delay
        if (RecivePause)
        {
            return;
        }

        byte[] packetNumber = receivedBytes.Take(8).ToArray();
        byte[] data = receivedBytes.TakeLast(4).ToArray();

        _packetCountChecker.ReceivedPacketNumber(packetNumber);
        _receivedEventArgs.Data = data;

        DataReceived?.Invoke(this, _receivedEventArgs);
    }
}

public class ReceivedEventArgs : EventArgs
{
    public byte[]? Data { get; set; }
}