using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnDeleteFavoritePawnReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_DELETE_FAVORITE_PAWN_REQ;

        public uint PawnId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SPawnDeleteFavoritePawnReq>
        {
            public override void Write(IBuffer buffer, C2SPawnDeleteFavoritePawnReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);
            }

            public override C2SPawnDeleteFavoritePawnReq Read(IBuffer buffer)
            {
                C2SPawnDeleteFavoritePawnReq obj = new C2SPawnDeleteFavoritePawnReq();
                obj.PawnId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

