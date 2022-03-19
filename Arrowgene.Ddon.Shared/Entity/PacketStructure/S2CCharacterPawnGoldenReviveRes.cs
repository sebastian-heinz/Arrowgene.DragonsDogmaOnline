using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCharacterPawnGoldenReviveRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CHARACTER_PAWN_GOLDEN_REVIVE_RES;

        public uint GoldenGemstonePoint { get; set; }
        public C2SCharacterPawnGoldenReviveReq PawnId { get; set; }

        public S2CCharacterPawnGoldenReviveRes()
        {
            GoldenGemstonePoint = 0;
        }

        public S2CCharacterPawnGoldenReviveRes(C2SCharacterPawnGoldenReviveReq req)
        {
            PawnId = req;
        }

        public class Serializer : PacketEntitySerializer<S2CCharacterPawnGoldenReviveRes>
        {
            public override void Write(IBuffer buffer, S2CCharacterPawnGoldenReviveRes obj)
            {
                C2SCharacterPawnGoldenReviveReq req = obj.PawnId;
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, req.PawnId);
                WriteUInt32(buffer, obj.GoldenGemstonePoint);
            }

            public override S2CCharacterPawnGoldenReviveRes Read(IBuffer buffer)
            {
                S2CCharacterPawnGoldenReviveRes obj = new S2CCharacterPawnGoldenReviveRes();
                return obj;
            }
        }
    }

}
