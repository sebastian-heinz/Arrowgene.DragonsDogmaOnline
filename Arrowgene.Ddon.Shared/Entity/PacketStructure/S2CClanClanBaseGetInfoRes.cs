using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanBaseGetInfoRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CLAN_CLAN_BASE_GET_INFO_RES;

        public S2CClanClanBaseGetInfoRes()
        {
            FunctionReleaseIds = new();
            DungeonReleaseIds = new();
            PawnExpeditionInfo = new();
            PartnerPawnInfo = new();
            ConciergeInfo = new();
            ShopLineupNameList = new();
            ClanValueInfoList = new();
            ClanFurnitureInfo = new();
        }

        public List<CDataCommonU32> FunctionReleaseIds { get; set; }
        public List<CDataCommonU32> DungeonReleaseIds { get; set; }
        public CDataPawnExpeditionInfo PawnExpeditionInfo { get; set; }
        public CDataClanPartnerPawnInfo PartnerPawnInfo { get; set; }
        public CDataClanConciergeInfo ConciergeInfo { get; set; }
        public List<CDataClanShopLineupName> ShopLineupNameList {  get; set; }
        public List<CDataClanValueInfo> ClanValueInfoList { get; set; }
        public List<CDataFurnitureLayout> ClanFurnitureInfo { get; set; }
        
        public class Serializer : PacketEntitySerializer<S2CClanClanBaseGetInfoRes>
        {
            public override void Write(IBuffer buffer, S2CClanClanBaseGetInfoRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataCommonU32>(buffer, obj.FunctionReleaseIds);
                WriteEntityList<CDataCommonU32>(buffer, obj.DungeonReleaseIds);
                WriteEntity<CDataPawnExpeditionInfo>(buffer, obj.PawnExpeditionInfo);
                WriteEntity<CDataClanPartnerPawnInfo>(buffer, obj.PartnerPawnInfo);
                WriteEntity<CDataClanConciergeInfo>(buffer, obj.ConciergeInfo);
                WriteEntityList<CDataClanShopLineupName>(buffer, obj.ShopLineupNameList);
                WriteEntityList<CDataClanValueInfo>(buffer, obj.ClanValueInfoList);
                WriteEntityList<CDataFurnitureLayout>(buffer, obj.ClanFurnitureInfo);
            }

            public override S2CClanClanBaseGetInfoRes Read(IBuffer buffer)
            {
                S2CClanClanBaseGetInfoRes obj = new S2CClanClanBaseGetInfoRes();
                ReadServerResponse(buffer, obj);

                obj.FunctionReleaseIds = ReadEntityList<CDataCommonU32>(buffer);
                obj.DungeonReleaseIds = ReadEntityList<CDataCommonU32>(buffer);
                obj.PawnExpeditionInfo = ReadEntity<CDataPawnExpeditionInfo>(buffer);
                obj.PartnerPawnInfo = ReadEntity<CDataClanPartnerPawnInfo>(buffer);
                obj.ConciergeInfo = ReadEntity<CDataClanConciergeInfo>(buffer);
                obj.ShopLineupNameList = ReadEntityList<CDataClanShopLineupName>(buffer);
                obj.ClanValueInfoList = ReadEntityList<CDataClanValueInfo>(buffer);
                obj.ClanFurnitureInfo = ReadEntityList<CDataFurnitureLayout>(buffer);
                return obj;
            }
        }
    }
}
