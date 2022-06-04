using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillGetCurrentSetSkillListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SKILL_GET_CURRENT_SET_SKILL_LIST_RES;

        public S2CSkillGetCurrentSetSkillListRes()
        {
            NormalSkillList=new List<CDataNormalSkillParam>();
            SetCustomSkillList=new List<CDataSetAcquirementParam>();
            SetAbilityList=new List<CDataSetAcquirementParam>();
        }
        public List<CDataNormalSkillParam> NormalSkillList { get; set; } // Ingame: Core Skills
        public List<CDataSetAcquirementParam> SetCustomSkillList { get; set; }
        public List<CDataSetAcquirementParam> SetAbilityList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSkillGetCurrentSetSkillListRes>
        {
            public override void Write(IBuffer buffer, S2CSkillGetCurrentSetSkillListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataNormalSkillParam>(buffer, obj.NormalSkillList);
                WriteEntityList<CDataSetAcquirementParam>(buffer, obj.SetCustomSkillList);
                WriteEntityList<CDataSetAcquirementParam>(buffer, obj.SetAbilityList);
            }

            public override S2CSkillGetCurrentSetSkillListRes Read(IBuffer buffer)
            {
                S2CSkillGetCurrentSetSkillListRes obj = new S2CSkillGetCurrentSetSkillListRes();
                ReadServerResponse(buffer, obj);
                obj.NormalSkillList = ReadEntityList<CDataNormalSkillParam>(buffer);
                obj.SetCustomSkillList = ReadEntityList<CDataSetAcquirementParam>(buffer);
                obj.SetAbilityList = ReadEntityList<CDataSetAcquirementParam>(buffer);
                return obj;
            }
        }
    }
}
