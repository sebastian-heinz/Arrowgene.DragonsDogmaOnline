using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SClanClanShopGetBuffItemListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CLAN_CLAN_SHOP_GET_BUFF_ITEM_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SClanClanShopGetBuffItemListReq>
        {
            public override void Write(IBuffer buffer, C2SClanClanShopGetBuffItemListReq obj)
            {
            }

            public override C2SClanClanShopGetBuffItemListReq Read(IBuffer buffer)
            {
                C2SClanClanShopGetBuffItemListReq obj = new C2SClanClanShopGetBuffItemListReq();
                return obj;
            }
        }
    }
}
