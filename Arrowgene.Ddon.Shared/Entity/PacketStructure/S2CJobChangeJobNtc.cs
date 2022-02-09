using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CJobChangeJobNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_JOB_CHANGE_JOB_NTC;

        public S2CJobChangeJobNtc()
        {
            CharacterId=0;
            CharacterJobData=new CDataCharacterJobData();
            EquipItemInfo=new List<CDataEquipItemInfo>();
            SetAcquierementParamList=new List<CDataSetAcquierementParam>();
            SetAbilityParamList=new List<CDataSetAcquierementParam>();
            LearnNormalSkillParamList=new List<CDataLearnNormalSkillParam>();
            EquipJobItemList=new List<CDataEquipJobItem>();
            Unk0=0;
            Unk1=new List<CDataEquipElementParam>();
        }

        public uint CharacterId { get; set; }
        public CDataCharacterJobData CharacterJobData { get; set; }
        public List<CDataEquipItemInfo> EquipItemInfo { get; set; }
        public List<CDataSetAcquierementParam> SetAcquierementParamList { get; set; }
        public List<CDataSetAcquierementParam> SetAbilityParamList { get; set; }
        public List<CDataLearnNormalSkillParam> LearnNormalSkillParamList { get; set; }
        public List<CDataEquipJobItem> EquipJobItemList { get; set; }
        public byte Unk0 { get; set; }
        public List<CDataEquipElementParam> Unk1 { get; set; }

        public class Serializer : EntitySerializer<S2CJobChangeJobNtc>
        {
            public override void Write(IBuffer buffer, S2CJobChangeJobNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteEntity<CDataCharacterJobData>(buffer, obj.CharacterJobData);
                WriteEntityList<CDataEquipItemInfo>(buffer, obj.EquipItemInfo);
                WriteEntityList<CDataSetAcquierementParam>(buffer, obj.SetAcquierementParamList);
                WriteEntityList<CDataSetAcquierementParam>(buffer, obj.SetAbilityParamList);
                WriteEntityList<CDataLearnNormalSkillParam>(buffer, obj.LearnNormalSkillParamList);
                WriteEntityList<CDataEquipJobItem>(buffer, obj.EquipJobItemList);
                WriteByte(buffer, obj.Unk0);
                WriteEntityList<CDataEquipElementParam>(buffer, obj.Unk1);
            }

            public override S2CJobChangeJobNtc Read(IBuffer buffer)
            {
                S2CJobChangeJobNtc obj = new S2CJobChangeJobNtc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.CharacterJobData = ReadEntity<CDataCharacterJobData>(buffer);
                obj.EquipItemInfo = ReadEntityList<CDataEquipItemInfo>(buffer);
                obj.SetAcquierementParamList = ReadEntityList<CDataSetAcquierementParam>(buffer);
                obj.SetAbilityParamList = ReadEntityList<CDataSetAcquierementParam>(buffer);
                obj.LearnNormalSkillParamList = ReadEntityList<CDataLearnNormalSkillParam>(buffer);
                obj.EquipJobItemList = ReadEntityList<CDataEquipJobItem>(buffer);
                obj.Unk0 = ReadByte(buffer);
                obj.Unk1 = ReadEntityList<CDataEquipElementParam>(buffer);
                return obj;
            }
        }
    }
}