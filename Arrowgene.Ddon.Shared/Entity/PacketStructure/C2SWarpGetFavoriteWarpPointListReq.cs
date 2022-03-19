using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SWarpGetFavoriteWarpPointListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_WARP_GET_FAVORITE_WARP_POINT_LIST_REQ;

        public C2SWarpGetFavoriteWarpPointListReq()
        {
            AreaId=0;
        }

        public uint AreaId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SWarpGetFavoriteWarpPointListReq> {
            public override void Write(IBuffer buffer, C2SWarpGetFavoriteWarpPointListReq obj)
            {
                WriteUInt32(buffer, obj.AreaId);
            }

            public override C2SWarpGetFavoriteWarpPointListReq Read(IBuffer buffer)
            {
                C2SWarpGetFavoriteWarpPointListReq obj = new C2SWarpGetFavoriteWarpPointListReq();
                obj.AreaId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}