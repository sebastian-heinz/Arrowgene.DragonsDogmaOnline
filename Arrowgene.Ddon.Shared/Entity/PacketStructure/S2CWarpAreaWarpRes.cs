using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CWarpAreaWarpRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_WARP_AREA_WARP_RES;

        public S2CWarpAreaWarpRes()
        {
            WarpPointId=0;
            Rim=0;
        }

        public uint WarpPointId { get; set; }
        public uint Rim { get; set; }

        public class Serializer : PacketEntitySerializer<S2CWarpAreaWarpRes>
        {
            public override void Write(IBuffer buffer, S2CWarpAreaWarpRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.WarpPointId);
                WriteUInt32(buffer, obj.Rim);
            }

            public override S2CWarpAreaWarpRes Read(IBuffer buffer)
            {
                S2CWarpAreaWarpRes obj = new S2CWarpAreaWarpRes();
                ReadServerResponse(buffer, obj);
                obj.WarpPointId = ReadUInt32(buffer);
                obj.Rim = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}