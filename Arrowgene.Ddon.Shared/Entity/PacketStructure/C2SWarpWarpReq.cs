using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SWarpWarpReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_WARP_WARP_REQ;
        
        public uint CurrentPointID { get; set; }
        public uint DestPointID { get; set; }
        public uint Price { get; set; }

        public C2SWarpWarpReq() {
            CurrentPointID = 0;
            DestPointID = 0;
            Price = 0;
        }

        public class Serializer : EntitySerializer<C2SWarpWarpReq> {
            
            static Serializer()
            {
                Id = PacketId.C2S_WARP_WARP_REQ;
            }
            
            public override void Write(IBuffer buffer, C2SWarpWarpReq obj)
            {
                WriteUInt32(buffer, obj.CurrentPointID);
                WriteUInt32(buffer, obj.DestPointID);
                WriteUInt32(buffer, obj.Price);
            }

            public override C2SWarpWarpReq Read(IBuffer buffer)
            {
                C2SWarpWarpReq obj = new C2SWarpWarpReq();
                obj.CurrentPointID = ReadUInt32(buffer);
                obj.DestPointID = ReadUInt32(buffer);
                obj.Price = ReadUInt32(buffer);
                return obj;
            }
        }

    }

}
