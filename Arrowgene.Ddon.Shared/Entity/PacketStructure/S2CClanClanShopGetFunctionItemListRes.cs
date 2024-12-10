using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanShopGetFunctionItemListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CLAN_CLAN_SHOP_GET_FUNCTION_ITEM_LIST_RES;

        public S2CClanClanShopGetFunctionItemListRes()
        {
            FunctionItemList = new();
            ShopInfo = new();
        }

        public List<CDataClanShopFunctionItem> FunctionItemList { get; set; }
        public uint ClanPoint { get; set; }
        public CDataClanShopInfo ShopInfo { get; set; }

        public class Serializer : PacketEntitySerializer<S2CClanClanShopGetFunctionItemListRes>
        {
            public override void Write(IBuffer buffer, S2CClanClanShopGetFunctionItemListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataClanShopFunctionItem>(buffer, obj.FunctionItemList);
                WriteUInt32(buffer, obj.ClanPoint);
                WriteEntity<CDataClanShopInfo>(buffer, obj.ShopInfo);
            }

            public override S2CClanClanShopGetFunctionItemListRes Read(IBuffer buffer)
            {
                S2CClanClanShopGetFunctionItemListRes obj = new S2CClanClanShopGetFunctionItemListRes();
                ReadServerResponse(buffer, obj);
                obj.FunctionItemList = ReadEntityList<CDataClanShopFunctionItem>(buffer);
                obj.ClanPoint = ReadUInt32(buffer);
                obj.ShopInfo = ReadEntity<CDataClanShopInfo>(buffer);
                return obj;
            }
        }
    }
}
