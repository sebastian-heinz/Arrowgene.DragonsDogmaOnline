using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Craft;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftSkillAnalyzeHandler : GameRequestPacketHandler<C2SCraftSkillAnalyzeReq, S2CCraftSkillAnalyzeRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftSkillAnalyzeHandler));

        public CraftSkillAnalyzeHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CCraftSkillAnalyzeRes Handle(GameClient client, C2SCraftSkillAnalyzeReq request)
        {
            Pawn leadPawn = Server.CraftManager.FindPawn(client, request.PawnId);

            List<CraftPawn> craftPawns = new()
            {
                new CraftPawn(leadPawn, CraftPosition.Leader)
            };
            craftPawns.AddRange(request.AssistPawnIds.Select(p => new CraftPawn(Server.CraftManager.FindPawn(client, p.Value), CraftPosition.Assistant)));


            S2CCraftSkillAnalyzeRes craftSkillAnalyzeRes = new S2CCraftSkillAnalyzeRes();

            switch (request.CraftType)
            {
                // uses different skills depending on recipe type
                // weapon/armor creation uses 3 / 5 skills: Production Speed, Equipment Quality, Cost Performance
                // usable item creation uses 3 / 5 skills: Production Speed, Consumable Quantity, Cost Performance
                // raw material creation uses 2 / 5 skills: Production Speed, Cost Performance
                // TODO: furniture/special item, currently recipes are missing, cannot check 

                // TODO: could be more accurate/improved by figuring out craft category via item category
                // ClientItemInfo.GetInfoForItemId(recipe.ItemID).Category
                // Note that item id is 0 for craft type CREATE/UPGRADE/QUALITY
                // Note that recipe id is 0 for craft type COLOR/ELEMENT
                case CraftType.CraftTypeCreate:
                    {
                        CDataMDataCraftRecipe recipe = Server.AssetRepository.CraftingRecipesAsset
                            .SelectMany(recipes => recipes.RecipeList)
                            .Single(recipe => recipe.RecipeID == request.RecipeId)
                            .AsCData();

                        ClientItemInfo itemInfo = ClientItemInfo.GetInfoForItemId(Server.AssetRepository.ClientItemInfos, recipe.ItemID);

                        craftSkillAnalyzeRes.AnalyzeResultList.Add(AnalyzeProductionSpeed(recipe.Time, itemInfo, craftPawns));
                        craftSkillAnalyzeRes.AnalyzeResultList.Add(AnalyzeCostPerformance(itemInfo, craftPawns));

                        if (itemInfo.StackLimit > 1)
                        {
                            craftSkillAnalyzeRes.AnalyzeResultList.Add(AnalyzeConsumableQuantity(itemInfo, craftPawns));
                        }
                        else if (!CraftStartCraftHandler.NoQualitySubCategories.Contains(itemInfo.SubCategory))
                        {
                            craftSkillAnalyzeRes.AnalyzeResultList.Add(AnalyzeEquipmentQuality(itemInfo, craftPawns));
                        }

                        break;
                    }
                // enhancement uses 2 / 5 skills: Equipment Enhancement, Cost Performance
                case CraftType.CraftTypeUpgrade:
                    {
                        CDataMDataCraftGradeupRecipe recipe = Server.AssetRepository.CraftingGradeUpRecipesAsset
                            .SelectMany(recipes => recipes.RecipeList)
                            .First(recipe => recipe.ItemID == request.ItemId);

                        ClientItemInfo itemInfo = ClientItemInfo.GetInfoForItemId(Server.AssetRepository.ClientItemInfos, recipe.ItemID);

                        craftSkillAnalyzeRes.AnalyzeResultList.Add(AnalyzeEquipmentEnhancement(itemInfo, craftPawns));
                        craftSkillAnalyzeRes.AnalyzeResultList.Add(AnalyzeCostPerformance(itemInfo, craftPawns));
                        break;
                    }
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
                    {
                        // The client returns the itemID being modified in the recipeID field.
                        ClientItemInfo itemInfo = ClientItemInfo.GetInfoForItemId(Server.AssetRepository.ClientItemInfos, request.RecipeId);

                        craftSkillAnalyzeRes.AnalyzeResultList.Add(AnalyzeEquipmentQuality(itemInfo, craftPawns));
                        craftSkillAnalyzeRes.AnalyzeResultList.Add(AnalyzeCostPerformance(itemInfo, craftPawns));
                        break;
                    }
            }

            return craftSkillAnalyzeRes;
        }

        /// <summary>
        /// For Production Speed Value0 & Value1 are unused, Rate is equal to % of crafting time reduction shown in the UI as "Working hours 50% OFF"
        /// </summary>
        /// <param name="productionSpeedLevel"></param>
        /// <returns></returns>
        private CDataCraftSkillAnalyzeResult AnalyzeProductionSpeed(uint recipeTime, ClientItemInfo itemInfo, List<CraftPawn> craftPawns)
        {
            uint reducedTime = Server.CraftManager.CalculateRecipeProductionSpeed(recipeTime, itemInfo, craftPawns);
            byte timeFactor = 0;
            if (reducedTime > recipeTime)
            {
                Logger.Error($"Overflow detected in [AnalyzeProductionSpeed]: {reducedTime} > {recipeTime} when crafting {itemInfo.Name}");
                timeFactor = 100;
            }
            else
            {
                uint deltaTime = recipeTime - reducedTime;
                timeFactor = (byte)(deltaTime * 100.0 / reducedTime);
            }

            CDataCraftSkillAnalyzeResult productionSpeedAnalysisResult = new CDataCraftSkillAnalyzeResult
            {
                SkillType = CraftSkillType.ProductionSpeed,
                Rate = timeFactor
            };
            return productionSpeedAnalysisResult;
        }

        /// <summary>
        /// For Equipment Quality Value0 & Value1 are unused, Rate is equal to % of success rate shown in the UI as "Success rate: 64%"
        /// </summary>
        /// <param name="equipmentQualityLevel"></param>
        /// <returns></returns>
        private CDataCraftSkillAnalyzeResult AnalyzeEquipmentQuality(ClientItemInfo itemInfo, List<CraftPawn> craftPawns)
        {
            CDataCraftSkillAnalyzeResult equipmentQualityAnalysisResult = new CDataCraftSkillAnalyzeResult
            {
                SkillType = CraftSkillType.EquipmentQuality,
                Rate = (byte)CraftManager.CalculateEquipmentQualityIncreaseRate(craftPawns)
            };
            return equipmentQualityAnalysisResult;
        }

        /// <summary>
        /// For Equip Enhancement Rate is unused, Value0 and Value1 indicate a range where Value0 is the lower bound and Value1 the upper bound shown in the UI as "Value~Value1 pt"
        /// </summary>
        /// <param name="equipmentEnhancementLevels"></param>
        /// <returns></returns>
        private CDataCraftSkillAnalyzeResult AnalyzeEquipmentEnhancement(ClientItemInfo itemInfo, List<CraftPawn> craftPawns)
        {
            CDataCraftSkillAnalyzeResult equipEnhancementAnalysisResult = new CDataCraftSkillAnalyzeResult
            {
                SkillType = CraftSkillType.EquipmentEnhancement,
                Value0 = (uint)CraftManager.GetEquipmentEnhancementPoints(craftPawns),
                Value1 = (uint)CraftManager.GetEquipmentEnhancementPointsGreatSuccess(craftPawns)
            };
            return equipEnhancementAnalysisResult;
        }

        /// <summary>
        /// For Consumable Quantity Value1 is unused, Value0 is equal to additional quantity/pieces, Rate is equal to % of occurrence chance shown in the UI as "34% chance of + 3 pieces"
        /// </summary>
        /// <param name="consumableQuantityLevels"></param>
        /// <returns></returns>
        private CDataCraftSkillAnalyzeResult AnalyzeConsumableQuantity(ClientItemInfo itemInfo, List<CraftPawn> craftPawns)
        {
            CDataCraftSkillAnalyzeResult consumableQuantityAnalysisResult = new CDataCraftSkillAnalyzeResult
            {
                SkillType = CraftSkillType.ConsumableQuantity,
                Rate = (byte)CraftManager.GetAdditionalConsumableQuantityRate(craftPawns),
                Value0 = (byte)CraftManager.GetAdditionalConsumableQuantityMaximum(craftPawns)
            };
            return consumableQuantityAnalysisResult;
        }

        /// <summary>
        /// For Cost Performance Value0 & Value1 are unused, Rate is equal to % of cost reduction shown in the UI as "Cost 13% OFF"
        /// </summary>
        /// <param name="costPerformanceLevels"></param>
        /// <returns></returns>
        private CDataCraftSkillAnalyzeResult AnalyzeCostPerformance(ClientItemInfo itemInfo, List<CraftPawn> craftPawns)
        {
            CDataCraftSkillAnalyzeResult costPerformanceAnalysisResult = new CDataCraftSkillAnalyzeResult
            {
                SkillType = CraftSkillType.CostPerformance,
                Rate = (byte)(100.0 - Server.CraftManager.GetCraftCostReductionRate(itemInfo, craftPawns) * 100.0)
            };
            return costPerformanceAnalysisResult;
        }
    }
}
