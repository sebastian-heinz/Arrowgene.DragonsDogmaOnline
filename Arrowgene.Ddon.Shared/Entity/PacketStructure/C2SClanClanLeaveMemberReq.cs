using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SClanClanLeaveMemberReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CLAN_CLAN_LEAVE_MEMBER_REQ;

        public C2SClanClanLeaveMemberReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SClanClanLeaveMemberReq>
        {
            public override void Write(IBuffer buffer, C2SClanClanLeaveMemberReq obj)
            {
            }

            public override C2SClanClanLeaveMemberReq Read(IBuffer buffer)
            {
                C2SClanClanLeaveMemberReq obj = new C2SClanClanLeaveMemberReq();
                return obj;
            }
        }
    }
}
