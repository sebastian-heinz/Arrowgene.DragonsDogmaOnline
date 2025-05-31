using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanCancelJoinRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CLAN_CLAN_CANCEL_JOIN_RES;

        public class Serializer : PacketEntitySerializer<S2CClanClanCancelJoinRes>
        {
            public override void Write(IBuffer buffer, S2CClanClanCancelJoinRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CClanClanCancelJoinRes Read(IBuffer buffer)
            {
                S2CClanClanCancelJoinRes obj = new S2CClanClanCancelJoinRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
