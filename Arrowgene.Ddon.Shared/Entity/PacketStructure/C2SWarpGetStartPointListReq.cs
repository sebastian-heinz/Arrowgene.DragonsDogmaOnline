using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SWarpGetStartPointListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_WARP_GET_START_POINT_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SWarpGetStartPointListReq>
        {
            public override void Write(IBuffer buffer, C2SWarpGetStartPointListReq obj)
            {
            }

            public override C2SWarpGetStartPointListReq Read(IBuffer buffer)
            {
                return new C2SWarpGetStartPointListReq();
            }
        }
    }
}