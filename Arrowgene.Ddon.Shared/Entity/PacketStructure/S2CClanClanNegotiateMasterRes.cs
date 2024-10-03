using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanNegotiateMasterRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CLAN_CLAN_NEGOTIATE_MASTER_RES;

        public uint MemberId { get; set; }

        public class Serializer : PacketEntitySerializer<S2CClanClanNegotiateMasterRes>
        {
            public override void Write(IBuffer buffer, S2CClanClanNegotiateMasterRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.MemberId);
            }

            public override S2CClanClanNegotiateMasterRes Read(IBuffer buffer)
            {
                S2CClanClanNegotiateMasterRes obj = new S2CClanClanNegotiateMasterRes();
                ReadServerResponse(buffer, obj);
                obj.MemberId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
