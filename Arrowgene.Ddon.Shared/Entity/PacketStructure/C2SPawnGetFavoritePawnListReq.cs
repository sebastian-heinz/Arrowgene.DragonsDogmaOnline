using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnGetFavoritePawnListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_GET_FAVORITE_PAWN_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SPawnGetFavoritePawnListReq>
        {
            public override void Write(IBuffer buffer, C2SPawnGetFavoritePawnListReq obj)
            {
            }

            public override C2SPawnGetFavoritePawnListReq Read(IBuffer buffer)
            {
                C2SPawnGetFavoritePawnListReq obj = new C2SPawnGetFavoritePawnListReq();
                return obj;
            }
        }
    }
}
