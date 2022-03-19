using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SWarpRegisterFavoriteWarpReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_WARP_REGISTER_FAVORITE_WARP_REQ;

        public uint SlotNo { get; set; }
        public uint WarpPointId { get; set; }

        public C2SWarpRegisterFavoriteWarpReq() {
            SlotNo = 0;
            WarpPointId = 0;
        }

        public class Serializer : PacketEntitySerializer<C2SWarpRegisterFavoriteWarpReq>
        {

            public override void Write(IBuffer buffer, C2SWarpRegisterFavoriteWarpReq obj)
            {
                WriteUInt32(buffer, obj.SlotNo);
                WriteUInt32(buffer, obj.WarpPointId);
            }

            public override C2SWarpRegisterFavoriteWarpReq Read(IBuffer buffer)
            {
                C2SWarpRegisterFavoriteWarpReq obj = new C2SWarpRegisterFavoriteWarpReq();
                obj.SlotNo = ReadUInt32(buffer);
                obj.WarpPointId = ReadUInt32(buffer);
                return obj;
            }
        }

    }

}
