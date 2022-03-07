using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SWarpGetWarpPointListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_WARP_GET_WARP_POINT_LIST_REQ;

        public C2SWarpGetWarpPointListReq()
        {
            CurrentPointId=0;
        }

        public uint CurrentPointId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SWarpGetWarpPointListReq>
        {
            public override void Write(IBuffer buffer, C2SWarpGetWarpPointListReq obj)
            {
                WriteUInt32(buffer, obj.CurrentPointId);
            }

            public override C2SWarpGetWarpPointListReq Read(IBuffer buffer)
            {
                C2SWarpGetWarpPointListReq obj = new C2SWarpGetWarpPointListReq();
                obj.CurrentPointId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}