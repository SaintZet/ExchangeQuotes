namespace ExchangeQuotes.Core.Communication.Udp;

internal class PacketCountControl
{
    private bool _firstCall = true;

    public long LastRecivedPacketNumber { get; set; }
    public long LastSendedPacketNumber { get; set; } = 1;
    public long PacketLoss { get; set; }
    private List<long> LostNumbers { get; set; } = new();

    internal void ReceivedPacketNumber(byte[] packetNumber)
    {
        long number = BitConverter.ToInt64(packetNumber, 0);

        if (_firstCall)
        {
            LastRecivedPacketNumber = number;

            _firstCall = false;

            return;
        }

        var countLostPacket = number - (LastRecivedPacketNumber + 1);

        if (countLostPacket is not 0)
        {
            for (int i = 1; i == countLostPacket; i++)
            {
                LostNumbers.Add(LastRecivedPacketNumber + i);
            }

            PacketLoss += countLostPacket;
        }

        LastRecivedPacketNumber = number;
    }
}