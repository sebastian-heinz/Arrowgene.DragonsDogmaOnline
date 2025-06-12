using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Scripting.Interfaces;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data.Common;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Quests.LightQuests
{
    public class LightQuestManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(LightQuestManager));

        private readonly DdonGameServer Server;

        private int GENERATOR_ATTEMPTS_PER_QUEST => Server.GameSettings.GameServerSettings.LightQuestGenerationAttemptsPerQuest;

        private uint CURRENT_VARIANT_ID = 0;

        // TODO: This should be configurable, but needs to be synced with the reset task or the boards act funny.
        private static readonly TimeSpan BOARD_QUEST_DURATION = TimeSpan.FromDays(1);

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

        #region Quest Generation
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

        public void InsertRecordsFromAsset()
        {
            var quests = Server.AssetRepository.LightQuestAsset.GeneratingAssets
                .SelectMany(x => GenerateRecordsFromAsset(x))
                .Select(x => GenerateQuestFromRecord(x));

            Server.Database.ExecuteInTransaction(connection =>
            {
                foreach (var quest in quests)
                {
                    if (quest.BackingObject is LightQuestQuest lightQuest)
                    {
                        Server.Database.InsertLightQuestRecord(lightQuest.QuestRecord, connection);
                    }
                }
            });
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
                List<(LightQuestAreaHuntSummaryRecord Record, ushort Level)> biasRolls = [];
                for (int biasRoll = 0; biasRoll <= generatingAsset.BiasRerolls; biasRoll++)
                {
                    var rollRecord = summary.Roll(rolledEnemyIds);
                    if (rollRecord is null)
                    {
                        Logger.Error($"Error in generator {generatingAsset.Name}; no valid enemies for region.");
                        break;
                    }
                    biasRolls.Add((rollRecord, rollRecord.RollLevel()));
                }

                if (biasRolls.Count == 0)
                {
                    Logger.Error($"Error in generator {generatingAsset.Name}; could not generate any hunt targets.");
                    attempt = GENERATOR_ATTEMPTS_PER_QUEST;
                    break;
                }

                if (generatingAsset.BiasLower)
                {
                    biasRolls = [.. biasRolls.OrderBy(x => x.Level)];
                }
                else
                {
                    biasRolls = [.. biasRolls.OrderByDescending(x => x.Level)];
                }

                var (chosenRecord, chosenLevel) = biasRolls.First();

                if (generatingAsset.MinLevel <= chosenLevel && chosenLevel <= generatingAsset.MaxLevel)
                {
                    var mixin = Server.ScriptManager.MixinModule.Get<ILightQuestRewardMixin>("light_hunt_quest_reward");
                    finalRecord = FinalizeRecord(quest, generatingAsset, (int)chosenRecord.EnemyUIId, chosenLevel, mixin, chosenRecord.Difficulty);

                    // Adjustment of count for boss hunts; applies after the rewards are counted.
                    finalRecord.Count = (int)(finalRecord.Count / (1 + chosenRecord.Difficulty));

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
                    finalRecord = FinalizeRecord(quest, generatingAsset, (int)chosenRoll.Item1, chosenRoll.Item2, mixin);
                    break;
                }

                attempt++;
            }

            return finalRecord;
        }

        private LightQuestRecord FinalizeRecord(LightQuestInfo quest, LightQuestGeneratingAsset generatingAsset, int target, ushort level, ILightQuestRewardMixin mixin, double difficulty = 0.0)
        {
            var finalRecord = new LightQuestRecord()
            {
                VariantIndex = CURRENT_VARIANT_ID++,
                QuestId = quest.QuestId,
                Target = target,
                Level = level,
                Count = Random.Shared.Next(generatingAsset.MinCount, generatingAsset.MaxCount + 1),
                DistributionStart = DateTimeOffset.UtcNow,
                DistributionEnd = DateTimeOffset.UtcNow + BOARD_QUEST_DURATION
            };

            finalRecord.RewardG = mixin.CalculateRewardG(finalRecord, difficulty);
            finalRecord.RewardR = mixin.CalculateRewardR(finalRecord, difficulty);
            finalRecord.RewardXP = mixin.CalculateRewardXP(finalRecord, difficulty);
            finalRecord.RewardAP = mixin.CalculateRewardAP(finalRecord, difficulty);

            string targetString = quest.Type == LightQuestType.Hunt ? ((EnemyUIId)target).ToString() : ((ItemId)target).ToString();

            Logger.Info($"Generating {generatingAsset.Name} : \"{quest.Name}\" -> {targetString} x{finalRecord.Count} (Lv. {level})");

            return finalRecord;
        }
        #endregion

        #region Quest Management
        public List<Quest> ReadQuests(bool clean = false)
        {
            List<LightQuestRecord> records = new();
            Server.Database.ExecuteInTransaction(connection =>
            {
                // Always read before cleanup so that you can properly increment the schedule ID.
                // Making the schedule ID increment monotonically is more important than keeping additional quests in memory.
                records = Server.Database.SelectLightQuestRecords(connection);
                CURRENT_VARIANT_ID = records.Count != 0 ? records.Max(x => x.VariantIndex)+1 : CURRENT_VARIANT_ID;

                // Head server is the only one who is allowed to clean the DB of dead quests.
                if (clean && ServerUtils.IsHeadServer(Server))
                {
                    foreach(var record in records.Where(x => x.DistributionEnd < DateTimeOffset.UtcNow))
                    {
                        Server.Database.DeleteLightQuestRecord(record.QuestScheduleId, connection);
                    }
                    records.RemoveAll(x => x.DistributionEnd < DateTimeOffset.UtcNow);
                }
            });

            return records.Select(x => GenerateQuestFromRecord(x)).ToList() ;
        }

        public static bool IsLightQuestScheduleId(uint scheduleId)
        {
            return QuestScheduleId.GetType(scheduleId) == QuestScheduleId.ScheduleIdType.Board;
        }

        public void CheckQuestScheduleIdForDecay(Character character, uint scheduleId, DbConnection? connectionIn = null)
        {
            // Only attempt to manage these rotating quests.
            // Missing scheduleIds for other quest types could be server setup error (i.e. missing files) and we don't want to interfere.
            if (!IsLightQuestScheduleId(scheduleId))
            {
                return;
            }

            var quest = QuestManager.GetQuestByScheduleId(scheduleId);

            // Checks for two kinds of decayed quest:
            // If quest is null, the quest is fully decayed and the record has been deleted.
            // If the quest exists, but is past distribution, characters logging in still need to have it wiped.
            if (quest is null || quest.DistributionEnd < DateTimeOffset.UtcNow)
            {
                Server.Database.ExecuteQuerySafe(connectionIn, connection =>
                {
                    Server.Database.DeletePriorityQuest(character.CommonId, scheduleId, connectionIn);
                    Server.Database.RemoveQuestProgress(character.CommonId, scheduleId, QuestType.Light, connectionIn);
                });
            }
        }

        public IEnumerable<uint> GetDecayedQuests(IEnumerable<uint> testIds)
        {
            var validQuestScheduleIds = QuestManager.GetQuestsByType(QuestType.Light)
                .Where(x => QuestManager.GetQuestByScheduleId(x).DistributionEnd > DateTimeOffset.UtcNow);

            return testIds.Except(validQuestScheduleIds);
        }

        public IEnumerable<uint> HandleQuestDecay(Character character, List<QuestProgress> questProgress, List<uint> questPriority, DbConnection? connectionIn = null)
        {
            var questsUnderInspection = questProgress.Select(x => x.QuestScheduleId)
                .Union(questPriority)
                .Where(x => IsLightQuestScheduleId(x));

            var questsToDrop = GetDecayedQuests(questsUnderInspection).ToImmutableHashSet();

            Server.Database.ExecuteQuerySafe(connectionIn, connection =>
            {
                foreach(var scheduleId in questsToDrop)
                {
                    Server.Database.RemoveQuestProgress(character.CommonId, scheduleId, QuestType.Light, connection);
                    Server.Database.DeletePriorityQuest(character.CommonId, scheduleId, connectionIn);
                }
            });

            questProgress.RemoveAll(x => questsToDrop.Contains(x.QuestScheduleId));
            questPriority.RemoveAll(x => questsToDrop.Contains(x));

            return questsToDrop;
        }

        #endregion

        #region Scraping for Quest Generation
        private readonly Dictionary<QuestAreaId, LightQuestAreaHuntSummary> EnemySummaries = Enum.GetValues<QuestAreaId>().ToDictionary(x => x, x => new LightQuestAreaHuntSummary(x));
        private readonly Dictionary<QuestAreaId, LightQuestAreaDeliverySummary> ItemSummaries = Enum.GetValues<QuestAreaId>().ToDictionary(x => x, x => new LightQuestAreaDeliverySummary(x));
        private readonly HashSet<ItemId> DeliverableItems;

        private static HashSet<ItemId> ParseGatherableItems(DdonGameServer server)
        {
            var craftableItems = server.AssetRepository.CraftingRecipesAsset
                .SelectMany(x => x.RecipeList)
                .Select(x => (ItemId)x.ItemID)
                .ToHashSet();
            return server.AssetRepository.ClientItemInfos.Values
                .Where(x => DeliverableSubCategories.Contains(x.SubCategory) && !craftableItems.Contains(x.ItemId))
                .Select(x => x.ItemId)
                .ToHashSet();
        }

        private void ParseStagesByQuestAreaId()
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
                    if (DeliverableItems.Contains(item.ItemId))
                    {
                        deliverySummary.AddItem(item);
                    }
                }
            }
        }

        private void ParseWorldQuestEnemies()
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

        private void ParseLestaniaSpots()
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
                    if (DeliverableItems.Contains(item.ItemId))
                    {
                        deliverySummary.AddItem(item);
                    }
                }
            }
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
                if (DeliverableItems.Contains(item.ItemId))
                {
                    deliverySummary.AddItem(item, Server.GameSettings.GameServerSettings.LightQuestGenerationDropItemWeight);
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
            Stage.SecretProvingGround,
            Stage.HidellCatacombs1
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

        private static readonly HashSet<EnemyUIId> InvalidEnemies = [
            EnemyUIId.None,
            EnemyUIId.Leech,
            EnemyUIId.Worm,
            EnemyUIId.Spider,
            EnemyUIId.Pig,
            EnemyUIId.Boar,
            EnemyUIId.GiantRat,
            EnemyUIId.Ox,
            EnemyUIId.Doe,
            EnemyUIId.Buck,
            EnemyUIId.Rabbit,
            EnemyUIId.Piglet,
            EnemyUIId.GiantUnspeakableMeat,
            EnemyUIId.UnspeakableMeat,
            EnemyUIId.Moth,
            EnemyUIId.Mandragora,
            EnemyUIId.Chicken,
            EnemyUIId.Goat,
            EnemyUIId.Frog,
            EnemyUIId.WildBoar,
        ];
        #endregion
    }
}
