using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SWarpGetReleaseWarpPointListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_WARP_GET_RELEASE_WARP_POINT_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SWarpGetReleaseWarpPointListReq>
        {
            public override void Write(IBuffer buffer, C2SWarpGetReleaseWarpPointListReq obj)
            {
            }

            public override C2SWarpGetReleaseWarpPointListReq Read(IBuffer buffer)
            {
                return new C2SWarpGetReleaseWarpPointListReq();
            }
        }
    }
}