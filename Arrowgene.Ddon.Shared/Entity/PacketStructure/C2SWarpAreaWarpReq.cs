using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SWarpAreaWarpReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_WARP_AREA_WARP_REQ;

        public C2SWarpAreaWarpReq()
        {
        }

        public uint CurrentAreaId { get; set; }
        public uint WarpPointId { get; set; }
        public uint Price { get; set; }

        public class Serializer : PacketEntitySerializer<C2SWarpAreaWarpReq>
        {
            public override void Write(IBuffer buffer, C2SWarpAreaWarpReq obj)
            {
                WriteUInt32(buffer, obj.CurrentAreaId);
                WriteUInt32(buffer, obj.WarpPointId);
                WriteUInt32(buffer, obj.Price);
            }

            public override C2SWarpAreaWarpReq Read(IBuffer buffer)
            {
                C2SWarpAreaWarpReq obj = new C2SWarpAreaWarpReq();
                obj.CurrentAreaId = ReadUInt32(buffer);
                obj.WarpPointId = ReadUInt32(buffer);
                obj.Price = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
