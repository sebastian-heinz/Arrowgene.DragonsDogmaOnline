using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanConciergeGetListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CLAN_CLAN_CONCIERGE_GET_LIST_RES;

        public S2CClanClanConciergeGetListRes() 
        {
            ConciergeItemList = new();
        }

        public List<CDataClanShopConciergeItem> ConciergeItemList { get; set; }
        public uint ClanPoint {  get; set; }
        
        public class Serializer : PacketEntitySerializer<S2CClanClanConciergeGetListRes>
        {
            public override void Write(IBuffer buffer, S2CClanClanConciergeGetListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataClanShopConciergeItem>(buffer, obj.ConciergeItemList);
                WriteUInt32(buffer, obj.ClanPoint);
            }

            public override S2CClanClanConciergeGetListRes Read(IBuffer buffer)
            {
                S2CClanClanConciergeGetListRes obj = new S2CClanClanConciergeGetListRes();
                ReadServerResponse(buffer, obj);
                obj.ConciergeItemList = ReadEntityList<CDataClanShopConciergeItem>(buffer);
                obj.ClanPoint = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
