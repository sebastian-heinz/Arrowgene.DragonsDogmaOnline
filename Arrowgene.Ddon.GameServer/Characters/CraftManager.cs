using System;
using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class CraftCalculationResult
    {
        public uint CalculatedValue { get; set; }
        public bool IsGreatSuccess { get; set; }
    }

    public class CraftManager
    {
        private const int GreatSuccessOddsDefault = 10;
        private const uint CraftSkillLevelMax = 70;
        private const double CraftPawnsMax = 4;

        private const uint ProductionSpeedMaximumTotal = 50;
        private const uint ProductionSpeedMinimumPerPawn = 4;
        private const double ProductionSpeedIncrementPerLevel = (ProductionSpeedMaximumTotal / CraftPawnsMax - ProductionSpeedMinimumPerPawn) / CraftSkillLevelMax;

        private const uint EquipmentQualityMaximumTotal = 50;
        private const uint EquipmentQualityMinimumPerPawn = 4;
        private const double EquipmentQualityIncrementPerLevel = (EquipmentQualityMaximumTotal / CraftPawnsMax - EquipmentQualityMinimumPerPawn) / CraftSkillLevelMax;

        private const uint EquipmentEnhancementMinimumTotal = 150;
        private const double EquipmentEnhancementMaximumPerPawn = 95;
        private const double EquipmentEnhancementGreatSuccessFactor = 1.2;
        private const double EquipmentEnhancementIncrementPerLevel = EquipmentEnhancementMaximumPerPawn / CraftSkillLevelMax;

        private const uint ConsumableQuantityMaximumQuantityPerPawn = 1;
        private const uint ConsumableQuantityMaximumTotal = 50;
        private const uint ConsumableQuantityMinimumPerPawn = 4;
        private const double ConsumableQuantityIncrementPerLevel = (ConsumableQuantityMaximumTotal / CraftPawnsMax - ConsumableQuantityMinimumPerPawn) / CraftSkillLevelMax;

        private const uint CostPerformanceMaximumTotal = 50;
        private const uint CostPerformanceMinimumPerPawn = 4;
        private const double CostPerformanceIncrementPerLevel = (CostPerformanceMaximumTotal / CraftPawnsMax - CostPerformanceMinimumPerPawn) / CraftSkillLevelMax;

        private readonly DdonGameServer _Server;

        public CraftManager(DdonGameServer server)
        {
            _Server = server;
        }

        #region production speed

        public double GetCraftingTimeReductionRate(List<uint> productionSpeedLevels)
        {
            return Math.Clamp(productionSpeedLevels.Select(level => level * ProductionSpeedIncrementPerLevel + ProductionSpeedMinimumPerPawn).Sum(), 0, 100) *
                   _Server.Setting.ServerSetting.AdditionalProductionSpeedFactor;
        }

        /// <summary>
        /// Takes craft skill level of all pawns into account and allows between 484-581 points.
        /// </summary>
        /// <param name="recipeTime"></param> Original item recipe's crafting time.
        /// <param name="productionSpeedLevel"></param> List of production speed skill levels for involved pawns.
        /// <returns></returns>
        public uint CalculateRecipeProductionSpeed(uint recipeTime, List<uint> productionSpeedLevels)
        {
            // TODO: Figure out actual formula + lower/upper bounds client uses
            double productionSpeedFactor = 100 - GetCraftingTimeReductionRate(productionSpeedLevels) / 100;
            return (uint)Math.Clamp(recipeTime * productionSpeedFactor, 0, recipeTime);
        }

        #endregion

        #region equipment quality

        public double GetEquipmentQualityIncreaseRate(List<uint> equipmentQualityLevels)
        {
            return Math.Clamp(equipmentQualityLevels.Select(level => level * EquipmentQualityIncrementPerLevel + EquipmentQualityMinimumPerPawn).Sum(), 0, 100);
        }

        /// <summary>
        /// Takes craft skill level of all pawns into account and allows between 484-581 points.
        /// </summary>
        /// <param name="craftRankAndEquipmentQualityLevel"></param> List of pairs of craft rank and craft skill level for involved pawns.
        /// <param name="refineMaterialItem"></param> 
        /// <returns></returns>
        public CraftCalculationResult CalculateEquipmentQuality(Item refineMaterialItem, List<uint> equipmentQualityLevels)
        {
            // TODO: Figure out actual formula + lower/upper bounds client uses
            // Based on season 1 evidence:
            //  3x lvl 45 1x lvl 44 => 79% 
            // 1x lvl44, 3x lvl 1 => 63

            byte greatSuccessValue = 1;
            int greatSuccessOdds = GreatSuccessOddsDefault;
            byte RandomQuality = 0;

            if (refineMaterialItem != null)
            {
                switch (refineMaterialItem.ItemId)
                {
                    // Quality Rocks (Tier2)
                    case 8036 or 8068:
                        RandomQuality = 2;
                        greatSuccessValue = 3;
                        break;
                    // WhiteDragon Rocks (Tier3)
                    case 8052 or 8084:
                        RandomQuality = 2;
                        greatSuccessValue = 3;
                        greatSuccessOdds = 5;
                        break;
                    // Standard Rocks (Tier1)
                    case 8035 or 8067:
                        RandomQuality = 1;
                        greatSuccessValue = 2;
                        break;
                }
            }

            var isGreatSuccess = CalculateIsGreatSuccess(greatSuccessOdds);

            if (isGreatSuccess)
            {
                RandomQuality = greatSuccessValue;
            }

            return new CraftCalculationResult()
            {
                CalculatedValue = RandomQuality,
                IsGreatSuccess = isGreatSuccess
            };
        }

        #endregion

        #region equipment enhancement

        public double GetEquipmentEnhancementPoints(List<uint> equipmentEnhancementLevels)
        {
            return EquipmentEnhancementMinimumTotal + equipmentEnhancementLevels.Select(level => level * EquipmentEnhancementIncrementPerLevel).Sum();
        }

        public double GetEquipmentEnhancementPointsGreatSuccess(List<uint> equipmentEnhancementLevels)
        {
            return GetEquipmentEnhancementPoints(equipmentEnhancementLevels) * EquipmentEnhancementGreatSuccessFactor;
        }

        /// <summary>
        /// Takes craft skill level of all pawns into account and allows between 484-581 points.
        /// </summary>
        /// <param name="craftRankAndEquipmentEnhancementLevel"></param> List of pairs of craft rank and craft skill level for involved pawns.
        /// <returns></returns>
        public CraftCalculationResult CalculateEquipmentEnhancement(List<uint> equipmentEnhancementLevels)
        {
            // TODO: Figure out actual formula + lower/upper bounds client uses
            // Based on season 1 evidence:
            //  4x lvl 45 => min 390 / max 468 pt
            //  1x lvl 45 + 3x lvl 1 => min 266 / max 319 pt
            // According to wikis: 150 + (levelValue - 1 ) * 1.73 => mostly season 1

            double equipmentEnhancementPoints = GetEquipmentEnhancementPoints(equipmentEnhancementLevels);
            bool isGreatSuccess = CalculateIsGreatSuccess();
            equipmentEnhancementPoints *= isGreatSuccess ? EquipmentEnhancementGreatSuccessFactor : 1;
            return new CraftCalculationResult()
            {
                CalculatedValue = (uint)equipmentEnhancementPoints,
                IsGreatSuccess = isGreatSuccess
            };
        }

        #endregion

        #region consumable quantity

        public double GetAdditionalConsumableQuantityRate(List<uint> consumableQuantityLevels)
        {
            return Math.Clamp(consumableQuantityLevels.Select(level => level * ConsumableQuantityIncrementPerLevel + ConsumableQuantityMinimumPerPawn).Sum(), 0, 100);
        }

        public double GetAdditionalConsumableQuantityMaximum(List<uint> consumableQuantityLevels)
        {
            return consumableQuantityLevels.Count * ConsumableQuantityMaximumQuantityPerPawn;
        }

        /// <summary>
        /// Takes craft rank and craft skill level of all pawns into account and allows to push the minimum chance to 50% to add up to 3 additional items.
        /// </summary>
        /// <param name="consumableQuantityLevels"></param> List of consumable quantity skill levels for involved pawns.
        /// <returns></returns>
        public CraftCalculationResult CalculateConsumableQuantity(List<uint> consumableQuantityLevels)
        {
            // TODO: Figure out actual formula + lower/upper bounds client uses
            double consumableQuantityChance = GetAdditionalConsumableQuantityRate(consumableQuantityLevels);
            uint quantity = 0;
            for (int i = 0; i < consumableQuantityLevels.Count; i++)
            {
                quantity += Random.Shared.Next(100) < consumableQuantityChance ? ConsumableQuantityMaximumQuantityPerPawn : 0;
            }

            bool isGreatSuccess = CalculateIsGreatSuccess();
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

        public double GetCraftCostReductionRate(List<uint> costPerformanceLevels)
        {
            return Math.Clamp(costPerformanceLevels.Select(level => level * CostPerformanceIncrementPerLevel + CostPerformanceMinimumPerPawn).Sum(), 0, 100) *
                   _Server.Setting.ServerSetting.AdditionalCostPerformanceFactor;
        }

        /// <summary>
        /// Takes craft skill level of all pawns into account and allows for a maximum of 50% reduction by default.
        /// </summary>
        /// <param name="recipeCost"></param> Original item recipe's crafting cost.
        /// <param name="costPerformanceLevels"></param> List of cost performance craft skill levels for involved pawns.
        /// <returns></returns>
        public uint CalculateRecipeCost(uint recipeCost, List<uint> costPerformanceLevels)
        {
            // TODO: Figure out actual formula + lower/upper bounds client uses
            // Based on season 1 evidence:
            //  1x lvl 2, 3x lvl 1 == 16% maximum
            double costPerformanceFactor = 100 - GetCraftCostReductionRate(costPerformanceLevels) / 100;
            return (uint)Math.Clamp(recipeCost * costPerformanceFactor, 0, recipeCost);
        }

        #endregion

        public static bool CalculateIsGreatSuccess(int odds = GreatSuccessOddsDefault)
        {
            return Random.Shared.Next(odds) == 0;
        }

        public static uint GetPawnProductionSpeedLevel(Pawn pawn)
        {
            return GetPawnCraftLevel(pawn, CraftSkillType.ProductionSpeed);
        }

        public static uint GetPawnEquipmentEnhancementLevel(Pawn pawn)
        {
            return GetPawnCraftLevel(pawn, CraftSkillType.EquipmentEnhancement);
        }

        public static uint GetPawnEquipmentQualityLevel(Pawn pawn)
        {
            return GetPawnCraftLevel(pawn, CraftSkillType.EquipmentQuality);
        }

        public static uint GetPawnConsumableQuantityLevel(Pawn pawn)
        {
            return GetPawnCraftLevel(pawn, CraftSkillType.ConsumableQuantity);
        }

        public static uint GetPawnCostPerformanceLevel(Pawn pawn)
        {
            return GetPawnCraftLevel(pawn, CraftSkillType.CostPerformance);
        }

        public static uint GetPawnCraftLevel(Pawn pawn, CraftSkillType craftSkillType)
        {
            return pawn.CraftData.PawnCraftSkillList.Find(skill => skill.Type == craftSkillType).Level;
        }

        // TODO: introduce new asset for craft level based on craft_common/craft/craft_exp.cee.json
        // TODO: handle potential multi-rank-ups especially at early levels this is quite possible
        public uint CalculatePawnRankUp(Pawn pawn)
        {
            bool canRankUp = pawn.CraftData.CraftExp > 100 && pawn.CraftData.CraftRank < pawn.CraftData.CraftRankLimit;
            uint rankUps = 0;
            if (canRankUp)
            {
                rankUps++;
            }

            return rankUps;
        }

        public void HandlePawnRankUp(GameClient client, Pawn leadPawn)
        {
            uint rankUps = CalculatePawnRankUp(leadPawn);
            if (rankUps > 0)
            {
                leadPawn.CraftData.CraftRank += rankUps;
                leadPawn.CraftData.CraftPoint += rankUps;
                S2CCraftCraftRankUpNtc rankUpNtc = new S2CCraftCraftRankUpNtc
                {
                    PawnId = leadPawn.PawnId,
                    CraftRank = leadPawn.CraftData.CraftRank,
                    AddCraftPoints = 1,
                    TotalCraftPoint = leadPawn.CraftData.CraftPoint
                };
                client.Send(rankUpNtc);
            }
        }

        public void HandlePawnExpUp(GameClient client, Pawn leadPawn, uint exp, uint bonusExp)
        {
            S2CCraftCraftExpUpNtc expNtc = new S2CCraftCraftExpUpNtc()
            {
                PawnId = leadPawn.PawnId,
                AddExp = exp,
                ExtraBonusExp = bonusExp,
                TotalExp = exp + bonusExp,
                CraftRankLimit = leadPawn.CraftData.CraftRankLimit
            };
            leadPawn.CraftData.CraftExp += expNtc.TotalExp;
            client.Send(expNtc);
        }
    }
}
