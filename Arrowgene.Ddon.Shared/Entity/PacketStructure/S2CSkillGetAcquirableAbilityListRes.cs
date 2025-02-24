using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillGetAcquirableAbilityListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SKILL_GET_ACQUIRABLE_ABILITY_LIST_RES;

        public S2CSkillGetAcquirableAbilityListRes()
        {
            AbilityParamList = new List<CDataAbilityParam>();
        }

        public List<CDataAbilityParam> AbilityParamList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSkillGetAcquirableAbilityListRes>
        {
            public override void Write(IBuffer buffer, S2CSkillGetAcquirableAbilityListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataAbilityParam>(buffer, obj.AbilityParamList);
            }

            public override S2CSkillGetAcquirableAbilityListRes Read(IBuffer buffer)
            {
                S2CSkillGetAcquirableAbilityListRes obj = new S2CSkillGetAcquirableAbilityListRes();
                ReadServerResponse(buffer, obj);
                obj.AbilityParamList = ReadEntityList<CDataAbilityParam>(buffer);
                return obj;
            }
        }
    }
}
