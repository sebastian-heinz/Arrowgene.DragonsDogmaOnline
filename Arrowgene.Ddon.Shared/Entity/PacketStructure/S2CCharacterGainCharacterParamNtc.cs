using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCharacterGainCharacterParamNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CHARACTER_GAIN_CHARACTER_PARAM_NTC;

        public uint CharacterId { get; set; }
        public uint HpMaxGain { get; set; }
        public uint StaminaMaxGain { get; set; }
        public uint AttackGain { get; set; }
        public uint DefenseGain { get; set; }
        public uint MagicAttackGain { get; set; }
        public uint MagicDefenseGain { get; set; }

        public class Serializer : PacketEntitySerializer<S2CCharacterGainCharacterParamNtc>
        {
            public override void Write(IBuffer buffer, S2CCharacterGainCharacterParamNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteUInt32(buffer, obj.HpMaxGain);
                WriteUInt32(buffer, obj.StaminaMaxGain);
                WriteUInt32(buffer, obj.AttackGain);
                WriteUInt32(buffer, obj.DefenseGain);
                WriteUInt32(buffer, obj.MagicAttackGain);
                WriteUInt32(buffer, obj.MagicDefenseGain);
            }

            public override S2CCharacterGainCharacterParamNtc Read(IBuffer buffer)
            {
                S2CCharacterGainCharacterParamNtc obj = new S2CCharacterGainCharacterParamNtc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.HpMaxGain = ReadUInt32(buffer);
                obj.StaminaMaxGain = ReadUInt32(buffer);
                obj.AttackGain = ReadUInt32(buffer);
                obj.DefenseGain = ReadUInt32(buffer);
                obj.MagicAttackGain = ReadUInt32(buffer);
                obj.MagicDefenseGain = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
