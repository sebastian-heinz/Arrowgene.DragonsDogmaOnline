#nullable enable
using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.Shared.Model
{
    public class Pawn : CharacterCommon
    {
        public Pawn()
        {
            Name = string.Empty;
            OnlineStatus = OnlineStatus.None;
            PawnReactionList = [];
            SpSkills = [];
            CraftData = new()
            {
                CraftExp = 0,
                CraftRank = 1,
                CraftRankLimit = 71,
                CraftPoint = 0,
                PawnCraftSkillList = [
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
                ]
            };
            TrainingStatus = [];
            IsRented = false;
            PawnState = PawnState.None;
            PartnerPawnData = new();
        }
        
        public Pawn(uint ownerCharacterId) : this()
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
        public PartnerPawnData PartnerPawnData { get; set; }

        public bool IsOfficialPawn {  get; set; }
        public bool IsRented {  get; set; }
        public PawnState PawnState { get; set; }

        public List<CDataPawnHistory> PawnHistory { get; set; } = [];
        public CDataPawnTotalScore PawnScore { get; set; } = new();

        #region CData Conversions

        public override CDataCommunityCharacterBaseInfo CDataCommunityCharacterBaseInfo
        {
            get
            {
                return new CDataCommunityCharacterBaseInfo()
                {
                    CharacterId = CharacterId,
                    CharacterName = CDataCharacterName
                };
            }
        }

        public override CDataCharacterName CDataCharacterName
        {
            get
            {
                return new CDataCharacterName()
                {
                    FirstName = Name,
                };
            }
        }

        public override CDataContextBase CDataContextBase
        {
            get
            {
                var context = base.CDataContextBase;
                context.CharacterId = CharacterId;
                context.FirstName = Name;
                context.PawnId = PawnId;
                context.HmType = HmType;
                context.PawnType = PawnType;

                return context;
            }
        }

        public virtual CDataPawnInfo CDataPawnInfo 
        { 
            get {
                var extendParams = CalculateFullExtendedParams();
                return new CDataPawnInfo()
                {
                    Version = 0,
                    Name = Name,
                    EditInfo = EditInfo,
                    State = PawnState,
                    MaxHp = StatusInfo.MaxHP,
                    MaxStamina = StatusInfo.MaxStamina,
                    JobId = Job,
                    CharacterJobDataList = CharacterJobDataList,
                    CharacterEquipDataList = [ new() { Equips = Equipment.AsCDataEquipItemInfo(EquipType.Performance) }],
                    CharacterEquipViewDataList = [ new() { Equips = Equipment.AsCDataEquipItemInfo(EquipType.Visual) }],
                    CharacterEquipJobItemList = EquipmentTemplate.JobItemsAsCDataEquipJobItem(Job),
                    JewelrySlotNum = JewelrySlotNum,
                    CraftData = CraftData,
                    PawnReactionList = PawnReactionList,
                    HideEquipHead = HideEquipHead,
                    HideEquipLantern = HideEquipLantern,
                    ContextNormalSkillList = [.. LearnedNormalSkills.Select(normalSkill => new CDataContextNormalSkillData(normalSkill))],
                    ContextSkillList = [.. EquippedCustomSkillsDictionary[Job]
                        .Select((skill, index) => skill?.AsCDataContextAcquirementData((byte)(index + 1)))
                        .Where(skill => skill != null)],
                    ContextAbilityList = [.. EquippedAbilitiesDictionary[Job]
                        .Select((ability, index) => ability?.AsCDataContextAcquirementData((byte)(index + 1)))
                        .Where(ability => ability != null)],
                    AbilityCostMax = BASE_ABILITY_COST_AMOUNT + extendParams.AbilityCost,
                    ExtendParam = extendParams,
                    PawnType = PawnType,
                    ShareRange = 1,
                    Likability = PartnerPawnData.NumGifts,
                    TrainingStatus = TrainingStatus.GetValueOrDefault(Job, new byte[64]),
                    PawnTrainingProfile = new() { 
                        TrainingExp = 30000, 
                        DialogCount = 3, 
                        DialogCountMax = 3, 
                        AttackFrequencyAndDistance = 1, 
                        TrainingLv = 3 
                    },
                    SpSkillList = SpSkills.GetValueOrDefault(Job, [])
                };
            } 
        }

        public CDataNoraPawnInfo CDataNoraPawnInfo
        {
            get
            {
                return new()
                {
                    Name = Name,
                    EditInfo = EditInfo,
                    Job = Job,
                    CharacterEquipData = [new() { Equips = Equipment.AsCDataEquipItemInfo(EquipType.Performance) }],
                    CharacterEquipViewData = [new() { Equips = Equipment.AsCDataEquipItemInfo(EquipType.Visual) }],
                };
            }
        }

        public CDataPartyContextPawn CDataPartyContextPawn
        { 
            get {
                return new()
                {
                    Base = CDataContextBase,
                    PlayerInfo = CDataContextPlayerInfo,
                    PawnReactionList = PawnReactionList,
                    TrainingStatus = TrainingStatus.GetValueOrDefault(Job, new byte[64]),
                    SpSkillList = SpSkills.GetValueOrDefault(Job, []),
                    ResistInfo = CDataContextResist,
                    EditInfo = EditInfo,
                };
            } 
        }

        public CDataRegisterdPawnList CDataRegisterdPawnList
        {
            get
            {
                return new()
                {
                    PawnId = PawnId,
                    Name = Name,
                    Sex = EditInfo.Sex,
                    PawnListData = new()
                    {
                        Job = Job,
                        Level = ActiveCharacterJobData!.Lv,
                        CraftRank = CraftData.CraftRank,
                        PawnCraftSkillList = CraftData.PawnCraftSkillList,
                    }
                };
            }
        }

        #endregion
    }
}
