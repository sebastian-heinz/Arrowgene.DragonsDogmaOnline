using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCharacterCharacterPointReviveRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CHARACTER_CHARACTER_POINT_REVIVE_RES;

        /**
         * The amount of revive points the player has after being revived
         * (Normally, current amount - 1)
        */ 
        public byte RevivePoint { get; set; }

        public S2CCharacterCharacterPointReviveRes()
        {
            RevivePoint = 0;
        }
        public class Serializer : PacketEntitySerializer<S2CCharacterCharacterPointReviveRes>
        {

            public override void Write(IBuffer buffer, S2CCharacterCharacterPointReviveRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteByte(buffer, obj.RevivePoint);
            }

            public override S2CCharacterCharacterPointReviveRes Read(IBuffer buffer)
            {
                S2CCharacterCharacterPointReviveRes obj = new S2CCharacterCharacterPointReviveRes();
                ReadServerResponse(buffer, obj);
                obj.RevivePoint = ReadByte(buffer);
                return obj;
            }
        }
    }
}
