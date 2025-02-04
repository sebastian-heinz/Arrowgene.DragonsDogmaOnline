using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SClanClanShopBuyFunctionItemReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CLAN_CLAN_SHOP_BUY_FUNCTION_ITEM_REQ;

        public uint LineupId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SClanClanShopBuyFunctionItemReq>
        {
            public override void Write(IBuffer buffer, C2SClanClanShopBuyFunctionItemReq obj)
            {
                WriteUInt32(buffer, obj.LineupId);
            }

            public override C2SClanClanShopBuyFunctionItemReq Read(IBuffer buffer)
            {
                C2SClanClanShopBuyFunctionItemReq obj = new C2SClanClanShopBuyFunctionItemReq();
                obj.LineupId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
