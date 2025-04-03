using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillGetReleaseAbilityListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SKILL_GET_RELEASE_ABILITY_LIST_RES;

        public S2CSkillGetReleaseAbilityListRes()
        {
            ReleaseAcquirementParamList = new List<CDataReleaseAcquirementParam>();
        }

        public List<CDataReleaseAcquirementParam> ReleaseAcquirementParamList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSkillGetReleaseAbilityListRes>
        {
            public override void Write(IBuffer buffer, S2CSkillGetReleaseAbilityListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.ReleaseAcquirementParamList);
            }

            public override S2CSkillGetReleaseAbilityListRes Read(IBuffer buffer)
            {
                S2CSkillGetReleaseAbilityListRes obj = new S2CSkillGetReleaseAbilityListRes();
                ReadServerResponse(buffer, obj);
                obj.ReleaseAcquirementParamList = ReadEntityList<CDataReleaseAcquirementParam>(buffer);
                return obj;
            }
        }
    }
}
