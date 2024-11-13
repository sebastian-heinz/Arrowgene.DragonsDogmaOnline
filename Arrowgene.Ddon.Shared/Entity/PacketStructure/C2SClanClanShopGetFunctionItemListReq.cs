using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SClanClanShopGetFunctionItemListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CLAN_CLAN_SHOP_GET_FUNCTION_ITEM_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SClanClanShopGetFunctionItemListReq>
        {
            public override void Write(IBuffer buffer, C2SClanClanShopGetFunctionItemListReq obj)
            {
            }

            public override C2SClanClanShopGetFunctionItemListReq Read(IBuffer buffer)
            {
                C2SClanClanShopGetFunctionItemListReq obj = new C2SClanClanShopGetFunctionItemListReq();
                return obj;
            }
        }
    }
}
