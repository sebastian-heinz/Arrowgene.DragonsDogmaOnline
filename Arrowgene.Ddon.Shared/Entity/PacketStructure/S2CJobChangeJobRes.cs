using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CJobChangeJobRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_JOB_CHANGE_JOB_RES;

        public S2CJobChangeJobRes()
        {
            CharacterJobData=new CDataCharacterJobData();
            CharacterEquipList=new List<CDataCharacterEquipInfo>();
            SetAcquirementParamList=new List<CDataSetAcquierementParam>();
            SetAbilityParamList=new List<CDataSetAcquierementParam>();
            LearnNormalSkillParamList=new List<CDataLearnNormalSkillParam>();
            EquipJobItemList=new List<CDataEquipJobItem>();
            PlayPointDataList=new List<CDataPlayPointData>();
            Unk0=new CDataJobChangeJobResUnk0();
        }

        public CDataCharacterJobData CharacterJobData { get; set; }
        public List<CDataCharacterEquipInfo> CharacterEquipList { get; set; }
        public List<CDataSetAcquierementParam> SetAcquirementParamList { get; set; }
        public List<CDataSetAcquierementParam> SetAbilityParamList { get; set; }
        public List<CDataLearnNormalSkillParam> LearnNormalSkillParamList { get; set; }
        public List<CDataEquipJobItem> EquipJobItemList { get; set; }
        public List<CDataPlayPointData> PlayPointDataList { get; set; }
        public CDataJobChangeJobResUnk0 Unk0 { get; set; }

        public class Serializer : EntitySerializer<S2CJobChangeJobRes>
        {
            public override void Write(IBuffer buffer, S2CJobChangeJobRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntity<CDataCharacterJobData>(buffer, obj.CharacterJobData);
                WriteEntityList<CDataCharacterEquipInfo>(buffer, obj.CharacterEquipList);
                WriteEntityList<CDataSetAcquierementParam>(buffer, obj.SetAcquirementParamList);
                WriteEntityList<CDataSetAcquierementParam>(buffer, obj.SetAbilityParamList);
                WriteEntityList<CDataLearnNormalSkillParam>(buffer, obj.LearnNormalSkillParamList);
                WriteEntityList<CDataEquipJobItem>(buffer, obj.EquipJobItemList);
                WriteEntityList<CDataPlayPointData>(buffer, obj.PlayPointDataList);
                WriteEntity<CDataJobChangeJobResUnk0>(buffer, obj.Unk0);
            }

            public override S2CJobChangeJobRes Read(IBuffer buffer)
            {
                S2CJobChangeJobRes obj = new S2CJobChangeJobRes();
                ReadServerResponse(buffer, obj);
                obj.CharacterJobData = ReadEntity<CDataCharacterJobData>(buffer);
                obj.CharacterEquipList = ReadEntityList<CDataCharacterEquipInfo>(buffer);
                obj.SetAcquirementParamList = ReadEntityList<CDataSetAcquierementParam>(buffer);
                obj.SetAbilityParamList = ReadEntityList<CDataSetAcquierementParam>(buffer);
                obj.LearnNormalSkillParamList = ReadEntityList<CDataLearnNormalSkillParam>(buffer);
                obj.EquipJobItemList = ReadEntityList<CDataEquipJobItem>(buffer);
                obj.PlayPointDataList = ReadEntityList<CDataPlayPointData>(buffer);
                obj.Unk0 = ReadEntity<CDataJobChangeJobResUnk0>(buffer);
                return obj;
            }
        }
    }
}