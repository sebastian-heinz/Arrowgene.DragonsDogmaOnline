using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillGetAcquirableSkillListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SKILL_GET_ACQUIRABLE_SKILL_LIST_RES;

        public S2CSkillGetAcquirableSkillListRes()
        {
            SkillParamList = new List<CDataSkillParam>();
        }

        public List<CDataSkillParam> SkillParamList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSkillGetAcquirableSkillListRes>
        {
            public override void Write(IBuffer buffer, S2CSkillGetAcquirableSkillListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataSkillParam>(buffer, obj.SkillParamList);
            }

            public override S2CSkillGetAcquirableSkillListRes Read(IBuffer buffer)
            {
                S2CSkillGetAcquirableSkillListRes obj = new S2CSkillGetAcquirableSkillListRes();
                ReadServerResponse(buffer, obj);
                obj.SkillParamList = ReadEntityList<CDataSkillParam>(buffer);
                return obj;
            }
        }
    }
}
