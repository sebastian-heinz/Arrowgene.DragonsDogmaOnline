using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillGetLearnedSkillListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SKILL_GET_LEARNED_SKILL_LIST_RES;

        public S2CSkillGetLearnedSkillListRes()
        {
            SetAcquierementParam=new List<CDataSetAcquierementParam>();
        }

        public List<CDataSetAcquierementParam> SetAcquierementParam { get; set; }

        public class Serializer : EntitySerializer<S2CSkillGetLearnedSkillListRes>
        {
            public override void Write(IBuffer buffer, S2CSkillGetLearnedSkillListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataSetAcquierementParam>(buffer, obj.SetAcquierementParam);
            }

            public override S2CSkillGetLearnedSkillListRes Read(IBuffer buffer)
            {
                S2CSkillGetLearnedSkillListRes obj = new S2CSkillGetLearnedSkillListRes();
                ReadServerResponse(buffer, obj);
                obj.SetAcquierementParam = ReadEntityList<CDataSetAcquierementParam>(buffer);
                return obj;
            }
        }
    }
}