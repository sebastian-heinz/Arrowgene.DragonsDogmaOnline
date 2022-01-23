using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCharacterGoldenReviveRes
    {
        public S2CCharacterGoldenReviveRes()
        {
            gp = 0;
        }

        /**
         * The amount of Golden Gemstones the player has after being revived
         * (Normally, current amount - 1)
        */ 
        public uint gp;
    }

    public class S2CCharacterGoldenReviveResSerializer : EntitySerializer<S2CCharacterGoldenReviveRes>
    {
        public override void Write(IBuffer buffer, S2CCharacterGoldenReviveRes obj)
        {
            WriteUInt32(buffer, obj.gp);
        }

        public override S2CCharacterGoldenReviveRes Read(IBuffer buffer)
        {
            S2CCharacterGoldenReviveRes obj = new S2CCharacterGoldenReviveRes();
            obj.gp = ReadUInt32(buffer);
            return obj;
        }
    }
}