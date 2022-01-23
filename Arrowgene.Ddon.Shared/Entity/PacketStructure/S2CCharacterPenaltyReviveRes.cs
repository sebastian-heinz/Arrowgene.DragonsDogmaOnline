using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCharacterPenaltyReviveRes
    {
        public S2CCharacterPenaltyReviveRes()
        {
        }
    }

    public class S2CCharacterPenaltyReviveResSerializer : EntitySerializer<S2CCharacterPenaltyReviveRes>
    {
        public override void Write(IBuffer buffer, S2CCharacterPenaltyReviveRes obj)
        {
        }

        public override S2CCharacterPenaltyReviveRes Read(IBuffer buffer)
        {
            return new S2CCharacterPenaltyReviveRes();
        }
    }
}