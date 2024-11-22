using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnExpeditionGetSallyInfoRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_SALLY_INFO_RES;

        public S2CPawnExpeditionGetSallyInfoRes()
        {
            AreaIdList = new();
            HotSpotInfoList = new();
            ActiveBuffLineupList = new();
            ClanSallySpotInfoList = new();
        }

        public List<CDataCommonU32> AreaIdList { get; set; }
        public List<CDataAreaSpotSet> HotSpotInfoList { get; set; }
        public List<CDataCommonU32> ActiveBuffLineupList { get; set; }
        public List<CDataPawnExpeditionClanSallySpotInfo> ClanSallySpotInfoList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPawnExpeditionGetSallyInfoRes>
        {
            public override void Write(IBuffer buffer, S2CPawnExpeditionGetSallyInfoRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.AreaIdList);
                WriteEntityList(buffer, obj.HotSpotInfoList);
                WriteEntityList(buffer, obj.ActiveBuffLineupList);
                WriteEntityList(buffer, obj.ClanSallySpotInfoList);
            }

            public override S2CPawnExpeditionGetSallyInfoRes Read(IBuffer buffer)
            {
                S2CPawnExpeditionGetSallyInfoRes obj = new S2CPawnExpeditionGetSallyInfoRes();
                ReadServerResponse(buffer, obj);
                obj.AreaIdList = ReadEntityList<CDataCommonU32>(buffer);
                obj.HotSpotInfoList = ReadEntityList<CDataAreaSpotSet>(buffer);
                obj.ActiveBuffLineupList = ReadEntityList<CDataCommonU32>(buffer);
                obj.ClanSallySpotInfoList = ReadEntityList<CDataPawnExpeditionClanSallySpotInfo>(buffer);
                return obj;
            }
        }
    }
}
