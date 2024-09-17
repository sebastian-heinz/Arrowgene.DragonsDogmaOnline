using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanLeaveMemberRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CLAN_CLAN_LEAVE_MEMBER_RES;

        public S2CClanClanLeaveMemberRes()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CClanClanLeaveMemberRes>
        {
            public override void Write(IBuffer buffer, S2CClanClanLeaveMemberRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CClanClanLeaveMemberRes Read(IBuffer buffer)
            {
                S2CClanClanLeaveMemberRes obj = new S2CClanClanLeaveMemberRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
