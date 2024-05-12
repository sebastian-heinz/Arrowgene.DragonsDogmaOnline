using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CJobChangePawnJobNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_JOB_CHANGE_PAWN_JOB_NTC;

        public S2CJobChangePawnJobNtc()
        {
            CharacterId=0;
            PawnId=0;
            CharacterJobData=new CDataCharacterJobData();
            EquipItemInfo=new List<CDataEquipItemInfo>();
            SetAcquirementParamList=new List<CDataSetAcquirementParam>();
            SetAbilityParamList=new List<CDataSetAcquirementParam>();
            LearnNormalSkillParamList=new List<CDataLearnNormalSkillParam>();
            EquipJobItemList=new List<CDataEquipJobItem>();
            Unk0=new CDataJobChangeJobResUnk0();
        }

        public uint CharacterId { get; set; }
        public uint PawnId { get; set; }
        public CDataCharacterJobData CharacterJobData { get; set; }
        public List<CDataEquipItemInfo> EquipItemInfo { get; set; }
        public List<CDataSetAcquirementParam> SetAcquirementParamList { get; set; }
        public List<CDataSetAcquirementParam> SetAbilityParamList { get; set; }
        public List<CDataLearnNormalSkillParam> LearnNormalSkillParamList { get; set; }
        public List<CDataEquipJobItem> EquipJobItemList { get; set; }
        public CDataJobChangeJobResUnk0 Unk0 { get; set; }

        public class Serializer : PacketEntitySerializer<S2CJobChangePawnJobNtc>
        {
            public override void Write(IBuffer buffer, S2CJobChangePawnJobNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteUInt32(buffer, obj.PawnId);
                WriteEntity<CDataCharacterJobData>(buffer, obj.CharacterJobData);
                WriteEntityList<CDataEquipItemInfo>(buffer, obj.EquipItemInfo);
                WriteEntityList<CDataSetAcquirementParam>(buffer, obj.SetAcquirementParamList);
                WriteEntityList<CDataSetAcquirementParam>(buffer, obj.SetAbilityParamList);
                WriteEntityList<CDataLearnNormalSkillParam>(buffer, obj.LearnNormalSkillParamList);
                WriteEntityList<CDataEquipJobItem>(buffer, obj.EquipJobItemList);
                WriteEntity<CDataJobChangeJobResUnk0>(buffer, obj.Unk0);
            }

            public override S2CJobChangePawnJobNtc Read(IBuffer buffer)
            {
                S2CJobChangePawnJobNtc obj = new S2CJobChangePawnJobNtc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.PawnId = ReadUInt32(buffer);
                obj.CharacterJobData = ReadEntity<CDataCharacterJobData>(buffer);
                obj.EquipItemInfo = ReadEntityList<CDataEquipItemInfo>(buffer);
                obj.SetAcquirementParamList = ReadEntityList<CDataSetAcquirementParam>(buffer);
                obj.SetAbilityParamList = ReadEntityList<CDataSetAcquirementParam>(buffer);
                obj.LearnNormalSkillParamList = ReadEntityList<CDataLearnNormalSkillParam>(buffer);
                obj.EquipJobItemList = ReadEntityList<CDataEquipJobItem>(buffer);
                obj.Unk0 = ReadEntity<CDataJobChangeJobResUnk0>(buffer);
                return obj;
            }
        }
    }
}
