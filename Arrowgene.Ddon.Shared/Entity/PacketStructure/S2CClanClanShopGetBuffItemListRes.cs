using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanShopGetBuffItemListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CLAN_CLAN_SHOP_GET_BUFF_ITEM_LIST_RES;

        public S2CClanClanShopGetBuffItemListRes()
        {
            BuffItemList = new();
            ShopInfo = new();
        }

        public List<CDataClanShopBuffItem> BuffItemList { get; set; }
        public uint ClanPoint { get; set; }
        public CDataClanShopInfo ShopInfo { get; set; }

        public class Serializer : PacketEntitySerializer<S2CClanClanShopGetBuffItemListRes>
        {
            public override void Write(IBuffer buffer, S2CClanClanShopGetBuffItemListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataClanShopBuffItem>(buffer, obj.BuffItemList);
                WriteUInt32(buffer, obj.ClanPoint);
                WriteEntity<CDataClanShopInfo>(buffer, obj.ShopInfo);
            }

            public override S2CClanClanShopGetBuffItemListRes Read(IBuffer buffer)
            {
                S2CClanClanShopGetBuffItemListRes obj = new S2CClanClanShopGetBuffItemListRes();
                ReadServerResponse(buffer, obj);
                obj.BuffItemList = ReadEntityList<CDataClanShopBuffItem>(buffer);
                obj.ClanPoint = ReadUInt32(buffer);
                obj.ShopInfo = ReadEntity<CDataClanShopInfo>(buffer);
                return obj;
            }
        }
    }
}
