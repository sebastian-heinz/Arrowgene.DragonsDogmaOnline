using Arrowgene.Ddon.Shared.Entity;
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

        public PacketId PacketId { get
            {
                return PacketId.GetGamePacketId(GroupId, HandlerId, HandlerSubId);
            } 
        }

        public Packet ToPacket()
        {
            return new Packet(PacketId, Data);
        }

        public static RpcPacketData FromPacket<T>(T packet, uint characterId, uint clanId) where T : class, IPacketStructure, new()
        {
            return new RpcPacketData()
            {
                GroupId = packet.Id.GroupId,
                HandlerId = packet.Id.HandlerId,
                HandlerSubId = packet.Id.HandlerSubId,
                CharacterId = characterId,
                ClanId = clanId,
                Data = EntitySerializer.Get<T>().Write(packet)
            };
        }
    }
}
