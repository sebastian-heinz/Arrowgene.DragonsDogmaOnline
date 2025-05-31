using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillGetLearnedSkillListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SKILL_GET_LEARNED_SKILL_LIST_RES;

        public List<CDataLearnedSetAcquirementParam> SetAcquirementParam { get; set; } = new();

        public class Serializer : PacketEntitySerializer<S2CSkillGetLearnedSkillListRes>
        {
            public override void Write(IBuffer buffer, S2CSkillGetLearnedSkillListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataLearnedSetAcquirementParam>(buffer, obj.SetAcquirementParam);
            }

            public override S2CSkillGetLearnedSkillListRes Read(IBuffer buffer)
            {
                S2CSkillGetLearnedSkillListRes obj = new S2CSkillGetLearnedSkillListRes();
                ReadServerResponse(buffer, obj);
                obj.SetAcquirementParam = ReadEntityList<CDataLearnedSetAcquirementParam>(buffer);
                return obj;
            }
        }
    }
}
