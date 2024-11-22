using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanShopBuyFunctionItemRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CLAN_CLAN_SHOP_BUY_FUNCTION_ITEM_RES;

        public S2CClanClanShopBuyFunctionItemRes()
        {
            LineupFunctionList = new();
        }
        
        public uint ClanPoint { get; set; }
        public List<CDataCommonU32> LineupFunctionList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CClanClanShopBuyFunctionItemRes>
        {
            public override void Write(IBuffer buffer, S2CClanClanShopBuyFunctionItemRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.ClanPoint);
                WriteEntityList(buffer, obj.LineupFunctionList);
            }

            public override S2CClanClanShopBuyFunctionItemRes Read(IBuffer buffer)
            {
                S2CClanClanShopBuyFunctionItemRes obj = new S2CClanClanShopBuyFunctionItemRes();
                ReadServerResponse(buffer, obj);
                obj.ClanPoint = ReadUInt32(buffer);
                obj.LineupFunctionList = ReadEntityList<CDataCommonU32>(buffer);
                return obj;
            }
        }
    }
}
