using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CAreaGetAreaReleaseListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_AREA_GET_AREA_RELEASE_LIST_RES;

        public S2CAreaGetAreaReleaseListRes()
        {
            ReleaseAreaInfoSetList = new();
        }

        public List<CDataReleaseAreaInfoSet> ReleaseAreaInfoSetList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CAreaGetAreaReleaseListRes>
        {
            public override void Write(IBuffer buffer, S2CAreaGetAreaReleaseListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.ReleaseAreaInfoSetList);
            }

            public override S2CAreaGetAreaReleaseListRes Read(IBuffer buffer)
            {
                S2CAreaGetAreaReleaseListRes obj = new S2CAreaGetAreaReleaseListRes();
                ReadServerResponse(buffer, obj);
                obj.ReleaseAreaInfoSetList = ReadEntityList<CDataReleaseAreaInfoSet>(buffer);
                return obj;
            }
        }
    }
}
