using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanShopBuyBuffItemRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CLAN_CLAN_SHOP_BUY_BUFF_ITEM_RES;

        public S2CClanClanShopBuyBuffItemRes()
        {
            LineupBuffList = new();
        }

        public uint ClanPoint { get; set; }
        public List<CDataCommonU32> LineupBuffList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CClanClanShopBuyBuffItemRes>
        {
            public override void Write(IBuffer buffer, S2CClanClanShopBuyBuffItemRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.ClanPoint);
                WriteEntityList(buffer, obj.LineupBuffList);
            }

            public override S2CClanClanShopBuyBuffItemRes Read(IBuffer buffer)
            {
                S2CClanClanShopBuyBuffItemRes obj = new S2CClanClanShopBuyBuffItemRes();
                ReadServerResponse(buffer, obj);
                obj.ClanPoint = ReadUInt32(buffer);
                obj.LineupBuffList = ReadEntityList<CDataCommonU32>(buffer);
                return obj;
            }
        }
    }
}
