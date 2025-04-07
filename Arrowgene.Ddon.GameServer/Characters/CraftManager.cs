using System;
using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Craft;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class CraftCalculationResult
    {
        public uint CalculatedValue { get; set; }
        public bool IsGreatSuccess { get; set; }
        public uint Exp { get; set; }
    }

    public class CraftManager
    {
        // TODO: introduce new assets instead
        private static readonly List<uint> craftRankExpLimit = new()
        {
            0, 8, 120, 240, 400, 800, 1400, 2150, 3050, 4100, 5300, 6600, 8000, 9500, 11000, 13000, 15000, 17500, 20000, 23000, 27000, 29500, 32500, 36000, 39500, 43500, 47000,
            51000, 55000, 60000, 63000, 67461, 72101, 76927, 81946, 87165, 92593, 98239, 104110, 110216, 116566, 123170, 130039, 137182, 144611, 152337, 160372, 168728, 177419,
            186457, 195857, 205633, 215800, 226374, 237370, 248807, 260701, 273070, 285935, 299314, 313228, 327699, 342748, 358400, 374678, 391606, 409212, 427522, 446565, 466369
        };

        private static readonly Dictionary<uint, uint> craftRankLimitPromotionRecipes = new()
        {
            { 8, 1406 }, { 16, 1488 }, { 21, 1490 }, { 26, 1523 }, { 31, 1841 }, { 36, 1903 }, { 41, 2282 }, { 46, 2283 }, { 51, 2466 }, { 56, 2680 }, { 61, 2826 }, { 66, 2949 }
        };

        private static readonly Dictionary<uint, uint> craftRankLimits = new()
        {
            { 8, 16 }, { 16, 21 }, { 21, 26 }, { 26, 31 }, { 31, 36 }, { 36, 41 }, { 41, 46 }, { 46, 51 }, { 51, 56 }, { 56, 61 }, { 61, 66 }, { 66, 71 }
        };

        private const int GreatSuccessOddsDefault = 10;
        private const uint CraftSkillLevelMax = 70;
        private const uint PawnCraftRankMaxLimit = 70;
        private const double CraftPawnsMax = 4;

        /// <summary>
        /// Randomly chosen maximum of 50% chance.
        /// </summary>  
        private const uint EquipmentQualityMaximumTotal = 50;

        /// <summary>
        /// Randomly chosen minimum of 4% chance added per pawn.
        /// </summary>
        private const uint EquipmentQualityMinimumPerPawn = 4;

        /// <summary>
        /// This calc is totally made up, but workable.
        /// Added a multiplication of 2 since the returned value was always exceptionally low, making it scale really badly.
        /// </summary>
        private const double EquipmentQualityIncrementPerLevel = 2 * (EquipmentQualityMaximumTotal - EquipmentQualityMinimumPerPawn * CraftPawnsMax) / (CraftSkillLevelMax * CraftPawnsMax);

        /// <summary>
        /// Minimum enhancement points at 150 total based on video evidence.
        /// </summary>
        private const uint EquipmentEnhancementMinimumTotal = 150;

        /// <summary>
        /// Maximum per pawn at 95 roughly based on maximum points observed based on video evidence, represents delta to minimum divided by max pawn count of 4.
        /// </summary>
        private const double EquipmentEnhancementMaximumPerPawn = 95;

        /// <summary>
        /// Great success factor 1.2 chosen based on Wiki and video evidence.
        /// </summary>
        private const double EquipmentEnhancementGreatSuccessFactor = 1.2;

        private const double EquipmentEnhancementIncrementPerLevel = EquipmentEnhancementMaximumPerPawn / CraftSkillLevelMax;

        /// <summary>
        /// Randomly chosen maximum quantity of 1 consumable each pawn can contribute. 
        /// </summary>
        private const uint ConsumableQuantityMaximumQuantityPerPawn = 1;

        /// <summary>
        /// Randomly chosen maximum of 50% chance.
        /// </summary>
        private const uint ConsumableQuantityMaximumTotal = 50;

        /// <summary>
        /// Randomly chosen minimum of 4% chance added per pawn.
        /// </summary>
        private const uint ConsumableQuantityMinimumPerPawn = 4;

        private const double ConsumableQuantityIncrementPerLevel = (ConsumableQuantityMaximumTotal / CraftPawnsMax - ConsumableQuantityMinimumPerPawn) / CraftSkillLevelMax;

        /// <summary>
        /// Used as part of the craft skill calculations. 
        /// Items of approximately this rank and above apply penalties to the pawns craft skills during crafting.
        /// TODO: Expose to settings.
        /// </summary>
        public const uint CraftItemLv = 15;

        /// <summary>
        /// Used as part of the craft skill calculations.
        /// Every ReasonableCraftLv/CraftItemLv item ranks, the penalty increases in magnitude.
        /// TODO: Expose to settings.
        /// </summary>
        public const uint ReasonableCraftLv = 5;

        private readonly DdonGameServer _server;

        public CraftManager(DdonGameServer server)
        {
            _server = server;
        }

        /// <summary>
        /// Calculates the penalty to craft skills based on the item being crafted.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static uint ItemDifficultyModifier(ClientItemInfo item)
        {
            if (item.Rank < CraftItemLv)
            {
                return 0;
            }
            uint modRank = item.Rank % CraftItemLv;
            uint score = (5000 * (item.Rank - modRank) / CraftItemLv) 
                + (1000 * ReasonableCraftLv * modRank / CraftItemLv)
                - 5000;
            return score / 1000;
        }

        #region production speed

        /// <summary>
        /// Calculates crafting time reduction to roughly match the values the client is calculating (plus or minus a few seconds).
        /// Can be manipulated using server setting AdditionalProductionSpeedFactor, which at 1.0 has no effect.
        /// If AdditionalProductionSpeedFactor isn't 1.0, the actual time will diverge further from the client's predictions.
        /// </summary>
        public uint CalculateRecipeProductionSpeed(uint recipeTime, ClientItemInfo itemInfo, List<CraftPawn> craftPawns)
        {
            uint difficultyModifier = ItemDifficultyModifier(itemInfo);
            int total = 0;
            int modifiedTime = (int)recipeTime;

            foreach (var pawn in craftPawns)
            {
                if (pawn.ProductionSpeed < difficultyModifier)
                {
                    continue;
                }
                int effSkill = (int)(pawn.ProductionSpeed - difficultyModifier);
                PawnCraftSkillSpeedRate speedRateAsset = _server.AssetRepository.PawnCraftSkillSpeedRateAsset.ElementAtOrDefault(effSkill)
                    ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_CRAFT_SKILL_LEVEL_OVER, $"No speed rate information found for level: {effSkill}");
                float speedRate = pawn.PositionModifier == 1.0 ? speedRateAsset.SpeedRate1 : speedRateAsset.SpeedRate2;

                total += effSkill;
                modifiedTime = (int)(modifiedTime * speedRate);
            }

            modifiedTime -= total;
            if (modifiedTime < 30)
            {
                modifiedTime = 30;
            }
            modifiedTime = (int)(modifiedTime * _server.GameSettings.GameServerSettings.AdditionalProductionSpeedFactor);
            return (uint)modifiedTime;
        }

        #endregion

        #region equipment quality

        public static double CalculateEquipmentQualityIncreaseRate(List<CraftPawn> craftPawns)
        {
            return Math.Clamp(craftPawns.Select(x => x.EquipmentQuality * EquipmentQualityIncrementPerLevel + EquipmentQualityMinimumPerPawn).Sum(), 0, 100);
        }

        /// <summary>
        /// Based on previously existing quality calculations. Currently, does not make use of quality skill levels.
        /// </summary>
        /// <param name="refineMaterialItem"></param>
        /// <param name="equipmentQualityLevels"></param>
        /// <returns></returns>
        public static CraftCalculationResult CalculateEquipmentQuality(Item refineMaterialItem, uint calculatedOdds, byte itemRank=0)
        {
            // TODO: Figure out actual formula + lower/upper bounds client uses
            // EXP has a base value and scales based on IR, standard always 2, Quality and WD share until above IR10, Quality stays 100 and WD goes up to 350 cap with IR35.

            byte greatSuccessValue = 1;
            int greatSuccessOdds = GreatSuccessOddsDefault;
            byte RandomQuality = 0;
            uint exp = 0;

            if (refineMaterialItem != null)
            {
                switch (refineMaterialItem.ItemId)
                {
                    // Quality Rocks (Tier2)
                    case 8036 or 8068:
                        RandomQuality = 2;
                        greatSuccessValue = 3;
                        exp = CalculateQualityExp(itemRank, false, false);
                        break;
                    // WhiteDragon Rocks (Tier3)
                    case 8052 or 8084:
                        RandomQuality = 2;
                        greatSuccessValue = 3;
                        greatSuccessOdds = 25;
                        exp = CalculateQualityExp(itemRank, true, false);
                        break;
                    // Standard Rocks (Tier1)
                    case 8035 or 8067:
                        RandomQuality = 1;
                        greatSuccessValue = 2;
                        exp = CalculateQualityExp(itemRank, false, true);
                        break;
                }
            }

            var isGreatSuccess = CalculateIsGreatSuccess(greatSuccessOdds, calculatedOdds);

            if (isGreatSuccess)
            {
                RandomQuality = greatSuccessValue;
            }

            return new CraftCalculationResult()
            {
                CalculatedValue = RandomQuality,
                IsGreatSuccess = isGreatSuccess,
                Exp = exp
            };
        }

        public static uint CalculateQualityExp(byte itemRank, bool isTier3, bool isTier1)
        {
            uint baseExp = 10;
            uint maxExp = isTier3 ? 350u : 100u; // Tier3 caps at 350, Tier2 caps at 100
            if (isTier1)
            {
                maxExp = 2;
                return maxExp;
            }
            return Math.Min(baseExp * itemRank, maxExp);
        }

        #endregion

        #region equipment enhancement

        public static double GetEquipmentEnhancementPoints(List<CraftPawn> craftPawns)
        {
            return EquipmentEnhancementMinimumTotal + craftPawns.Select(x => x.EquipmentEnhancement * EquipmentEnhancementIncrementPerLevel).Sum();
        }

        public static double GetEquipmentEnhancementPointsGreatSuccess(List<CraftPawn> craftPawns)
        {
            return GetEquipmentEnhancementPoints(craftPawns) * EquipmentEnhancementGreatSuccessFactor;
        }

        public CraftCalculationResult CalculateEquipmentEnhancement(List<CraftPawn> craftPawns, uint calculatedOdds)
        {
            // TODO: Figure out actual formula + lower/upper bounds client uses
            // Based on season 1 evidence:
            //  4x lvl 45 => min 390 / max 468 pt
            //  1x lvl 45 + 3x lvl 1 => min 266 / max 319 pt
            // According to wikis: 150 + (levelValue - 1 ) * 1.73 => mostly season 1

            double equipmentEnhancementPoints = GetEquipmentEnhancementPoints(craftPawns);
            bool isGreatSuccess = CalculateIsGreatSuccess(GreatSuccessOddsDefault, calculatedOdds);
            equipmentEnhancementPoints *= isGreatSuccess ? EquipmentEnhancementGreatSuccessFactor : 1;
            return new CraftCalculationResult()
            {
                CalculatedValue = (uint)equipmentEnhancementPoints,
                IsGreatSuccess = isGreatSuccess
            };
        }

        #endregion

        #region consumable quantity

        public static double GetAdditionalConsumableQuantityRate(List<CraftPawn> craftPawns)
        {
            return Math.Clamp(craftPawns.Select(x => x.ConsumableQuantity * ConsumableQuantityIncrementPerLevel + ConsumableQuantityMinimumPerPawn).Sum(), 0, 100);
        }

        public static double GetAdditionalConsumableQuantityMaximum(List<CraftPawn> craftPawns)
        {
            return craftPawns.Count * ConsumableQuantityMaximumQuantityPerPawn;
        }

        /// <summary>
        /// Takes craft rank and craft skill level of all pawns into account and allows to push the minimum chance to 50% to add up to 3 additional items.
        /// </summary>
        public static CraftCalculationResult CalculateConsumableQuantity(List<CraftPawn> craftPawns, uint calculatedOdds)
        {
            // TODO: Figure out actual formula + lower/upper bounds client uses
            double consumableQuantityChance = GetAdditionalConsumableQuantityRate(craftPawns);
            uint quantity = 0;
            for (int i = 0; i < craftPawns.Count; i++)
            {
                quantity += Random.Shared.Next(100) < consumableQuantityChance ? ConsumableQuantityMaximumQuantityPerPawn : 0;
            }

            bool isGreatSuccess = CalculateIsGreatSuccess(GreatSuccessOddsDefault, calculatedOdds);
            if (isGreatSuccess)
            {
                quantity++;
            }

            return new CraftCalculationResult
            {
                CalculatedValue = quantity,
                IsGreatSuccess = isGreatSuccess
            };
        }

        #endregion

        #region cost performance

        public double GetCraftCostReductionRate(ClientItemInfo item, List<CraftPawn> craftPawns)
        {
            uint difficultyModifier = ItemDifficultyModifier(item);
            int total = (int)craftPawns
                .Where(x => x.CostPerformance > difficultyModifier)
                .Sum(x => (x.CostPerformance - difficultyModifier) * x.PositionModifier);

            PawnCraftSkillCostRate costRateAsset = _server.AssetRepository.PawnCraftSkillCostRateAsset.ElementAtOrDefault(total)
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_CRAFT_SKILL_LEVEL_OVER, $"No cost reduction information found for total level: {total}");

            int numberOfPawns = craftPawns.Count;

            switch (numberOfPawns)
            {
                case 1:
                    return costRateAsset.CostRate1;
                case 2:
                    return costRateAsset.CostRate2;
                case 3:
                    return costRateAsset.CostRate3;
                case 4:
                    return costRateAsset.CostRate4;
                default:
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_CRAFT_INTERNAL, $"Number of pawns {numberOfPawns} is out of expected range (1-4).");
            }
        }

        /// <summary>
        /// Takes craft skill level of all pawns into account.
        /// </summary>
        /// <param name="recipeCost"></param> Original item recipe's crafting cost.
        /// <param name="item"></param> Item being crafted.
        /// <param name="craftPawns"></param> List of participating pawns.
        /// <returns></returns>
        public uint CalculateRecipeCost(uint recipeCost, ClientItemInfo item, List<CraftPawn> craftPawns)
        {
            double discountValue = GetCraftCostReductionRate(item, craftPawns);
            double finalCost = discountValue * recipeCost;
            return (uint)finalCost;
        }

        #endregion

        public static bool CalculateIsGreatSuccess(int baseOdds, uint calculatedOdds)
        {
            int adjustedOdds = baseOdds + (int)calculatedOdds;
            int roll = Random.Shared.Next(100);

            return roll < adjustedOdds;
        }

        public static bool IsCraftRankLimitPromotionRecipe(Pawn pawn, uint recipeId)
        {
            return craftRankLimitPromotionRecipes.ContainsKey(pawn.CraftData.CraftRank) && craftRankLimitPromotionRecipes[pawn.CraftData.CraftRank] == recipeId;
        }

        public static void PromotePawnRankLimit(Pawn pawn)
        {
            if (craftRankLimitPromotionRecipes.ContainsKey(pawn.CraftData.CraftRank))
            {
                pawn.CraftData.CraftRankLimit = craftRankLimits[pawn.CraftData.CraftRankLimit];
            }
        }

        public static bool CanPawnRankUp(Pawn pawn)
        {
            return pawn.CraftData.CraftExp >= craftRankExpLimit[(int)pawn.CraftData.CraftRank] && pawn.CraftData.CraftRank < pawn.CraftData.CraftRankLimit;
        }

        public static uint CalculatePawnRankUp(Pawn pawn)
        {
            uint rankUps = 0;

            if (CanPawnRankUp(pawn))
            {
                for (int i = (int)pawn.CraftData.CraftRank; i < pawn.CraftData.CraftRankLimit; i++)
                {
                    if (pawn.CraftData.CraftExp >= craftRankExpLimit[i])
                    {
                        rankUps++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return rankUps;
        } 

        public static S2CCraftCraftRankUpNtc HandlePawnRankUpNtc(GameClient client, Pawn leadPawn)
        {
            S2CCraftCraftRankUpNtc rankUpNtc = new S2CCraftCraftRankUpNtc
            {
                PawnId = leadPawn.PawnId,
                CraftRank = leadPawn.CraftData.CraftRank
            };
            
            uint rankUps = CalculatePawnRankUp(leadPawn);
            if (rankUps > 0)
            {
                uint rankUpDelta = Math.Clamp(leadPawn.CraftData.CraftRank + rankUps, leadPawn.CraftData.CraftRank, PawnCraftRankMaxLimit) - leadPawn.CraftData.CraftRank;
                
                leadPawn.CraftData.CraftRank += rankUpDelta;
                leadPawn.CraftData.CraftPoint += rankUpDelta;

                rankUpNtc.AddCraftPoints = rankUpDelta;
                rankUpNtc.CraftRank = leadPawn.CraftData.CraftRank;
                rankUpNtc.TotalCraftPoint = leadPawn.CraftData.CraftPoint;
                
                leadPawn.CraftData.CraftExp = Math.Clamp(leadPawn.CraftData.CraftExp, 0, craftRankExpLimit[(int)leadPawn.CraftData.CraftRankLimit-1]);
            }

            return rankUpNtc;
        }

        public static bool CanPawnExpUp(Pawn pawn)
        {
            return pawn.CraftData.CraftRank < pawn.CraftData.CraftRankLimit;
        }

        public static S2CCraftCraftExpUpNtc HandlePawnExpUpNtc(GameClient client, Pawn leadPawn, uint exp, double BonusExpMultiplier)
        {

            uint expUp = exp;
            double totalAddedExp = 0;
            uint bonusExpUp = 0;
            if (BonusExpMultiplier > 0)
            {
                totalAddedExp = expUp * BonusExpMultiplier;
                bonusExpUp = (uint)totalAddedExp - exp;
                totalAddedExp = expUp + bonusExpUp;
            }
            else
            {
                totalAddedExp = expUp + bonusExpUp;
            }

            S2CCraftCraftExpUpNtc expNtc = new S2CCraftCraftExpUpNtc()
            {
                PawnId = leadPawn.PawnId,
                CraftRankLimit = leadPawn.CraftData.CraftRankLimit
            };
            
            if (CanPawnExpUp(leadPawn))
            {
                expNtc.AddExp = (uint)totalAddedExp; // this has to be totaladded to show exp in the correct format.
                expNtc.ExtraBonusExp = bonusExpUp;
                expNtc.TotalExp = (uint)totalAddedExp; // presumably this should be pawns literal TotalEXP, but my testing had me level up several times.
                
                leadPawn.CraftData.CraftExp += expNtc.TotalExp;
                
                leadPawn.CraftData.CraftExp = Math.Clamp(leadPawn.CraftData.CraftExp, 0, craftRankExpLimit[(int)leadPawn.CraftData.CraftRankLimit]);
            }

            return expNtc;
        }

        public Pawn FindPawn(GameClient client, uint pawnId)
        {
            return client.Character.Pawns.Find(p => p.PawnId == pawnId)
                ?? client.Character.RentedPawns.Find(p => p.PawnId == pawnId)
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_INVALID, "Couldn't find the Pawn ID.");
        }

        public List<CDataMDataCraftMaterial> GetRecipeMaterialsForItemId(ItemId itemId)
        {
            var itemInfo = _server.ItemManager.LookupInfoByItemID(_server, (uint) itemId);
            var recipeList = _server.AssetRepository.CraftingRecipesAsset
                .Where(recipes => recipes.Category == itemInfo.RecipeCategory)
                .Select(recipes => recipes.RecipeList)
                .SingleOrDefault(new List<CraftingRecipe>());
            var recipe = recipeList.Where(x => x.ItemID == (uint)itemId).FirstOrDefault();
            return (recipe != null) ? recipe.CraftMaterialList : new List<CDataMDataCraftMaterial>();
        }

        public ItemId GetItemBaseItemId(ItemId itemId)
        {
            var itemInfo = _server.ItemManager.LookupInfoByItemID(_server, (uint)itemId);
            if (itemInfo == null || itemInfo.Quality == 0)
            {
                return itemId;
            }

            return _server.AssetRepository.ClientItemInfos.Values
                .Where(x => x.Name == itemInfo.Name && x.Quality == 0)
                .Select(x => (ItemId) x.ItemId)
                .FirstOrDefault(itemId);
        }
    }
}
