using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SWarpWarpReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_WARP_WARP_REQ;
        
        public uint CurrentPointId { get; set; }
        public uint DestPointId { get; set; }
        public uint Price { get; set; }

        public C2SWarpWarpReq() {
            CurrentPointId = 0;
            DestPointId = 0;
            Price = 0;
        }

        public class Serializer : PacketEntitySerializer<C2SWarpWarpReq> {
            
            public override void Write(IBuffer buffer, C2SWarpWarpReq obj)
            {
                WriteUInt32(buffer, obj.CurrentPointId);
                WriteUInt32(buffer, obj.DestPointId);
                WriteUInt32(buffer, obj.Price);
            }

            public override C2SWarpWarpReq Read(IBuffer buffer)
            {
                C2SWarpWarpReq obj = new C2SWarpWarpReq();
                obj.CurrentPointId = ReadUInt32(buffer);
                obj.DestPointId = ReadUInt32(buffer);
                obj.Price = ReadUInt32(buffer);
                return obj;
            }
        }

    }

}
