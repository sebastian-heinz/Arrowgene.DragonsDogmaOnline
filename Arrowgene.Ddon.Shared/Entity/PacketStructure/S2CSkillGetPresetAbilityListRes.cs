using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillGetPresetAbilityListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SKILL_GET_PRESET_ABILITY_LIST_RES;

        public S2CSkillGetPresetAbilityListRes()
        {
            PresetAbilityParamList=new List<CDataPresetAbilityParam>();
        }

        public List<CDataPresetAbilityParam> PresetAbilityParamList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSkillGetPresetAbilityListRes>
        {
            public override void Write(IBuffer buffer, S2CSkillGetPresetAbilityListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataPresetAbilityParam>(buffer, obj.PresetAbilityParamList);
            }

            public override S2CSkillGetPresetAbilityListRes Read(IBuffer buffer)
            {
                S2CSkillGetPresetAbilityListRes obj = new S2CSkillGetPresetAbilityListRes();
                ReadServerResponse(buffer, obj);
                obj.PresetAbilityParamList = ReadEntityList<CDataPresetAbilityParam>(buffer);
                return obj;
            }
        }
    }
}
