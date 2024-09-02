using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillSetPresetPawnAbilityNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_SKILL_SET_PRESET_PAWN_ABILITY_NTC;

        public S2CSkillSetPresetPawnAbilityNtc()
        {
            AbilityDataList = new List<CDataContextAcquirementData>();
        }

        public uint PawnId { get; set; }
        public List<CDataContextAcquirementData> AbilityDataList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSkillSetPresetPawnAbilityNtc>
        {
            public override void Write(IBuffer buffer, S2CSkillSetPresetPawnAbilityNtc obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteEntityList<CDataContextAcquirementData>(buffer, obj.AbilityDataList);
            }

            public override S2CSkillSetPresetPawnAbilityNtc Read(IBuffer buffer)
            {
                S2CSkillSetPresetPawnAbilityNtc obj = new S2CSkillSetPresetPawnAbilityNtc();
                obj.PawnId = ReadUInt32(buffer);
                obj.AbilityDataList = ReadEntityList<CDataContextAcquirementData>(buffer);
                return obj;
            }
        }
    }
}
