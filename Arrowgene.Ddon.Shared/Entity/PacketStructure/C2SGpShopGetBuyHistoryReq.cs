using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SGpShopGetBuyHistoryReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_GP_GP_SHOP_GET_BUY_HISTORY_REQ;

        public C2SGpShopGetBuyHistoryReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SGpShopGetBuyHistoryReq>
        {
            public override void Write(IBuffer buffer, C2SGpShopGetBuyHistoryReq obj)
            {
            }

            public override C2SGpShopGetBuyHistoryReq Read(IBuffer buffer)
            {
                C2SGpShopGetBuyHistoryReq obj = new C2SGpShopGetBuyHistoryReq();

                return obj;
            }
        }
    }
}
