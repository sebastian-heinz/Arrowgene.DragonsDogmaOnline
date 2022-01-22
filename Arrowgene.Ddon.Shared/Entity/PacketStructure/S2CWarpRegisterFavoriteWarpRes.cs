using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CWarpRegisterFavoriteWarpRes
    {
        public S2CWarpRegisterFavoriteWarpRes() {
            slotNo = 0;
            warpPointID = 0;
        }

        public uint slotNo;
        public uint warpPointID;
    }

    public class S2CWarpRegisterFavoriteWarpResSerializer : EntitySerializer<S2CWarpRegisterFavoriteWarpRes>
    {
        public override void Write(IBuffer buffer, S2CWarpRegisterFavoriteWarpRes obj)
        {
            WriteUInt32(buffer, obj.slotNo);
            WriteUInt32(buffer, obj.warpPointID);
        }

        public override S2CWarpRegisterFavoriteWarpRes Read(IBuffer buffer)
        {
            S2CWarpRegisterFavoriteWarpRes obj = new S2CWarpRegisterFavoriteWarpRes();
            obj.slotNo = ReadUInt32(buffer);
            obj.warpPointID = ReadUInt32(buffer);
            return obj;
        }
    }
}