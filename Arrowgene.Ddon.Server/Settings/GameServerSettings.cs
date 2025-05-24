using Arrowgene.Ddon.Server.Scripting.utils;
using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Arrowgene.Ddon.Server.Settings
{
    public class GameServerSettings : IGameSettings
    {
        public GameServerSettings(ScriptableSettings settingsData) : base(settingsData, typeof(GameServerSettings).Name)
        {
        }

        /// <summary>
        /// Additional factor to change how long crafting a recipe will take to finish.
        /// </summary>
        [DefaultValue(_AdditionalProductionSpeedFactor)]
        public double AdditionalProductionSpeedFactor
        {
            set
            {
                SetSetting("AdditionalProductionSpeedFactor", value);
            }
            get
            {
                return TryGetSetting("AdditionalProductionSpeedFactor", _AdditionalProductionSpeedFactor);
            }
        }
        private const double _AdditionalProductionSpeedFactor = 1.0;

        /// <summary>
        /// Additional factor to change how much a recipe will cost.
        /// </summary>
        [DefaultValue(_AdditionalCostPerformanceFactor)]
        public double AdditionalCostPerformanceFactor
        {
            set
            {
                SetSetting("AdditionalCostPerformanceFactor", value);
            }
            get
            {
                return TryGetSetting("AdditionalCostPerformanceFactor", _AdditionalCostPerformanceFactor);
            }
        }
        private const double _AdditionalCostPerformanceFactor = 1.0;

        /// <summary>
        /// The amount of seconds that the partner pawn must be a member of the
        /// party, adventuring in a non-safe area to receive adventure credit
        /// for the day.
        /// </summary>
        [DefaultValue(_PartnerPawnAdventureDurationInSeconds)]
        public uint PartnerPawnAdventureDurationInSeconds
        {
            set
            {
                SetSetting("PartnerPawnAdventureDurationInSeconds", value);
            }
            get
            {
                return TryGetSetting("PartnerPawnAdventureDurationInSeconds", _PartnerPawnAdventureDurationInSeconds);
            }
        }
        private const uint _PartnerPawnAdventureDurationInSeconds = 1800;

        /// <summary>
        /// Determines the maximum amount of consumable items that can be crafted in one go with a pawn.
        /// The default is a value of 10 which is equivalent to the original game's behavior.
        /// </summary>
        [DefaultValue(_CraftConsumableProductionTimesMax)]
        public byte CraftConsumableProductionTimesMax
        {
            set
            {
                SetSetting("CraftConsumableProductionTimesMax", value);
            }
            get
            {
                return TryGetSetting("CraftConsumableProductionTimesMax", _CraftConsumableProductionTimesMax);
            }
        }
        private const byte _CraftConsumableProductionTimesMax = 10;

        /// <summary>
        /// Determines the maximum amount of items you can recycle/disassemble at Craig before a reset is required.
        /// </summary>
        [DefaultValue(_CraftItemRecycleMax)]
        public byte CraftItemRecycleMax
        {
            set
            {
                SetSetting("CraftItemRecycleMax", value);
            }
            get
            {
                return TryGetSetting("CraftItemRecycleMax", _CraftItemRecycleMax);
            }
        }
        private const byte _CraftItemRecycleMax = 10;

        /// <summary>
        /// The amount of Golden Gemstones (GG) required to reset the recycle/disassemble count.
        /// </summary>
        [DefaultValue(_CraftItemRecycleResetCost)]
        public byte CraftItemRecycleResetGGCost
        {
            set
            {
                SetSetting("CraftItemRecycleResetCost", value);
            }
            get
            {
                return TryGetSetting("CraftItemRecycleResetCost", _CraftItemRecycleResetCost);
            }
        }
        private const byte _CraftItemRecycleResetCost = 1;

        /// <summary>
        /// Modifier used to skew the randomness during equipment unlimit.
        ///
        /// Example bias values (note that fractional amounts are also valid):
        /// Bias of -1.0 Inverts the bias, favoring higher indices
        /// Bias of 0.0 No bias, equal probabilty for all.
        /// Bias of 1.0 Balanced bias towers lower indices
        /// Bias of 2.0 strongly prefers lower indices
        /// </summary>
        [DefaultValue(_EquipmentLimitBreakBias)]
        public double EquipmentLimitBreakBias
        {
            set
            {
                SetSetting("EquipmentLimitBreakBias", value);
            }
            get
            {
                return TryGetSetting("EquipmentLimitBreakBias", _EquipmentLimitBreakBias);
            }
        }
        private const double _EquipmentLimitBreakBias = 1.5;


        /// <summary>
        /// The number of real world minutes that make up an in-game day.
        /// </summary>
        [DefaultValue(_GameClockTimescale)]
        public uint GameClockTimescale
        {
            set
            {
                SetSetting("GameClockTimescale", value);
            }
            get
            {
                return TryGetSetting("GameClockTimescale", _GameClockTimescale);
            }
        }
        private const uint _GameClockTimescale = 90;

        /// <summary>
        /// Use a poisson process to randomly generate a weather cycle containing this many events, using the statistics in WeatherStatistics.
        /// </summary>
        [DefaultValue(_WeatherSequenceLength)]
        public uint WeatherSequenceLength
        {
            set
            {
                SetSetting("WeatherSequenceLength", value);
            }
            get
            {
                return TryGetSetting("WeatherSequenceLength", _WeatherSequenceLength);
            }
        }
        private const uint _WeatherSequenceLength = 20;

        /// <summary>
        /// Statistics that drive semirandom weather generation. List is expected to be in (Fair, Cloudy, Rainy) order.
        /// meanLength: Average length of the weather, in seconds, when it gets rolled.
        /// weight: Relative weight of rolling that weather. Set to 0 to disable.
        /// </summary>
        [DefaultValue("new List<(uint MeanLength, uint Weight)>\n" +
            "{\n" +
            "    (60 * 30, 1), // Fair\n" +
            "    (60 * 30, 1), // Cloudy\n" +
            "    (60 * 30, 1), // Windy\n" +
            "}"
        )]
        public List<(uint MeanLength, uint Weight)> WeatherStatistics
        {
            set
            {
                SetSetting("WeatherStatistics", value);
            }
            get
            {
                return TryGetSetting("WeatherStatistics", new List<(uint MeanLength, uint Weight)>
                {
                    (60 * 30, 1), // Fair
                    (60 * 30, 1), // Cloudy
                    (60 * 30, 1), // Windy
                });
            }
        }

        /// <summary>
        /// Configures the default time in seconds a lantern is active after igniting it.
        /// </summary>
        [DefaultValue(_LanternBurnTimeInSeconds)]
        public uint LanternBurnTimeInSeconds
        {
            set
            {
                SetSetting("LanternBurnTimeInSeconds", value);
            }
            get
            {
                return TryGetSetting("LanternBurnTimeInSeconds", _LanternBurnTimeInSeconds);
            }
        }
        private const uint _LanternBurnTimeInSeconds = 1500;

        /// <summary>
        /// When using the adventure guide, configures the listing level range +/- the value
        /// of the level of the current job when displaying world quests.
        /// </summary>
        [DefaultValue(_AdventureGuideLevelRangeFilter)]
        public uint AdventureGuideLevelRangeFilter
        {
            set
            {
                SetSetting("AdventureGuideLevelRangeFilter", value);
            }
            get
            {
                return TryGetSetting("AdventureGuideLevelRangeFilter", _AdventureGuideLevelRangeFilter);
            }
        }
        private const uint _AdventureGuideLevelRangeFilter = 10;

        /// <summary>
        /// Configures the maximum amount of quests to display in the adventure guide
        /// at one time.
        /// </summary>
        [DefaultValue(_AdventureGuideMaxQuestList)]
        public uint AdventureGuideMaxQuestList
        {
            set
            {
                SetSetting("AdventureGuideMaxQuestList", value);
            }
            get
            {
                return TryGetSetting("AdventureGuideMaxQuestList", _AdventureGuideMaxQuestList);
            }
        }
        private const uint _AdventureGuideMaxQuestList = 50;

        /// <summary>
        /// Uses the automatic exp calculation system for all enemies instead of just using the
        /// ones marked in quest files.
        /// </summary>
        [DefaultValue(_EnableAutomaticExpCalculationForAll)]
        public bool EnableAutomaticExpCalculationForAll
        {
            set
            {
                SetSetting("EnableAutomaticExpCalculationForAll", value);
            }
            get
            {
                return TryGetSetting("EnableAutomaticExpCalculationForAll", _EnableAutomaticExpCalculationForAll);
            }
        }
        private const bool _EnableAutomaticExpCalculationForAll = false;

        /// <summary>
        /// When set to true, if the party leader has the content unlock of "OrbEnemy", random enemies
        /// will appear as "Blood Orb [name]" each time the instance is reset. The amount of BO will
        /// be calculated based on the enemy level.
        /// </summary>
        [DefaultValue(_EnableRandomizedBoEnemies)]
        public bool EnableRandomizedBoEnemies
        {
            set
            {
                SetSetting("EnableRandomizedBoEnemies", value);
            }
            get
            {
                return TryGetSetting("EnableRandomizedBoEnemies", _EnableRandomizedBoEnemies);
            }
        }
        private const bool _EnableRandomizedBoEnemies = false;

        /// <summary>
        /// If EnableRandomizedBoEnemies is set to true, this setting configures the chance % that
        /// an enemy will be upgraded to being a BoEnemy instead of a normal enemy.
        /// </summary>
        [DefaultValue(_RandomizedBoEnemyChance)]
        public double RandomizedBoEnemyChance
        {
            set
            {
                SetSetting("RandomizedBoEnemyChance", value);
            }
            get
            {
                return TryGetSetting("RandomizedBoEnemyChance", _RandomizedBoEnemyChance);
            }
        }
        private const double _RandomizedBoEnemyChance = 0.05;


        /// <summary>
        /// Maximum amount of play points the client will display in the UI. 
        /// Play points past this point will also trigger a chat log message saying you've reached the cap.
        /// </summary>
        [DefaultValue(_PlayPointMax)]
        public uint PlayPointMax
        {
            set
            {
                SetSetting("PlayPointMax", value);
            }
            get
            {
                return TryGetSetting("PlayPointMax", _PlayPointMax);
            }
        }
        private const uint _PlayPointMax = 2000;

        /// <summary>
        /// Maximum level for each job. 
        /// Shared with the login server.
        /// Level caps based on season release
        /// Alpha:        10
        /// CBT           15
        /// Season 1.0:   40
        /// Season 1.1:   45
        /// Season 1.2:   55
        /// Season 1.3:   60
        /// Season 2.0:   65
        /// Season 2.1:   70
        /// Season 2.2:   75
        /// Season 2.3:   80
        /// Season 3.0:   85
        /// Season 3.1:   90
        /// Season 3.2:   95
        /// Season 3.3:  100
        /// Season 3.41: 105
        /// Season 3.42: 110
        /// Season 3.43: 120
        /// </summary>
        [DefaultValue(_JobLevelMax)]
        public uint JobLevelMax
        {
            set
            {
                SetSetting("JobLevelMax", value);
            }
            get
            {
                return TryGetSetting("JobLevelMax", _JobLevelMax);
            }
        }
        private const uint _JobLevelMax = 120;

        /// <summary>
        /// Maximum number of members in a single clan. 
        /// Shared with the login server.
        /// </summary>
        [DefaultValue(_ClanMemberMax)]
        public uint ClanMemberMax
        {
            set
            {
                SetSetting("ClanMemberMax", value);
            }
            get
            {
                return TryGetSetting("ClanMemberMax", _ClanMemberMax);
            }
        }
        private const uint _ClanMemberMax = 100;

        /// <summary>
        /// Maximum number of characters per account. 
        /// Shared with the login server.
        /// </summary>
        [DefaultValue(_CharacterNumMax)]
        public byte CharacterNumMax
        {
            set
            {
                SetSetting("CharacterNumMax", value);
            }
            get
            {
                return TryGetSetting("CharacterNumMax", _CharacterNumMax);
            }
        }
        private const byte _CharacterNumMax = 4;

        /// <summary>
        /// Toggles the visual equip set for all characters. 
        /// Shared with the login server.
        /// </summary>
        [DefaultValue(_EnableVisualEquip)]
        public bool EnableVisualEquip
        {
            set
            {
                SetSetting("EnableVisualEquip", value);
            }
            get
            {
                return TryGetSetting("EnableVisualEquip", _EnableVisualEquip);
            }
        }
        private const bool _EnableVisualEquip = true;

        /// <summary>
        /// Maximum entries in the friends list. 
        /// Shared with the login server.
        /// </summary>
        [DefaultValue(_FriendListMax)]
        public uint FriendListMax
        {
            set
            {
                SetSetting("FriendListMax", value);
            }
            get
            {
                return TryGetSetting("FriendListMax", _FriendListMax);
            }
        }
        private const uint _FriendListMax = 200;

        /// <summary>
        /// Limits for each wallet type.
        /// </summary>
        [DefaultValue("new Dictionary<WalletType, uint>()\n" +
            "{\n" +
            "    {WalletType.Gold, 999999999},\n" +
            "    {WalletType.RiftPoints, 999999999},\n" +
            "    {WalletType.BloodOrbs, 500000},\n" +
            "    {WalletType.SilverTickets, 999999999},\n" +
            "    {WalletType.GoldenGemstones, 99999},\n" +
            "    {WalletType.RentalPoints, 99999},\n" +
            "    {WalletType.ResetJobPoints, 99},\n" +
            "    {WalletType.ResetCraftSkills, 99},\n" +
            "    {WalletType.HighOrbs, 5000},\n" +
            "    {WalletType.DominionPoints, 999999999},\n" +
            "    {WalletType.AdventurePassPoints, 80},\n" +
            "    {WalletType.CustomMadeServiceTickets, 999999999},\n" +
            "    {WalletType.BitterblackMazeResetTicket, 3},\n" +
            "    {WalletType.GoldenDragonMark, 30},\n" +
            "    {WalletType.SilverDragonMark, 150},\n" +
            "    {WalletType.RedDragonMark, 99999},\n" +
            "}"
        )]
        public Dictionary<WalletType, uint> WalletLimits
        {
            set
            {
                SetSetting("WalletLimits", value);
            }
            get
            {
                return TryGetSetting("WalletLimits", new Dictionary<WalletType, uint>()
                {
                    {WalletType.Gold, 999999999},
                    {WalletType.RiftPoints, 999999999},
                    {WalletType.BloodOrbs, 500000},
                    {WalletType.SilverTickets, 999999999},
                    {WalletType.GoldenGemstones, 99999},
                    {WalletType.RentalPoints, 99999},
                    {WalletType.ResetJobPoints, 99},
                    {WalletType.ResetCraftSkills, 99},
                    {WalletType.HighOrbs, 5000},
                    {WalletType.DominionPoints, 999999999},
                    {WalletType.AdventurePassPoints, 80},
                    {WalletType.CustomMadeServiceTickets, 999999999},
                    {WalletType.BitterblackMazeResetTicket, 3},
                    {WalletType.GoldenDragonMark, 30},
                    {WalletType.SilverDragonMark, 150},
                    {WalletType.RedDragonMark, 99999},
                });
            }
        }

        /// <summary>
        /// Number of bazaar entries that are given to new characters.
        /// </summary>
        [DefaultValue(_DefaultMaxBazaarExhibits)]
        public uint DefaultMaxBazaarExhibits
        {
            set
            {
                SetSetting("DefaultMaxBazaarExhibits", value);
            }
            get
            {
                return TryGetSetting("DefaultMaxBazaarExhibits", _DefaultMaxBazaarExhibits);
            }
        }
        private const uint _DefaultMaxBazaarExhibits = 5;

        /// <summary>
        /// Number of favorite warps that are given to new characters.
        /// </summary>
        [DefaultValue(_DefaultWarpFavorites)]
        public uint DefaultWarpFavorites
        {
            set
            {
                SetSetting("DefaultWarpFavorites", value);
            }
            get
            {
                return TryGetSetting("DefaultWarpFavorites", _DefaultWarpFavorites);
            }
        }
        private const uint _DefaultWarpFavorites = 5;

        /// <summary>
        /// Controls the party size for regular adventuring content. 
        /// Used to control main pawns auto-joining parties alongside their owners.
        /// </summary>
        [DefaultValue(_NormalPartySize)]
        public uint NormalPartySize
        {
            set
            {
                SetSetting("NormalPartySize", value);
            }
            get
            {
                return TryGetSetting("NormalPartySize", _NormalPartySize);
            }
        }
        private const uint _NormalPartySize = 4;

        /// <summary>
        /// Global modifier for enemy exp calculations to scale up or down.
        /// </summary>
        [DefaultValue(_EnemyExpModifier)]
        public double EnemyExpModifier
        {
            set
            {
                SetSetting("EnemyExpModifier", value);
            }
            get
            {
                return TryGetSetting("EnemyExpModifier", _EnemyExpModifier);
            }
        }
        private const double _EnemyExpModifier = 1.0;

        /// <summary>
        /// Global modifier for BBM enemy exp calculations to scale up or down.
        /// </summary>
        [DefaultValue(_BBMEnemyExpModifier)]
        public double BBMEnemyExpModifier
        {
            set
            {
                SetSetting("BBMEnemyExpModifier", value);
            }
            get
            {
                return TryGetSetting("BBMEnemyExpModifier", _BBMEnemyExpModifier);
            }
        }
        private const double _BBMEnemyExpModifier = 1.0;

        /// <summary>
        /// Global modifier for quest exp calculations to scale up or down.
        /// </summary>
        [DefaultValue(_QuestExpModifier)]
        public double QuestExpModifier
        {
            set
            {
                SetSetting("QuestExpModifier", value);
            }
            get
            {
                return TryGetSetting("QuestExpModifier", _QuestExpModifier);
            }
        }
        private const double _QuestExpModifier = 1.0;

        /// <summary>
        /// Global modifier for playpoint calculations to scale up or down.
        /// </summary>
        [DefaultValue(_PpModifier)]
        public double PpModifier
        {
            set
            {
                SetSetting("PpModifier", value);
            }
            get
            {
                return TryGetSetting("PpModifier", _PpModifier);
            }
        }
        private const double _PpModifier = 1.0;

        /// <summary>
        /// Global modifier for Gold calculations to scale up or down.
        /// </summary>
        [DefaultValue(_GoldModifier)]
        public double GoldModifier
        {
            set
            {
                SetSetting("GoldModifier", value);
            }
            get
            {
                return TryGetSetting("GoldModifier", _GoldModifier);
            }
        }
        private const double _GoldModifier = 1.0;

        /// <summary>
        /// Global modifier for Rift calculations to scale up or down.
        /// </summary>
        [DefaultValue(_RiftModifier)]
        public double RiftModifier
        {
            set
            {
                SetSetting("RiftModifier", value);
            }
            get
            {
                return TryGetSetting("RiftModifier", _RiftModifier);
            }
        }
        private const double _RiftModifier = 1.0;

        /// <summary>
        /// Global modifier for BO calculations to scale up or down.
        /// </summary>
        [DefaultValue(_BoModifier)]
        public double BoModifier
        {
            set
            {
                SetSetting("BoModifier", value);
            }
            get
            {
                return TryGetSetting("BoModifier", _BoModifier);
            }
        }
        private const double _BoModifier = 1.0;

        /// <summary>
        /// Global modifier for HO calculations to scale up or down.
        /// </summary>
        [DefaultValue(_HoModifier)]
        public double HoModifier
        {
            set
            {
                SetSetting("HoModifier", value);
            }
            get
            {
                return TryGetSetting("HoModifier", _HoModifier);
            }
        }
        private const double _HoModifier = 1.0;

        /// <summary>
        /// Global modifier for JP calculations to scale up or down.
        /// </summary>
        [DefaultValue(_JpModifier)]
        public double JpModifier
        {
            set
            {
                SetSetting("JpModifier", value);
            }
            get
            {
                return TryGetSetting("JpModifier", _JpModifier);
            }
        }
        private const double _JpModifier = 1.0;

        /// <summary>
        /// Global modifier for AP calculations to scale up or down.
        /// </summary>
        [DefaultValue(_ApModifier)]
        public double ApModifier
        {
            set
            {
                SetSetting("ApModifier", value);
            }
            get
            {
                return TryGetSetting("ApModifier", _ApModifier);
            }
        }
        private const double _ApModifier = 1.0;

        /// <summary>
        /// Configures the maximum amount of reward box slots.
        /// </summary>
        [DefaultValue(_RewardBoxMax)]
        public byte RewardBoxMax
        {
            set
            {
                SetSetting("RewardBoxMax", value);
            }
            get
            {
                return TryGetSetting("RewardBoxMax", _RewardBoxMax);
            }
        }
        private const byte _RewardBoxMax = 100;

        /// <summary>
        /// Configures the maximum amount of quests that can be ordered at one time.
        /// </summary>
        [DefaultValue(_QuestOrderMax)]
        public byte QuestOrderMax
        {
            set
            {
                SetSetting("QuestOrderMax", value);
            }
            get
            {
                return TryGetSetting("QuestOrderMax", _QuestOrderMax);
            }
        }
        private const byte _QuestOrderMax = 20;

        /// <summary>
        /// Configures if epitaph rewards are limited once per weekly reset.
        /// </summary>
        [DefaultValue(_EnableEpitaphWeeklyRewards)]
        public bool EnableEpitaphWeeklyRewards
        {
            set
            {
                SetSetting("EnableEpitaphWeeklyRewards", value);
            }
            get
            {
                return TryGetSetting("EnableEpitaphWeeklyRewards", _EnableEpitaphWeeklyRewards);
            }
        }
        private const bool _EnableEpitaphWeeklyRewards = true;

        /// <summary>
        /// Enables main pawns in party to gain EXP and JP from quests
        /// Original game apparantly did not have pawns share quest reward, so will set to false for default, 
        /// change as needed
        /// </summary>
        [DefaultValue(_EnableMainPartyPawnsQuestRewards)]
        public bool EnableMainPartyPawnsQuestRewards
        {
            set
            {
                SetSetting("EnableMainPartyPawnsQuestRewards", value);
            }
            get
            {
                return TryGetSetting("EnableMainPartyPawnsQuestRewards", _EnableMainPartyPawnsQuestRewards);
            }
        }
        private const bool _EnableMainPartyPawnsQuestRewards = false;

        /// <summary>
        /// Specifies the time in seconds that a bazaar exhibit will last.
        /// By default, the equivalent of 3 days
        /// </summary>
        [DefaultValue("(ulong) TimeSpan.FromDays(3).TotalSeconds")]
        public ulong BazaarExhibitionTimeSeconds
        {
            set
            {
                SetSetting("BazaarExhibitionTimeSeconds", value);
            }
            get
            {
                return TryGetSetting<ulong>("BazaarExhibitionTimeSeconds", (ulong) TimeSpan.FromDays(3).TotalSeconds);
            }
        }

        /// <summary>
        /// Specifies the time in seconds that a slot in the bazaar won't be able to be used again.
        /// By default, the equivalent of 1 day
        /// </summary>
        [DefaultValue("(ulong) TimeSpan.FromDays(1).TotalSeconds")]
        public ulong BazaarCooldownTimeSeconds
        {
            set
            {
                SetSetting("BazaarCooldownTimeSeconds", value);
            }
            get
            {
                return TryGetSetting("BazaarCooldownTimeSeconds", (ulong)TimeSpan.FromDays(1).TotalSeconds);
            }
        }

        /// <summary>
        /// Ties area rank progress to various paths to dungeons.
        /// </summary>
        [DefaultValue(_EnableAreaRankSpotLocks)]
        public bool EnableAreaRankSpotLocks
        {
            set
            {
                SetSetting("EnableAreaRankSpotLocks", value);
            }
            get
            {
                return TryGetSetting("EnableAreaRankSpotLocks", _EnableAreaRankSpotLocks);
            }
        }
        private const bool _EnableAreaRankSpotLocks = true;

        /// <summary>
        /// Configures the chance that various gathering tools can break
        /// when the player performs a gathering action.
        /// </summary>
        [DefaultValue(@"new Dictionary<ItemId,double>
{
    [ItemId.Pickaxe] = 0.3,
    [ItemId.EnhancedPickaxe] = 0.2,
    [ItemId.ArtisansPickaxe] = 0.1,
    [ItemId.LumberKnife] = 0.3,
    [ItemId.EnhancedLumberKnife] = 0.2,
    [ItemId.ArtisansLumberKnife] = 0.1,
    [ItemId.Lockpick] = 0.3,
    [ItemId.EnhancedLockpick] = 0.2,
    [ItemId.AllPurposeLockpick] = 0.1,
};")]
        public Dictionary<ItemId,double> ToolBreakChance
        {
            set
            {
                SetSetting("ToolBreakChance", value);
            }
            get
            {
                return TryGetSetting("ToolBreakChance", new Dictionary<ItemId,double>
                {
                    [ItemId.Pickaxe] = 0.3,
                    [ItemId.EnhancedPickaxe] = 0.2,
                    [ItemId.ArtisansPickaxe] = 0.1,
                    [ItemId.LumberKnife] = 0.3,
                    [ItemId.EnhancedLumberKnife] = 0.2,
                    [ItemId.ArtisansLumberKnife] = 0.1,
                    [ItemId.Lockpick] = 0.3,
                    [ItemId.EnhancedLockpick] = 0.2,
                    [ItemId.AllPurposeLockpick] = 0.1,
                });
            }
        }

        /// <summary>
        /// The maximum number of drop slots in a gather point
        /// generated by the default drop generator.
        /// </summary>
        [DefaultValue(_DefaultGatherDropMaxSlots)]
        public int DefaultGatherDropMaxSlots
        {
            set
            {
                SetSetting("DefaultGatherDropMaxSlots", value);
            }
            get
            {
                return TryGetSetting("DefaultGatherDropMaxSlots", _DefaultGatherDropMaxSlots);
            }
        }
        private const int _DefaultGatherDropMaxSlots = 3;

        /// <summary>
        /// The maximum number of drops to be generated on a single roll
        /// when auto generating gathering drops.
        /// </summary>
        [DefaultValue(_MaximumDropsPerDefaultGatherRoll)]
        public int MaximumDropsPerDefaultGatherRoll
        {
            set
            {
                SetSetting("MaximumDropsPerDefaultGatherRoll", value);
            }
            get
            {
                return TryGetSetting("MaximumDropsPerDefaultGatherRoll", _MaximumDropsPerDefaultGatherRoll);
            }
        }
        private const int _MaximumDropsPerDefaultGatherRoll = 3;

        /// <summary>
        /// Controls how punishing the gathering results are.
        /// A high value is more punishing than a lower value.
        /// </summary>
        [DefaultValue(_DefaultGatherDropsRandomBias)]
        public double DefaultGatherDropsRandomBias
        {
            set
            {
                SetSetting("DefaultGatherDropsRandomBias", value);
            }
            get
            {
                return TryGetSetting("DefaultGatherDropsRandomBias", _DefaultGatherDropsRandomBias);
            }
        }
        private const double _DefaultGatherDropsRandomBias = 2.0;

        /// <summary>
        /// If set to true, enables the server to generate gathering drops
        /// populated by ddon-tools.
        /// </summary>
        [DefaultValue(_EnableToolGatheringDrops)]
        public bool EnableToolGatheringDrops
        {
            set
            {
                SetSetting("EnableToolGatheringDrops", value);
            }
            get
            {
                return TryGetSetting("EnableToolGatheringDrops", _EnableToolGatheringDrops);
            }
        }
        private const bool _EnableToolGatheringDrops = true;

        /// <summary>
        /// If set to true, enables the automatically generate gathering drops
        /// based on data scraped from wikis.
        /// @note Experimental: This feature is still in development and needs
        ///                     more balance and testing before being enabled
        ///                     all the time.
        /// </summary>
        [DefaultValue(_EnableDefaultGatheringDrops)]
        public bool EnableDefaultGatheringDrops
        {
            set
            {
                SetSetting("EnableDefaultGatheringDrops", value);
            }
            get
            {
                return TryGetSetting("EnableDefaultGatheringDrops", _EnableDefaultGatheringDrops);
            }
        }
        private const bool _EnableDefaultGatheringDrops = false;

        /// <summary>
        /// The amount of golden gemstones it costs to use the beauty parlor.
        /// </summary>
        [DefaultValue(_BeautyParlorGGPrice)]
        public uint BeautyParlorGGPrice
        {
            set
            {
                SetSetting("BeautyParlorGGPrice", value);
            }
            get
            {
                return TryGetSetting("BeautyParlorGGPrice", _BeautyParlorGGPrice);
            }
        }
        private const uint _BeautyParlorGGPrice = 5;

        /// <summary>
        /// The amount of silver tickets it costs to use the beauty parlor.
        /// </summary>
        [DefaultValue(_BeautyParlorSTPrice)]
        public uint BeautyParlorSTPrice
        {
            set
            {
                SetSetting("BeautyParlorSTPrice", value);
            }
            get
            {
                return TryGetSetting("BeautyParlorSTPrice", _BeautyParlorSTPrice);
            }
        }
        private const uint _BeautyParlorSTPrice = 200;

        /// <summary>
        /// The amount of golden gemstones it costs to use the reincarnation menu.
        /// </summary>
        [DefaultValue(_ReincarnationGGPrice)]
        public uint ReincarnationGGPrice
        {
            set
            {
                SetSetting("ReincarnationGGPrice", value);
            }
            get
            {
                return TryGetSetting("ReincarnationGGPrice", _ReincarnationGGPrice);
            }
        }
        private const uint _ReincarnationGGPrice = 5;

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(_UrlDomain)]
        public string UrlDomain
        {
            set
            {
                SetSetting("UrlDomain", value);
            }
            get
            {
                return TryGetSetting("UrlDomain", _UrlDomain);
            }
        }
        private const string _UrlDomain = "http://localhost:{52099}";

        /// <summary>
        /// Various URLs used by the client.
        /// Shared with the login server.
        /// </summary>
        [DefaultValue(_UrlManual)]
        public string UrlManual
        {
            set
            {
                SetSetting("UrlManual", value);
            }
            get
            {
                return TryGetSetting("UrlManual", _UrlManual);
            }
        }
        private const string _UrlManual = "http://localhost:{52099}/manual_nfb/";

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(_UrlShopDetail)]
        public string UrlShopDetail
        {
            set
            {
                SetSetting("UrlShopDetail", value);
            }
            get
            {
                return TryGetSetting("UrlShopDetail", _UrlShopDetail);
            }
        }
        private const string _UrlShopDetail = "http://localhost:{52099}/shop/ingame/stone/detail";

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(_UrlShopCounterA)]
        public string UrlShopCounterA
        {
            set
            {
                SetSetting("UrlShopCounterA", value);
            }
            get
            {
                return TryGetSetting("UrlShopCounterA", _UrlShopCounterA);
            }
        }
        private const string _UrlShopCounterA = "http://localhost:{52099}/shop/ingame/counter?";

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(_UrlShopAttention)]
        public string UrlShopAttention
        {
            set
            {
                SetSetting<string>("UrlShopAttention", value);
            }
            get
            {
                return TryGetSetting("UrlShopAttention", _UrlShopAttention);
            }
        }
        private const string _UrlShopAttention = "http://localhost:{52099}/shop/ingame/attention?";

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(_UrlShopStoneLimit)]
        public string UrlShopStoneLimit
        {
            set
            {
                SetSetting("UrlShopStoneLimit", value);
            }
            get
            {
                return TryGetSetting("UrlShopStoneLimit", _UrlShopStoneLimit);
            }
        }
        private const string _UrlShopStoneLimit = "http://localhost:{52099}/shop/ingame/stone/limit";

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(_UrlShopCounterB)]
        public string UrlShopCounterB
        {
            set
            {
                SetSetting("UrlShopCounterB", value);
            }
            get
            {
                return TryGetSetting("UrlShopCounterB", _UrlShopCounterB);
            }
        }
        private const string _UrlShopCounterB = "http://localhost:{52099}/shop/ingame/counter?";

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(_UrlChargeCallback)]
        public string UrlChargeCallback
        {
            set
            {
                SetSetting("UrlChargeCallback", value);
            }
            get
            {
                return TryGetSetting("UrlChargeCallback", _UrlChargeCallback);
            }
        }
        private const string _UrlChargeCallback = "http://localhost:{52099}/opening/entry/ddo/cog_callback/charge";

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(_UrlChargeA)]
        public string UrlChargeA
        {
            set
            {
                SetSetting("UrlChargeA", value);
            }
            get
            {
                return TryGetSetting("UrlChargeA", _UrlChargeA);
            }
        }
        private const string _UrlChargeA = "http://localhost:{52099}/sp_ingame/charge/";

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(_UrlSample9)]
        public string UrlSample9
        {
            set
            {
                SetSetting("UrlSample9", value);
            }
            get
            {
                return TryGetSetting("UrlSample9", _UrlSample9);
            }
        }
        private const string _UrlSample9 = "http://sample09.html";

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(_UrlSample10)]
        public string UrlSample10
        {
            set
            {
                SetSetting("UrlSample10", value);
            }
            get
            {
                return TryGetSetting("UrlSample10", _UrlSample10);
            }
        }
        private const string _UrlSample10 = "http://sample10.html";

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(_UrlCampaignBanner)]
        public string UrlCampaignBanner
        {
            set
            {
                SetSetting("UrlCampaignBanner", value);
            }
            get
            {
                return TryGetSetting("UrlCampaignBanner", _UrlCampaignBanner);
            }
        }
        private const string _UrlCampaignBanner = "http://localhost:{52099}/sp_ingame/campaign/bnr/bnr01.html?";

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(_UrlSupportIndex)]
        public string UrlSupportIndex
        {
            set
            {
                SetSetting("UrlSupportIndex", value);
            }
            get
            {
                return TryGetSetting("UrlSupportIndex", _UrlSupportIndex);
            }
        }
        private const string _UrlSupportIndex = "http://localhost:{52099}/sp_ingame/support/index.html";

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(_UrlPhotoupAuthorize)]
        public string UrlPhotoupAuthorize
        {
            set
            {
                SetSetting("UrlPhotoupAuthorize", value);
            }
            get
            {
                return TryGetSetting("UrlPhotoupAuthorize", _UrlPhotoupAuthorize);
            }
        }
        private const string _UrlPhotoupAuthorize = "http://localhost:{52099}/api/photoup/authorize";

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(_UrlApiA)]
        public string UrlApiA
        {
            set
            {
                SetSetting("UrlApiA", value);
            }
            get
            {
                return TryGetSetting("UrlApiA", _UrlApiA);
            }
        }
        private const string _UrlApiA = "http://localhost:{52099}/link/api";

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(_UrlApiB)]
        public string UrlApiB
        {
            set
            {
                SetSetting("UrlApiB", value);
            }
            get
            {
                return TryGetSetting("UrlApiB", _UrlApiB);
            }
        }
        private const string _UrlApiB = "http://localhost:{52099}/link/api";

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(_UrlIndex)]
        public string UrlIndex
        {
            set
            {
                SetSetting("UrlIndex", value);
            }
            get
            {
                return TryGetSetting("UrlIndex", _UrlIndex);
            }
        }
        private const string _UrlIndex = "http://localhost:{52099}/sp_ingame/link/index.html";

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(_UrlCampaign)]
        public string UrlCampaign
        {
            set
            {
                SetSetting("UrlCampaign", value);
            }
            get
            {
                return TryGetSetting("UrlCampaign", _UrlCampaign);
            }
        }
        private const string _UrlCampaign = "http://localhost:{52099}/sp_ingame/campaign/bnr/slide.html";

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(_UrlChargeB)]
        public string UrlChargeB
        {
            set
            {
                SetSetting("UrlChargeB", value);
            }
            get
            {
                return TryGetSetting("UrlChargeB", _UrlChargeB);
            }
        }
        private const string _UrlChargeB = "http://localhost:{52099}/sp_ingame/charge/";

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(_UrlCompanionImage)]
        public string UrlCompanionImage
        {
            set
            {
                SetSetting("UrlCompanionImage", value);
            }
            get
            {
                return TryGetSetting("UrlCompanionImage", _UrlCompanionImage);
            }
        }
        private const string _UrlCompanionImage = "http://localhost:{52099}/";
        
        /// <summary>
        /// How many pawns to consider for random sampling e.g. for clan hall pawns.
        /// Specifically this affects how many rows of the DB should be considered for randomization.
        /// 0 disables random pawns, which might cause undefined behavior, a minimum of 100 is advised.
        /// Avoid very large values like Integer.MAX_VALUE to not degrade performance.
        /// </summary>
        [DefaultValue(_RandomPawnMaxSample)]
        public uint RandomPawnMaxSample
        {
            set
            {
                SetSetting("RandomPawnMaxSample", value);
            }
            get
            {
                return TryGetSetting("RandomPawnMaxSample", _RandomPawnMaxSample);
            }
        }
        private const uint _RandomPawnMaxSample = 10000;

        /// <summary>
        /// The bonus for Job Training kills with a Partner Pawn present.
        /// Setting this to 0 effectively disables bonus kills with a Partner Pawn.
        /// </summary>
        [DefaultValue(_JobTrainingPartnerBonus)]
        public uint JobTrainingPartnerBonus
        {
            set
            {
                SetSetting("JobTrainingPartnerBonus", value);
            }
            get
            {
                return TryGetSetting("JobTrainingPartnerBonus", _JobTrainingPartnerBonus);
            }
        }
        private const uint _JobTrainingPartnerBonus = 1;
    }
}
