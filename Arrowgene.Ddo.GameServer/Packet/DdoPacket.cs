using Arrowgene.Ddo.GameServer.Network;

namespace Arrowgene.Ddo.GameServer.Paket
{
    public abstract class DdoPacket : Packet
    {
        public static PacketId DdoPacketId;
        
        public DdoPacket(PacketId id, byte[] data) : base(id, data)
        {
        }

        public DdoPacket(PacketId id) : base(id)
        {
        }

        public DdoPacket(byte[] data) : base(data)
        {
        }
    }
}
