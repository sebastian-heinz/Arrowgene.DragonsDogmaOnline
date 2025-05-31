using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnSetFavoritePawnRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_SET_FAVORITE_PAWN_RES;

        public class Serializer : PacketEntitySerializer<S2CPawnSetFavoritePawnRes>
        {
            public override void Write(IBuffer buffer, S2CPawnSetFavoritePawnRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CPawnSetFavoritePawnRes Read(IBuffer buffer)
            {
                S2CPawnSetFavoritePawnRes obj = new S2CPawnSetFavoritePawnRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
