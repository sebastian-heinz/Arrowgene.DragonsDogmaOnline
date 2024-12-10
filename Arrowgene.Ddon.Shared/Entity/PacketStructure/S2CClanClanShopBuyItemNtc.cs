using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanShopBuyItemNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CLAN_CLAN_SHOP_BUY_ITEM_NTC;

        public List<CDataClanShopFunctionItem> FunctionList { get; set; }
        public List<CDataClanShopBuffItem> BuffList { get; set; }
        public List<CDataClanShopConciergeItem> ConciergeList { get; set; }
        public CDataCharacterName BuyerName { get; set; }
        public uint ClanPoint { get; set; }

        public S2CClanClanShopBuyItemNtc()
        {
            FunctionList = new();
            BuffList = new();
            ConciergeList = new();
            BuyerName = new();
        }

        public class Serializer : PacketEntitySerializer<S2CClanClanShopBuyItemNtc>
        {
            public override void Write(IBuffer buffer, S2CClanClanShopBuyItemNtc obj)
            {
                WriteEntityList<CDataClanShopFunctionItem>(buffer, obj.FunctionList);
                WriteEntityList<CDataClanShopBuffItem>(buffer, obj.BuffList);
                WriteEntityList<CDataClanShopConciergeItem>(buffer, obj.ConciergeList);
                WriteEntity<CDataCharacterName>(buffer, obj.BuyerName);
                WriteUInt32(buffer, obj.ClanPoint);
            }

            public override S2CClanClanShopBuyItemNtc Read(IBuffer buffer)
            {
                S2CClanClanShopBuyItemNtc obj = new S2CClanClanShopBuyItemNtc();

                obj.FunctionList = ReadEntityList<CDataClanShopFunctionItem>(buffer);
                obj.BuffList = ReadEntityList<CDataClanShopBuffItem>(buffer);
                obj.ConciergeList = ReadEntityList<CDataClanShopConciergeItem>(buffer);
                obj.BuyerName = ReadEntity<CDataCharacterName>(buffer);
                obj.ClanPoint = ReadUInt32(buffer);

                return obj;
            }
        }
    }
}
