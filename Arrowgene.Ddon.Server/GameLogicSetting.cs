using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Arrowgene.Ddon.Server
{
    public class GameLogicSetting
    {
        /// <summary>
        /// Additional factor to change how long crafting a recipe will take to finish.
        /// </summary>
        public double AdditionalProductionSpeedFactor { get; set; }

        /// <summary>
        /// Additional factor to change how much a recipe will cost.
        /// </summary>
        public double AdditionalCostPerformanceFactor { get; set; }

        /// <summary>
        /// Sets the maximim level that the exp ring will reward a bonus.
        /// </summary>
        public uint RookiesRingMaxLevel { get; set; }

        /// <summary>
        /// The multiplier applied to the bonus amount of exp rewarded.
        /// Must be a non-negtive value. If it is less than 0.0, a default of 1.0
        /// will be selected.
        /// </summary>
        public double RookiesRingBonus { get; set; }

        /// <summary>
        /// Controls whether to pass lobby context packets on demand or only on entry to the server.
        /// True = Server entry only. Lower packet load, but also causes invisible people in lobbies.
        /// False = On-demand. May cause performance issues due to packet load.
        /// </summary>
        public bool NaiveLobbyContextHandling { get; set; }

        /// <summary>
        /// Determines the maximum amount of consumable items that can be crafted in one go with a pawn.
        /// The default is a value of 10 which is equivalent to the original game's behavior.
        /// </summary>
        public byte CraftConsumableProductionTimesMax { get; set; }

        /// <summary>
        /// Configures if party exp is adjusted based on level differences of members.
        /// </summary>
        public bool EnableAdjustPartyEnemyExp { get; set; }

        /// <summary>
        /// List of the inclusive ranges of (MinLv, Maxlv, ExpMultiplier). ExpMultiplier is a value
        /// from (0.0 - 1.0) which is multipled into the base exp amount to determine the adjusted exp.
        /// The minlv and maxlv determine the relative level range that this multiplier should be applied to.
        /// </summary>
        public List<(uint MinLv, uint MaxLv, double ExpMultiplier)> AdjustPartyEnemyExpTiers { get; set; }

        /// <summary>
        /// Configures if exp is adjusted based on level differences of members vs target level.
        /// </summary>
        public bool EnableAdjustTargetLvEnemyExp { get; set; }

        /// <summary>
        /// List of the inclusive ranges of (MinLv, Maxlv, ExpMultiplier). ExpMultiplier is a value from
        /// (0.0 - 1.0) which is multipled into the base exp amount to determine the adjusted exp.
        /// The minlv and maxlv determine the relative level range that this multiplier should be applied to.
        /// </summary>
        public List<(uint MinLv, uint MaxLv, double ExpMultiplier)> AdjustTargetLvEnemyExpTiers { get; set; }

        /// <summary>
        /// The number of real world minutes that make up an in-game day.
        /// </summary>
        public uint GameClockTimescale { get; set; }

        /// <summary>
        /// Use a poisson process to randomly generate a weather cycle containing this many events, using the statistics in WeatherStatistics.
        /// </summary>
        public uint WeatherSequenceLength { get; set; }

        /// <summary>
        /// Statistics that drive semirandom weather generation. List is expected to be in (Fair, Cloudy, Rainy) order.
        /// meanLength: Average length of the weather, in seconds, when it gets rolled.
        /// weight: Relative weight of rolling that weather. Set to 0 to disable.
        /// </summary>
        public List<(uint MeanLength, uint Weight)> WeatherStatistics { get; set; }

        /// <summary>
        /// Configures if the Pawn Exp Catchup mechanic is enabled. This mechanic still rewards the player pawn EXP when the pawn is outside
        /// the allowed level range and a lower level than the owner.
        /// </summary>
        public bool EnablePawnCatchup { get; set; }

        /// <summary>
        /// If the flag EnablePawnCatchup=true, this is the multiplier value used when calculating exp to catch the pawns level back up to the player.
        /// </summary>
        public double PawnCatchupMultiplier { get; set; }

        /// <summary>
        /// If the flag EnablePawnCatchup=true, this is the range of level that the pawn falls behind the player before the catchup mechanic kicks in.
        /// </summary>
        public uint PawnCatchupLvDiff { get; set; }

        /// <summary>
        /// Configures the default time in seconds a latern is active after igniting it.
        /// </summary>
        public uint LaternBurnTimeInSeconds { get; set; }

        /// <summary>
        /// Maximum amount of play points the client will display in the UI. 
        /// Play points past this point will also trigger a chat log message saying you've reached the cap.
        /// </summary>
        public uint PlayPointMax { get; set; }

        /// <summary>
        /// Maximum level for each job. 
        /// Shared with the login server.
        /// </summary>
        public uint JobLevelMax { get; set; }

        /// <summary>
        /// Maximum number of members in a single clan. 
        /// Shared with the login server.
        /// </summary>
        public uint ClanMemberMax { get; set; }

        /// <summary>
        /// Maximum number of characters per account. 
        /// Shared with the login server.
        /// </summary>
        public byte CharacterNumMax { get; set; }

        /// <summary>
        /// Toggles the visual equip set for all characters. 
        /// Shared with the login server.
        /// </summary>
        public bool EnableVisualEquip { get; set; }

        /// <summary>
        /// Maximum entries in the friends list. 
        /// Shared with the login server.
        /// </summary>
        public uint FriendListMax { get; set; }

        /// <summary>
        /// Limits for each wallet type.
        /// </summary>
        public Dictionary<WalletType, uint> WalletLimits { get; set; }

        /// <summary>
        /// Number of bazaar entries that are given to new characters.
        /// </summary>
        public uint DefaultMaxBazaarExhibits { get; set; }

        /// <summary>
        /// Number of favorite warps that are given to new characters.
        /// </summary>
        public uint DefaultWarpFavorites { get; set; }

        /// <summary>
        /// Disables the exp correction if all party members are owned by the same character.
        /// </summary>
        public bool DisableExpCorrectionForMyPawn { get; set; }

        /// <summary>
        /// Global modifier for enemy exp calculations to scale up or down.
        /// </summary>
        public double EnemyExpModifier { get; set; }

        /// <summary>
        /// Global modifier for quest exp calculations to scale up or down.
        /// </summary>
        public double QuestExpModifier { get; set; }

        /// <summary>
        /// Global modifier for pp calculations to scale up or down.
        /// </summary>
        public double PpModifier { get; set; }

        /// <summary>
        /// Global modifier for Gold calculations to scale up or down.
        /// </summary>
        public double GoldModifier { get; set; }

        /// <summary>
        /// Global modifier for Rift calculations to scale up or down.
        /// </summary>
        public double RiftModifier { get; set; }

        /// <summary>
        /// Global modifier for BO calculations to scale up or down.
        /// </summary>
        public double BoModifier { get; set; }

        /// <summary>
        /// Global modifier for HO calculations to scale up or down.
        /// </summary>
        public double HoModifier { get; set; }

        /// <summary>
        /// Global modifier for JP calculations to scale up or down.
        /// </summary>
        public double JpModifier { get; set; }

        /// <summary>
        /// Configures the maximum amount of reward box slots.
        /// </summary>
        public byte RewardBoxMax { get; set; }

        /// <summary>
        /// Configures the maximum amount of quests that can be ordered at one time.
        /// </summary>
        public byte QuestOrderMax { get; set; }

        /// <summary>
        /// Configures if epitaph rewards are limited once per weekly reset.
        /// </summary>
        public bool EnableEpitaphWeeklyRewards { get; set; }

        /// <summary>
        /// Enables main pawns in party to gain EXP and JP from quests
        /// Original game apparantly did not have pawns share quest reward, so will set to false for default, 
        /// change as needed
        /// </summary>
        public bool EnableMainPartyPawnsQuestRewards { get; set; }

        /// <summary>
        /// Specifies the time in seconds that a bazaar exhibit will last.
        /// By default, the equivalent of 3 days
        /// </summary>
        public ulong BazaarExhibitionTimeSeconds { get; set; }

        /// <summary>
        /// Specifies the time in seconds that a slot in the bazaar won't be able to be used again.
        /// By default, the equivalent of 1 day
        /// </summary>
        public ulong BazaarCooldownTimeSeconds { get; set; }

        /// <summary>
        /// Various URLs used by the client.
        /// Shared with the login server.
        /// </summary>
        public string UrlManual { get; set; }
        public string UrlShopDetail { get; set; }
        public string UrlShopCounterA { get; set; }
        public string UrlShopAttention { get; set; }
        public string UrlShopStoneLimit { get; set; }
        public string UrlShopCounterB { get; set; }
        public string UrlChargeCallback { get; set; }
        public string UrlChargeA { get; set; }
        public string UrlSample9 { get; set; }
        public string UrlSample10 { get; set; }
        public string UrlCampaignBanner { get; set; }
        public string UrlSupportIndex { get; set; }
        public string UrlPhotoupAuthorize { get; set; }
        public string UrlApiA { get; set; }
        public string UrlApiB { get; set; }
        public string UrlIndex { get; set; }
        public string UrlCampaign { get; set; }
        public string UrlChargeB { get; set; }
        public string UrlCompanionImage { get; set; }

        public GameLogicSetting()
        {
        }

        void ValidateSettings()
        {
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
        }
    }
}
