using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SWarpReleaseWarpPointReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_WARP_RELEASE_WARP_POINT_REQ;

        public C2SWarpReleaseWarpPointReq()
        {
            WarpPointId=0;
        }

        public uint WarpPointId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SWarpReleaseWarpPointReq>
        {
            public override void Write(IBuffer buffer, C2SWarpReleaseWarpPointReq obj)
            {
                WriteUInt32(buffer, obj.WarpPointId);
            }

            public override C2SWarpReleaseWarpPointReq Read(IBuffer buffer)
            {
                C2SWarpReleaseWarpPointReq obj = new C2SWarpReleaseWarpPointReq();
                obj.WarpPointId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
