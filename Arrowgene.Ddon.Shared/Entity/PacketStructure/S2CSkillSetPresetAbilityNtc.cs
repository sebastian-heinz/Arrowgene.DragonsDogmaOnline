using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillSetPresetAbilityNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_SKILL_SET_PRESET_ABILITY_NTC;

        public S2CSkillSetPresetAbilityNtc()
        {
            AbilityDataList = new List<CDataContextAcquirementData>();
        }

        public uint CharacterId { get; set; }
        public List<CDataContextAcquirementData> AbilityDataList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSkillSetPresetAbilityNtc>
        {
            public override void Write(IBuffer buffer, S2CSkillSetPresetAbilityNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteEntityList<CDataContextAcquirementData>(buffer, obj.AbilityDataList);
            }

            public override S2CSkillSetPresetAbilityNtc Read(IBuffer buffer)
            {
                S2CSkillSetPresetAbilityNtc obj = new S2CSkillSetPresetAbilityNtc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.AbilityDataList = ReadEntityList<CDataContextAcquirementData>(buffer);
                return obj;
            }
        }
    }
}
