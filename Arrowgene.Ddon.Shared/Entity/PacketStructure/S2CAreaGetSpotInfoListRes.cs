using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CAreaGetSpotInfoListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_AREA_GET_SPOT_INFO_LIST_RES;

        public S2CAreaGetSpotInfoListRes()
        {
            SpotInfoList = new();
            Unk0 = new();
            Unk1 = new();
            Unk2 = new();
        }

        public List<CDataSpotInfo> SpotInfoList { get; set; }
        public List<CDataAreaRankUnk0> Unk0 { get; set; }
        public List<CDataAreaRankSeason3> Unk1 { get; set; }
        public List<CDataCommonU32> Unk2 { get; set; }

        public class Serializer : PacketEntitySerializer<S2CAreaGetSpotInfoListRes>
        {
            public override void Write(IBuffer buffer, S2CAreaGetSpotInfoListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.SpotInfoList);
                WriteEntityList(buffer, obj.Unk0);
                WriteEntityList(buffer, obj.Unk1);
                WriteEntityList(buffer, obj.Unk2);
            }

            public override S2CAreaGetSpotInfoListRes Read(IBuffer buffer)
            {
                S2CAreaGetSpotInfoListRes obj = new S2CAreaGetSpotInfoListRes();
                ReadServerResponse(buffer, obj);
                obj.SpotInfoList = ReadEntityList<CDataSpotInfo>(buffer);
                obj.Unk0 = ReadEntityList<CDataAreaRankUnk0>(buffer);
                obj.Unk1 = ReadEntityList<CDataAreaRankSeason3>(buffer);
                obj.Unk2 = ReadEntityList<CDataCommonU32>(buffer);
                return obj;
            }
        }
    }
}
