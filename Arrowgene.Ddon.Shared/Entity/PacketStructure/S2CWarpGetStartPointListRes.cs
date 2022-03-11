using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CWarpGetStartPointListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_WARP_GET_START_POINT_LIST_RES;

        public S2CWarpGetStartPointListRes()
        {
            WarpPointIdList = new List<CDataCommonU32>();
        }

        public List<CDataCommonU32> WarpPointIdList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CWarpGetStartPointListRes>
        {
            public override void Write(IBuffer buffer, S2CWarpGetStartPointListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataCommonU32>(buffer, obj.WarpPointIdList);
            }

            public override S2CWarpGetStartPointListRes Read(IBuffer buffer)
            {
                S2CWarpGetStartPointListRes obj = new S2CWarpGetStartPointListRes();
                ReadServerResponse(buffer, obj);
                obj.WarpPointIdList = ReadEntityList<CDataCommonU32>(buffer);
                return obj;
            }
        }
    }
}
