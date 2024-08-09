using System;
using System.Collections.Generic;
using System.Linq;
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

            Dictionary<uint, uint> craftRankAndProductionSpeedLevel = new Dictionary<uint, uint>();
            Dictionary<uint, uint> craftRankAndEquipmentEnhancementLevel = new Dictionary<uint, uint>();
            Dictionary<uint, uint> craftRankAndEquipmentQualityLevel = new Dictionary<uint, uint>();
            Dictionary<uint, uint> craftRankAndConsumableQuantityLevel = new Dictionary<uint, uint>();
            Dictionary<uint, uint> craftRankAndCostPerformanceLevel = new Dictionary<uint, uint>();

            foreach (uint pawnId in pawnIds)
            {
                Pawn pawn = client.Character.Pawns.Find(p => p.PawnId == pawnId) ?? Server.Database.SelectPawn(pawnId);
                craftRankAndProductionSpeedLevel.Add(pawn.CraftData.CraftRank, pawn.CraftData.PawnCraftSkillList.Find(skill => skill.Type == CraftSkillType.ProductionSpeed).Level);
                craftRankAndEquipmentEnhancementLevel.Add(pawn.CraftData.CraftRank,
                    pawn.CraftData.PawnCraftSkillList.Find(skill => skill.Type == CraftSkillType.EquipmentEnhancement).Level);
                craftRankAndEquipmentQualityLevel.Add(pawn.CraftData.CraftRank,
                    pawn.CraftData.PawnCraftSkillList.Find(skill => skill.Type == CraftSkillType.EquipmentQuality).Level);
                craftRankAndConsumableQuantityLevel.Add(pawn.CraftData.CraftRank,
                    pawn.CraftData.PawnCraftSkillList.Find(skill => skill.Type == CraftSkillType.ConsumableQuantity).Level);
                craftRankAndCostPerformanceLevel.Add(pawn.CraftData.CraftRank, pawn.CraftData.PawnCraftSkillList.Find(skill => skill.Type == CraftSkillType.CostPerformance).Level);
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
                    craftSkillAnalyzeRes.AnalyzeResultList.Add(AnalyzeProductionSpeed(craftRankAndProductionSpeedLevel));
                    craftSkillAnalyzeRes.AnalyzeResultList.Add(AnalyzeEquipmentQuality(craftRankAndEquipmentQualityLevel));
                    craftSkillAnalyzeRes.AnalyzeResultList.Add(AnalyzeConsumableQuantity(craftRankAndConsumableQuantityLevel));
                    craftSkillAnalyzeRes.AnalyzeResultList.Add(AnalyzeCostPerformance(craftRankAndCostPerformanceLevel));
                    break;
                // enhancement uses 2 / 5 skills: Equipment Enhancement, Cost Performance
                case CraftType.CraftTypeUpgrade:
                    craftSkillAnalyzeRes.AnalyzeResultList.Add(AnalyzeEquipEnhancement(craftRankAndEquipmentEnhancementLevel));
                    craftSkillAnalyzeRes.AnalyzeResultList.Add(AnalyzeCostPerformance(craftRankAndCostPerformanceLevel));
                    break;
                // element/crest uses 1 / 5 skills: Cost Performance
                case CraftType.CraftTypeElement:
                    // TODO: client times out for crest/color, maybe because assistants are not allowed and only a single result is expected?
                    //craftSkillAnalyzeRes.AnalyzeResultList.Add(AnalyzeCostPerformance(craftRankAndCostPerformanceLevel));
                    break;
                // color uses 1 / 5 skills: Cost Performance
                case CraftType.CraftTypeColor:
                    // TODO: client times out for crest/color, maybe because assistants are not allowed and only a single result is expected?
                    //craftSkillAnalyzeRes.AnalyzeResultList.Add(AnalyzeCostPerformance(craftRankAndCostPerformanceLevel));
                    break;
                // change quality uses 2 / 5 skills: Equipment Quality, Cost Performance
                case CraftType.CraftTypeQuality:
                    craftSkillAnalyzeRes.AnalyzeResultList.Add(AnalyzeEquipmentQuality(craftRankAndEquipmentQualityLevel));
                    craftSkillAnalyzeRes.AnalyzeResultList.Add(AnalyzeCostPerformance(craftRankAndCostPerformanceLevel));
                    break;
            }

            client.Send(craftSkillAnalyzeRes);
        }

        private static CDataCraftSkillAnalyzeResult AnalyzeProductionSpeed(Dictionary<uint, uint> craftRankAndProductionSpeedLevel)
        {
            // For Production Speed Value0 & Value1 are unused, Rate is equal to % of crafting time reduction
            // shown in the UI as "Working hours 50% OFF"
            // TODO: Figure out actual formula + lower/upper bounds client uses
            //  4x lvl 1 => 1% 
            CDataCraftSkillAnalyzeResult productionSpeedAnalysisResult = new CDataCraftSkillAnalyzeResult
            {
                SkillType = CraftSkillType.ProductionSpeed,
                Rate = (byte)Math.Clamp(craftRankAndProductionSpeedLevel.Select(d => Math.Floor((d.Key + d.Value) * 1.5)).Sum(), 0, 100),
                Value0 = 0,
                Value1 = 0
            };
            return productionSpeedAnalysisResult;
        }

        private static CDataCraftSkillAnalyzeResult AnalyzeEquipmentQuality(Dictionary<uint, uint> craftRankAndEquipmentQualityLevel)
        {
            // For Equipment Quality Value0 & Value1 are unused, Rate is equal to % of success rate
            // shown in the UI as "Success rate: 64%"
            // TODO: Figure out actual formula + lower/upper bounds client uses
            // Based on season 1 evidence:
            //  3x lvl 45 1x lvl 44 => 79% 
            // 1x lvl44, 3x lvl 1 => 63%
            CDataCraftSkillAnalyzeResult equipmentQualityAnalysisResult = new CDataCraftSkillAnalyzeResult
            {
                SkillType = CraftSkillType.EquipmentQuality,
                Rate = (byte)Math.Clamp(craftRankAndEquipmentQualityLevel.Select(d => Math.Floor((d.Key + d.Value) / 10f)).Sum(), 0, 100),
                Value0 = 0,
                Value1 = 0
            };
            return equipmentQualityAnalysisResult;
        }

        private static CDataCraftSkillAnalyzeResult AnalyzeEquipEnhancement(Dictionary<uint, uint> craftRankAndEquipmentEnhancementLevel)
        {
            // For Equip Enhancement Rate is unused, Value0 and Value1 indicate a range where Value0 is the lower bound and Value1 the upper bound
            // shown in the UI as "Value~Value1 pt"
            // TODO: Figure out actual formula + lower/upper bounds client uses
            // Based on season 1 evidence:
            //  4x lvl 45 => min 390 / max 468 pt
            //  1x lvl 45 + 3x lvl 1 => min 266 / max 319 pt
            CDataCraftSkillAnalyzeResult equipEnhancementAnalysisResult = new CDataCraftSkillAnalyzeResult
            {
                SkillType = CraftSkillType.EquipmentEnhancement,
                Rate = 0,
                Value0 = (uint)Math.Clamp(craftRankAndEquipmentEnhancementLevel.Select(d => d.Value * 2.16).Sum(), 0, 605),
                Value1 = (uint)Math.Clamp(craftRankAndEquipmentEnhancementLevel.Select(d => d.Value * 2.6).Sum(), 0, 728),
            };
            return equipEnhancementAnalysisResult;
        }

        private static CDataCraftSkillAnalyzeResult AnalyzeConsumableQuantity(Dictionary<uint, uint> craftRankAndConsumableQuantityLevel)
        {
            // For Consumable Quantity Value1 is unused, Value0 is equal to additional quantity/pieces, Rate is equal to % of occurrence chance
            // shown in the UI as "34% chance of + 3 pieces"
            // TODO: Figure out actual formula + lower/upper bounds client uses
            CDataCraftSkillAnalyzeResult consumableQuantityAnalysisResult = new CDataCraftSkillAnalyzeResult
            {
                SkillType = CraftSkillType.ConsumableQuantity,
                Rate = (byte)Math.Clamp(craftRankAndConsumableQuantityLevel.Select(d => Math.Floor((d.Key + d.Value) / 10f)).Sum(), 0, 100),
                Value0 = (byte)Math.Clamp(craftRankAndConsumableQuantityLevel.Select(d => Math.Floor(d.Value / 10f)).Sum(), 0, 100),
                Value1 = 0
            };
            return consumableQuantityAnalysisResult;
        }

        private static CDataCraftSkillAnalyzeResult AnalyzeCostPerformance(Dictionary<uint, uint> craftRankAndCostPerformanceLevel)
        {
            // For Cost Performance Value0 & Value1 are unused, Rate is equal to % of cost reduction
            // shown in the UI as "Cost 13% OFF"
            // TODO: Figure out actual formula + lower/upper bounds client uses
            // Based on season 1 evidence:
            //  1x lvl 2, 3x lvl 1 == 16% maximum
            CDataCraftSkillAnalyzeResult costPerformanceAnalysisResult = new CDataCraftSkillAnalyzeResult
            {
                SkillType = CraftSkillType.CostPerformance,
                Rate = (byte)Math.Clamp(craftRankAndCostPerformanceLevel.Select(d => Math.Floor((d.Key + d.Value) / 5f)).Sum(), 0, 100),
                Value0 = 0,
                Value1 = 0
            };
            return costPerformanceAnalysisResult;
        }

        private static float MapToRange(float value, float oldMin, float oldMax, float newMin, float newMax)
        {
            return newMin + (value - oldMin) * (newMax - newMin) / (oldMax - oldMin);
        }
    }
}
