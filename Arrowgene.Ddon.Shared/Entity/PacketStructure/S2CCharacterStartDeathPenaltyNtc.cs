using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCharacterStartDeathPenaltyNtc : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CHARACTER_START_DEATH_PENALTY_NTC;

        public uint RemainTime { get; set; }

        public class Serializer : PacketEntitySerializer<S2CCharacterStartDeathPenaltyNtc>
        {
            public override void Write(IBuffer buffer, S2CCharacterStartDeathPenaltyNtc obj)
            {
                WriteUInt32(buffer, obj.RemainTime);
            }

            public override S2CCharacterStartDeathPenaltyNtc Read(IBuffer buffer)
            {
                S2CCharacterStartDeathPenaltyNtc obj = new S2CCharacterStartDeathPenaltyNtc();
                obj.RemainTime = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}