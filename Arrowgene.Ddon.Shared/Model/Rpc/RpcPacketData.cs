using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Model.Rpc
{
    public class RpcPacketData
    {
        public RpcPacketData() 
        { 
        }

        public byte GroupId { get; set; }
        public ushort HandlerId { get; set; }
        public byte HandlerSubId { get; set; }
        public uint CharacterId { get; set; }
        public uint ClanId { get; set; }
        public byte[] Data { get; set; }

        public Packet ToPacket()
        {
            PacketId id = PacketId.GetGamePacketId(GroupId, HandlerId, HandlerSubId);
            return new Packet(id, Data);
        }
    }
}
