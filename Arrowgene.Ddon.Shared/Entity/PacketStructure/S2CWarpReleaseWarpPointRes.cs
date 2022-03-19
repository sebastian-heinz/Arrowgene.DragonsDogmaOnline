using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CWarpReleaseWarpPointRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_WARP_RELEASE_WARP_POINT_RES;

        public S2CWarpReleaseWarpPointRes()
        {
            WarpPointId = 0;
        }

        public uint WarpPointId { get; set; } // It's called only "ID" in the PS4 build

        public class Serializer : PacketEntitySerializer<S2CWarpReleaseWarpPointRes>
        {
            public override void Write(IBuffer buffer, S2CWarpReleaseWarpPointRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.WarpPointId);
            }

            public override S2CWarpReleaseWarpPointRes Read(IBuffer buffer)
            {
                S2CWarpReleaseWarpPointRes obj = new S2CWarpReleaseWarpPointRes();
                ReadServerResponse(buffer, obj);
                obj.WarpPointId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
