using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure {
    public class S2CWarpRes {
        public S2CWarpRes() {
            warpPointID = 0;
            rim = 0;
        }

        public uint warpPointID;
        public uint rim;
    }

    public class S2CWarpResSerializer : EntitySerializer<S2CWarpRes> {
        public override void Write(IBuffer buffer, S2CWarpRes obj)
        {
            WriteUInt32(buffer, obj.warpPointID);
            WriteUInt32(buffer, obj.rim);
        }

        public override S2CWarpRes Read(IBuffer buffer)
        {
            S2CWarpRes obj = new S2CWarpRes();
            obj.warpPointID = ReadUInt32(buffer);
            obj.rim = ReadUInt32(buffer);
            return obj;
        }
    }
}