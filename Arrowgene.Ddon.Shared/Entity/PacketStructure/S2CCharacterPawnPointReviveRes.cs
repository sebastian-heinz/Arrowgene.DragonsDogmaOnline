using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCharacterPawnPointReviveRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CHARACTER_PAWN_POINT_REVIVE_RES;

        public uint PawnId { get; set; }
        public byte RevivePoint { get; set; }

        public S2CCharacterPawnPointReviveRes()
        {
            PawnId = 0;
            RevivePoint = 0;
        }

        public class Serializer : PacketEntitySerializer<S2CCharacterPawnPointReviveRes>
        {

            public override void Write(IBuffer buffer, S2CCharacterPawnPointReviveRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.PawnId);
                WriteByte(buffer, obj.RevivePoint);
            }

            public override S2CCharacterPawnPointReviveRes Read(IBuffer buffer)
            {
                S2CCharacterPawnPointReviveRes obj = new S2CCharacterPawnPointReviveRes();
                ReadServerResponse(buffer, obj);
                obj.PawnId = ReadUInt32(buffer);
                obj.RevivePoint = ReadByte(buffer);
                return obj;
            }
        }
    }

}
