using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SWarpGetReturnLocationReq
    {
        public C2SWarpGetReturnLocationReq()
        {
        }

        // Doesn't have any fields
    }

    public class C2SWarpGetReturnLocationReqSerializer : EntitySerializer<C2SWarpGetReturnLocationReq>
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