using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCharacterPawnPointReviveRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CHARACTER_PAWN_POINT_REVIVE_RES;

        public byte RevivePoint { get; set; }
        public C2SCharacterPawnPointReviveReq PawnId { get; set; }

        public S2CCharacterPawnPointReviveRes()
        {
            RevivePoint = 0;
        }

        public S2CCharacterPawnPointReviveRes(C2SCharacterPawnPointReviveReq req)
        {
            PawnId = req;
        }

        public class Serializer : PacketEntitySerializer<S2CCharacterPawnPointReviveRes>
        {

            public override void Write(IBuffer buffer, S2CCharacterPawnPointReviveRes obj)
            {
                C2SCharacterPawnPointReviveReq req = obj.PawnId;
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, req.PawnId);
                WriteByte(buffer, obj.RevivePoint);
            }

            public override S2CCharacterPawnPointReviveRes Read(IBuffer buffer)
            {
                S2CCharacterPawnPointReviveRes obj = new S2CCharacterPawnPointReviveRes();
                return obj;
            }
        }
    }

}
