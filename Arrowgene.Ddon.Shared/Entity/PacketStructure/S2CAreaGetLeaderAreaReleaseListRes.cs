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
        }

        public List<CDataReleaseAreaInfoSet> ReleaseAreaInfoSetList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CAreaGetLeaderAreaReleaseListRes>
        {
            public override void Write(IBuffer buffer, S2CAreaGetLeaderAreaReleaseListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataReleaseAreaInfoSet>(buffer, obj.ReleaseAreaInfoSetList);
            }

            public override S2CAreaGetLeaderAreaReleaseListRes Read(IBuffer buffer)
            {
                S2CAreaGetLeaderAreaReleaseListRes obj = new S2CAreaGetLeaderAreaReleaseListRes();
                ReadServerResponse(buffer, obj);
                obj.ReleaseAreaInfoSetList = ReadEntityList<CDataReleaseAreaInfoSet>(buffer);
                return obj;
            }
        }
    }
}
