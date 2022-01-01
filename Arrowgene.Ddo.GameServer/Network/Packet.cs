using Arrowgene.Buffers;

namespace Arrowgene.Ddo.GameServer.Network
{
    public class Packet
    {
        public Packet(PacketId id, byte[] data)
        {
            Id = id;
            Data = data;
        }

        public Packet(PacketId id)
        {
            Id = id;
            Data = new byte[0];
        }

        public Packet(byte[] data)
        {
            Id = PacketId.Unknown;
            Data = data;
        }

        public PacketId Id { get; }
        public byte[] Data { get; }

        public IBuffer AsBuffer()
        {
            return new StreamBuffer(Data);
        }
    }
}
