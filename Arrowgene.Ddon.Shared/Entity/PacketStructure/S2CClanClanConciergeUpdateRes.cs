using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanConciergeUpdateRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CLAN_CLAN_CONCIERGE_UPDATE_RES;

        public S2CClanClanConciergeUpdateRes() { }

        public uint NpcId { get; set; }
        public uint ClanPoint { get; set; }

        public class Serializer : PacketEntitySerializer<S2CClanClanConciergeUpdateRes>
        {
            public override void Write(IBuffer buffer, S2CClanClanConciergeUpdateRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.NpcId);
                WriteUInt32(buffer, obj.ClanPoint);
            }

            public override S2CClanClanConciergeUpdateRes Read(IBuffer buffer)
            {
                S2CClanClanConciergeUpdateRes obj = new S2CClanClanConciergeUpdateRes();
                ReadServerResponse(buffer, obj);
                obj.NpcId = ReadUInt32(buffer);
                obj.ClanPoint = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}
