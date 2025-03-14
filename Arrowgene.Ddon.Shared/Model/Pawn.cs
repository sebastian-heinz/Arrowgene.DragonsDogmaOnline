#nullable enable
using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Model
{
    public class Pawn : CharacterCommon
    {
        public Pawn()
        {
            Name = string.Empty;
            OnlineStatus = OnlineStatus.None;
            PawnReactionList = new List<CDataPawnReaction>();
            SpSkills = new Dictionary<JobId, List<CDataSpSkill>>();
            CraftData = new CDataPawnCraftData() {
                CraftExp = 0,
                CraftRank = 1,
                CraftRankLimit = 71,
                CraftPoint = 0,
                PawnCraftSkillList = new List<CDataPawnCraftSkill>() {
                    new() {Type = CraftSkillType.ProductionSpeed, Level = 0},
                    new() {Type = CraftSkillType.EquipmentEnhancement, Level = 0},
                    new() {Type = CraftSkillType.EquipmentQuality, Level = 0},
                    new() {Type = CraftSkillType.ConsumableQuantity, Level = 0},
                    new() {Type = CraftSkillType.CostPerformance, Level = 0},
                    new() {Type = CraftSkillType.ConsumableProductionIsAlwaysGreatSuccess, Level = 0},
                    new() {Type = CraftSkillType.CreatingHighQualityEquipmentIsAlwaysGreatSuccess, Level = 0},
                    new() {Type = CraftSkillType.CostPerformanceEffectUpFactor1, Level = 0},
                    new() {Type = CraftSkillType.CostPerformanceEffectUpFactor2, Level = 0},
                    new() {Type = CraftSkillType.UnknownEffect10, Level = 0}
                }
            };
            TrainingStatus = new Dictionary<JobId, byte[]>();
            IsRented = false;
            PawnState = PawnState.None;
        }
        
        public Pawn(uint ownerCharacterId):this()
        {
            CharacterId = ownerCharacterId;
        }

        /// <summary>
        /// Id of Pawn
        /// </summary>
        public uint PawnId  { get; set; }
        
        /// <summary>
        /// Id of character who this pawn belongs to
        /// </summary>
        public uint CharacterId { get; set; }

        public string Name { get; set; }
        
        public byte HmType { get; set; }
        public PawnType PawnType { get; set; }

        public List<CDataPawnReaction> PawnReactionList { get; set; }
        public CDataPawnCraftData CraftData { get; set; }

        public Dictionary<JobId, byte[]> TrainingStatus { get; set; }
        public Dictionary<JobId, List<CDataSpSkill>> SpSkills { get; set; }
        public uint TrainingPoints { get; set; } // Training xp?
        public uint AvailableTraining { get; set; } // Training lv?
        public bool IsOfficialPawn {  get; set; }
        public bool IsRented {  get; set; }
        public PawnState PawnState { get; set; }
        
        public CDataPawnInfo AsCDataPawnInfo()
        {
            return new CDataPawnInfo()
            {
                Name = Name,
                EditInfo = EditInfo,
                Version = 0,
                MaxHp = StatusInfo.MaxHP,
                MaxStamina = StatusInfo.MaxStamina,
                JewelrySlotNum = JewelrySlotNum,
                JobId = ActiveCharacterJobData.Job,
                CharacterJobDataList = CharacterJobDataList,
                CharacterEquipDataList = new List<CDataCharacterEquipData>() { new CDataCharacterEquipData { Equips = Equipment.AsCDataEquipItemInfo(EquipType.Performance) } },
                CharacterEquipViewDataList = new List<CDataCharacterEquipData>() { new CDataCharacterEquipData { Equips = Equipment.AsCDataEquipItemInfo(EquipType.Visual) } },
                CharacterEquipJobItemList = EquipmentTemplate.JobItemsAsCDataEquipJobItem(ActiveCharacterJobData.Job),
                HideEquipHead = HideEquipHead,
                HideEquipLantern = HideEquipLantern,
                PawnType = PawnType,
                ContextAbilityList = EquippedAbilitiesDictionary[ActiveCharacterJobData.Job]
                    .Select((ability, index) => ability?
                    .AsCDataContextAcquirementData((byte)(index + 1)))
                    .Where(ability => ability != null)
                    .ToList(),
                ContextNormalSkillList = LearnedNormalSkills.Select(normalSkill => new CDataContextNormalSkillData(normalSkill)).ToList(),
                ContextSkillList = EquippedCustomSkillsDictionary[ActiveCharacterJobData.Job]
                        .Select((skill, index) => skill?.AsCDataContextAcquirementData((byte)(index + 1)))
                        .Where(skill => skill != null)
                        .ToList(),
                ExtendParam = ExtendedParams,
                PawnReactionList = PawnReactionList,
                // TODO: Add rest of fileds so full structure can be populated here
            };
        }
    }
}
