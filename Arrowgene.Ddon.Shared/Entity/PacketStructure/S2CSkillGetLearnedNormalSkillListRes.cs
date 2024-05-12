using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillGetLearnedNormalSkillListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SKILL_GET_LEARNED_NORMAL_SKILL_LIST_RES;

        public S2CSkillGetLearnedNormalSkillListRes()
        {
            NormalSkillParamList=new List<CDataNormalSkillParam>();
        }

        public List<CDataNormalSkillParam> NormalSkillParamList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSkillGetLearnedNormalSkillListRes>
        {
            public override void Write(IBuffer buffer, S2CSkillGetLearnedNormalSkillListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataNormalSkillParam>(buffer, obj.NormalSkillParamList);
            }

            public override S2CSkillGetLearnedNormalSkillListRes Read(IBuffer buffer)
            {
                S2CSkillGetLearnedNormalSkillListRes obj = new S2CSkillGetLearnedNormalSkillListRes();
                ReadServerResponse(buffer, obj);
                obj.NormalSkillParamList = ReadEntityList<CDataNormalSkillParam>(buffer);

                return obj;
            }
        }
    }
}
