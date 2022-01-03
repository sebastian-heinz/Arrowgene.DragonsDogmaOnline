using Arrowgene.Buffers;

namespace Arrowgene.Ddo.GameServer.Network
{
    public class Packet
    {
        public Packet(PacketId id, byte[] data, uint count, PacketSource source)
        {
            Id = id;
            Source = source;
            Data = data;
            Count = count;
        }
        
        public Packet(PacketId id, byte[] data)
        {
            Id = id;
            Source = PacketSource.Unknown;
            Data = data;
        }

        public Packet(PacketId id)
        {
            Id = id;
            Source = PacketSource.Unknown;
            Data = new byte[0];
        }

        public Packet(byte[] data)
        {
            Id = PacketId.UNKNOWN;
            Source = PacketSource.Unknown;
            Data = data;
        }
        
        public PacketSource Source { get; }
        public PacketId Id { get; }
        public uint Count { get; }
        public byte[] Data { get; }

        public IBuffer AsBuffer()
        {
            IBuffer buffer = new StreamBuffer(Data);
            buffer.SetPositionStart();
            return buffer;
        }
    }
}
