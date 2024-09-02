using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftSkillAnalyzeHandler : GameStructurePacketHandler<C2SCraftSkillAnalyzeReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftSkillAnalyzeHandler));

        public CraftSkillAnalyzeHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SCraftSkillAnalyzeReq> packet)
        {
            List<uint> pawnIds = new List<uint> { packet.Structure.PawnId };
            pawnIds.AddRange(packet.Structure.AssistPawnIds.Select(p => p.Value));

            List<uint> productionSpeedLevels = new List<uint>();
            List<uint> equipmentEnhancementLevels = new List<uint>();
            List<uint> equipmentQualityLevels = new List<uint>();
            List<uint> consumableQuantityLevels = new List<uint>();
            List<uint> costPerformanceLevels = new List<uint>();

            foreach (uint pawnId in pawnIds)
            {
                Pawn pawn = client.Character.Pawns.Find(p => p.PawnId == pawnId) ?? Server.Database.SelectPawn(pawnId);
                productionSpeedLevels.Add(CraftManager.GetPawnProductionSpeedLevel(pawn));
                equipmentEnhancementLevels.Add(CraftManager.GetPawnEquipmentEnhancementLevel(pawn));
                equipmentQualityLevels.Add(CraftManager.GetPawnEquipmentQualityLevel(pawn));
                consumableQuantityLevels.Add(CraftManager.GetPawnConsumableQuantityLevel(pawn));
                costPerformanceLevels.Add(CraftManager.GetPawnCostPerformanceLevel(pawn));
            }

            S2CCraftSkillAnalyzeRes craftSkillAnalyzeRes = new S2CCraftSkillAnalyzeRes();

            switch (packet.Structure.CraftType)
            {
                // uses different skills depending on recipe type
                // weapon/armor creation uses 3 / 5 skills: Production Speed, Equipment Quality, Cost Performance
                // usable item creation uses 3 / 5 skills: Production Speed, Consumable Quantity, Cost Performance
                // raw material creation uses 2 / 5 skills: Production Speed, Cost Performance
                // TODO: furniture/special item, currently recipes are missing, cannot check 
                case CraftType.CraftTypeCreate:
                    // TODO: could be more accurate/improved by figuring out craft category via item category
                    // ClientItemInfo.GetInfoForItemId(recipe.ItemID).Category
                    // Note that item id is 0 for craft type CREATE/UPGRADE/QUALITY
                    // Note that recipe id is 0 for craft type COLOR/ELEMENT
                    // ClientItemInfo item = ClientItemInfo.GetInfoForItemId(Server.AssetRepository.ClientItemInfos, packet.Structure.ItemId);
                    // CDataMDataCraftRecipe recipe = Server.AssetRepository.CraftingRecipesAsset
                    // .SelectMany(recipes => recipes.RecipeList)
                    // .Single(recipe => recipe.RecipeID == packet.Structure.RecipeId);
                    craftSkillAnalyzeRes.AnalyzeResultList.Add(AnalyzeProductionSpeed(productionSpeedLevels));
                    craftSkillAnalyzeRes.AnalyzeResultList.Add(AnalyzeEquipmentQuality(equipmentQualityLevels));
                    craftSkillAnalyzeRes.AnalyzeResultList.Add(AnalyzeConsumableQuantity(consumableQuantityLevels));
                    craftSkillAnalyzeRes.AnalyzeResultList.Add(AnalyzeCostPerformance(costPerformanceLevels));
                    break;
                // enhancement uses 2 / 5 skills: Equipment Enhancement, Cost Performance
                case CraftType.CraftTypeUpgrade:
                    craftSkillAnalyzeRes.AnalyzeResultList.Add(AnalyzeEquipmentEnhancement(equipmentEnhancementLevels));
                    craftSkillAnalyzeRes.AnalyzeResultList.Add(AnalyzeCostPerformance(costPerformanceLevels));
                    break;
                // element/crest uses 1 / 5 skills: Cost Performance
                case CraftType.CraftTypeElement:
                    // TODO: client times out for crest/color, maybe because assistants are not allowed and only a single result is expected?
                    //craftSkillAnalyzeRes.AnalyzeResultList.Add(AnalyzeCostPerformance(costPerformanceLevels));
                    break;
                // color uses 1 / 5 skills: Cost Performance
                case CraftType.CraftTypeColor:
                    // TODO: client times out for crest/color, maybe because assistants are not allowed and only a single result is expected?
                    //craftSkillAnalyzeRes.AnalyzeResultList.Add(AnalyzeCostPerformance(costPerformanceLevels));
                    break;
                // change quality uses 2 / 5 skills: Equipment Quality, Cost Performance
                case CraftType.CraftTypeQuality:
                    craftSkillAnalyzeRes.AnalyzeResultList.Add(AnalyzeEquipmentQuality(equipmentQualityLevels));
                    craftSkillAnalyzeRes.AnalyzeResultList.Add(AnalyzeCostPerformance(costPerformanceLevels));
                    break;
            }

            client.Send(craftSkillAnalyzeRes);
        }

        /// <summary>
        /// For Production Speed Value0 & Value1 are unused, Rate is equal to % of crafting time reduction shown in the UI as "Working hours 50% OFF"
        /// </summary>
        /// <param name="productionSpeedLevel"></param>
        /// <returns></returns>
        private CDataCraftSkillAnalyzeResult AnalyzeProductionSpeed(List<uint> productionSpeedLevel)
        {
            CDataCraftSkillAnalyzeResult productionSpeedAnalysisResult = new CDataCraftSkillAnalyzeResult
            {
                SkillType = CraftSkillType.ProductionSpeed,
                Rate = (byte)Server.CraftManager.GetCraftingTimeReductionRate(productionSpeedLevel)
            };
            return productionSpeedAnalysisResult;
        }

        /// <summary>
        /// For Equipment Quality Value0 & Value1 are unused, Rate is equal to % of success rate shown in the UI as "Success rate: 64%"
        /// </summary>
        /// <param name="equipmentQualityLevel"></param>
        /// <returns></returns>
        private CDataCraftSkillAnalyzeResult AnalyzeEquipmentQuality(List<uint> equipmentQualityLevel)
        {
            CDataCraftSkillAnalyzeResult equipmentQualityAnalysisResult = new CDataCraftSkillAnalyzeResult
            {
                SkillType = CraftSkillType.EquipmentQuality,
                Rate = (byte)CraftManager.CalculateEquipmentQualityIncreaseRate(equipmentQualityLevel)
            };
            return equipmentQualityAnalysisResult;
        }

        /// <summary>
        /// For Equip Enhancement Rate is unused, Value0 and Value1 indicate a range where Value0 is the lower bound and Value1 the upper bound shown in the UI as "Value~Value1 pt"
        /// </summary>
        /// <param name="equipmentEnhancementLevels"></param>
        /// <returns></returns>
        private CDataCraftSkillAnalyzeResult AnalyzeEquipmentEnhancement(List<uint> equipmentEnhancementLevels)
        {
            CDataCraftSkillAnalyzeResult equipEnhancementAnalysisResult = new CDataCraftSkillAnalyzeResult
            {
                SkillType = CraftSkillType.EquipmentEnhancement,
                Value0 = (uint)CraftManager.GetEquipmentEnhancementPoints(equipmentEnhancementLevels),
                Value1 = (uint)CraftManager.GetEquipmentEnhancementPointsGreatSuccess(equipmentEnhancementLevels)
            };
            return equipEnhancementAnalysisResult;
        }

        /// <summary>
        /// For Consumable Quantity Value1 is unused, Value0 is equal to additional quantity/pieces, Rate is equal to % of occurrence chance shown in the UI as "34% chance of + 3 pieces"
        /// </summary>
        /// <param name="consumableQuantityLevels"></param>
        /// <returns></returns>
        private CDataCraftSkillAnalyzeResult AnalyzeConsumableQuantity(List<uint> consumableQuantityLevels)
        {
            CDataCraftSkillAnalyzeResult consumableQuantityAnalysisResult = new CDataCraftSkillAnalyzeResult
            {
                SkillType = CraftSkillType.ConsumableQuantity,
                Rate = (byte)CraftManager.GetAdditionalConsumableQuantityRate(consumableQuantityLevels),
                Value0 = (byte)CraftManager.GetAdditionalConsumableQuantityMaximum(consumableQuantityLevels)
            };
            return consumableQuantityAnalysisResult;
        }

        /// <summary>
        /// For Cost Performance Value0 & Value1 are unused, Rate is equal to % of cost reduction shown in the UI as "Cost 13% OFF"
        /// </summary>
        /// <param name="costPerformanceLevels"></param>
        /// <returns></returns>
        private CDataCraftSkillAnalyzeResult AnalyzeCostPerformance(List<uint> costPerformanceLevels)
        {
            CDataCraftSkillAnalyzeResult costPerformanceAnalysisResult = new CDataCraftSkillAnalyzeResult
            {
                SkillType = CraftSkillType.CostPerformance,
                Rate = (byte)Server.CraftManager.GetCraftCostReductionRate(costPerformanceLevels)
            };
            return costPerformanceAnalysisResult;
        }
    }
}
