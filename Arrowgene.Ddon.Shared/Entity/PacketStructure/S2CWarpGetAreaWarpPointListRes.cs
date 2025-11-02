using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CWarpGetAreaWarpPointListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_WARP_GET_AREA_WARP_POINT_LIST_RES;

        public List<CDataAreaWarpPoint> AreaWarpPointList { get; set; } = [];

        public class Serializer : PacketEntitySerializer<S2CWarpGetAreaWarpPointListRes>
        {
            public override void Write(IBuffer buffer, S2CWarpGetAreaWarpPointListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.AreaWarpPointList);
            }

            public override S2CWarpGetAreaWarpPointListRes Read(IBuffer buffer)
            {
                S2CWarpGetAreaWarpPointListRes obj = new S2CWarpGetAreaWarpPointListRes();
                ReadServerResponse(buffer, obj);
                obj.AreaWarpPointList = ReadEntityList<CDataAreaWarpPoint>(buffer);
                return obj;
            }
        }
    }
}
