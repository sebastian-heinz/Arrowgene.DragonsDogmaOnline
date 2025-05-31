using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnGetFavoritePawnListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_GET_FAVORITE_PAWN_LIST_RES;

        public List<CDataRegisterdPawnList> FavoritePawnList { get; set; } = new();
        public List<CDataRentedPawnList> FavoriteRentedPawnList { get; set; } = new();

        public class Serializer : PacketEntitySerializer<S2CPawnGetFavoritePawnListRes>
        {
            public override void Write(IBuffer buffer, S2CPawnGetFavoritePawnListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.FavoritePawnList);
                WriteEntityList(buffer, obj.FavoriteRentedPawnList);
            }

            public override S2CPawnGetFavoritePawnListRes Read(IBuffer buffer)
            {
                S2CPawnGetFavoritePawnListRes obj = new S2CPawnGetFavoritePawnListRes();
                ReadServerResponse(buffer, obj);
                obj.FavoritePawnList = ReadEntityList<CDataRegisterdPawnList>(buffer);
                obj.FavoriteRentedPawnList = ReadEntityList<CDataRentedPawnList>(buffer);
                return obj;
            }
        }
    }
}
