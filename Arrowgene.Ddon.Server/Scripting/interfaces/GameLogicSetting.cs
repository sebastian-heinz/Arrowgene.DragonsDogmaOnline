using Arrowgene.Ddon.Server.Scripting.utils;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Server.Scripting.interfaces
{
    public class GameLogicSetting
    {
        private ScriptableSettings SettingsData { get; set; }
        public GameLogicSetting(ScriptableSettings settingsData)
        {
            SettingsData = settingsData;
        }

        private T GetSetting<T>(string key)
        {
            return SettingsData.Get<T>("GameLogicSettings", key);
        }

        public T Get<T>(string scriptName, string key)
        {
            return SettingsData.Get<T>(scriptName, key);
        }

        /// <summary>
        /// Additional factor to change how long crafting a recipe will take to finish.
        /// </summary>
        public double AdditionalProductionSpeedFactor
        { 
            get
            {
                return GetSetting<double>("AdditionalProductionSpeedFactor");
            }
        }

        /// <summary>
        /// Additional factor to change how much a recipe will cost.
        /// </summary>
        public double AdditionalCostPerformanceFactor
        { 
            get
            {
                return GetSetting<double>("AdditionalCostPerformanceFactor");
            }
        }

        /// <summary>
        /// Controls whether to pass lobby context packets on demand or only on entry to the server.
        /// True = Server entry only. Lower packet load, but also causes invisible people in lobbies.
        /// False = On-demand. May cause performance issues due to packet load.
        /// </summary>
        public bool NaiveLobbyContextHandling 
        { 
            get
            {
                return GetSetting<bool>("NaiveLobbyContextHandling");
            }
        }

        /// <summary>
        /// Determines the maximum amount of consumable items that can be crafted in one go with a pawn.
        /// The default is a value of 10 which is equivalent to the original game's behavior.
        /// </summary>
        public byte CraftConsumableProductionTimesMax 
        { 
            get
            {
                return GetSetting<byte>("CraftConsumableProductionTimesMax");
            }
        }

        /// <summary>
        /// Configures if party exp is adjusted based on level differences of members.
        /// </summary>
        public bool EnableAdjustPartyEnemyExp
        {
            get
            {
                return GetSetting<bool>("EnableAdjustPartyEnemyExp");
            }
        }

        /// <summary>
        /// List of the inclusive ranges of (MinLv, Maxlv, ExpMultiplier). ExpMultiplier is a value
        /// from (0.0 - 1.0) which is multipled into the base exp amount to determine the adjusted exp.
        /// The minlv and maxlv determine the relative level range that this multiplier should be applied to.
        /// </summary>
        public List<(uint MinLv, uint MaxLv, double ExpMultiplier)> AdjustPartyEnemyExpTiers
        {
            get
            {
                return GetSetting<List<(uint MinLv, uint MaxLv, double ExpMultiplier)>>("AdjustPartyEnemyExpTiers");
            }
        }

        /// <summary>
        /// Configures if exp is adjusted based on level differences of members vs target level.
        /// </summary>
        public bool EnableAdjustTargetLvEnemyExp
        {
            get
            {
                return GetSetting<bool>("EnableAdjustTargetLvEnemyExp");
            }
        }

        /// <summary>
        /// List of the inclusive ranges of (MinLv, Maxlv, ExpMultiplier). ExpMultiplier is a value from
        /// (0.0 - 1.0) which is multipled into the base exp amount to determine the adjusted exp.
        /// The minlv and maxlv determine the relative level range that this multiplier should be applied to.
        /// </summary>
        public List<(uint MinLv, uint MaxLv, double ExpMultiplier)> AdjustTargetLvEnemyExpTiers
        {
            get
            {
                return GetSetting<List<(uint MinLv, uint MaxLv, double ExpMultiplier)>>("AdjustTargetLvEnemyExpTiers");
            }
        }

        /// <summary>
        /// The number of real world minutes that make up an in-game day.
        /// </summary>
        public uint GameClockTimescale
        {
            get
            {
                return GetSetting<uint>("GameClockTimescale");
            }
        }

        /// <summary>
        /// Use a poisson process to randomly generate a weather cycle containing this many events, using the statistics in WeatherStatistics.
        /// </summary>
        public uint WeatherSequenceLength
        {
            get
            {
                return GetSetting<uint>("WeatherSequenceLength");
            }
        }

        /// <summary>
        /// Statistics that drive semirandom weather generation. List is expected to be in (Fair, Cloudy, Rainy) order.
        /// meanLength: Average length of the weather, in seconds, when it gets rolled.
        /// weight: Relative weight of rolling that weather. Set to 0 to disable.
        /// </summary>
        public List<(uint MeanLength, uint Weight)> WeatherStatistics
        {
            get
            {
                return GetSetting<List<(uint MeanLength, uint Weight)>>("WeatherStatistics");
            }
        }

        /// <summary>
        /// Configures if the Pawn Exp Catchup mechanic is enabled. This mechanic still rewards the player pawn EXP when the pawn is outside
        /// the allowed level range and a lower level than the owner.
        /// </summary>
        public bool EnablePawnCatchup
        {
            get
            {
                return GetSetting<bool>("EnablePawnCatchup");
            }
        }

        /// <summary>
        /// If the flag EnablePawnCatchup=true, this is the multiplier value used when calculating exp to catch the pawns level back up to the player.
        /// </summary>
        public double PawnCatchupMultiplier
        {
            get
            {
                return GetSetting<double>("PawnCatchupMultiplier");
            }
        }

        /// <summary>
        /// If the flag EnablePawnCatchup=true, this is the range of level that the pawn falls behind the player before the catchup mechanic kicks in.
        /// </summary>
        public uint PawnCatchupLvDiff
        {
            get
            {
                return GetSetting<uint>("PawnCatchupLvDiff");
            }
        }

        /// <summary>
        /// Configures the default time in seconds a latern is active after igniting it.
        /// </summary>
        public uint LaternBurnTimeInSeconds
        {
            get
            {
                return GetSetting<uint>("LaternBurnTimeInSeconds");
            }
        }

        /// <summary>
        /// Maximum amount of play points the client will display in the UI. 
        /// Play points past this point will also trigger a chat log message saying you've reached the cap.
        /// </summary>
        public uint PlayPointMax
        {
            get
            {
                return GetSetting<uint>("PlayPointMax");
            }
        }

        /// <summary>
        /// Maximum level for each job. 
        /// Shared with the login server.
        /// </summary>
        public uint JobLevelMax
        {
            get
            {
                return GetSetting<uint>("JobLevelMax");
            }
        }

        /// <summary>
        /// Maximum number of members in a single clan. 
        /// Shared with the login server.
        /// </summary>
        public uint ClanMemberMax
        {
            get
            {
                return GetSetting<uint>("ClanMemberMax");
            }
        }

        /// <summary>
        /// Maximum number of characters per account. 
        /// Shared with the login server.
        /// </summary>
        public byte CharacterNumMax
        {
            get
            {
                return GetSetting<byte>("CharacterNumMax");
            }
        }

        /// <summary>
        /// Toggles the visual equip set for all characters. 
        /// Shared with the login server.
        /// </summary>
        public bool EnableVisualEquip
        {
            get
            {
                return GetSetting<bool>("EnableVisualEquip");
            }
        }

        /// <summary>
        /// Maximum entries in the friends list. 
        /// Shared with the login server.
        /// </summary>
        public uint FriendListMax
        {
            get
            {
                return GetSetting<uint>("FriendListMax");
            }
        }

        /// <summary>
        /// Limits for each wallet type.
        /// </summary>
        public Dictionary<WalletType, uint> WalletLimits
        {
            get
            {
                return GetSetting<Dictionary<WalletType, uint>>("WalletLimits");
            }
        }

        /// <summary>
        /// Number of bazaar entries that are given to new characters.
        /// </summary>
        public uint DefaultMaxBazaarExhibits
        {
            get
            {
                return GetSetting<uint>("DefaultMaxBazaarExhibits");
            }
        }

        /// <summary>
        /// Number of favorite warps that are given to new characters.
        /// </summary>
        public uint DefaultWarpFavorites
        {
            get
            {
                return GetSetting<uint>("DefaultWarpFavorites");
            }
        }

        /// <summary>
        /// Disables the exp correction if all party members are owned by the same character.
        /// </summary>
        public bool DisableExpCorrectionForMyPawn
        {
            get
            {
                return GetSetting<bool>("DisableExpCorrectionForMyPawn");
            }
        }

        /// <summary>
        /// Global modifier for enemy exp calculations to scale up or down (does not apply to Bitterblack Maze).
        /// </summary>
        public double EnemyExpModifier
        {
            get
            {
                return GetSetting<double>("EnemyExpModifier");
            }
        }

        /// <summary>
        /// Global modifier for BBM enemy exp calculations to scale up or down.
        /// </summary>
        public double BBMEnemyExpModifier
        {
            get
            {
                return GetSetting<double>("BBMEnemyExpModifier");
            }
        }

        /// <summary>
        /// Global modifier for quest exp calculations to scale up or down.
        /// </summary>
        public double QuestExpModifier
        {
            get
            {
                return GetSetting<double>("QuestExpModifier");
            }
        }

        /// <summary>
        /// Global modifier for pp calculations to scale up or down.
        /// </summary>
        public double PpModifier
        {
            get
            {
                return GetSetting<double>("PpModifier");
            }
        }

        /// <summary>
        /// Global modifier for Gold calculations to scale up or down.
        /// </summary>
        public double GoldModifier
        {
            get
            {
                return GetSetting<double>("GoldModifier");
            }
        }

        /// <summary>
        /// Global modifier for Rift calculations to scale up or down.
        /// </summary>
        public double RiftModifier
        {
            get
            {
                return GetSetting<double>("RiftModifier");
            }
        }

        /// <summary>
        /// Global modifier for BO calculations to scale up or down.
        /// </summary>
        public double BoModifier
        {
            get
            {
                return GetSetting<double>("BoModifier");
            }
        }

        /// <summary>
        /// Global modifier for HO calculations to scale up or down.
        /// </summary>
        public double HoModifier
        {
            get
            {
                return GetSetting<double>("HoModifier");
            }
        }

        /// <summary>
        /// Global modifier for JP calculations to scale up or down.
        /// </summary>
        public double JpModifier
        {
            get
            {
                return GetSetting<double>("JpModifier");
            }
        }

        /// <summary>
        /// Global modifier for AP calculations to scale up or down.
        /// </summary>
        public double ApModifier
        {
            get
            {
                return GetSetting<double>("ApModifier");
            }
        }

        /// <summary>
        /// Configures the maximum amount of reward box slots.
        /// </summary>
        public byte RewardBoxMax
        {
            get
            {
                return GetSetting<byte>("RewardBoxMax");
            }
        }

        /// <summary>
        /// Configures the maximum amount of quests that can be ordered at one time.
        /// </summary>
        public byte QuestOrderMax
        {
            get
            {
                return GetSetting<byte>("QuestOrderMax");
            }
        }

        /// <summary>
        /// Configures if epitaph rewards are limited once per weekly reset.
        /// </summary>
        public bool EnableEpitaphWeeklyRewards
        {
            get
            {
                return GetSetting<bool>("EnableEpitaphWeeklyRewards");
            }
        }

        /// <summary>
        /// Enables main pawns in party to gain EXP and JP from quests
        /// Original game apparantly did not have pawns share quest reward, so will set to false for default, 
        /// change as needed
        /// </summary>
        public bool EnableMainPartyPawnsQuestRewards
        {
            get
            {
                return GetSetting<bool>("EnableMainPartyPawnsQuestRewards");
            }
        }

        /// <summary>
        /// Specifies the time in seconds that a bazaar exhibit will last.
        /// By default, the equivalent of 3 days
        /// </summary>
        public ulong BazaarExhibitionTimeSeconds
        {
            get
            {
                return GetSetting<ulong>("BazaarExhibitionTimeSeconds");
            }
        }

        /// <summary>
        /// Specifies the time in seconds that a slot in the bazaar won't be able to be used again.
        /// By default, the equivalent of 1 day
        /// </summary>
        public ulong BazaarCooldownTimeSeconds
        {
            get
            {
                return GetSetting<ulong>("BazaarCooldownTimeSeconds");
            }
        }

        /// <summary>
        /// Various URLs used by the client.
        /// Shared with the login server.
        /// </summary>
        public string UrlManual
        {
            get
            {
                return GetSetting<string>("UrlManual");
            }
        }

        public string UrlShopDetail
        {
            get
            {
                return GetSetting<string>("UrlShopDetail");
            }
        }

        public string UrlShopCounterA
        {
            get
            {
                return GetSetting<string>("UrlShopCounterA");
            }
        }

        public string UrlShopAttention
        {
            get
            {
                return GetSetting<string>("UrlShopAttention");
            }
        }

        public string UrlShopStoneLimit
        {
            get
            {
                return GetSetting<string>("UrlShopStoneLimit");
            }
        }

        public string UrlShopCounterB
        {
            get
            {
                return GetSetting<string>("UrlShopCounterB");
            }
        }

        public string UrlChargeCallback
        {
            get
            {
                return GetSetting<string>("UrlChargeCallback");
            }
        }

        public string UrlChargeA
        {
            get
            {
                return GetSetting<string>("UrlChargeA");
            }
        }

        public string UrlSample9
        {
            get
            {
                return GetSetting<string>("UrlSample9");
            }
        }

        public string UrlSample10
        {
            get
            {
                return GetSetting<string>("UrlSample10");
            }
        }

        public string UrlCampaignBanner
        {
            get
            {
                return GetSetting<string>("UrlCampaignBanner");
            }
        }

        public string UrlSupportIndex
        {
            get
            {
                return GetSetting<string>("UrlSupportIndex");
            }
        }

        public string UrlPhotoupAuthorize
        {
            get
            {
                return GetSetting<string>("UrlPhotoupAuthorize");
            }
        }

        public string UrlApiA
        {
            get
            {
                return GetSetting<string>("UrlApiA");
            }
        }

        public string UrlApiB
        {
            get
            {
                return GetSetting<string>("UrlApiB");
            }
        }

        public string UrlIndex
        {
            get
            {
                return GetSetting<string>("UrlIndex");
            }
        }

        public string UrlCampaign
        {
            get
            {
                return GetSetting<string>("UrlCampaign");
            }
        }

        public string UrlChargeB
        {
            get
            {
                return GetSetting<string>("UrlChargeB");
            }
        }

        public string UrlCompanionImage
        {
            get
            {
                return GetSetting<string>("UrlCompanionImage");
            }
        }
    }
}
