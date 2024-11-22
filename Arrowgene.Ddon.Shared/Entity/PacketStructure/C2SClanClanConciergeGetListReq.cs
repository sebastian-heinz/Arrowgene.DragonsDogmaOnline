using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SClanClanConciergeGetListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CLAN_CLAN_CONCIERGE_GET_LIST_REQ;

        public C2SClanClanConciergeGetListReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SClanClanConciergeGetListReq>
        {
            public override void Write(IBuffer buffer, C2SClanClanConciergeGetListReq obj)
            {
            }

            public override C2SClanClanConciergeGetListReq Read(IBuffer buffer)
            {
                C2SClanClanConciergeGetListReq obj = new C2SClanClanConciergeGetListReq();
                return obj;
            }
        }
    }
}
