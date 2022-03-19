using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SWarpGetReturnLocationReq : IPacketStructure
    {
        // Doesn't have any fields
        public PacketId Id => PacketId.C2S_WARP_GET_RETURN_LOCATION_REQ;

        public class Serializer : PacketEntitySerializer<C2SWarpGetReturnLocationReq>
        {

            public override void Write(IBuffer buffer, C2SWarpGetReturnLocationReq obj)
            {
            }

            public override C2SWarpGetReturnLocationReq Read(IBuffer buffer)
            {
                return new C2SWarpGetReturnLocationReq();
            }
        }
    }

}
