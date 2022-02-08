using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure {
    public class S2CWarpWarpRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_WARP_WARP_RES;

        public uint WarpPointID { get; set; }
        public uint Rim { get; set; }
        public S2CWarpWarpRes() {
            WarpPointID = 0;
            Rim = 0;
        }

        public class Serializer : EntitySerializer<S2CWarpWarpRes> {
            public override void Write(IBuffer buffer, S2CWarpWarpRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.WarpPointID);
                WriteUInt32(buffer, obj.Rim);
            }

            public override S2CWarpWarpRes Read(IBuffer buffer)
            {
                S2CWarpWarpRes obj = new S2CWarpWarpRes();
                ReadServerResponse(buffer, obj);
                obj.WarpPointID = ReadUInt32(buffer);
                obj.Rim = ReadUInt32(buffer);
                return obj;
            }
        }

    }

}