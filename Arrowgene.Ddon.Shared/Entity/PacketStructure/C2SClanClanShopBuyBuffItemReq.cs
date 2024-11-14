using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SClanClanShopBuyBuffItemReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CLAN_CLAN_SHOP_BUY_BUFF_ITEM_REQ;

        public uint LineupId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SClanClanShopBuyBuffItemReq>
        {
            public override void Write(IBuffer buffer, C2SClanClanShopBuyBuffItemReq obj)
            {
                WriteUInt32(buffer, obj.LineupId);
            }

            public override C2SClanClanShopBuyBuffItemReq Read(IBuffer buffer)
            {
                C2SClanClanShopBuyBuffItemReq obj = new C2SClanClanShopBuyBuffItemReq();
                obj.LineupId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
