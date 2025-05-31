using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanJoinRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CLAN_CLAN_REGISTER_JOIN_RES;

        public class Serializer : PacketEntitySerializer<S2CClanClanJoinRes>
        {
            public override void Write(IBuffer buffer, S2CClanClanJoinRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CClanClanJoinRes Read(IBuffer buffer)
            {
                S2CClanClanJoinRes obj = new S2CClanClanJoinRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
