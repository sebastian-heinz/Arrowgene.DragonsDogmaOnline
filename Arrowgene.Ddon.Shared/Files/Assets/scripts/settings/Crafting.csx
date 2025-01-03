/**
 * Settings file for Server customization.
 * This file supports hotloading.
 */

/**
 * @brief Additional factor to change how long crafting a recipe will take to finish.
 */
double AdditionalProductionSpeedFactor = 1.0;

/**
 * @brief Additional factor to change how much a recipe will cost.
 */
double AdditionalCostPerformanceFactor = 1.0;

/**
 * @brief Determines the maximum amount of consumable items that can be crafted in one go with a pawn.
 * The default is a value of 10 which is equivalent to the original game's behavior.
 */
byte CraftConsumableProductionTimesMax = 10;

/**
 * @brief
 */
int GreatSuccessOddsDefault = 10;

/**
 * @brief
 */
uint CraftSkillLevelMax = 70;

/**
 * @brief
 */
uint PawnCraftRankMaxLimit = 70;

/**
 * @brief
 */
double CraftPawnsMax = 4;

/**
 * @brief Randomly chosen maximum of 50% chance.
 */
uint EquipmentQualityMaximumTotal = 50;

/**
 * @brief Randomly chosen minimum of 4% chance added per pawn.
 */
uint EquipmentQualityMinimumPerPawn = 4;

/**
 * @brief Exp values for each crafting rank.
 */
var CraftRankExpLimit = new List<uint>
{
    0, 8, 120, 240, 400, 800, 1400, 2150, 3050, 4100, 5300, 6600, 8000, 9500, 11000, 13000, 15000, 17500, 20000, 23000, 27000, 29500, 32500, 36000, 39500, 43500, 47000,
    51000, 55000, 60000, 63000, 67461, 72101, 76927, 81946, 87165, 92593, 98239, 104110, 110216, 116566, 123170, 130039, 137182, 144611, 152337, 160372, 168728, 177419,
    186457, 195857, 205633, 215800, 226374, 237370, 248807, 260701, 273070, 285935, 299314, 313228, 327699, 342748, 358400, 374678, 391606, 409212, 427522, 446565, 466369
};

/**
 * This calc is totally made up, but workable.
 * Added a multiplication of 2 since the returned value was always exceptionally low, making it scale really badly.
 */
double EquipmentQualityIncrementPerLevel = 2 * (EquipmentQualityMaximumTotal - EquipmentQualityMinimumPerPawn * CraftPawnsMax) / (CraftSkillLevelMax * CraftPawnsMax);

/**
 * @brief Minimum enhancement points at 150 total based on video evidence.
 */
uint EquipmentEnhancementMinimumTotal = 150;

/**
 * @brief Maximum per pawn at 95 roughly based on maximum points observed based on video evidence, represents delta to minimum divided by max pawn count of 4.
 */
double EquipmentEnhancementMaximumPerPawn = 95;

/**
 * @brief Great success factor 1.2 chosen based on Wiki and video evidence.
 */
double EquipmentEnhancementGreatSuccessFactor = 1.2;

/**
 * @brief
 */
double EquipmentEnhancementIncrementPerLevel = EquipmentEnhancementMaximumPerPawn / CraftSkillLevelMax;

/**
 * @brief Randomly chosen maximum quantity of 1 consumable each pawn can contribute. 
 */
uint ConsumableQuantityMaximumQuantityPerPawn = 1;

/**
 * @brief Randomly chosen maximum of 50% chance.
 */
uint ConsumableQuantityMaximumTotal = 50;

/**
 * @brief Randomly chosen minimum of 4% chance added per pawn.
 */
uint ConsumableQuantityMinimumPerPawn = 4;

/**
 * @brief 
 */
double ConsumableQuantityIncrementPerLevel = (ConsumableQuantityMaximumTotal / CraftPawnsMax - ConsumableQuantityMinimumPerPawn) / CraftSkillLevelMax;


/**
 * @brief Used as part of the craft skill calculations. 
 * Items of approximately this rank and above apply penalties to the pawns craft skills during crafting.
 */
uint CraftItemLv = 15;

/**
 * @brief Used as part of the craft skill calculations.
 * Every ReasonableCraftLv/CraftItemLv item ranks, the penalty increases in magnitude.
 */
uint ReasonableCraftLv = 5;

/**
 * @brief Recipies required for rankup.
 * @note If changed on a live server, the client caches the results
 * of the crafting menu, so players need to log out and back in for
 * the change to show.
 */
var CraftRankLimitPromotionRecipes = new Dictionary<uint, uint>()
{
    // Lv, Recipie Id
    [8] = 2949,
    [16] = 1488,
    [21] = 1490,
    [26] = 1523,
    [31] = 1841,
    [36] = 1903,
    [41] = 2282,
    [46] = 2283,
    [51] = 2466,
    [56] = 2680,
    [61] = 2826,
    [66] = 1406,
};

/**
 * @brief
 */
var CraftRankLimits = new Dictionary<uint, uint>()
{
    [8] = 16,
    [16] = 21,
    [21] = 26,
    [26] = 31,
    [31] = 36,
    [36] = 41,
    [41] = 46,
    [46] = 51,
    [51] = 56,
    [56] = 61,
    [61] = 66,
    [66] = 71
};