using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CWarpGetReleaseWarpPointListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_WARP_GET_RELEASE_WARP_POINT_LIST_RES;

        public S2CWarpGetReleaseWarpPointListRes()
        {
            WarpPointIdList = new List<CDataCommonU32>();
        }

        public List<CDataCommonU32> WarpPointIdList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CWarpGetReleaseWarpPointListRes>
        {
            public override void Write(IBuffer buffer, S2CWarpGetReleaseWarpPointListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataCommonU32>(buffer, obj.WarpPointIdList);
            }

            public override S2CWarpGetReleaseWarpPointListRes Read(IBuffer buffer)
            {
                S2CWarpGetReleaseWarpPointListRes obj = new S2CWarpGetReleaseWarpPointListRes();
                ReadServerResponse(buffer, obj);
                obj.WarpPointIdList = ReadEntityList<CDataCommonU32>(buffer);
                return obj;
            }
        }
    }
}
