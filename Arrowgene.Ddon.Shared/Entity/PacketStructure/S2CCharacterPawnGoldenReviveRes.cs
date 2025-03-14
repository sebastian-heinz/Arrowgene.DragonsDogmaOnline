using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCharacterPawnGoldenReviveRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CHARACTER_PAWN_GOLDEN_REVIVE_RES;

        public uint GoldenGemstonePoint { get; set; }
        public uint PawnId { get; set; }

        public S2CCharacterPawnGoldenReviveRes()
        {
        }

        public S2CCharacterPawnGoldenReviveRes(C2SCharacterPawnGoldenReviveReq req)
        {
            PawnId = req.PawnId;
        }

        public class Serializer : PacketEntitySerializer<S2CCharacterPawnGoldenReviveRes>
        {
            public override void Write(IBuffer buffer, S2CCharacterPawnGoldenReviveRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.PawnId);
                WriteUInt32(buffer, obj.GoldenGemstonePoint);
            }

            public override S2CCharacterPawnGoldenReviveRes Read(IBuffer buffer)
            {
                S2CCharacterPawnGoldenReviveRes obj = new S2CCharacterPawnGoldenReviveRes();
                ReadServerResponse(buffer, obj);
                obj.PawnId = ReadUInt32(buffer);
                obj.GoldenGemstonePoint = ReadUInt32(buffer);
                return obj;
            }
        }
    }

}
