using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnSetFavoritePawnReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_SET_FAVORITE_PAWN_REQ;

        public uint PawnId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SPawnSetFavoritePawnReq>
        {
            public override void Write(IBuffer buffer, C2SPawnSetFavoritePawnReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);
            }

            public override C2SPawnSetFavoritePawnReq Read(IBuffer buffer)
            {
                C2SPawnSetFavoritePawnReq obj = new C2SPawnSetFavoritePawnReq();
                obj.PawnId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
