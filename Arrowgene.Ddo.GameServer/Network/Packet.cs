using Arrowgene.Buffers;

namespace Arrowgene.Ddo.GameServer.Network
{
    public class Packet
    {
        public Packet(in ushort id, IBuffer data)
        {
            Id = id;
            Data = data;
        }
        
        public Packet(IBuffer data)
        {
            Id = 0;
            Data = data;
        }

        public ushort Id { get; }
        public IBuffer Data { get; }
    }
}
