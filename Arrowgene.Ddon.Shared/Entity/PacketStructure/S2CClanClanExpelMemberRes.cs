using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanExpelMemberRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CLAN_CLAN_EXPEL_MEMBER_RES;

        public class Serializer : PacketEntitySerializer<S2CClanClanExpelMemberRes>
        {
            public override void Write(IBuffer buffer, S2CClanClanExpelMemberRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CClanClanExpelMemberRes Read(IBuffer buffer)
            {
                S2CClanClanExpelMemberRes obj = new S2CClanClanExpelMemberRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
