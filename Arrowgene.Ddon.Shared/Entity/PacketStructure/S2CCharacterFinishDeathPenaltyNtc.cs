using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCharacterFinishDeathPenaltyNtc : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CHARACTER_FINISH_DEATH_PENALTY_NTC;

        public class Serializer : PacketEntitySerializer<S2CCharacterFinishDeathPenaltyNtc>
        {
            public override void Write(IBuffer buffer, S2CCharacterFinishDeathPenaltyNtc obj)
            {                
            }

            public override S2CCharacterFinishDeathPenaltyNtc Read(IBuffer buffer)
            {
                S2CCharacterFinishDeathPenaltyNtc obj = new S2CCharacterFinishDeathPenaltyNtc();   
                return obj;
            }
        }
    }
}