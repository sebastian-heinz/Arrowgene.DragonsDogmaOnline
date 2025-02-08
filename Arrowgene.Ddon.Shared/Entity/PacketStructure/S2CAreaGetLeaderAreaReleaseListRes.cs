using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CAreaGetLeaderAreaReleaseListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_AREA_GET_LEADER_AREA_RELEASE_LIST_RES;

        public S2CAreaGetLeaderAreaReleaseListRes()
        {
            ReleaseAreaInfoSetList = new List<CDataReleaseAreaInfoSet>();
            Unk0 = new();
            Unk1 = new();
        }

        public List<CDataReleaseAreaInfoSet> ReleaseAreaInfoSetList { get; set; }
        public List<CDataAreaRankUnk0> Unk0 { get; set; }
        public List<CDataAreaRankSeason3> Unk1 { get; set; }

        public class Serializer : PacketEntitySerializer<S2CAreaGetLeaderAreaReleaseListRes>
        {
            public override void Write(IBuffer buffer, S2CAreaGetLeaderAreaReleaseListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.ReleaseAreaInfoSetList);
                WriteEntityList(buffer, obj.Unk0);
                WriteEntityList(buffer, obj.Unk1);
            }

            public override S2CAreaGetLeaderAreaReleaseListRes Read(IBuffer buffer)
            {
                S2CAreaGetLeaderAreaReleaseListRes obj = new S2CAreaGetLeaderAreaReleaseListRes();
                ReadServerResponse(buffer, obj);
                obj.ReleaseAreaInfoSetList = ReadEntityList<CDataReleaseAreaInfoSet>(buffer);
                obj.Unk0 = ReadEntityList<CDataAreaRankUnk0>(buffer);
                obj.Unk1 = ReadEntityList<CDataAreaRankSeason3>(buffer);
                return obj;
            }
        }
    }
}
