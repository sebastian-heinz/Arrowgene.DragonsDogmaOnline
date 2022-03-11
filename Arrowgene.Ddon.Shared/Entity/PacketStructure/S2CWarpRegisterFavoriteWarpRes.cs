using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CWarpRegisterFavoriteWarpRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_WARP_REGISTER_FAVORITE_WARP_RES;

        public uint SlotNo { get; set; }
        public uint WarpPointId { get; set; }

        public S2CWarpRegisterFavoriteWarpRes()
        {
            SlotNo = 0;
            WarpPointId = 0;
        }

        public class Serializer : PacketEntitySerializer<S2CWarpRegisterFavoriteWarpRes>
        {

            public override void Write(IBuffer buffer, S2CWarpRegisterFavoriteWarpRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.SlotNo);
                WriteUInt32(buffer, obj.WarpPointId);
            }

            public override S2CWarpRegisterFavoriteWarpRes Read(IBuffer buffer)
            {
                S2CWarpRegisterFavoriteWarpRes obj = new S2CWarpRegisterFavoriteWarpRes();
                ReadServerResponse(buffer, obj);
                obj.SlotNo = ReadUInt32(buffer);
                obj.WarpPointId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
