using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillGetSetAbilityListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SKILL_GET_SET_ABILITY_LIST_RES;

        public S2CSkillGetSetAbilityListRes()
        {
            SetAcquierementParam=new List<CDataSetAcquirementParam>();
        }

        public List<CDataSetAcquirementParam> SetAcquierementParam { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSkillGetSetAbilityListRes>
        {
            public override void Write(IBuffer buffer, S2CSkillGetSetAbilityListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataSetAcquirementParam>(buffer, obj.SetAcquierementParam);
            }

            public override S2CSkillGetSetAbilityListRes Read(IBuffer buffer)
            {
                S2CSkillGetSetAbilityListRes obj = new S2CSkillGetSetAbilityListRes();
                ReadServerResponse(buffer, obj);
                obj.SetAcquierementParam = ReadEntityList<CDataSetAcquirementParam>(buffer);
                return obj;
            }
        }
    }
}
