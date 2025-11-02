using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SWarpGetAreaWarpPointListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_WARP_GET_AREA_WARP_POINT_LIST_REQ;

        public uint CurrentAreaId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SWarpGetAreaWarpPointListReq>
        {
            public override void Write(IBuffer buffer, C2SWarpGetAreaWarpPointListReq obj)
            {
                WriteUInt32(buffer, obj.CurrentAreaId);
            }

            public override C2SWarpGetAreaWarpPointListReq Read(IBuffer buffer)
            {
                C2SWarpGetAreaWarpPointListReq obj = new C2SWarpGetAreaWarpPointListReq();
                obj.CurrentAreaId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
