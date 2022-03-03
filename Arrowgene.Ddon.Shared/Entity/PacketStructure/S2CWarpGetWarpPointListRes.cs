using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CWarpGetWarpPointListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_WARP_GET_WARP_POINT_LIST_RES;

        public S2CWarpGetWarpPointListRes()
        {
            WarpPointList = new List<CDataWarpPoint>();
        }

        public List<CDataWarpPoint> WarpPointList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CWarpGetWarpPointListRes>
        {
            public override void Write(IBuffer buffer, S2CWarpGetWarpPointListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataWarpPoint>(buffer, obj.WarpPointList);
            }

            public override S2CWarpGetWarpPointListRes Read(IBuffer buffer)
            {
                S2CWarpGetWarpPointListRes obj = new S2CWarpGetWarpPointListRes();
                ReadServerResponse(buffer, obj);
                obj.WarpPointList = ReadEntityList<CDataWarpPoint>(buffer);
                return obj;
            }
        }
    }
}