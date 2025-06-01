using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillGetLearnedAbilityListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SKILL_GET_LEARNED_ABILITY_LIST_RES;

        public S2CSkillGetLearnedAbilityListRes()
        {
            SetAcquirementParam=new List<CDataLearnedSetAcquirementParam>();
        }

        public List<CDataLearnedSetAcquirementParam> SetAcquirementParam { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSkillGetLearnedAbilityListRes>
        {
            public override void Write(IBuffer buffer, S2CSkillGetLearnedAbilityListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataLearnedSetAcquirementParam>(buffer, obj.SetAcquirementParam);
            }

            public override S2CSkillGetLearnedAbilityListRes Read(IBuffer buffer)
            {
                S2CSkillGetLearnedAbilityListRes obj = new S2CSkillGetLearnedAbilityListRes();
                ReadServerResponse(buffer, obj);
                obj.SetAcquirementParam = ReadEntityList<CDataLearnedSetAcquirementParam>(buffer);
                return obj;
            }
        }
    }
}
