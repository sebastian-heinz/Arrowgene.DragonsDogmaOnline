using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillSetPresetAbilityListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SKILL_SET_PRESET_ABILITY_LIST_RES;

        public S2CSkillSetPresetAbilityListRes()
        {
            SetAcquirementParamList = new List<CDataSetAcquirementParam>();
        }

        public uint PawnId;
        public List<CDataSetAcquirementParam> SetAcquirementParamList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSkillSetPresetAbilityListRes>
        {
            public override void Write(IBuffer buffer, S2CSkillSetPresetAbilityListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.PawnId);
                WriteEntityList<CDataSetAcquirementParam>(buffer, obj.SetAcquirementParamList);
            }

            public override S2CSkillSetPresetAbilityListRes Read(IBuffer buffer)
            {
                S2CSkillSetPresetAbilityListRes obj = new S2CSkillSetPresetAbilityListRes();
                ReadServerResponse(buffer, obj);
                obj.PawnId = ReadUInt32(buffer);
                obj.SetAcquirementParamList = ReadEntityList<CDataSetAcquirementParam>(buffer);
                return obj;
            }
        }
    }
}
