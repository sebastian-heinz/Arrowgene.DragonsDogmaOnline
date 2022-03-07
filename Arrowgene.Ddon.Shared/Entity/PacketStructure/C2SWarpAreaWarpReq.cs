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

        public uint CurrentAreaID { get; set; }
        public uint WarpPointID { get; set; }
        public uint Price { get; set; }

        public class Serializer : PacketEntitySerializer<C2SWarpAreaWarpReq>
        {
            public override void Write(IBuffer buffer, C2SWarpAreaWarpReq obj)
            {
                WriteUInt32(buffer, obj.CurrentAreaID);
                WriteUInt32(buffer, obj.WarpPointID);
                WriteUInt32(buffer, obj.Price);
            }

            public override C2SWarpAreaWarpReq Read(IBuffer buffer)
            {
                C2SWarpAreaWarpReq obj = new C2SWarpAreaWarpReq();
                obj.CurrentAreaID = ReadUInt32(buffer);
                obj.WarpPointID = ReadUInt32(buffer);
                obj.Price = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}