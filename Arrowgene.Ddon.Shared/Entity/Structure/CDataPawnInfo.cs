using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataPawnInfo
    {
        public CDataPawnInfo()
        {
            Name = string.Empty;
            EditInfo = new CDataEditInfo();
            CharacterJobDataList = new List<CDataCharacterJobData>();
            CharacterEquipDataList = new List<CDataCharacterEquipData>();
            CharacterEquipViewDataList = new List<CDataCharacterEquipData>();
            CharacterEquipJobItemList = new List<CDataEquipJobItem>();
            CharacterItemSlotInfoList = new List<CDataCharacterItemSlotInfo>();
            CraftData = new CDataPawnCraftData();
            PawnReactionList = new List<CDataPawnReaction>();
            ContextNormalSkillList = new List<CDataContextNormalSkillData>();
            ContextSkillList = new List<CDataContextAcquirementData>();
            ContextAbilityList = new List<CDataContextAcquirementData>();
            ExtendParam = new CDataOrbGainExtendParam();
            TrainingStatus = new byte[64];
            PawnTrainingProfile = new CDataPawnTrainingProfile();
            SpSkillList = new List<CDataSpSkill>();
        }

        public uint Version { get; set; }
        public string Name { get; set; }
        public CDataEditInfo EditInfo { get; set; }
        /// <summary>
        /// TODO: Update this appropriately and store in DB whenever a packet manipulates the state
        /// </summary>
        public PawnState State { get; set; }
        public uint MaxHp { get; set; }
        public uint MaxStamina { get; set; }
        public JobId JobId { get; set; }
        public List<CDataCharacterJobData> CharacterJobDataList { get; set; }
        public List<CDataCharacterEquipData> CharacterEquipDataList { get; set; }
        public List<CDataCharacterEquipData> CharacterEquipViewDataList { get; set; }
        public List<CDataEquipJobItem> CharacterEquipJobItemList { get; set; }
        public byte JewelrySlotNum { get; set; }
        public List<CDataCharacterItemSlotInfo> CharacterItemSlotInfoList { get; set; }
        public CDataPawnCraftData CraftData { get; set; }
        public List<CDataPawnReaction> PawnReactionList { get; set; }
        public bool HideEquipHead { get; set; }
        public bool HideEquipLantern { get; set; }
        public byte AdventureCount { get; set; }
        public byte CraftCount { get; set; }
        public byte MaxAdventureCount { get; set; }
        public byte MaxCraftCount { get; set; }
        public List<CDataContextNormalSkillData> ContextNormalSkillList { get; set; }
        public List<CDataContextAcquirementData> ContextSkillList { get; set; }
        public List<CDataContextAcquirementData> ContextAbilityList { get; set; }
        public uint AbilityCostMax { get; set; }
        public CDataOrbGainExtendParam ExtendParam { get; set; }
        public PawnType PawnType { get; set; }
        public byte ShareRange { get; set; }
        public uint Likability { get; set; }
        public byte[] TrainingStatus { get; set; }
        public CDataPawnTrainingProfile PawnTrainingProfile { get; set; }
        public List<CDataSpSkill> SpSkillList { get; set; }

        public class Serializer : EntitySerializer<CDataPawnInfo>
        {
            public override void Write(IBuffer buffer, CDataPawnInfo obj)
            {
                WriteUInt32(buffer, obj.Version);
                WriteMtString(buffer, obj.Name);
                WriteEntity<CDataEditInfo>(buffer, obj.EditInfo);
                WriteByte(buffer, (byte) obj.State);
                WriteUInt32(buffer, obj.MaxHp);
                WriteUInt32(buffer, obj.MaxStamina);
                WriteByte(buffer, (byte) obj.JobId);
                WriteEntityList<CDataCharacterJobData>(buffer, obj.CharacterJobDataList);
                WriteEntityList<CDataCharacterEquipData>(buffer, obj.CharacterEquipDataList);
                WriteEntityList<CDataCharacterEquipData>(buffer, obj.CharacterEquipViewDataList);
                WriteEntityList<CDataEquipJobItem>(buffer, obj.CharacterEquipJobItemList);
                WriteByte(buffer, obj.JewelrySlotNum);
                WriteEntityList<CDataCharacterItemSlotInfo>(buffer, obj.CharacterItemSlotInfoList);
                WriteEntity<CDataPawnCraftData>(buffer, obj.CraftData);
                WriteEntityList<CDataPawnReaction>(buffer, obj.PawnReactionList);
                WriteBool(buffer, obj.HideEquipHead);
                WriteBool(buffer, obj.HideEquipLantern);
                WriteByte(buffer, obj.AdventureCount);
                WriteByte(buffer, obj.CraftCount);
                WriteByte(buffer, obj.MaxAdventureCount);
                WriteByte(buffer, obj.MaxCraftCount);
                WriteEntityList<CDataContextNormalSkillData>(buffer, obj.ContextNormalSkillList);
                WriteEntityList<CDataContextAcquirementData>(buffer, obj.ContextSkillList);
                WriteEntityList<CDataContextAcquirementData>(buffer, obj.ContextAbilityList);
                WriteUInt32(buffer, obj.AbilityCostMax);
                WriteEntity<CDataOrbGainExtendParam>(buffer, obj.ExtendParam);
                WriteByte(buffer, (byte) obj.PawnType);
                WriteByte(buffer, obj.ShareRange);
                WriteUInt32(buffer, obj.Likability);
                WriteByteArray(buffer, obj.TrainingStatus);
                WriteEntity<CDataPawnTrainingProfile>(buffer, obj.PawnTrainingProfile);
                WriteEntityList<CDataSpSkill>(buffer, obj.SpSkillList);
            }

            public override CDataPawnInfo Read(IBuffer buffer)
            {
                CDataPawnInfo obj = new CDataPawnInfo();
                obj.Version = ReadUInt32(buffer);
                obj.Name = ReadMtString(buffer);
                obj.EditInfo = ReadEntity<CDataEditInfo>(buffer);
                obj.State = (PawnState) ReadByte(buffer);
                obj.MaxHp = ReadUInt32(buffer);
                obj.MaxStamina = ReadUInt32(buffer);
                obj.JobId = (JobId) ReadByte(buffer);
                obj.CharacterJobDataList = ReadEntityList<CDataCharacterJobData>(buffer);
                obj.CharacterEquipDataList = ReadEntityList<CDataCharacterEquipData>(buffer);
                obj.CharacterEquipViewDataList = ReadEntityList<CDataCharacterEquipData>(buffer);
                obj.CharacterEquipJobItemList = ReadEntityList<CDataEquipJobItem>(buffer);
                obj.JewelrySlotNum = ReadByte(buffer);
                obj.CharacterItemSlotInfoList = ReadEntityList<CDataCharacterItemSlotInfo>(buffer);
                obj.CraftData = ReadEntity<CDataPawnCraftData>(buffer);
                obj.PawnReactionList = ReadEntityList<CDataPawnReaction>(buffer);
                obj.HideEquipHead = ReadBool(buffer);
                obj.HideEquipLantern = ReadBool(buffer);
                obj.AdventureCount = ReadByte(buffer);
                obj.CraftCount = ReadByte(buffer);
                obj.MaxAdventureCount = ReadByte(buffer);
                obj.MaxCraftCount = ReadByte(buffer);
                obj.ContextNormalSkillList = ReadEntityList<CDataContextNormalSkillData>(buffer);
                obj.ContextSkillList = ReadEntityList<CDataContextAcquirementData>(buffer);
                obj.ContextAbilityList = ReadEntityList<CDataContextAcquirementData>(buffer);
                obj.AbilityCostMax = ReadUInt32(buffer);
                obj.ExtendParam = ReadEntity<CDataOrbGainExtendParam>(buffer);
                obj.PawnType = (PawnType) ReadByte(buffer);
                obj.ShareRange = ReadByte(buffer);
                obj.Likability = ReadUInt32(buffer);
                obj.TrainingStatus = ReadByteArray(buffer, 64);
                obj.PawnTrainingProfile = ReadEntity<CDataPawnTrainingProfile>(buffer);
                obj.SpSkillList = ReadEntityList<CDataSpSkill>(buffer);
                return obj ;
            }
        }
    }
}
