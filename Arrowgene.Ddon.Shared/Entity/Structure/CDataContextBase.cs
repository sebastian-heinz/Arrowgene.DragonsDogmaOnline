using System.Collections.Generic;
using System.Linq;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataContextBase
    {
        public CDataContextBase(Character character)
        {
            CharacterId = character.Id;
            FirstName = character.FirstName;
            LastName = character.LastName;
            StageNo = 200; // TODO: Replace with the actual stage the player is in. As it is right now it'll probably give issues when new players join outside of WDT
            Sex = character.EditInfo.Sex;
            HideEquipHead = character.HideEquipHead;
            HideEquipLantern = character.HideEquipLantern;
            // In the context equipment lists, the index is the slot. A 0,0,0 element has to be in place if a slot is not filled
            ContextEquipPerformanceList = Enumerable.Range(1, 15)
                .Select(i => new CDataContextEquipData(character.CharacterEquipDataListDictionary[character.Job].SelectMany(x => x.Equips).Where(x => x.EquipSlot == i).SingleOrDefault(new CDataEquipItemInfo())))
                .ToList();
            ContextEquipVisualList = Enumerable.Range(1, 15)
                .Select(i => new CDataContextEquipData(character.CharacterEquipViewDataListDictionary[character.Job].SelectMany(x => x.Equips).Where(x => x.EquipSlot == i).SingleOrDefault(new CDataEquipItemInfo())))
                .ToList();
            ContextEquipJobItemList = character.CharacterEquipJobItemListDictionary[character.Job]
                .Select(x => new CDataContextEquipJobItemData(x)).ToList();
            ContextNormalSkillList = character.NormalSkills
                .Where(x => x.Job == character.Job)
                .Select(x => new CDataContextNormalSkillData(x)).ToList();
            ContextSkillList = character.CustomSkills
                .Where(x => x.Job == character.Job)
                .Select(x => x.AsCDataContextAcquirementData()).ToList();
            ContextAbilityList = character.Abilities
                .Where(x => x.Job == character.Job)
                .Select(x => x.AsCDataContextAcquirementData()).ToList();
            Unk0List=new List<CDataContextBaseUnk0>(); // TODO: Figure this one out
        }

        public CDataContextBase()
        {
            FirstName=string.Empty;
            LastName=string.Empty;
            ContextEquipPerformanceList=new List<CDataContextEquipData>();
            ContextEquipVisualList=new List<CDataContextEquipData>();
            ContextEquipJobItemList=new List<CDataContextEquipJobItemData>();
            ContextNormalSkillList=new List<CDataContextNormalSkillData>();
            ContextSkillList=new List<CDataContextAcquirementData>();
            ContextAbilityList=new List<CDataContextAcquirementData>();
            Unk0List=new List<CDataContextBaseUnk0>();
        }

        public int MemberIndex { get; set; }
        public uint PawnId { get; set; }
        public int StageNo { get; set; }
        public int StartPosNo { get; set; }
        public double PosX { get; set; }
        public float PosY { get; set; }
        public double PosZ { get; set; }
        public float AngleY { get; set; }
        public byte Sex { get; set; }
        public byte Color { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<CDataContextEquipData> ContextEquipPerformanceList { get; set; }
        public List<CDataContextEquipData> ContextEquipVisualList { get; set; }
        public List<CDataContextEquipJobItemData> ContextEquipJobItemList { get; set; }
        public bool HideEquipHead { get; set; }
        public bool HideEquipLantern { get; set; }
        public byte HmType { get; set; }
        public byte PawnType { get; set; }
        public uint CharacterId { get; set; }
        public bool SetWaitFlag { get; set; }
        public List<CDataContextNormalSkillData> ContextNormalSkillList { get; set; }
        public List<CDataContextAcquirementData> ContextSkillList { get; set; }
        public List<CDataContextAcquirementData> ContextAbilityList { get; set; }
        public uint AbilityCostSum { get; set; }
        public uint AbilityCostMax { get; set; }
        public List<CDataContextBaseUnk0> Unk0List { get; set; }

        public class Serializer : EntitySerializer<CDataContextBase>
        {
            public override void Write(IBuffer buffer, CDataContextBase obj)
            {
                WriteInt32(buffer, obj.MemberIndex);
                WriteUInt32(buffer, obj.PawnId);
                WriteInt32(buffer, obj.StageNo);
                WriteInt32(buffer, obj.StartPosNo);
                WriteDouble(buffer, obj.PosX);
                WriteFloat(buffer, obj.PosY);
                WriteDouble(buffer, obj.PosZ);
                WriteFloat(buffer, obj.AngleY);
                WriteByte(buffer, obj.Sex);
                WriteByte(buffer, obj.Color);
                WriteMtString(buffer, obj.FirstName);
                WriteMtString(buffer, obj.LastName);
                WriteEntityList<CDataContextEquipData>(buffer, obj.ContextEquipPerformanceList);
                WriteEntityList<CDataContextEquipData>(buffer, obj.ContextEquipVisualList);
                WriteEntityList<CDataContextEquipJobItemData>(buffer, obj.ContextEquipJobItemList);
                WriteBool(buffer, obj.HideEquipHead);
                WriteBool(buffer, obj.HideEquipLantern);
                WriteByte(buffer, obj.HmType);
                WriteByte(buffer, obj.PawnType);
                WriteUInt32(buffer, obj.CharacterId);
                WriteBool(buffer, obj.SetWaitFlag);
                WriteEntityList<CDataContextNormalSkillData>(buffer, obj.ContextNormalSkillList);
                WriteEntityList<CDataContextAcquirementData>(buffer, obj.ContextSkillList);
                WriteEntityList<CDataContextAcquirementData>(buffer, obj.ContextAbilityList);
                WriteUInt32(buffer, obj.AbilityCostSum);
                WriteUInt32(buffer, obj.AbilityCostMax);
                WriteEntityList<CDataContextBaseUnk0>(buffer, obj.Unk0List);
            }

            public override CDataContextBase Read(IBuffer buffer)
            {
                CDataContextBase obj = new CDataContextBase();
                obj.MemberIndex = ReadInt32(buffer);
                obj.PawnId = ReadUInt32(buffer);
                obj.StageNo = ReadInt32(buffer);
                obj.StartPosNo = ReadInt32(buffer);
                obj.PosX = ReadDouble(buffer);
                obj.PosY = ReadFloat(buffer);
                obj.PosZ = ReadDouble(buffer);
                obj.AngleY = ReadFloat(buffer);
                obj.Sex = ReadByte(buffer);
                obj.Color = ReadByte(buffer);
                obj.FirstName = ReadMtString(buffer);
                obj.LastName = ReadMtString(buffer);
                obj.ContextEquipPerformanceList = ReadEntityList<CDataContextEquipData>(buffer);
                obj.ContextEquipVisualList = ReadEntityList<CDataContextEquipData>(buffer);
                obj.ContextEquipJobItemList = ReadEntityList<CDataContextEquipJobItemData>(buffer);
                obj.HideEquipHead = ReadBool(buffer);
                obj.HideEquipLantern = ReadBool(buffer);
                obj.HmType = ReadByte(buffer);
                obj.PawnType = ReadByte(buffer);
                obj.CharacterId = ReadUInt32(buffer);
                obj.SetWaitFlag = ReadBool(buffer);
                obj.ContextNormalSkillList = ReadEntityList<CDataContextNormalSkillData>(buffer);
                obj.ContextSkillList = ReadEntityList<CDataContextAcquirementData>(buffer);
                obj.ContextAbilityList = ReadEntityList<CDataContextAcquirementData>(buffer);
                obj.AbilityCostSum = ReadUInt32(buffer);
                obj.AbilityCostMax = ReadUInt32(buffer);
                obj.Unk0List = ReadEntityList<CDataContextBaseUnk0>(buffer);
                return obj;
            }
        }
    }
}