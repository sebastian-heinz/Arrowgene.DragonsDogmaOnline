using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCharacterPointReviveRes
    {
        public S2CCharacterPointReviveRes()
        {
            revivePoint = 0;
        }

        /**
         * The amount of revive points the player has after being revived
         * (Normally, current amount - 1)
        */ 
        public byte revivePoint;
    }

    public class S2CCharacterPointReviveResSerializer : EntitySerializer<S2CCharacterPointReviveRes>
    {
        public override void Write(IBuffer buffer, S2CCharacterPointReviveRes obj)
        {
            WriteByte(buffer, obj.revivePoint);
        }

        public override S2CCharacterPointReviveRes Read(IBuffer buffer)
        {
            S2CCharacterPointReviveRes obj = new S2CCharacterPointReviveRes();
            obj.revivePoint = ReadByte(buffer);
            return obj;
        }
    }
}