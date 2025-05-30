using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Quests.LightQuests
{
    public abstract class LightQuestSpawnGroup
    {
        protected static readonly HashSet<ItemSubCategory> DeliverableSubCategories =
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

        public string Name { get; set; } = string.Empty;
        public Dictionary<EnemyUIId, List<(uint, bool)>> EnemyData { get; set; } = [];
        public Dictionary<ItemId, int> ItemData { get; set; } = [];

        public abstract void UpdateData(DdonGameServer server);

        public void AddGroup(LightQuestSpawnGroup group)
        {
            foreach (var (enemyId, instances) in group.EnemyData)
            {
                if (!EnemyData.TryGetValue(enemyId, out List<(uint, bool)> value))
                {
                    value = ([]);
                    EnemyData[enemyId] = value;
                }

                value.AddRange(instances);
            }

            foreach (var (itemId, count) in group.ItemData)
            {
                ItemData[itemId] = ItemData.GetValueOrDefault(itemId) + count;
            }
        }

        public void AddEnemy(DdonGameServer server, Enemy enemy)
        {
            var uiNameId = enemy.UINameId;
            if (!EnemyData.TryGetValue(uiNameId, out List<(uint, bool)> value))
            {
                value = ([]);
                EnemyData[uiNameId] = value;
            }

            value.Add((enemy.Lv, enemy.IsBossGauge));

            foreach(var item in enemy.DropsTable.Items)
            {
                AddItem(server, item);
            }
        }

        public void AddItem(DdonGameServer server, GatheringItem item)
        {
            var itemInfo = server.AssetRepository.ClientItemInfos[item.ItemId];

            if (DeliverableSubCategories.Contains(itemInfo.SubCategory))
            {
                if (!ItemData.ContainsKey(item.ItemId))
                {
                    ItemData[item.ItemId] = 0;
                }
                ItemData[item.ItemId] += (int)item.ItemNum;
            }
        }

        public static double CountToWeight(int count, double scalingFactor)
        {
            return Math.Pow(count, 0.7) * scalingFactor;
        }

        public (EnemyUIId EnemyId, uint Level) RollEnemy()
        {
            if (EnemyData.Count == 0)
            {
                return (0, 0);
            }

            List<EnemyUIId> keys = [.. EnemyData.Keys];

            double rt = 0.0;
            List<double> typeWeights = keys.Select(x => { 
                rt += CountToWeight(
                    EnemyData[x].Count, 
                    8.0 * EnemyData[x].Count(x => x.Item2) / EnemyData[x].Count // Average boss-ness of the collected data
                ); 
                return rt; 
            }).ToList();

            double typeRoll = Random.Shared.NextDouble() * typeWeights.Last();
            int typeIndex = typeWeights.Count(x => x < typeRoll);
            var enemyType = keys[typeIndex];

            List<double> enemyLevels = EnemyData[enemyType].Select(x => (double)x.Item1).ToList();
            double avgLevel = enemyLevels.Average();
            double stdLevel = Math.Sqrt(enemyLevels.Average(x => Math.Pow(x - avgLevel, 2)));

            double u1 = 1.0 - Random.Shared.NextDouble(); //uniform(0,1] random doubles
            double u2 = 1.0 - Random.Shared.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            double randNormal = avgLevel + stdLevel * randStdNormal; //random normal(mean,stdDev^2)

            uint chosenLevel = (uint)Math.Round(Math.Clamp(randNormal, enemyLevels.Min(), enemyLevels.Max()));

            return (enemyType, chosenLevel);
        }

        public ItemId RollItem()
        {
            if (ItemData.Count == 0)
            {
                return 0;
            }

            List<ItemId> keys = [.. ItemData.Keys];

            double rt = 0.0;
            List<double> cumulativeWeights = keys.Select(x => { rt += CountToWeight(ItemData[x], 1.0); return rt; }).ToList();

            double roll = Random.Shared.NextDouble() * cumulativeWeights.Last();
            int index = cumulativeWeights.Count(x => x < roll);
            return keys[index];
        }
    }

    public class LightQuestSpawnGroupComposite(IEnumerable<LightQuestSpawnGroup> groups) : LightQuestSpawnGroup
    {
        public List<LightQuestSpawnGroup> SpawnGroups { get; set; }

        public override void UpdateData(DdonGameServer server) {
            EnemyData.Clear();
            ItemData.Clear();

            foreach (LightQuestSpawnGroup group in groups)
            {
                group.UpdateData(server);
                AddGroup(group);
            }
        }
    }

    public class LightQuestSpawnGroupByStageGroup(QuestAreaId questAreaId, HashSet<StageInfo> exceptions) : LightQuestSpawnGroup
    {
        public QuestAreaId QuestAreaId { get; set; } = questAreaId;
        public HashSet<StageInfo> Exceptions { get; set; } = exceptions;

        public override void UpdateData(DdonGameServer server)
        {
            EnemyData.Clear();
            ItemData.Clear();

            HashSet<uint> stageIds = Stage.StageInfoFromQuestAreaId(QuestAreaId).Where(x => !Exceptions.Contains(x)).Select(x => x.StageId).ToHashSet();

            var enemies = server.AssetRepository.EnemySpawnAsset.Enemies.Where(x => stageIds.Contains(x.Key.Id)).SelectMany(x => x.Value);
            foreach (var enemy in enemies)
            {
                AddEnemy(server, enemy);
            }

            var gathering = server.AssetRepository.GatheringItems.Where(x => stageIds.Contains(x.Key.Item1.Id)).SelectMany(x => x.Value);
            foreach (var item in gathering)
            {
                AddItem(server, item);
            }
        }
    }

    public class LightQuestSpawnGroupByNode(StageInfo stageInfo, HashSet<uint> enemyList, HashSet<(uint, uint)> gatheringList) : LightQuestSpawnGroup
    {
        public StageInfo StageInfo { get; set; } = stageInfo;
        public HashSet<uint> EnemyList { get; set; } = enemyList;
        public HashSet<(uint, uint)> GatheringList { get; set; } = gatheringList;

        public override void UpdateData(DdonGameServer server)
        {
            EnemyData.Clear();
            ItemData.Clear();
            
            foreach (var group in EnemyList)
            {
                var enemiesInStage = server.AssetRepository.EnemySpawnAsset.Enemies.GetValueOrDefault(StageInfo.AsStageLayoutId(0, group)) ?? [];
                foreach(var enemy in enemiesInStage)
                {
                    AddEnemy(server, enemy);
                }
            }

            foreach (var group in GatheringList)
            {
                var stageId = StageInfo.AsStageLayoutId(0, group.Item1);

                if (server.GameSettings.GameServerSettings.EnableDefaultGatheringDrops)
                {
                    // TODO: Figure this out.
                    continue;
                }
                else
                {
                    var items = server.AssetRepository.GatheringItems.GetValueOrDefault((stageId, group.Item2)) ?? [];
                    foreach(var item in items)
                    {
                        AddItem(server, item);
                    }
                }
            }
        }
    }

    public class LightQuestSpawnGroupByQuest(QuestAreaId questAreaId, HashSet<QuestId> exceptions) : LightQuestSpawnGroup 
    {
        public QuestAreaId QuestAreaId { get; set; } = questAreaId;
        public HashSet<QuestId> Exceptions { get; set; } = exceptions;

        public override void UpdateData(DdonGameServer server)
        {
            EnemyData.Clear();
            ItemData.Clear();

            var worldQuests = QuestManager.GetWorldQuestIdsByAreaId(QuestAreaId).Except(Exceptions);
            foreach (var worldQuest in worldQuests)
            {
                var quest = QuestManager.GetQuestByScheduleId((uint)worldQuest);
                foreach (var enemy in quest.EnemyGroups.Values.SelectMany(x => x.Enemies))
                {
                    AddEnemy(server, enemy);
                }
            }
        }

    }
}
