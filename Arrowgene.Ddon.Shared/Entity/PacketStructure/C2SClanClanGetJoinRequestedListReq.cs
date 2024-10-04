using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SClanClanGetJoinRequestedListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CLAN_CLAN_GET_JOIN_REQUESTED_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SClanClanGetJoinRequestedListReq>
        {

            public override void Write(IBuffer buffer, C2SClanClanGetJoinRequestedListReq obj)
            {
            }

            public override C2SClanClanGetJoinRequestedListReq Read(IBuffer buffer)
            {
                return new C2SClanClanGetJoinRequestedListReq();
            }
        }
    }
}
