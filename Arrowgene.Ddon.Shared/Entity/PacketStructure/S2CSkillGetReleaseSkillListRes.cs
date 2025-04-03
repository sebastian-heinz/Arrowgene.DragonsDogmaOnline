using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillGetReleaseSkillListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SKILL_GET_RELEASE_SKILL_LIST_RES;

        public S2CSkillGetReleaseSkillListRes()
        {
            ReleaseAcquirementParamList = new List<CDataReleaseAcquirementParam>();
        }

        public List<CDataReleaseAcquirementParam> ReleaseAcquirementParamList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSkillGetReleaseSkillListRes>
        {
            public override void Write(IBuffer buffer, S2CSkillGetReleaseSkillListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.ReleaseAcquirementParamList);
            }

            public override S2CSkillGetReleaseSkillListRes Read(IBuffer buffer)
            {
                S2CSkillGetReleaseSkillListRes obj = new S2CSkillGetReleaseSkillListRes();
                ReadServerResponse(buffer, obj);
                obj.ReleaseAcquirementParamList = ReadEntityList<CDataReleaseAcquirementParam>(buffer);
                return obj;
            }
        }
    }
}
