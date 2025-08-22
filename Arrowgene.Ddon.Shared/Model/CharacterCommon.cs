#nullable enable
using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.Shared.Model
{
    // TODO: Better name
    // This class contains data common to both players (Character) and pawns (Pawn)
    public abstract class CharacterCommon
    {
        public static readonly uint BASE_HEALTH = 760U;
        public static readonly uint BASE_STAMINA = 450U;
        public static readonly uint BBM_BASE_HEALTH = 990U;
        public static readonly uint BBM_BASE_STAMINA = 589U;

        public static readonly uint DEFAULT_RING_COUNT = 1;
        public static readonly uint BASE_ABILITY_COST_AMOUNT = 15;

        public static readonly uint MAX_PLAYER_HP = uint.MaxValue;
        public static readonly uint MAX_PLAYER_STAMINA = uint.MaxValue;

        public CharacterCommon()
        {
            EquippedCustomSkillsDictionary = System.Enum.GetValues<JobId>()
                .Select(jobId => (jobId, Enumerable.Repeat<CustomSkill?>(null, 0x14).ToList())) // Main Palette slots: 0x1, 0x2, 0x3, 0x4 || Sub Palette slots: 0x11, 0x12, 0x13, 0x14
                .ToDictionary(pair => pair.jobId, pair => pair.Item2);
            EquippedAbilitiesDictionary = System.Enum.GetValues<JobId>()
                .Select(jobId => (jobId, Enumerable.Repeat<Ability?>(null, 10).ToList()))
                .ToDictionary(pair => pair.jobId, pair => pair.Item2);
        }

        public CDataCharacterJobData? ActiveCharacterJobData
        {
            get { return CharacterJobDataList.Where(x => x.Job == Job).SingleOrDefault(); }
        }

        public uint CommonId { get; set; }
        public CDataGameServerListInfo Server { get; set; } = new();
        public CDataEditInfo EditInfo { get; set; } = new();
        public CDataStatusInfo StatusInfo { get; set; } = new();
        public JobId Job { get; set; }
        public bool HideEquipHead { get; set; }
        public bool HideEquipLantern { get; set; }
        public List<CDataCharacterJobData> CharacterJobDataList { get; set; } = [];
        public EquipmentTemplate EquipmentTemplate { get; set; } = new();
        public Equipment Equipment { get; set; }
        public byte JewelrySlotNum { get; set; }
        public List<CDataNormalSkillParam> LearnedNormalSkills { get; set; } = [];
        public List<CustomSkill> LearnedCustomSkills { get; set; } = [];
        public Dictionary<JobId, List<CustomSkill?>> EquippedCustomSkillsDictionary { get; set; }
        public List<Ability> LearnedAbilities { get; set; } = [];
        public Dictionary<JobId, List<Ability?>> EquippedAbilitiesDictionary { get; set; }
        public OnlineStatus OnlineStatus { get; set; } = OnlineStatus.Offline;
        public CDataOrbGainExtendParam ExtendedParams { get; set; } = new();
        public Dictionary<JobId, CDataOrbGainExtendParam> ExtendedJobParams { get; set; } = [];
        public Dictionary<JobId, HashSet<uint>> ReleasedExtendedJobParams { get; set; } = [];
        public Dictionary<JobId, HashSet<uint>> UnlockedCustomSkills { get; set; } = [];
        public Dictionary<JobId, HashSet<AbilityId>> UnlockedAbilities { get; set; } = [];
        public List<CDataEquipStatParam> EmblemStatList { get; set; } = [];
        public List<CDataReleaseOrbElement> OrbRelease { get; set; } = [];
        public CharacterProfile CharacterProfile { get; set; } = new();

        /// TODO combine into a location class ?
        public StageLayoutId Stage { get; set; }

        public uint StageNo { get; set; }
        public double X { get; set; }
        public float Y { get; set; }
        public double Z { get; set; }

        public uint GreenHp { get; set; }
        public uint WhiteHp { get; set; }

        #region CData Conversions

        public CDataOrbGainExtendParam CalculateFullExtendedParams()
        {
            return new CDataOrbGainExtendParam()
            {
                HpMax = (ushort)StatusInfo.GainHP,
                StaminaMax = (ushort)StatusInfo.GainStamina,
                Attack = (ushort)StatusInfo.GainAttack,
                Defence = (ushort)StatusInfo.GainDefense,
                MagicAttack = (ushort)StatusInfo.GainMagicAttack,
                MagicDefence = (ushort)StatusInfo.GainMagicDefense,
                AbilityCost = ExtendedParams.AbilityCost,
                JewelrySlot = ExtendedParams.JewelrySlot,
                UseItemSlot = ExtendedParams.UseItemSlot,
                MaterialItemSlot = ExtendedParams.MaterialItemSlot,
                EquipItemSlot = ExtendedParams.EquipItemSlot,
                MainPawnSlot = ExtendedParams.MainPawnSlot,
                SupportPawnSlot = ExtendedParams.SupportPawnSlot
            };
        }

        public CDataContextResist CDataContextResist
        {
            get
            {
                return new()
                {
                    FireResist = ActiveCharacterJobData?.FireResist ?? 0,
                    IceResist = ActiveCharacterJobData?.IceResist ?? 0,
                    ThunderResist = ActiveCharacterJobData?.ThunderResist ?? 0,
                    HolyResist = ActiveCharacterJobData?.HolyResist ?? 0,
                    DarkResist = ActiveCharacterJobData?.DarkResist ?? 0,
                    SpreadResist = ActiveCharacterJobData?.SpreadResist ?? 0,
                    FreezeResist = ActiveCharacterJobData?.FreezeResist ?? 0,
                    ShockResist = ActiveCharacterJobData?.ShockResist ?? 0,
                    AbsorbResist = ActiveCharacterJobData?.AbsorbResist ?? 0,
                    DarkElmResist = ActiveCharacterJobData?.DarkElmResist ?? 0,
                    PoisonResist = ActiveCharacterJobData?.PoisonResist ?? 0,
                    SlowResist = ActiveCharacterJobData?.SlowResist ?? 0,
                    SleepResist = ActiveCharacterJobData?.SleepResist ?? 0,
                    StunResist = ActiveCharacterJobData?.StunResist ?? 0,
                    WetResist = ActiveCharacterJobData?.WetResist ?? 0,
                    OilResist = ActiveCharacterJobData?.OilResist ?? 0,
                    SealResist = ActiveCharacterJobData?.SealResist ?? 0,
                    CurseResist = ActiveCharacterJobData?.CurseResist ?? 0,
                    SoftResist = ActiveCharacterJobData?.SoftResist ?? 0,
                    StoneResist = ActiveCharacterJobData?.StoneResist ?? 0,
                    GoldResist = ActiveCharacterJobData?.GoldResist ?? 0,
                    FireReduceResist = ActiveCharacterJobData?.FireReduceResist ?? 0,
                    IceReduceResist = ActiveCharacterJobData?.IceReduceResist ?? 0,
                    ThunderReduceResist = ActiveCharacterJobData?.ThunderReduceResist ?? 0,
                    HolyReduceResist = ActiveCharacterJobData?.HolyReduceResist ?? 0,
                    DarkReduceResist = ActiveCharacterJobData?.DarkReduceResist ?? 0,
                    AtkDownResist = ActiveCharacterJobData?.AtkDownResist ?? 0,
                    DefDownResist = ActiveCharacterJobData?.DefDownResist ?? 0,
                    MAtkDownResist = ActiveCharacterJobData?.MAtkDownResist ?? 0,
                    MDefDownResist = ActiveCharacterJobData?.MDefDownResist ?? 0,
                };
            }
        }

        public CDataContextPlayerInfo CDataContextPlayerInfo
        {
            get
            {
                return new()
                {
                    Job = Job,
                    HP = StatusInfo.HP,
                    MaxHP = StatusInfo.MaxHP,
                    WhiteHP = StatusInfo.WhiteHP,
                    Stamina = StatusInfo.Stamina,
                    MaxStamina = StatusInfo.MaxStamina,
                    // Weight?
                    Lv = (ushort)(ActiveCharacterJobData?.Lv ?? 0),
                    Exp = ActiveCharacterJobData?.Exp ?? 0,
                    Atk = ActiveCharacterJobData?.Atk ?? 0,
                    Def = ActiveCharacterJobData?.Def ?? 0,
                    MAtk = ActiveCharacterJobData?.MAtk ?? 0,
                    MDef = ActiveCharacterJobData?.MDef ?? 0,
                    Strength = ActiveCharacterJobData?.Strength ?? 0,
                    DownPower = ActiveCharacterJobData?.DownPower ?? 0,
                    ShakePower = ActiveCharacterJobData?.ShakePower ?? 0,
                    StunPower = ActiveCharacterJobData?.StunPower ?? 0,
                    Constitution = ActiveCharacterJobData?.Constitution ?? 0,
                    Guts = ActiveCharacterJobData?.Guts ?? 0,
                    JobPoint = ActiveCharacterJobData?.JobPoint ?? 0,
                    GainHp = StatusInfo.GainHP,
                    GainStamina = StatusInfo.GainStamina,
                    GainAttack = StatusInfo.GainAttack,
                    GainDefense = StatusInfo.GainDefense,
                    GainMagicAttack = StatusInfo.GainMagicAttack,
                    GainMagicDefense = StatusInfo.GainMagicDefense,
                    // ActNo?
                    RevivePoint = StatusInfo.RevivePoint,
                    // CustomSkillGroup?
                    JobList = [.. CharacterJobDataList.Select(x => new CDataContextJobData(x))],
                    ChargeEffectList = [], // TODO
                    OcdActiveList = [], // TODO
                                        // CatchType?
                                        // CatchJointNo?
                                        // CustomWork?
                };
            }
        }

        public virtual CDataContextBase CDataContextBase { get
            {
                return new()
                {
                    StageNo = (int)(StageNo != 0 ? StageNo : 200),
                    Sex = EditInfo.Sex,
                    HideEquipHead = HideEquipHead,
                    HideEquipLantern = HideEquipLantern,
                    ContextEquipJobItemList = [.. EquipmentTemplate.JobItemsAsCDataEquipJobItem(Job)
                        .Select(x => new CDataContextEquipJobItemData(x))],
                    ContextNormalSkillList = [.. LearnedNormalSkills
                        .Where(x => x.Job == Job)
                        .Select(x => new CDataContextNormalSkillData(x))],
                    ContextSkillList = [.. EquippedCustomSkillsDictionary[Job]
                        .Select((x, index) => x?.AsCDataContextAcquirementData((byte)(index + 1)))
                        .Where(x => x != null)],
                    ContextAbilityList = [.. EquippedAbilitiesDictionary[Job]
                        .Select((x, index) => x?.AsCDataContextAcquirementData((byte)(index + 1)))
                        .Where(x => x != null)],
                    EmblemStatList = EmblemStatList,
                    ContextEquipPerformanceList = Equipment.AsCDataContextEquipData(EquipType.Performance),
                    ContextEquipVisualList = Equipment.AsCDataContextEquipData(EquipType.Visual),
                };
            } 
        }

        public CDataCharacterLevelParam CDataCharacterLevelParam
        {
            get
            {
                return new()
                {
                    Attack = ActiveCharacterJobData?.Atk ?? 0,
                    MagAttack = ActiveCharacterJobData?.MAtk ?? 0,
                    Defence = ActiveCharacterJobData?.Def ?? 0,
                    MagDefence = ActiveCharacterJobData?.MDef ?? 0,
                    Strength = ActiveCharacterJobData?.Strength ?? 0,
                    DownPower = ActiveCharacterJobData?.DownPower ?? 0,
                    ShakePower = ActiveCharacterJobData?.ShakePower ?? 0,
                    StunPower = ActiveCharacterJobData?.StunPower ?? 0,
                    Constitution = ActiveCharacterJobData?.Constitution ?? 0,
                    Guts = ActiveCharacterJobData?.Guts ?? 0,
                };
            }
        }

        public CDataCharacterListElement CDataCharacterListElement { 
            get {
                return new()
                {
                    CommunityCharacterBaseInfo = CDataCommunityCharacterBaseInfo,
                    ServerId = Server.Id,
                    OnlineStatus = OnlineStatus,
                    CurrentJobBaseInfo = new()
                    {
                        Job = Job,
                        Level = (byte)(ActiveCharacterJobData?.Lv ?? 0)
                    },
                    EntryJobBaseInfo = new() // TODO
                    {
                        Job = Job,
                        Level = (byte)(ActiveCharacterJobData?.Lv ?? 0)
                    }
                };
            } 
        }

        public abstract CDataCommunityCharacterBaseInfo CDataCommunityCharacterBaseInfo { get; }
        public abstract CDataCharacterName CDataCharacterName { get; }

        #endregion
    }
}
