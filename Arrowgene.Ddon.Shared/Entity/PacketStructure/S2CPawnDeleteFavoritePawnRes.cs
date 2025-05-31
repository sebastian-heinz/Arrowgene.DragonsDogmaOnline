using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnDeleteFavoritePawnRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_DELETE_FAVORITE_PAWN_RES;

        public class Serializer : PacketEntitySerializer<S2CPawnDeleteFavoritePawnRes>
        {
            public override void Write(IBuffer buffer, S2CPawnDeleteFavoritePawnRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CPawnDeleteFavoritePawnRes Read(IBuffer buffer)
            {
                S2CPawnDeleteFavoritePawnRes obj = new S2CPawnDeleteFavoritePawnRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
