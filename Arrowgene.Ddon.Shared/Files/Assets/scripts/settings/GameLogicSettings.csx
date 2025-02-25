/**
 * Settings file for Server customization.
 * This file supports hotloading.
 */

// Generic Server Settings
bool NaiveLobbyContextHandling = true;
uint GameClockTimescale = 90;

// Game Settings
byte RewardBoxMax = 100;
byte QuestOrderMax = 20;
byte CharacterNumMax = 4;
uint FriendListMax = 200;
uint JobLevelMax = 120;
bool EnableVisualEquip = true;
uint DefaultWarpFavorites = 3;
uint LanternBurnTimeInSeconds = 1500;
uint NormalPartySize = 4;

bool EnableAreaRankSpotLocks = false;

// Crafting Settings
double AdditionalProductionSpeedFactor = 1.0;
double AdditionalCostPerformanceFactor = 1.0;
byte CraftConsumableProductionTimesMax = 10;

// Weather Settings
uint WeatherSequenceLength = 20;
var WeatherStatistics = new List<(uint MeanLength, uint Weight)>()
{
    (60 * 30, 1), // Fair
    (60 * 30, 1), // Cloudy
    (60 * 30, 1), // Rainy
};

// Pawn Settings
bool EnableMainPartyPawnsQuestRewards = false;

// Bazaar Settings
uint DefaultMaxBazaarExhibits = 5;
ulong BazaarExhibitionTimeSeconds = (ulong) TimeSpan.FromDays(3).TotalSeconds;
ulong BazaarCooldownTimeSeconds = (ulong) TimeSpan.FromDays(1).TotalSeconds;

// Clan Settings
uint ClanMemberMax = 100;

// Epitaph Settings
bool EnableEpitaphWeeklyRewards = true;

// Point Settings
uint PlayPointMax = 2000;

// Global Point Modifiers
double EnemyExpModifier = 1;
double BBMEnemyExpModifier = 1;
double QuestExpModifier = 1;
double PpModifier = 1;
double GoldModifier = 1;
double RiftModifier = 1;
double BoModifier = 1;
double HoModifier = 1;
double JpModifier = 1;
double ApModifier = 1;

// Wallet Settings
var WalletLimits = new Dictionary<WalletType, uint>()
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
string UrlManual = $"{urlDomain}/manual_nfb/";
string UrlShopDetail = $"{urlDomain}/shop/ingame/stone/detail";
string UrlShopCounterA = $"{urlDomain}/shop/ingame/counter?";
string UrlShopAttention = $"{urlDomain}/shop/ingame/attention?";
string UrlShopStoneLimit = $"{urlDomain}/shop/ingame/stone/limit";
string UrlShopCounterB = $"{urlDomain}/shop/ingame/counter?";
string UrlChargeCallback = $"{urlDomain}/opening/entry/ddo/cog_callback/charge";
string UrlChargeA = $"{urlDomain}/sp_ingame/charge/";
string UrlSample9 = "http://sample09.html";
string UrlSample10 = "http://sample10.html";
string UrlCampaignBanner = $"{urlDomain}/sp_ingame/campaign/bnr/bnr01.html?";
string UrlSupportIndex = $"{urlDomain}/sp_ingame/support/index.html";
string UrlPhotoupAuthorize = $"{urlDomain}/api/photoup/authorize";
string UrlApiA = $"{urlDomain}/link/api";
string UrlApiB = $"{urlDomain}/link/api";
string UrlIndex = $"{urlDomain}/sp_ingame/link/index.html";
string UrlCampaign = $"{urlDomain}/sp_ingame/campaign/bnr/slide.html";
string UrlChargeB = $"{urlDomain}/sp_ingame/charge/";
string UrlCompanionImage = $"{urlDomain}/";
