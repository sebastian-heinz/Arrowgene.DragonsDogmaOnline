using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnLostPawnGoldenReviveReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_LOST_PAWN_GOLDEN_REVIVE_REQ;

        public uint PawnId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SPawnLostPawnGoldenReviveReq>
        {
            public override void Write(IBuffer buffer, C2SPawnLostPawnGoldenReviveReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);
            }

            public override C2SPawnLostPawnGoldenReviveReq Read(IBuffer buffer)
            {
                C2SPawnLostPawnGoldenReviveReq obj = new C2SPawnLostPawnGoldenReviveReq();
                obj.PawnId = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}