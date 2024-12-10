using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SClanClanGetMyMemberListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CLAN_CLAN_GET_MY_MEMBER_LIST_REQ;

        public C2SClanClanGetMyMemberListReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SClanClanGetMyMemberListReq>
        {
            public override void Write(IBuffer buffer, C2SClanClanGetMyMemberListReq obj)
            {
            }

            public override C2SClanClanGetMyMemberListReq Read(IBuffer buffer)
            {
                C2SClanClanGetMyMemberListReq obj = new C2SClanClanGetMyMemberListReq();
                return obj;
            }
        }
    }
}
