using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CJobChangePawnJobRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_JOB_CHANGE_PAWN_JOB_RES;

        public S2CJobChangePawnJobRes()
        {
            CharacterJobData=new CDataCharacterJobData();
            CharacterEquipList=new List<CDataCharacterEquipInfo>();
            SetAcquirementParamList=new List<CDataSetAcquirementParam>();
            SetAbilityParamList=new List<CDataSetAcquirementParam>();
            LearnNormalSkillParamList=new List<CDataLearnNormalSkillParam>();
            EquipJobItemList=new List<CDataEquipJobItem>();
            Unk0=new CDataJobChangeJobResUnk0();
            TrainingStatus = new byte[64];
            SpSkillList = new List<CDataSpSkill>();
        }

        public uint PawnId { get; set; }
        public CDataCharacterJobData CharacterJobData { get; set; }
        public List<CDataCharacterEquipInfo> CharacterEquipList { get; set; }
        public List<CDataSetAcquirementParam> SetAcquirementParamList { get; set; }
        public List<CDataSetAcquirementParam> SetAbilityParamList { get; set; }
        public List<CDataLearnNormalSkillParam> LearnNormalSkillParamList { get; set; }
        public List<CDataEquipJobItem> EquipJobItemList { get; set; }
        public CDataJobChangeJobResUnk0 Unk0 { get; set; }
        public byte[] TrainingStatus { get; set; }
        public List<CDataSpSkill> SpSkillList { get; set; }


        public class Serializer : PacketEntitySerializer<S2CJobChangePawnJobRes>
        {
            public override void Write(IBuffer buffer, S2CJobChangePawnJobRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.PawnId);
                WriteEntity<CDataCharacterJobData>(buffer, obj.CharacterJobData);
                WriteEntityList<CDataCharacterEquipInfo>(buffer, obj.CharacterEquipList);
                WriteEntityList<CDataSetAcquirementParam>(buffer, obj.SetAcquirementParamList);
                WriteEntityList<CDataSetAcquirementParam>(buffer, obj.SetAbilityParamList);
                WriteEntityList<CDataLearnNormalSkillParam>(buffer, obj.LearnNormalSkillParamList);
                WriteEntityList<CDataEquipJobItem>(buffer, obj.EquipJobItemList);
                WriteEntity<CDataJobChangeJobResUnk0>(buffer, obj.Unk0);
                WriteByteArray(buffer, obj.TrainingStatus);
                WriteEntityList<CDataSpSkill>(buffer, obj.SpSkillList);
            }

            public override S2CJobChangePawnJobRes Read(IBuffer buffer)
            {
                S2CJobChangePawnJobRes obj = new S2CJobChangePawnJobRes();
                ReadServerResponse(buffer, obj);
                obj.PawnId = ReadUInt32(buffer);
                obj.CharacterJobData = ReadEntity<CDataCharacterJobData>(buffer);
                obj.CharacterEquipList = ReadEntityList<CDataCharacterEquipInfo>(buffer);
                obj.SetAcquirementParamList = ReadEntityList<CDataSetAcquirementParam>(buffer);
                obj.SetAbilityParamList = ReadEntityList<CDataSetAcquirementParam>(buffer);
                obj.LearnNormalSkillParamList = ReadEntityList<CDataLearnNormalSkillParam>(buffer);
                obj.EquipJobItemList = ReadEntityList<CDataEquipJobItem>(buffer);
                obj.Unk0 = ReadEntity<CDataJobChangeJobResUnk0>(buffer);
                obj.TrainingStatus = ReadByteArray(buffer, 64);
                obj.SpSkillList = ReadEntityList<CDataSpSkill>(buffer);
                return obj;
            }
        }
    }
}
