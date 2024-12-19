using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using YamlDotNet.Serialization;

namespace Arrowgene.Ddon.Server
{
    [DataContract]
    public class DefaultDataMember<T>
    {
        private T _DefaultValue;
        private T? _Value;

        [DataMember]
        public T Value { 
            get => _Value ?? _DefaultValue;
            set => _Value = Value;
        }

        public DefaultDataMember(T defaultValue)
        {
            _DefaultValue = defaultValue;
        }
    }


    [DataContract]
    public class GameLogicSetting
    {
        /// <summary>
        /// Additional factor to change how long crafting a recipe will take to finish.
        /// </summary>
        [DataMember(Order = 0)]
        public double AdditionalProductionSpeedFactor { get; set; }

        /// <summary>
        /// Additional factor to change how much a recipe will cost.
        /// </summary>
        [DataMember(Order = 1)]
        public double AdditionalCostPerformanceFactor { get; set; }

        /// <summary>
        /// Sets the maximim level that the exp ring will reward a bonus.
        /// </summary>
        [DataMember(Order = 2)]
        public uint RookiesRingMaxLevel { get; set; }

        /// <summary>
        /// The multiplier applied to the bonus amount of exp rewarded.
        /// Must be a non-negtive value. If it is less than 0.0, a default of 1.0
        /// will be selected.
        /// </summary>
        [DataMember(Order = 3)]
        public double RookiesRingBonus { get; set; }

        /// <summary>
        /// Controls whether to pass lobby context packets on demand or only on entry to the server.
        /// True = Server entry only. Lower packet load, but also causes invisible people in lobbies.
        /// False = On-demand. May cause performance issues due to packet load.
        /// </summary>
        [DataMember(Order = 4)]
        public bool NaiveLobbyContextHandling { get; set; }

        /// <summary>
        /// Determines the maximum amount of consumable items that can be crafted in one go with a pawn.
        /// The default is a value of 10 which is equivalent to the original game's behavior.
        /// </summary>
        [DataMember(Order = 5)] public byte CraftConsumableProductionTimesMax { get; set; }

        /// <summary>
        /// Configures if party exp is adjusted based on level differences of members.
        /// </summary>
        [DataMember(Order = 6)] public bool AdjustPartyEnemyExp { get; set; }

        /// <summary>
        /// List of the inclusive ranges of (MinLv, Maxlv, ExpMultiplier). ExpMultiplier is a value
        /// from (0.0 - 1.0) which is multipled into the base exp amount to determine the adjusted exp.
        /// The minlv and maxlv determine the relative level range that this multiplier should be applied to.
        /// </summary>
        [DataMember(Order = 7)] public List<(uint MinLv, uint MaxLv, double ExpMultiplier)> AdjustPartyEnemyExpTiers { get; set; }

        /// <summary>
        /// Configures if exp is adjusted based on level differences of members vs target level.
        /// </summary>
        [DataMember(Order = 8)] public bool AdjustTargetLvEnemyExp { get; set; }

        /// <summary>
        /// List of the inclusive ranges of (MinLv, Maxlv, ExpMultiplier). ExpMultiplier is a value from
        /// (0.0 - 1.0) which is multipled into the base exp amount to determine the adjusted exp.
        /// The minlv and maxlv determine the relative level range that this multiplier should be applied to.
        /// </summary>
        [DataMember(Order = 9)] public List<(uint MinLv, uint MaxLv, double ExpMultiplier)> AdjustTargetLvEnemyExpTiers { get; set; }

        /// <summary>
        /// The number of real world minutes that make up an in-game day.
        /// </summary>
        [DataMember(Order = 10)] public uint GameClockTimescale { get; set; }

        /// <summary>
        /// Use a poisson process to randomly generate a weather cycle containing this many events, using the statistics in WeatherStatistics.
        /// </summary>
        [DataMember(Order = 11)] public uint WeatherSequenceLength { get; set; }

        /// <summary>
        /// Statistics that drive semirandom weather generation. List is expected to be in (Fair, Cloudy, Rainy) order.
        /// meanLength: Average length of the weather, in seconds, when it gets rolled.
        /// weight: Relative weight of rolling that weather. Set to 0 to disable.
        /// </summary>
        [DataMember(Order = 12)] public List<(uint MeanLength, uint Weight)> WeatherStatistics { get; set; }

        /// <summary>
        /// Configures if the Pawn Exp Catchup mechanic is enabled. This mechanic still rewards the player pawn EXP when the pawn is outside
        /// the allowed level range and a lower level than the owner.
        /// </summary>
        [DataMember(Order = 13)] public bool EnablePawnCatchup { get; set; }

        /// <summary>
        /// If the flag EnablePawnCatchup=true, this is the multiplier value used when calculating exp to catch the pawns level back up to the player.
        /// </summary>
        [DataMember(Order = 14)] public double PawnCatchupMultiplier { get; set; }

        /// <summary>
        /// If the flag EnablePawnCatchup=true, this is the range of level that the pawn falls behind the player before the catchup mechanic kicks in.
        /// </summary>
        [DataMember(Order = 15)] public uint PawnCatchupLvDiff { get; set; }

        /// <summary>
        /// Configures the default time in seconds a latern is active after igniting it.
        /// </summary>
        [DataMember(Order = 16)] public uint LaternBurnTimeInSeconds { get; set; }

        /// <summary>
        /// Maximum amount of play points the client will display in the UI. 
        /// Play points past this point will also trigger a chat log message saying you've reached the cap.
        /// </summary>
        [DataMember(Order = 17)] public uint PlayPointMax { get; set; }

        /// <summary>
        /// Maximum level for each job. 
        /// Shared with the login server.
        /// </summary>
        [DataMember(Order = 18)] public uint JobLevelMax { get; set; }

        /// <summary>
        /// Maximum number of members in a single clan. 
        /// Shared with the login server.
        /// </summary>
        [DataMember(Order = 19)] public uint ClanMemberMax { get; set; }

        /// <summary>
        /// Maximum number of characters per account. 
        /// Shared with the login server.
        /// </summary>
        [DataMember(Order = 20)] public byte CharacterNumMax { get; set; }

        /// <summary>
        /// Toggles the visual equip set for all characters. 
        /// Shared with the login server.
        /// </summary>
        [DataMember(Order = 21)] public bool EnableVisualEquip { get; set; }

        /// <summary>
        /// Maximum entries in the friends list. 
        /// Shared with the login server.
        /// </summary>
        [DataMember(Order = 22)] public uint FriendListMax { get; set; }

        /// <summary>
        /// Limits for each wallet type.
        /// </summary>
        [DataMember(Order = 23)] public Dictionary<WalletType, uint> WalletLimits { get; set; }

        /// <summary>
        /// Number of bazaar entries that are given to new characters.
        /// </summary>
        [DataMember(Order = 24)] public uint DefaultMaxBazaarExhibits { get; set; }

        /// <summary>
        /// Number of favorite warps that are given to new characters.
        /// </summary>
        [DataMember(Order = 25)] public uint DefaultWarpFavorites { get; set; }

        /// <summary>
        /// Disables the exp correction if all party members are owned by the same character.
        /// </summary>
        [DataMember(Order = 26)] public bool DisableExpCorrectionForMyPawn { get; set; }

        /// <summary>
        /// Global modifier for enemy exp calculations to scale up or down.
        /// </summary>
        [DataMember(Order = 27)] public double? EnemyExpModifier { get; set; } = 1.0;

        /// <summary>
        /// Global modifier for quest exp calculations to scale up or down.
        /// </summary>
        
        [DataMember(Order = 28)] public double? QuestExpModifier { get; set; } = 1.0;

        /// <summary>
        /// Global modifier for pp calculations to scale up or down.
        /// </summary>
        [DataMember(Order = 29)] public double? PpModifier { get; set; } = 1.0;

        /// <summary>
        /// Global modifier for Gold calculations to scale up or down.
        /// </summary>
        [DataMember(Order = 30)] public double? GoldModifier { get; set; } = 1.0;

        /// <summary>
        /// Global modifier for Rift calculations to scale up or down.
        /// </summary>
        [DataMember(Order = 31)] public double? RiftModifier { get; set; } = 1.0;

        /// <summary>
        /// Global modifier for BO calculations to scale up or down.
        /// </summary>
        [DataMember(Order = 32)] public double? BoModifier { get; set; } = 1.0;

        /// <summary>
        /// Global modifier for HO calculations to scale up or down.
        /// </summary>
        [DataMember(Order = 33)] public double? HoModifier { get; set; } = 1.0;

        /// <summary>
        /// Global modifier for JP calculations to scale up or down.
        /// </summary>
        [DataMember(Order = 34)] public double? JpModifier { get; set; } = 1.0;

        /// <summary>
        /// Configures the maximum amount of reward box slots.
        /// </summary>
        [DataMember(Order = 35)] public byte? RewardBoxMax { get; set; } = 100;

        /// <summary>
        /// Configures the maximum amount of quests that can be ordered at one time.
        /// </summary>
        [DataMember(Order = 36)] public byte? QuestOrderMax { get; set; } = 20;

        /// <summary>
        /// Configures if epitaph rewards are limited once per weekly reset.
        /// </summary>
        [DataMember(Order = 37)] public bool? EnableEpitaphWeeklyRewards { get; set; } = true;

        /// <summary>
        /// Enables main pawns in party to gain EXP and JP from quests
        /// Original game apparantly did not have pawns share quest reward, so will set to false for default, 
        /// change as needed
        /// </summary>
        [DataMember(Order = 38)] public bool EnableMainPartyPawnsQuestRewards { get; set; }

        /// <summary>
        /// Specifies the time in seconds that a bazaar exhibit will last.
        /// By default, the equivalent of 3 days
        /// </summary>
        [DataMember(Order = 37)] public ulong BazaarExhibitionTimeSeconds { get; set; }

        /// <summary>
        /// Specifies the time in seconds that a slot in the bazaar won't be able to be used again.
        /// By default, the equivalent of 1 day
        /// </summary>
        [DataMember(Order = 38)] public ulong BazaarCooldownTimeSeconds { get; set; }

        /// <summary>
        /// Various URLs used by the client.
        /// Shared with the login server.
        /// </summary>
        [DataMember(Order = 200)] public string UrlManual { get; set; }
        [DataMember(Order = 200)] public string UrlShopDetail { get; set; }
        [DataMember(Order = 200)] public string UrlShopCounterA { get; set; }
        [DataMember(Order = 200)] public string UrlShopAttention { get; set; }
        [DataMember(Order = 200)] public string UrlShopStoneLimit { get; set; }
        [DataMember(Order = 200)] public string UrlShopCounterB { get; set; }
        [DataMember(Order = 200)] public string UrlChargeCallback { get; set; }
        [DataMember(Order = 200)] public string UrlChargeA { get; set; }
        [DataMember(Order = 200)] public string UrlSample9 { get; set; }
        [DataMember(Order = 200)] public string UrlSample10 { get; set; }
        [DataMember(Order = 200)] public string UrlCampaignBanner { get; set; }
        [DataMember(Order = 200)] public string UrlSupportIndex { get; set; }
        [DataMember(Order = 200)] public string UrlPhotoupAuthorize { get; set; }
        [DataMember(Order = 200)] public string UrlApiA { get; set; }
        [DataMember(Order = 200)] public string UrlApiB { get; set; }
        [DataMember(Order = 200)] public string UrlIndex { get; set; }
        [DataMember(Order = 200)] public string UrlCampaign { get; set; }
        [DataMember(Order = 200)] public string UrlChargeB { get; set; }
        [DataMember(Order = 200)] public string UrlCompanionImage { get; set; }

        public GameLogicSetting()
        {
            LaternBurnTimeInSeconds = 1500;
            AdditionalProductionSpeedFactor = 1.0;
            AdditionalCostPerformanceFactor = 1.0;
            RookiesRingMaxLevel = 89;
            RookiesRingBonus = 1.0;
            NaiveLobbyContextHandling = true;
            CraftConsumableProductionTimesMax = 10;

            AdjustPartyEnemyExp = true;
            AdjustPartyEnemyExpTiers = new List<(uint MinLv, uint MaxLv, double ExpMultiplier)>()
            {
                (0, 2, 1.0),
                (3, 4, 0.9),
                (5, 6, 0.8),
                (7, 8, 0.6),
                (9, 10, 0.5),
            };

            AdjustTargetLvEnemyExp = false;
            AdjustTargetLvEnemyExpTiers = new List<(uint MinLv, uint MaxLv, double ExpMultiplier)>()
            {
                (0, 2, 1.0),
                (3, 4, 0.9),
                (5, 6, 0.8),
                (7, 8, 0.6),
                (9, 10, 0.5),
            };

            EnablePawnCatchup = true;
            PawnCatchupMultiplier = 1.5;
            PawnCatchupLvDiff = 5;

            DisableExpCorrectionForMyPawn = true;

            GameClockTimescale = 90;

            WeatherSequenceLength = 20;
            WeatherStatistics = new List<(uint MeanLength, uint Weight)>
            {
                (60 * 30, 1), //Fair
                (60 * 30, 1), //Cloudy
                (60 * 30, 1), //Rainy
            };

            PlayPointMax = 2000;

            JobLevelMax = 120;
            ClanMemberMax = 100;
            CharacterNumMax = 4;
            EnableVisualEquip = true;
            FriendListMax = 200;

            WalletLimits = DefaultWalletLimits;

            DefaultMaxBazaarExhibits = 5;
            DefaultWarpFavorites = 3;

            EnableEpitaphWeeklyRewards = false;
            EnableMainPartyPawnsQuestRewards = false;

            BazaarExhibitionTimeSeconds = (ulong) TimeSpan.FromDays(3).TotalSeconds;
            BazaarCooldownTimeSeconds = (ulong) TimeSpan.FromDays(1).TotalSeconds;

            string urlDomain = $"http://localhost:{52099}";
            UrlManual = $"{urlDomain}/manual_nfb/";
            UrlShopDetail = $"{urlDomain}/shop/ingame/stone/detail";
            UrlShopCounterA = $"{urlDomain}/shop/ingame/counter?";
            UrlShopAttention = $"{urlDomain}/shop/ingame/attention?";
            UrlShopStoneLimit = $"{urlDomain}/shop/ingame/stone/limit";
            UrlShopCounterB = $"{urlDomain}/shop/ingame/counter?";
            UrlChargeCallback = $"{urlDomain}/opening/entry/ddo/cog_callback/charge";
            UrlChargeA = $"{urlDomain}/sp_ingame/charge/";
            UrlSample9 = "http://sample09.html";
            UrlSample10 = "http://sample10.html";
            UrlCampaignBanner = $"{urlDomain}/sp_ingame/campaign/bnr/bnr01.html?";
            UrlSupportIndex = $"{urlDomain}/sp_ingame/support/index.html";
            UrlPhotoupAuthorize = $"{urlDomain}/api/photoup/authorize";
            UrlApiA = $"{urlDomain}/link/api";
            UrlApiB = $"{urlDomain}/link/api";
            UrlIndex = $"{urlDomain}/sp_ingame/link/index.html";
            UrlCampaign = $"{urlDomain}/sp_ingame/campaign/bnr/slide.html";
            UrlChargeB = $"{urlDomain}/sp_ingame/charge/";
            UrlCompanionImage = $"{urlDomain}/";
        }

        public GameLogicSetting(GameLogicSetting setting)
        {
            LaternBurnTimeInSeconds = setting.LaternBurnTimeInSeconds;
            AdditionalProductionSpeedFactor = setting.AdditionalProductionSpeedFactor;
            AdditionalCostPerformanceFactor = setting.AdditionalCostPerformanceFactor;
            RookiesRingMaxLevel = setting.RookiesRingMaxLevel;
            RookiesRingBonus = setting.RookiesRingBonus;
            NaiveLobbyContextHandling = setting.NaiveLobbyContextHandling;
            CraftConsumableProductionTimesMax = setting.CraftConsumableProductionTimesMax;
            AdjustPartyEnemyExp = setting.AdjustPartyEnemyExp;
            AdjustPartyEnemyExpTiers = setting.AdjustPartyEnemyExpTiers;
            AdjustTargetLvEnemyExp = setting.AdjustTargetLvEnemyExp;
            AdjustTargetLvEnemyExpTiers = setting.AdjustTargetLvEnemyExpTiers;
            GameClockTimescale = setting.GameClockTimescale;
            WeatherSequenceLength = setting.WeatherSequenceLength;
            WeatherStatistics = setting.WeatherStatistics;
            EnablePawnCatchup = setting.EnablePawnCatchup;
            PawnCatchupMultiplier = setting.PawnCatchupMultiplier;
            PawnCatchupLvDiff = setting.PawnCatchupLvDiff;
            DisableExpCorrectionForMyPawn = setting.DisableExpCorrectionForMyPawn;
            PlayPointMax = setting.PlayPointMax;
            JobLevelMax = setting.JobLevelMax;
            ClanMemberMax = setting.ClanMemberMax;
            CharacterNumMax = setting.CharacterNumMax;
            EnableVisualEquip = setting.EnableVisualEquip;
            FriendListMax = setting.FriendListMax;
            WalletLimits = setting.WalletLimits;
            DefaultMaxBazaarExhibits = setting.DefaultMaxBazaarExhibits;
            DefaultWarpFavorites = setting.DefaultWarpFavorites;

            EnemyExpModifier = setting.EnemyExpModifier;
            QuestExpModifier = setting.QuestExpModifier;
            PpModifier = setting.PpModifier;
            GoldModifier = setting.GoldModifier;
            RiftModifier = setting.RiftModifier;
            BoModifier = setting.BoModifier;
            HoModifier = setting.HoModifier;
            JpModifier = setting.JpModifier;
            RewardBoxMax = setting.RewardBoxMax;
            QuestOrderMax = setting.QuestOrderMax;

            EnableEpitaphWeeklyRewards = setting.EnableEpitaphWeeklyRewards;
            EnableMainPartyPawnsQuestRewards = setting.EnableMainPartyPawnsQuestRewards;

            BazaarExhibitionTimeSeconds = setting.BazaarExhibitionTimeSeconds;
            BazaarCooldownTimeSeconds = setting.BazaarCooldownTimeSeconds;

            UrlManual = setting.UrlManual;
            UrlShopDetail = setting.UrlShopDetail;
            UrlShopCounterA = setting.UrlShopCounterA;
            UrlShopAttention = setting.UrlShopAttention;
            UrlShopStoneLimit = setting.UrlShopStoneLimit;
            UrlShopCounterB = setting.UrlShopCounterB;
            UrlChargeCallback = setting.UrlChargeCallback;
            UrlChargeA = setting.UrlChargeA;
            UrlSample9 = setting.UrlSample9;
            UrlSample10 = setting.UrlSample10;
            UrlCampaignBanner = setting.UrlCampaignBanner;
            UrlSupportIndex = setting.UrlSupportIndex;
            UrlPhotoupAuthorize = setting.UrlPhotoupAuthorize;
            UrlApiA = setting.UrlApiA;
            UrlApiB = setting.UrlApiB;
            UrlIndex = setting.UrlIndex;
            UrlCampaign = setting.UrlCampaign;
            UrlChargeB = setting.UrlChargeB;
            UrlCompanionImage = setting.UrlCompanionImage;
        }

        // Note: method is called after the object is completely deserialized - constructors are skipped.
        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
        {
            // Initialize reference types so tests work properly.
            AdjustPartyEnemyExpTiers ??= new();
            AdjustTargetLvEnemyExpTiers ??= new();
            WeatherStatistics ??= new();
            WalletLimits ??= new();
            UrlManual ??= string.Empty;
            UrlShopDetail ??= string.Empty;
            UrlShopCounterA ??= string.Empty;
            UrlShopAttention ??= string.Empty;
            UrlShopStoneLimit ??= string.Empty;
            UrlShopCounterB ??= string.Empty;
            UrlChargeCallback ??= string.Empty;
            UrlChargeA ??= string.Empty;
            UrlSample9 ??= string.Empty;
            UrlSample10 ??= string.Empty;
            UrlCampaignBanner ??= string.Empty;
            UrlSupportIndex ??= string.Empty;
            UrlPhotoupAuthorize ??= string.Empty;
            UrlApiA ??= string.Empty;
            UrlApiB ??= string.Empty;
            UrlIndex ??= string.Empty;
            UrlCampaign ??= string.Empty;
            UrlChargeB ??= string.Empty;
            UrlCompanionImage ??= string.Empty;

            EnableEpitaphWeeklyRewards ??= true;

            EnemyExpModifier ??= 1;
            QuestExpModifier ??= 1;
            PpModifier ??= 1;
            GoldModifier ??= 1;
            RiftModifier ??= 1;
            BoModifier ??= 1;
            HoModifier ??= 1;
            JpModifier ??= 1;
            RewardBoxMax ??= 100;
            QuestOrderMax ??= 20;

            if (RookiesRingBonus < 0)
            {
                RookiesRingBonus = 1.0;
            }
            if (AdditionalProductionSpeedFactor < 0)
            {
                CraftConsumableProductionTimesMax = 1;
            }
            if (AdditionalCostPerformanceFactor < 0)
            {
                CraftConsumableProductionTimesMax = 1;
            }
            if (CraftConsumableProductionTimesMax < 1)
            {
                CraftConsumableProductionTimesMax = 10;
            }
            if (GameClockTimescale <= 0)
            {
                GameClockTimescale = 90;
            }
            if (PawnCatchupMultiplier < 0)
            {
                PawnCatchupMultiplier = 1.0;
            }
            if (EnemyExpModifier < 0)
            {
                EnemyExpModifier = 1.0;
            }
            if (QuestExpModifier < 0)
            {
                QuestExpModifier = 1.0;
            }
            if (PpModifier < 0)
            {
                PpModifier = 1.0;
            }
            if (GoldModifier < 0)
            {
                GoldModifier = 1.0;
            }
            if (RiftModifier < 0)
            {
                RiftModifier = 1.0;
            }
            if (BoModifier < 0)
            {
                BoModifier = 1.0;
            }
            if (HoModifier < 0)
            {
                HoModifier = 1.0;
            }
            if (JpModifier < 0)
            {
                JpModifier = 1.0;
            }
       
            foreach (var walletMax in DefaultWalletLimits)
            {
                if (!WalletLimits.ContainsKey(walletMax.Key))
                {
                    WalletLimits.Add(walletMax.Key, walletMax.Value);
                }
            }
        }

        private static readonly Dictionary<WalletType, uint> DefaultWalletLimits = new()
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
            {WalletType.UnknownTickets, 999999999},
            {WalletType.BitterblackMazeResetTicket, 3},
            {WalletType.GoldenDragonMark, 30},
            {WalletType.SilverDragonMark, 150},
            {WalletType.RedDragonMark, 99999},
        };
    }
}
