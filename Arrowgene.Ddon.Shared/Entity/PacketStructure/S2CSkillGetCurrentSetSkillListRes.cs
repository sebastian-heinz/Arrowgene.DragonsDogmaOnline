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
        public List<CDataNormalSkillParam> NormalSkillList;
        public List<CDataSetAcquirementParam> SetCustomSkillList;
        public List<CDataSetAcquirementParam> SetAbilityList;
        public List<ArisenCsv> ArisenCsv { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSkillGetCurrentSetSkillListRes>
        {
            public override void Write(IBuffer buffer, S2CSkillGetCurrentSetSkillListRes obj)
            {
                /*
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataNormalSkillParam>(buffer, obj.NormalSkillList);
                WriteEntityList<CDataSetAcquirementParam>(buffer, obj.SetCustomSkillList);
                WriteEntityList<CDataSetAcquirementParam>(buffer, obj.SetAbilityList);
                */

                ArisenCsv arisenCsv = obj.ArisenCsv[0];
                WriteServerResponse(buffer, obj);
                //NormalSkill
                WriteUInt32(buffer, 3);
                WriteByte(buffer, arisenCsv.Job);
                WriteUInt32(buffer, 1);
                WriteUInt32(buffer, 0);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, arisenCsv.Job);
                WriteUInt32(buffer, 2);
                WriteUInt32(buffer, 0);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, arisenCsv.Job);
                WriteUInt32(buffer, 3);
                WriteUInt32(buffer, 0);
                WriteUInt32(buffer, 0);
                //CustomSkill
                WriteUInt32(buffer, 8);
                WriteByte(buffer, arisenCsv.Job);
                WriteUInt16(buffer, 1);
                WriteUInt32(buffer, arisenCsv.Cs1MpId);
                WriteByte(buffer, arisenCsv.Cs1MpLv);
                WriteByte(buffer, arisenCsv.Job);
                WriteUInt16(buffer, 2);
                WriteUInt32(buffer, arisenCsv.Cs2MpId);
                WriteByte(buffer, arisenCsv.Cs2MpLv);
                WriteByte(buffer, arisenCsv.Job);
                WriteUInt16(buffer, 3);
                WriteUInt32(buffer, arisenCsv.Cs3MpId);
                WriteByte(buffer, arisenCsv.Cs3MpLv);
                WriteByte(buffer, arisenCsv.Job);
                WriteUInt16(buffer, 4);
                WriteUInt32(buffer, arisenCsv.Cs4MpId);
                WriteByte(buffer, arisenCsv.Cs4MpLv);
                //SubPalette
                WriteByte(buffer, arisenCsv.Job);
                WriteUInt16(buffer, 17);
                WriteUInt32(buffer, arisenCsv.Cs1SpId);
                WriteByte(buffer, arisenCsv.Cs1SpLv);
                WriteByte(buffer, arisenCsv.Job);
                WriteUInt16(buffer, 18);
                WriteUInt32(buffer, arisenCsv.Cs2SpId);
                WriteByte(buffer, arisenCsv.Cs2SpLv);
                WriteByte(buffer, arisenCsv.Job);
                WriteUInt16(buffer, 19);
                WriteUInt32(buffer, arisenCsv.Cs3SpId);
                WriteByte(buffer, arisenCsv.Cs3SpLv);
                WriteByte(buffer, arisenCsv.Job);
                WriteUInt16(buffer, 20);
                WriteUInt32(buffer, arisenCsv.Cs4SpId);
                WriteByte(buffer, arisenCsv.Cs4SpLv);
                //Abilities
                WriteUInt32(buffer, 10);
                WriteByte(buffer, arisenCsv.Ab1Jb);
                WriteUInt16(buffer, 1);
                WriteUInt32(buffer, arisenCsv.Ab1Id);
                WriteByte(buffer, arisenCsv.Ab1Lv);
                WriteByte(buffer, arisenCsv.Ab2Jb);
                WriteUInt16(buffer, 2);
                WriteUInt32(buffer, arisenCsv.Ab2Id);
                WriteByte(buffer, arisenCsv.Ab2Lv);
                WriteByte(buffer, arisenCsv.Ab3Jb);
                WriteUInt16(buffer, 3);
                WriteUInt32(buffer, arisenCsv.Ab3Id);
                WriteByte(buffer, arisenCsv.Ab3Lv);
                WriteByte(buffer, arisenCsv.Ab4Jb);
                WriteUInt16(buffer, 4);
                WriteUInt32(buffer, arisenCsv.Ab4Id);
                WriteByte(buffer, arisenCsv.Ab4Lv);
                WriteByte(buffer, arisenCsv.Ab5Jb);
                WriteUInt16(buffer, 5);
                WriteUInt32(buffer, arisenCsv.Ab5Id);
                WriteByte(buffer, arisenCsv.Ab5Lv);
                WriteByte(buffer, arisenCsv.Ab6Jb);
                WriteUInt16(buffer, 6);
                WriteUInt32(buffer, arisenCsv.Ab6Id);
                WriteByte(buffer, arisenCsv.Ab6Lv);
                WriteByte(buffer, arisenCsv.Ab7Jb);
                WriteUInt16(buffer, 7);
                WriteUInt32(buffer, arisenCsv.Ab7Id);
                WriteByte(buffer, arisenCsv.Ab7Lv);
                WriteByte(buffer, arisenCsv.Ab8Jb);
                WriteUInt16(buffer, 8);
                WriteUInt32(buffer, arisenCsv.Ab8Id);
                WriteByte(buffer, arisenCsv.Ab8Lv);
                WriteByte(buffer, arisenCsv.Ab9Jb);
                WriteUInt16(buffer, 9);
                WriteUInt32(buffer, arisenCsv.Ab9Id);
                WriteByte(buffer, arisenCsv.Ab9Lv);
                WriteByte(buffer, arisenCsv.Ab10Jb);
                WriteUInt16(buffer, 10);
                WriteUInt32(buffer, arisenCsv.Ab10Id);
                WriteByte(buffer, arisenCsv.Ab10Lv);
                WriteUInt32(buffer, 0);
                WriteUInt64(buffer, 0);
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
