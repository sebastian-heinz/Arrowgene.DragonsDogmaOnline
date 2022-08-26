using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillGetSetSkillListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SKILL_GET_SET_SKILL_LIST_RES;

        public S2CSkillGetSetSkillListRes()
        {
            SetAcquierementParam=new List<CDataSetAcquirementParam>();
        }

        public List<CDataSetAcquirementParam> SetAcquierementParam { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSkillGetSetSkillListRes>
        {
            public override void Write(IBuffer buffer, S2CSkillGetSetSkillListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataSetAcquirementParam>(buffer, obj.SetAcquierementParam);
            }

            public override S2CSkillGetSetSkillListRes Read(IBuffer buffer)
            {
                S2CSkillGetSetSkillListRes obj = new S2CSkillGetSetSkillListRes();
                ReadServerResponse(buffer, obj);
                obj.SetAcquierementParam = ReadEntityList<CDataSetAcquirementParam>(buffer);
                return obj;
            }
        }
    }
}
