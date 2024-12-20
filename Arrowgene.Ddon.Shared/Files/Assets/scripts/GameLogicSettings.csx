/**
 * Settings file for Server customization.
 * This file supports hotloading.
 */

// Generic Server Settings
GameLogicSetting.NaiveLobbyContextHandling = true;

// Crafting Settings
GameLogicSetting.AdditionalProductionSpeedFactor = 1.0;
GameLogicSetting.AdditionalCostPerformanceFactor = 1.0;
GameLogicSetting.CraftConsumableProductionTimesMax = 10;

// Exp Ring Settings
GameLogicSetting.RookiesRingMaxLevel = 89;
GameLogicSetting.RookiesRingBonus = 1.0;

// EXP Penalty Settings
GameLogicSetting.AdjustPartyEnemyExp = true;
GameLogicSetting.AdjustPartyEnemyExpTiers = new List<(uint MinLv, uint MaxLv, double ExpMultiplier)>()
{
    // 1.0 = 100%, 0 = 0%
    // If the range is larger than the last entry,
    // a 0% exp rate is automatically applied if
    // AdjustPartyEnemyExp = true
    //
    // MinLv, MaxLv, ExpMultiplier
    (      0,     2,           1.0),
    (      3,     4,           0.9),
    (      5,     6,           0.8),
    (      7,     8,           0.6),
    (      9,    10,           0.5),
};

GameLogicSetting.AdjustTargetLvEnemyExp = false;
GameLogicSetting.AdjustTargetLvEnemyExpTiers = new List<(uint MinLv, uint MaxLv, double ExpMultiplier)>()
{
    // MinLv, MaxLv, ExpMultiplier
    (      0,     2,           1.0),
    (      3,     4,           0.9),
    (      5,     6,           0.8),
    (      7,     8,           0.6),
    (      9,    10,           0.5),
};

// Pawn Catchup Settings
GameLogicSetting.EnablePawnCatchup = true;
GameLogicSetting.PawnCatchupMultiplier = 1.5;
GameLogicSetting.PawnCatchupLvDiff = 5;

// Game Time Settings
GameLogicSetting.GameClockTimescale = 90;

// Weather Settings
GameLogicSetting.WeatherSequenceLength = 20;
GameLogicSetting.WeatherStatistics = new List<(uint MeanLength, uint Weight)>()
{
    (60 * 30, 1), // Fair
    (60 * 30, 1), // Cloudy
    (60 * 30, 1), // Rainy
};

// Account Settings
GameLogicSetting.CharacterNumMax = 4;
GameLogicSetting.FriendListMax = 200;

// Player Settings
GameLogicSetting.JobLevelMax = 120;
GameLogicSetting.EnableVisualEquip = true;
GameLogicSetting.DefaultWarpFavorites = 3;
GameLogicSetting.LaternBurnTimeInSeconds = 1500;

// Pawn Settings
GameLogicSetting.EnableMainPartyPawnsQuestRewards = false;

// Bazaar Settings
GameLogicSetting.DefaultMaxBazaarExhibits = 5;
GameLogicSetting.BazaarExhibitionTimeSeconds = (ulong) TimeSpan.FromDays(3).TotalSeconds;
GameLogicSetting.BazaarCooldownTimeSeconds = (ulong) TimeSpan.FromDays(1).TotalSeconds;

// Clan Settings
GameLogicSetting.ClanMemberMax = 100;

// Epitaph Settings
GameLogicSetting.EnableEpitaphWeeklyRewards = false;

// Point Settings
GameLogicSetting.PlayPointMax = 2000;

// Global Point Modifiers
GameLogicSetting.EnemyExpModifier = 1;
GameLogicSetting.QuestExpModifier = 1;
GameLogicSetting.PpModifier = 1;
GameLogicSetting.GoldModifier = 1;
GameLogicSetting.RiftModifier = 1;
GameLogicSetting.BoModifier = 1;
GameLogicSetting.HoModifier = 1;
GameLogicSetting.JpModifier = 1;
GameLogicSetting.RewardBoxMax = 100;
GameLogicSetting.QuestOrderMax = 20;

// Wallet Settings
GameLogicSetting.WalletLimits = new Dictionary<WalletType, uint>()
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

// URL Settings
string urlDomain = $"http://localhost:{52099}";
GameLogicSetting.UrlManual = $"{urlDomain}/manual_nfb/";
GameLogicSetting.UrlShopDetail = $"{urlDomain}/shop/ingame/stone/detail";
GameLogicSetting.UrlShopCounterA = $"{urlDomain}/shop/ingame/counter?";
GameLogicSetting.UrlShopAttention = $"{urlDomain}/shop/ingame/attention?";
GameLogicSetting.UrlShopStoneLimit = $"{urlDomain}/shop/ingame/stone/limit";
GameLogicSetting.UrlShopCounterB = $"{urlDomain}/shop/ingame/counter?";
GameLogicSetting.UrlChargeCallback = $"{urlDomain}/opening/entry/ddo/cog_callback/charge";
GameLogicSetting.UrlChargeA = $"{urlDomain}/sp_ingame/charge/";
GameLogicSetting.UrlSample9 = "http://sample09.html";
GameLogicSetting.UrlSample10 = "http://sample10.html";
GameLogicSetting.UrlCampaignBanner = $"{urlDomain}/sp_ingame/campaign/bnr/bnr01.html?";
GameLogicSetting.UrlSupportIndex = $"{urlDomain}/sp_ingame/support/index.html";
GameLogicSetting.UrlPhotoupAuthorize = $"{urlDomain}/api/photoup/authorize";
GameLogicSetting.UrlApiA = $"{urlDomain}/link/api";
GameLogicSetting.UrlApiB = $"{urlDomain}/link/api";
GameLogicSetting.UrlIndex = $"{urlDomain}/sp_ingame/link/index.html";
GameLogicSetting.UrlCampaign = $"{urlDomain}/sp_ingame/campaign/bnr/slide.html";
GameLogicSetting.UrlChargeB = $"{urlDomain}/sp_ingame/charge/";
GameLogicSetting.UrlCompanionImage = $"{urlDomain}/";
