using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Scripting.Interfaces;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.GameServer.Quests.LightQuests
{
    public class LightQuestManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(LightQuestManager));

        private readonly DdonGameServer Server;

        private readonly Dictionary<QuestAreaId, LightQuestAreaHuntSummary> EnemySummaries = Enum.GetValues<QuestAreaId>().ToDictionary(x => x, x => new LightQuestAreaHuntSummary(x));
        private readonly Dictionary<QuestAreaId, LightQuestAreaDeliverySummary> ItemSummaries = Enum.GetValues<QuestAreaId>().ToDictionary(x => x, x => new LightQuestAreaDeliverySummary(x));

        private readonly HashSet<uint> DeliverableItems;

        /// <summary>
        /// Scales the likelihood of rolling delivery quests for materials that drop rather than are gathered.
        /// </summary>
        private static readonly double DROP_MATERIAL_FACTOR = 0.5;

        /// <summary>
        /// Attempts to generate a quest that meets requirements on min/max levels. Will warn when exceeded.
        /// </summary>
        private static readonly int GENERATOR_ATTEMPTS_PER_QUEST = 20;

        public LightQuestManager(DdonGameServer server)
        {
            Server = server;
            DeliverableItems = ParseGatherableItems(server);

            ParseStagesByQuestAreaId();
            ParseWorldQuestEnemies();
            ParseLestaniaSpots();

            foreach (var summary in EnemySummaries.Values)
            {
                summary.Summarize();
            }
        }

        private static HashSet<uint> ParseGatherableItems(DdonGameServer server)
        {
            var craftableItems = server.AssetRepository.CraftingRecipesAsset.SelectMany(x => x.RecipeList).Select(x => x.ItemID).ToHashSet();
            return server.AssetRepository.ClientItemInfos.Values.Where(x => DeliverableSubCategories.Contains(x.SubCategory ?? 0) && !craftableItems.Contains(x.ItemId)).Select(x => x.ItemId).ToHashSet();
        }

        public void ParseStagesByQuestAreaId()
        {
            foreach (var (stage, enemies) in Server.AssetRepository.EnemySpawnAsset.Enemies)
            {
                StageInfo stageInfo = Stage.StageInfoFromStageLayoutId(stage);
                QuestAreaId areaId = stageInfo.AreaId;

                if (areaId == QuestAreaId.None)
                {
                    // This skips Lestania as well as several debug stages.
                    continue;
                }
                
                if (SpecialStageIds.Contains(stage.Id))
                {
                    // Skips certain quest stages that really shouldn't have regular spawns anyways.
                    continue;
                }

                var (huntSummary, deliverySummary) = GetSummary(areaId);

                foreach (var enemy in enemies)
                {
                    HandleEnemy(enemy, huntSummary, deliverySummary);
                }
            }

            foreach (var ((stage, group), items) in Server.AssetRepository.GatheringItems)
            {
                StageInfo stageInfo = Stage.StageInfoFromStageLayoutId(stage);
                QuestAreaId areaId = stageInfo.AreaId;

                if (areaId == QuestAreaId.None)
                {
                    // This skips Lestania as well as several debug stages.
                    continue;
                }

                if (SpecialStageIds.Contains(stage.Id))
                {
                    // Skips certain quest stages that really shouldn't have regular spawns anyways.
                    continue;
                }

                var (huntSummary, deliverySummary) = GetSummary(areaId);

                foreach (var item in items)
                {
                    if (DeliverableItems.Contains((uint)item.ItemId))
                    {
                        deliverySummary.AddItem(item);
                    }
                }
            }
        }

        public void ParseWorldQuestEnemies()
        {
            var quests = QuestManager.GetQuestsByType(QuestType.World);
            foreach (var quest in quests)
            {
                var worldQuest = QuestManager.GetQuestByScheduleId(quest);
                foreach (var enemyGroup in worldQuest.EnemyGroups.Values)
                {
                    // Enemies are assigned boards based on where they're located, not what area the world quest may or may not be associated with.
                    var (huntSummary, deliverySummary) = GetSummary(enemyGroup.StageLayoutId);
                        
                    foreach (var enemy in enemyGroup.Enemies)
                    {
                        HandleEnemy(enemy, huntSummary, deliverySummary);
                    }
                }
            }
        }

        public void ParseLestaniaSpots()
        {
            // Invert the asset mapping
            Dictionary<uint, QuestAreaId> invertedEnemyMap = Server.AssetRepository.LightQuestAsset.LestaniaEnemyNodes
                .SelectMany(x => x.Value.Select(y => (NodeId: y, AreaId: x.Key)))
                .ToDictionary(k => k.NodeId, v => v.AreaId);

            foreach (var (stage, enemies) in Server.AssetRepository.EnemySpawnAsset.Enemies.Where(x => x.Key.Id == Stage.Lestania.StageId))
            {
                QuestAreaId areaId = invertedEnemyMap.GetValueOrDefault(stage.GroupId);

                if (areaId == QuestAreaId.None)
                {
                    Logger.Debug($"Skipping Lestania Enemy Node {stage}; not assigned a questAreaId.");
                    continue;
                }

                var (huntSummary, deliverySummary) = GetSummary(areaId);

                foreach (var enemy in enemies)
                {
                    HandleEnemy(enemy, huntSummary, deliverySummary);
                }
            }

            // Invert the asset mapping
            Dictionary<uint, QuestAreaId> invertedGatheringMap = Server.AssetRepository.LightQuestAsset.LestaniaGatheringNodes
                .SelectMany(x => x.Value.Select(y => (NodeId: y, AreaId: x.Key)))
                .ToDictionary(k => k.NodeId, v => v.AreaId);

            foreach (var ((stage, group), items) in Server.AssetRepository.GatheringItems.Where(x => x.Key.Item1.Id == Stage.Lestania.StageId))
            {
                QuestAreaId areaId = invertedGatheringMap.GetValueOrDefault(stage.GroupId);

                if (areaId == QuestAreaId.None)
                {
                    Logger.Debug($"Skipping Lestania Gathering Node {stage}; not assigned a questAreaId.");
                    continue;
                }

                var (huntSummary, deliverySummary) = GetSummary(areaId);

                foreach (var item in items)
                {
                    if (DeliverableItems.Contains((uint)item.ItemId))
                    {
                        deliverySummary.AddItem(item);
                    }
                }
            }
        }

        public Quest GenerateQuestFromRecord(LightQuestRecord record)
        {
            LightQuestInfo info = LightQuestId.FromQuestId(record.QuestId);
            return info.Type switch
            {
                LightQuestType.Hunt => new LightQuestHuntQuest(record).GenerateQuest(Server),
                LightQuestType.Delivery => new LightQuestDeliveryQuest(record).GenerateQuest(Server),
                _ => throw new Exception($"Invalid light quest type {info.Type} for record q{record.QuestId}:s{record.QuestScheduleId}"),
            };
        }

        public List<LightQuestRecord> GenerateRecordsFromAsset(LightQuestGeneratingAsset asset)
        {
            List<LightQuestInfo> lightQuestInfos = LightQuestId.FromBoardId(asset.BoardId)
                .Where(x => x.Type == asset.Type)
                .Where(x => (x.IsAreaOrder && asset.AllowAreaOrders) || (!x.IsAreaOrder && asset.AllowNormalQuests))
                .OrderBy(x => Random.Shared.Next())
                .ToList();

            int nQuests = Random.Shared.Next(asset.MinQuests, asset.MaxQuests + 1);

            List<LightQuestRecord> records = new();
            for (int i = 0; i < nQuests; i++)
            {
                if (lightQuestInfos.Count == 0)
                {
                    Logger.Error($"Error in generator {asset.Name}; not enough unique quest ids.");
                    break;
                }

                LightQuestRecord newRecord = null;
                switch (asset.Type)
                {
                    case LightQuestType.Hunt:
                        {
                            newRecord = RollHuntQuest(lightQuestInfos.First(), asset, records);
                            break;
                        }
                    case LightQuestType.Delivery:
                        {
                            newRecord = RollDeliveryQuest(lightQuestInfos.First(), asset, records);
                            break;
                        }
                    default:
                        break;
                }

                if (newRecord is not null)
                {
                    records.Add(newRecord);
                    lightQuestInfos.RemoveAt(0);
                }
            }

            return records;
        }

        private LightQuestRecord RollHuntQuest(LightQuestInfo quest, LightQuestGeneratingAsset generatingAsset, List<LightQuestRecord> extantQuests)
        {
            if (generatingAsset.Type != LightQuestType.Hunt)
            {
                return null;
            }

            LightQuestRecord finalRecord = null;

            var summary = EnemySummaries[quest.AreaId];
            HashSet<EnemyUIId> rolledEnemyIds = extantQuests.Select(x => (EnemyUIId)x.Target).ToHashSet();

            int attempt = 0;
            while (attempt < GENERATOR_ATTEMPTS_PER_QUEST)
            {
                List<(EnemyUIId, ushort)> biasRolls = [];
                for (int biasRoll = 0; biasRoll <= generatingAsset.BiasRerolls; biasRoll++)
                {
                    var (rollEnemy, rollLevel) = summary.Roll(rolledEnemyIds);
                    if (rollEnemy == 0)
                    {
                        Logger.Error($"Error in generator {generatingAsset.Name}; no valid enemies for region.");
                        break;
                    }
                    biasRolls.Add((rollEnemy, rollLevel));
                }

                if (biasRolls.Count == 0)
                {
                    Logger.Error($"Error in generator {generatingAsset.Name}; could not generate any hunt targets.");
                    attempt = GENERATOR_ATTEMPTS_PER_QUEST;
                    break;
                }

                if (generatingAsset.BiasLower)
                {
                    biasRolls = biasRolls.OrderBy(x => x.Item2).ToList();
                }
                else
                {
                    biasRolls = biasRolls.OrderByDescending(x => x.Item2).ToList();
                }

                var chosenRoll = biasRolls.First();

                if (generatingAsset.MinLevel <= chosenRoll.Item2 && chosenRoll.Item2 <= generatingAsset.MaxLevel)
                {
                    var mixin = Server.ScriptManager.MixinModule.Get<ILightQuestRewardMixin>("light_hunt_quest_reward");

                    finalRecord = new LightQuestRecord()
                    {
                        QuestId = quest.QuestId,
                        Target = (int)chosenRoll.Item1,
                        Level = chosenRoll.Item2,
                        Count = Random.Shared.Next(generatingAsset.MinCount, generatingAsset.MaxCount + 1),
                        DistributionStart = DateTimeOffset.UtcNow,
                        DistributionEnd = DateTimeOffset.UtcNow.AddDays(1),
                        DiscardDate = DateTimeOffset.UtcNow.AddDays(7)
                    };

                    var chosenRecord = summary.HuntRecords[chosenRoll.Item1];
                    var bossFactor = (double)chosenRecord.BossCount / chosenRecord.Count;

                    finalRecord.RewardG = mixin.CalculateRewardG(finalRecord);
                    finalRecord.RewardR = mixin.CalculateRewardR(finalRecord);
                    finalRecord.RewardXP = mixin.CalculateRewardXP(finalRecord);

                    Logger.Info($"Generating {generatingAsset.Name} | \"{quest.Name}\" -> {chosenRoll.Item1} x{finalRecord.Count} (Lv. {chosenRoll.Item2})");
                    break;
                }

                attempt++;
            }

            return finalRecord;
        }

        private LightQuestRecord RollDeliveryQuest(LightQuestInfo quest, LightQuestGeneratingAsset generatingAsset, List<LightQuestRecord> extantQuests)
        {
            if (generatingAsset.Type != LightQuestType.Delivery)
            {
                return null;
            }

            LightQuestRecord finalRecord = null;

            var summary = ItemSummaries[quest.AreaId];
            HashSet<ItemId> rolledItemIds = extantQuests.Select(x => (ItemId)x.Target).ToHashSet();

            int attempt = 0;
            while (attempt < GENERATOR_ATTEMPTS_PER_QUEST)
            {
                List<(ItemId, ushort)> biasRolls = [];
                for (int biasRoll = 0; biasRoll <= generatingAsset.BiasRerolls; biasRoll++)
                {
                    var (rollItem, rollLevel) = summary.Roll(Server, rolledItemIds);
                    if (rollItem == 0)
                    {
                        Logger.Error($"Error in generator {generatingAsset.Name}; no valid items for region.");
                        break;
                    }
                    biasRolls.Add((rollItem, rollLevel));
                }

                if (biasRolls.Count == 0)
                {
                    Logger.Error($"Error in generator {generatingAsset.Name}; could not generate any delivery targets.");
                    attempt = GENERATOR_ATTEMPTS_PER_QUEST;
                    break;
                }

                if (generatingAsset.BiasLower)
                {
                    biasRolls = biasRolls.OrderBy(x => x.Item2).ToList();
                }
                else
                {
                    biasRolls = biasRolls.OrderByDescending(x => x.Item2).ToList();
                }

                var chosenRoll = biasRolls.First();

                if (generatingAsset.MinLevel <= chosenRoll.Item2 && chosenRoll.Item2 <= generatingAsset.MaxLevel)
                {
                    var mixin = Server.ScriptManager.MixinModule.Get<ILightQuestRewardMixin>("light_delivery_quest_reward");

                    finalRecord = new LightQuestRecord()
                    {
                        QuestId = quest.QuestId,
                        Target = (int)chosenRoll.Item1,
                        Level = chosenRoll.Item2,
                        Count = Random.Shared.Next(generatingAsset.MinCount, generatingAsset.MaxCount + 1),
                        DistributionStart = DateTimeOffset.UtcNow,
                        DistributionEnd = DateTimeOffset.UtcNow.AddDays(1),
                        DiscardDate = DateTimeOffset.UtcNow.AddDays(7)
                    };

                    finalRecord.RewardG = mixin.CalculateRewardG(finalRecord);
                    finalRecord.RewardR = mixin.CalculateRewardR(finalRecord);
                    finalRecord.RewardXP = mixin.CalculateRewardXP(finalRecord);

                    Logger.Info($"Generating {generatingAsset.Name} | \"{quest.Name}\" -> {chosenRoll.Item1} x{finalRecord.Count} (Lv. {chosenRoll.Item2})");
                    break;
                }

                attempt++;
            }

            return finalRecord;
        }

        private (LightQuestAreaHuntSummary HuntSummary, LightQuestAreaDeliverySummary DeliverySummary) GetSummary(QuestAreaId areaId)
        {
            if (!EnemySummaries.TryGetValue(areaId, out LightQuestAreaHuntSummary huntSummary))
            {
                huntSummary = new(areaId);
                EnemySummaries[areaId] = huntSummary;
            }

            if (!ItemSummaries.TryGetValue(areaId, out LightQuestAreaDeliverySummary deliverySummary))
            {
                deliverySummary = new(areaId);
                ItemSummaries[areaId] = deliverySummary;
            }

            return (huntSummary, deliverySummary);
        }

        private (LightQuestAreaHuntSummary HuntSummary, LightQuestAreaDeliverySummary DeliverySummary) GetSummary(StageLayoutId stageLayoutId)
        {
            StageInfo stageInfo = Stage.StageInfoFromStageLayoutId(stageLayoutId);
            QuestAreaId areaId = stageInfo.AreaId;
            return GetSummary(areaId);
        }

        private void HandleEnemy(Enemy enemy, LightQuestAreaHuntSummary huntSummary, LightQuestAreaDeliverySummary deliverySummary)
        {
            if (InvalidEnemies.Contains(enemy.UINameId))
            {
                return;
            }

            huntSummary.AddEnemy(enemy);
            foreach (var item in enemy.DropsTable.Items)
            {
                if (DeliverableItems.Contains((uint)item.ItemId))
                {
                    deliverySummary.AddItem(item, DROP_MATERIAL_FACTOR);
                }
            }
        }

        private static readonly HashSet<uint> SpecialStageIds = new HashSet<StageInfo>()
        {
            Stage.DemoRoom,
            Stage.BattlefieldOutsideGrittenFort,
            Stage.BonusDungeonBO,
            Stage.BonusDungeonG,
            Stage.BonusDungeonHO,
            Stage.BonusDungeonJP,
            Stage.BonusDungeonLobby,
            Stage.BonusDungeonR,
            Stage.BonusDungeonSP,
            Stage.BonusDungeonXP,
            Stage.BreyaCoast,
            Stage.DarkTreeGreatCatacombs,
            Stage.DarknessShroudedDreedCastle0,
            Stage.DarknessShroudedMegadoCorridor0,
            Stage.DarknessShroudedMergodaRoyalPalaceLevel0,
            Stage.DarknessShroudedMergodaSecurityDistrict0,
            Stage.DarknessShroudedShadoleanGreatTemple0,
            Stage.EastLandofDarkness,
            Stage.EternasOutfitter,
            Stage.EventDungeon0,
            Stage.EventDungeon1,
            Stage.EventDungeon2,
            Stage.MakaimuraCatacombs,
            Stage.NorthLandofDarkness,
            Stage.PedrosGeneralStore,
            Stage.PlainsAbandonedHouseCellar,
            Stage.SouthLandofDarkness,
            Stage.TheForeignersCave,
            Stage.TheForeignersRemains,
            Stage.TheForeignersTower0,
            Stage.TheForeignersTower1,
            Stage.TheForeignersUndergroundTomb,
            Stage.TheGoldenPalace2,
            Stage.TheWhiteDragonTemple0,
            Stage.TheWhiteDragonTemple1,
            Stage.WestLandofDarkness,

            Stage.TheRift0,
            Stage.TheRift1,
            Stage.TheRiftsInnerDepths,

            Stage.TheThirdArk0,
            Stage.TheThirdArk1,
            Stage.TheArksLowerLevel,
            Stage.MoltovaRuins,
            Stage.AncientBurialMound,
            Stage.ScorchingBlockedPassage,
            Stage.ForsakenWell,
            Stage.MistyIllusionTerrace,
            Stage.HeavenlyTerrace,
            Stage.Penitentiary,
            Stage.GardnoxWastewaterTunnel,
            Stage.PitofScreams,
            Stage.MysteriousMausoleum,
            Stage.SecretProvingGround
        }.Select(x => x.StageId).ToHashSet();

        private static readonly HashSet<ItemSubCategory> DeliverableSubCategories =
        [
            ItemSubCategory.MaterialInorganicMetal,
            ItemSubCategory.MaterialInorganicOre,
            ItemSubCategory.MaterialInorganicSand,
            ItemSubCategory.MaterialInorganicLiquid,
            ItemSubCategory.MaterialInorganicGem,
            ItemSubCategory.MaterialSewingCloth,
            ItemSubCategory.MaterialSewingString,
            ItemSubCategory.MaterialSewingFur,
            ItemSubCategory.MaterialAnimalSkin,
            ItemSubCategory.MaterialAnimalBone,
            ItemSubCategory.MaterialAnimalFang,
            ItemSubCategory.MaterialAnimalHorn,
            ItemSubCategory.MaterialAnimalFeather,
            ItemSubCategory.MaterialAnimalMeat,
            ItemSubCategory.MaterialPlantGrass,
            ItemSubCategory.MaterialPlantMushroom,
            ItemSubCategory.MaterialPlantLumber,
        ];

        private static readonly HashSet<uint> InvalidEnemies =
        [
            0, 68, 69, 107, 108, 109, 110, 111, 112, 113, 114, 172, 173, 174, 217, 235, 237, 264, 266, 267, 269, 283,
        ];

        
    }
}
