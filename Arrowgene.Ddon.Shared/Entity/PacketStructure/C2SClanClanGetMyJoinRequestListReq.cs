using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SClanClanGetMyJoinRequestListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CLAN_CLAN_GET_MY_JOIN_REQUEST_LIST_REQ;

        public C2SClanClanGetMyJoinRequestListReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SClanClanGetMyJoinRequestListReq>
        {
            public override void Write(IBuffer buffer, C2SClanClanGetMyJoinRequestListReq obj)
            {
            }

            public override C2SClanClanGetMyJoinRequestListReq Read(IBuffer buffer)
            {
                C2SClanClanGetMyJoinRequestListReq obj = new C2SClanClanGetMyJoinRequestListReq();
                return obj;
            }
        }
    }
}
