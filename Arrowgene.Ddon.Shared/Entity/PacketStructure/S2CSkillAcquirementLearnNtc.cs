using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillAcquirementLearnNtc : IPacketStructure
    {
        public S2CSkillAcquirementLearnNtc()
        {
            SkillParamList = new();
            AbilityParamList = new();
            NormalSkillParamList = new();
        }

        public PacketId Id => PacketId.S2C_SKILL_ACQUIREMENT_LEARN_NTC;

        public List<CDataSkillLevelBaseParam> SkillParamList { get; set; }
        public List<CDataAbilityLevelBaseParam> AbilityParamList { get; set; }
        public List<CDataNormalSkillParam> NormalSkillParamList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSkillAcquirementLearnNtc>
        {
            public override void Write(IBuffer buffer, S2CSkillAcquirementLearnNtc obj)
            {
                WriteEntityList(buffer, obj.SkillParamList);
                WriteEntityList(buffer, obj.AbilityParamList);
                WriteEntityList(buffer, obj.NormalSkillParamList);
            }

            public override S2CSkillAcquirementLearnNtc Read(IBuffer buffer)
            {
                S2CSkillAcquirementLearnNtc obj = new S2CSkillAcquirementLearnNtc();
                obj.SkillParamList = ReadEntityList<CDataSkillLevelBaseParam>(buffer);
                obj.AbilityParamList = ReadEntityList<CDataAbilityLevelBaseParam>(buffer);
                obj.NormalSkillParamList = ReadEntityList<CDataNormalSkillParam>(buffer);
                return obj;
            }
        }
    }
}
