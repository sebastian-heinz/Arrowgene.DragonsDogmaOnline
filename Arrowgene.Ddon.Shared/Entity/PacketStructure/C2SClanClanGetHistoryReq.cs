using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SClanClanGetHistoryReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CLAN_CLAN_GET_HISTORY_REQ;

        public C2SClanClanGetHistoryReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SClanClanGetHistoryReq>
        {
            public override void Write(IBuffer buffer, C2SClanClanGetHistoryReq obj)
            {
            }

            public override C2SClanClanGetHistoryReq Read(IBuffer buffer)
            {
                C2SClanClanGetHistoryReq obj = new C2SClanClanGetHistoryReq();
                return obj;
            }
        }
    }
}
