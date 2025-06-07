using Arrowgene.Ddon.GameServer.Utils;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.GameServer.Quests.LightQuests
{
    public class LightQuestAreaHuntSummary(QuestAreaId questAreaId)
    {
        public QuestAreaId QuestAreaId { get; set; } = questAreaId;
        public Dictionary<EnemyUIId, LightQuestAreaHuntSummaryRecord> HuntRecords { get; set; } = [];
        public void AddEnemy(Enemy enemy)
        {
            EnemyUIId enemyUIId = enemy.UINameId;
            if (!HuntRecords.TryGetValue(enemyUIId, out LightQuestAreaHuntSummaryRecord value))
            {
                value = new(enemyUIId);
                HuntRecords[enemyUIId] = value;
            }

            value.AddEnemy(enemy);
        }

        public void Summarize()
        {
            foreach (var record in HuntRecords.Values)
            {
                record.Summarize();
            }
        }

        public LightQuestAreaHuntSummaryRecord Roll(HashSet<EnemyUIId> exceptions = null)
        {
            var enemies = HuntRecords.Values.Where(x => exceptions is null || !exceptions.Contains(x.EnemyUIId)).ToList();
            var weights = enemies.Select(x => x.Weight).ToList();

            if (enemies.Count == 0)
            {
                // Failure; no valid enemies.
                return null;
            }

            var chosenRecord = Random.Shared.ChooseWeighted(enemies, weights);
            var chosenLevel = chosenRecord.RollLevel();

            return chosenRecord;
        }

    }

    public class LightQuestAreaHuntSummaryRecord(EnemyUIId enemyUIId)
    {
        public EnemyUIId EnemyUIId { get; set; } = enemyUIId;

        public int Count { get; set; }
        public int BossCount { get; set; }

        public HashSet<ushort> ExtantLevels { get; set; }
        public double MeanLevel { get; set; }
        public double StdLevel { get; set; }

        public double Difficulty { get { return ((double)BossCount) / Count; } }

        private List<(ushort Level, bool IsBoss)> EnemyData { get; set; } = [];

        public void Summarize()
        {
            Count = EnemyData.Count;
            BossCount = EnemyData.Where(x => x.IsBoss).Count();

            ExtantLevels = EnemyData.Select(x => x.Level).ToHashSet();
            MeanLevel = EnemyData.Average(x => x.Level);
            StdLevel = Math.Sqrt(EnemyData.Average(x => Math.Pow(x.Level - MeanLevel, 2)));

            EnemyData = null;
        }

        public void AddEnemy(Enemy enemy)
        {
            EnemyData.Add((enemy.Lv, enemy.IsBossGauge));
        }

        public double Weight
        {
            get
            {
                double bossScore = (double)BossCount / Count;
                double adjustedCount = (1 + bossScore * 1.5) * Count;
                double score = Math.Pow(adjustedCount, 0.7);
                return score;
            }
        }

        public ushort RollLevel()
        {
            uint minLevel = ExtantLevels.Min();
            double p = 1.0 / (1 + MeanLevel - minLevel);
            long randGeo = Random.Shared.NextGeometric(p) + minLevel;
            //double randNormal = Random.Shared.NextNormal(MeanLevel, StdLevel);
            ushort chosenLevel = (ushort)Math.Clamp(randGeo, ExtantLevels.Min(), ExtantLevels.Max());
            if (!ExtantLevels.Contains(chosenLevel))
            {
                // Round down to the next lowest extant level
                chosenLevel = ExtantLevels.Where(x => x < chosenLevel).Max();
            }

            return chosenLevel;
        }
    }

    public class LightQuestAreaDeliverySummary(QuestAreaId questAreaId)
    {
        public QuestAreaId QuestAreaId { get; set; } = questAreaId;
        public Dictionary<ItemId, LightQuestAreaDeliverySummaryRecord> ItemRecords { get; set; } = [];
        public void AddItem(GatheringItem item, double weight = 1.0)
        {
            if (!ItemRecords.TryGetValue(item.ItemId, out LightQuestAreaDeliverySummaryRecord value))
            {
                value = new(item.ItemId);
                ItemRecords[item.ItemId] = value;
            }

            value.Count += item.ItemNum * weight;
        }

        public (ItemId ItemId, ushort Level) Roll(DdonGameServer server, HashSet<ItemId> exceptions = null)
        {
            var items = ItemRecords.Values.Where(x => exceptions is null || !exceptions.Contains(x.ItemId)).ToList();
            var weights = items.Select(x => x.Weight).ToList();

            if (items.Count == 0)
            {
                // Failure; no valid items.
                return (0, 0);
            }

            var chosenRecord = Random.Shared.ChooseWeighted(items, weights);
            var chosenLevel = chosenRecord.RollLevel(server);

            return (chosenRecord.ItemId, chosenLevel);
        }
    }

    public class LightQuestAreaDeliverySummaryRecord(ItemId itemId)
    {
        public ItemId ItemId { get; set; } = itemId;
        public double Count { get; set; }
        public double Weight
        {
            get
            {
                return Math.Pow(Count, 0.5);
            }
        }

        public ushort RollLevel(DdonGameServer server)
        {
            var itemRank = server.AssetRepository.ClientItemInfos[ItemId].Rank;
            var adjustedRank = itemRank <= 12 ? itemRank : itemRank / 5 + 10;
            if (adjustedRank <= 10)
            {
                return (ushort)(5 * adjustedRank - 4);
            }
            else 
            {
                return (ushort)(2 * adjustedRank + 28);
            }
        }
    }
}
