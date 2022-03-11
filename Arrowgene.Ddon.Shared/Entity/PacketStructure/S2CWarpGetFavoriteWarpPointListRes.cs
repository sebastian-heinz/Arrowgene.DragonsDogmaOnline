using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CWarpGetFavoriteWarpPointListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_WARP_GET_FAVORITE_WARP_POINT_LIST_RES;

        public S2CWarpGetFavoriteWarpPointListRes()
        {
            FavoriteWarpPointList = new List<CDataFavoriteWarpPoint>();
            SlotIdMax = 0;
        }

        public List<CDataFavoriteWarpPoint> FavoriteWarpPointList { get; set; }
        public uint SlotIdMax { get; set; }

        public class Serializer : PacketEntitySerializer<S2CWarpGetFavoriteWarpPointListRes>
        {
            public override void Write(IBuffer buffer, S2CWarpGetFavoriteWarpPointListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataFavoriteWarpPoint>(buffer, obj.FavoriteWarpPointList);
                WriteUInt32(buffer, obj.SlotIdMax);
            }

            public override S2CWarpGetFavoriteWarpPointListRes Read(IBuffer buffer)
            {
                S2CWarpGetFavoriteWarpPointListRes obj = new S2CWarpGetFavoriteWarpPointListRes();
                ReadServerResponse(buffer, obj);
                obj.FavoriteWarpPointList = ReadEntityList<CDataFavoriteWarpPoint>(buffer);
                obj.SlotIdMax = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
