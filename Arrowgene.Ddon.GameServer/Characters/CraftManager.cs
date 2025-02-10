using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Craft;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class CraftCalculationResult
    {
        public uint CalculatedValue { get; set; }
        public bool IsGreatSuccess { get; set; }
        public uint Exp { get; set; }
    }

    public class CraftSettings
    {
        private DdonGameServer Server;
        public CraftSettings(DdonGameServer server)
        {
            Server = server;
        }

        private T GetSetting<T>(string key)
        {
            return Server.GameLogicSettings.Get<T>("Crafting", key);
        }

        public double AdditionalProductionSpeedFactor
        {
            get
            {
                return GetSetting<double>("AdditionalProductionSpeedFactor");
            }
        }

        public double AdditionalCostPerformanceFactor
        {
            get
            {
                return GetSetting<double>("AdditionalCostPerformanceFactor");
            }
        }

        public byte CraftConsumableProductionTimesMax
        {
            get
            {
                return GetSetting<byte>("CraftConsumableProductionTimesMax");
            }
        }

        public int GreatSuccessOddsDefault
        {
            get
            {
                return GetSetting<int>("GreatSuccessOddsDefault");
            }
        }

        public uint CraftSkillLevelMax
        {
            get
            {
                return GetSetting<uint>("CraftSkillLevelMax");
            }
        }

        public uint PawnCraftRankMaxLimit
        {
            get
            {
                return GetSetting<uint>("PawnCraftRankMaxLimit");
            }
        }

        public double CraftPawnsMax
        {
            get
            {
                return GetSetting<double>("CraftPawnsMax");
            }
        }

        public uint EquipmentQualityMaximumTotal
        {
            get
            {
                return GetSetting<uint>("EquipmentQualityMaximumTotal");
            }
        }

        public uint EquipmentQualityMinimumPerPawn
        {
            get
            {
                return GetSetting<uint>("EquipmentQualityMinimumPerPawn");
            }
        }

        public List<uint> CraftRankExpLimit
        {
            get
            {
                return GetSetting<List<uint>>("CraftRankExpLimit");
            }
        }

        public double EquipmentQualityIncrementPerLevel
        {
            get
            {
                return GetSetting<double>("EquipmentQualityIncrementPerLevel");
            }
        }

        public uint EquipmentEnhancementMinimumTotal
        {
            get
            {
                return GetSetting<uint>("EquipmentEnhancementMinimumTotal");
            }
        }

        public double EquipmentEnhancementMaximumPerPawn
        {
            get
            {
                return GetSetting<double>("EquipmentEnhancementMaximumPerPawn");
            }
        }

        public double EquipmentEnhancementGreatSuccessFactor
        {
            get
            {
                return GetSetting<double>("EquipmentEnhancementGreatSuccessFactor");
            }
        }

        public double EquipmentEnhancementIncrementPerLevel
        {
            get
            {
                return GetSetting<double>("EquipmentEnhancementIncrementPerLevel");
            }
        }

        public uint ConsumableQuantityMaximumQuantityPerPawn
        {
            get
            {
                return GetSetting<uint>("ConsumableQuantityMaximumQuantityPerPawn");
            }
        }

        public uint ConsumableQuantityMaximumTotal
        {
            get
            {
                return GetSetting<uint>("ConsumableQuantityMaximumTotal");
            }
        }

        public uint ConsumableQuantityMinimumPerPawn
        {
            get
            {
                return GetSetting<uint>("ConsumableQuantityMinimumPerPawn");
            }
        }

        public double ConsumableQuantityIncrementPerLevel
        {
            get
            {
                return GetSetting<double>("ConsumableQuantityIncrementPerLevel");
            }
        }

        public uint CraftItemLv
        {
            get
            {
                return GetSetting<uint>("CraftItemLv");
            }
        }

        public uint ReasonableCraftLv
        {
            get
            {
                return GetSetting<uint>("ReasonableCraftLv");
            }
        }

        public Dictionary<uint, uint> CraftRankLimitPromotionRecipes
        {
            get
            {
                return GetSetting<Dictionary<uint, uint>>("CraftRankLimitPromotionRecipes");
            }
        }

        public Dictionary<uint, uint> CraftRankLimits
        {
            get
            {
                return GetSetting<Dictionary<uint, uint>>("CraftRankLimits");
            }
        }
    }

    public class CraftManager
    {
        private readonly DdonGameServer _server;

        public CraftSettings Settings { get; private set; }

        public CraftManager(DdonGameServer server)
        {
            _server = server;
            Settings = new CraftSettings(server);
        }

        /// <summary>
        /// Calculates the penalty to craft skills based on the item being crafted.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public uint ItemDifficultyModifier(ClientItemInfo item)
        {
            if (item.Rank < Settings.CraftItemLv)
            {
                return 0;
            }
            uint modRank = item.Rank % Settings.CraftItemLv;
            uint score = (5000 * (item.Rank - modRank) / Settings.CraftItemLv) 
                + (1000 * Settings.ReasonableCraftLv * modRank / Settings.CraftItemLv)
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
            modifiedTime = (int)(modifiedTime * Settings.AdditionalProductionSpeedFactor);
            return (uint)modifiedTime;
        }

        #endregion

        #region equipment quality

        public double CalculateEquipmentQualityIncreaseRate(List<CraftPawn> craftPawns)
        {
            return Math.Clamp(craftPawns.Select(x => x.EquipmentQuality * Settings.EquipmentQualityIncrementPerLevel + Settings.EquipmentQualityMinimumPerPawn).Sum(), 0, 100);
        }

        /// <summary>
        /// Based on previously existing quality calculations. Currently, does not make use of quality skill levels.
        /// </summary>
        /// <param name="refineMaterialItem"></param>
        /// <param name="equipmentQualityLevels"></param>
        /// <returns></returns>
        public CraftCalculationResult CalculateEquipmentQuality(Item refineMaterialItem, uint calculatedOdds, byte itemRank=0)
        {
            // TODO: Figure out actual formula + lower/upper bounds client uses
            // EXP has a base value and scales based on IR, standard always 2, Quality and WD share until above IR10, Quality stays 100 and WD goes up to 350 cap with IR35.

            byte greatSuccessValue = 1;
            int greatSuccessOdds = Settings.GreatSuccessOddsDefault;
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

        public double GetEquipmentEnhancementPoints(List<CraftPawn> craftPawns)
        {
            return Settings.EquipmentEnhancementMinimumTotal + craftPawns.Select(x => x.EquipmentEnhancement * Settings.EquipmentEnhancementIncrementPerLevel).Sum();
        }

        public double GetEquipmentEnhancementPointsGreatSuccess(List<CraftPawn> craftPawns)
        {
            return GetEquipmentEnhancementPoints(craftPawns) * Settings.EquipmentEnhancementGreatSuccessFactor;
        }

        public CraftCalculationResult CalculateEquipmentEnhancement(List<CraftPawn> craftPawns, uint calculatedOdds)
        {
            // TODO: Figure out actual formula + lower/upper bounds client uses
            // Based on season 1 evidence:
            //  4x lvl 45 => min 390 / max 468 pt
            //  1x lvl 45 + 3x lvl 1 => min 266 / max 319 pt
            // According to wikis: 150 + (levelValue - 1 ) * 1.73 => mostly season 1

            double equipmentEnhancementPoints = GetEquipmentEnhancementPoints(craftPawns);
            bool isGreatSuccess = CalculateIsGreatSuccess(Settings.GreatSuccessOddsDefault, calculatedOdds);
            equipmentEnhancementPoints *= isGreatSuccess ? Settings.EquipmentEnhancementGreatSuccessFactor : 1;
            return new CraftCalculationResult()
            {
                CalculatedValue = (uint)equipmentEnhancementPoints,
                IsGreatSuccess = isGreatSuccess
            };
        }

        #endregion

        #region consumable quantity

        public double GetAdditionalConsumableQuantityRate(List<CraftPawn> craftPawns)
        {
            return Math.Clamp(craftPawns.Select(x => x.ConsumableQuantity * Settings.ConsumableQuantityIncrementPerLevel + Settings.ConsumableQuantityMinimumPerPawn).Sum(), 0, 100);
        }

        public double GetAdditionalConsumableQuantityMaximum(List<CraftPawn> craftPawns)
        {
            return craftPawns.Count * Settings.ConsumableQuantityMaximumQuantityPerPawn;
        }

        /// <summary>
        /// Takes craft rank and craft skill level of all pawns into account and allows to push the minimum chance to 50% to add up to 3 additional items.
        /// </summary>
        public CraftCalculationResult CalculateConsumableQuantity(List<CraftPawn> craftPawns, uint calculatedOdds)
        {
            // TODO: Figure out actual formula + lower/upper bounds client uses
            double consumableQuantityChance = GetAdditionalConsumableQuantityRate(craftPawns);
            uint quantity = 0;
            for (int i = 0; i < craftPawns.Count; i++)
            {
                quantity += Random.Shared.Next(100) < consumableQuantityChance ? Settings.ConsumableQuantityMaximumQuantityPerPawn : 0;
            }

            bool isGreatSuccess = CalculateIsGreatSuccess(Settings.GreatSuccessOddsDefault, calculatedOdds);
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

        public bool IsCraftRankLimitPromotionRecipe(Pawn pawn, uint recipeId)
        {
            return Settings.CraftRankLimitPromotionRecipes.ContainsKey(pawn.CraftData.CraftRank) && Settings.CraftRankLimitPromotionRecipes[pawn.CraftData.CraftRank] == recipeId;
        }

        public void PromotePawnRankLimit(Pawn pawn)
        {
            if (Settings.CraftRankLimitPromotionRecipes.ContainsKey(pawn.CraftData.CraftRank))
            {
                pawn.CraftData.CraftRankLimit = Settings.CraftRankLimits[pawn.CraftData.CraftRankLimit];
            }
        }

        public bool CanPawnRankUp(Pawn pawn)
        {
            return pawn.CraftData.CraftExp >= Settings.CraftRankExpLimit[(int)pawn.CraftData.CraftRank] && pawn.CraftData.CraftRank < pawn.CraftData.CraftRankLimit;
        }

        public uint CalculatePawnRankUp(Pawn pawn)
        {
            uint rankUps = 0;

            if (CanPawnRankUp(pawn))
            {
                for (int i = (int)pawn.CraftData.CraftRank; i < pawn.CraftData.CraftRankLimit; i++)
                {
                    if (pawn.CraftData.CraftExp >= Settings.CraftRankExpLimit[i])
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

        public void HandlePawnRankUpNtc(GameClient client, Pawn leadPawn)
        {
            S2CCraftCraftRankUpNtc rankUpNtc = new S2CCraftCraftRankUpNtc
            {
                PawnId = leadPawn.PawnId,
                CraftRank = leadPawn.CraftData.CraftRank
            };
            
            uint rankUps = CalculatePawnRankUp(leadPawn);
            if (rankUps > 0)
            {
                uint rankUpDelta = Math.Clamp(leadPawn.CraftData.CraftRank + rankUps, leadPawn.CraftData.CraftRank, Settings.PawnCraftRankMaxLimit) - leadPawn.CraftData.CraftRank;
                
                leadPawn.CraftData.CraftRank += rankUpDelta;
                leadPawn.CraftData.CraftPoint += rankUpDelta;

                rankUpNtc.AddCraftPoints = rankUpDelta;
                rankUpNtc.CraftRank = leadPawn.CraftData.CraftRank;
                rankUpNtc.TotalCraftPoint = leadPawn.CraftData.CraftPoint;
                
                leadPawn.CraftData.CraftExp = Math.Clamp(leadPawn.CraftData.CraftExp, 0, Settings.CraftRankExpLimit[(int)leadPawn.CraftData.CraftRankLimit-1]);
            }

            client.Send(rankUpNtc);
        }

        public bool CanPawnExpUp(Pawn pawn)
        {
            return pawn.CraftData.CraftRank < pawn.CraftData.CraftRankLimit;
        }

        public void HandlePawnExpUpNtc(GameClient client, Pawn leadPawn, uint exp, double BonusExpMultiplier)
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
                
                leadPawn.CraftData.CraftExp = Math.Clamp(leadPawn.CraftData.CraftExp, 0, Settings.CraftRankExpLimit[(int)leadPawn.CraftData.CraftRankLimit]);
            }

            client.Send(expNtc);
        }

        public Pawn FindPawn(GameClient client, uint pawnId)
        {
            return client.Character.Pawns.Find(p => p.PawnId == pawnId)
                ?? client.Character.RentedPawns.Find(p => p.PawnId == pawnId)
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_INVALID, "Couldn't find the Pawn ID.");
        }

        public List<CDataMDataCraftRecipe> DeterminePromotionRecipies()
        {
            var promotionRecipies = new SortedList<uint, CDataMDataCraftRecipe>();
            foreach (var recipieCategory in _server.AssetRepository.CraftingRecipesAsset)
            {
                var matches = recipieCategory.RecipeList.Where(r => Settings.CraftRankLimitPromotionRecipes.Values.Contains(r.RecipeID)).ToList();
                foreach (var match in matches)
                {
                    var key = Settings.CraftRankLimitPromotionRecipes.FirstOrDefault(x => (x.Value == match.RecipeID)).Key;
                    promotionRecipies[key] = match;
                }
            }
            return promotionRecipies.Values.ToList();
        }
    }
}
