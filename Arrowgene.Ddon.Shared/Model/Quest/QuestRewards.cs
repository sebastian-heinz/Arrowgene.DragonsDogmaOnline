using Arrowgene.Ddon.Shared.Entity.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public abstract class LootPoolItem
    {
        public ushort Num { get; set; }
        public uint ItemId { get; set; }

        public virtual string GetUID()
        {
            IncrementalHash hash = IncrementalHash.CreateHash(HashAlgorithmName.MD5);
            hash.AppendData(BitConverter.GetBytes(ItemId));
            return BitConverter.ToString(hash.GetHashAndReset()).Replace("-", string.Empty).Substring(0, 8);
        }
    }

    public class FixedLootPoolItem : LootPoolItem
    {
    }

    public class SelectLootPoolItem : LootPoolItem
    {
    }

    public class RandomLootPoolItem : LootPoolItem
    {
        public double Chance { get; set; }

        public override string GetUID()
        {
            IncrementalHash hash = IncrementalHash.CreateHash(HashAlgorithmName.MD5);
            hash.AppendData(BitConverter.GetBytes(ItemId));
            return BitConverter.ToString(hash.GetHashAndReset()).Replace("-", string.Empty).Substring(0, 8);
        }
    }

    public abstract class QuestRewardItem
    {
        public QuestRewardType RewardType { get; protected set; }

        public QuestRewardItem(QuestRewardType rewardType)
        {
            RewardType = rewardType;
            LootPool = new List<LootPoolItem>();
        }

        public List<LootPoolItem> LootPool { get; set; }

        public virtual List<CDataRewardItem> AsCDataRewardItems()
        {
            List<CDataRewardItem> rewards = new List<CDataRewardItem>();

            foreach (var item in LootPool)
            {
                rewards.Add(new CDataRewardItem()
                {
                    ItemId = item.ItemId,
                    Num = item.Num,
                });
            }

            return rewards;
        }

        public virtual List<CDataRewardBoxItem> AsCDataRewardBoxItems()
        {
            List<CDataRewardBoxItem> rewards = new List<CDataRewardBoxItem>();
            foreach (var item in LootPool)
            {
                rewards.Add(new CDataRewardBoxItem()
                {
                    ItemId = item.ItemId,
                    Num = item.Num,
                    Type = (byte)RewardType,
                    UID = item.GetUID()
                });
            }
            return rewards;
        }
    }

    public class QuestFixedRewardItem : QuestRewardItem
    {
        public QuestFixedRewardItem() : base(QuestRewardType.Fixed)
        {

        }
    }

    public class QuestRandomRewardItem : QuestRewardItem
    {
        public QuestRandomRewardItem() : base(QuestRewardType.Random)
        {

        }

        public override List<CDataRewardBoxItem> AsCDataRewardBoxItems()
        {
            return new List<CDataRewardBoxItem>()
            {
                AsCDataRewardBoxItem()
            };
        }

        private CDataRewardBoxItem AsCDataRewardBoxItem()
        {
            var itemIndex = Roll();
            var item = LootPool[itemIndex];

            return new CDataRewardBoxItem()
            {
                ItemId = item.ItemId,
                Num = item.Num,
                Type = (byte)RewardType
            };
        }

        private int Roll()
        {
            Random rnd = new Random();
            double target = rnd.NextDouble();

            double sum = 0.0;
            for (int i = 0; i < LootPool.Count; i++)
            {
                RandomLootPoolItem item = (RandomLootPoolItem)LootPool[i];
                sum += item.Chance;
                if (target <= sum)
                {
                    return i;
                }
            }

            return 0;
        }
    }

    public class QuestSelectRewardItem : QuestRewardItem
    {
        public QuestSelectRewardItem() : base(QuestRewardType.Select)
        {
        }
    }

    public class QuestBoxRewards
    {
        public QuestBoxRewards()
        {
            // Rewards = new List<CDataRewardBoxItem>();
            Rewards = new Dictionary<string, CDataRewardBoxItem>();
        }

        public QuestId QuestId { get; set; }
        // public List<CDataRewardBoxItem> Rewards { get; set; }
        public Dictionary<string, CDataRewardBoxItem> Rewards { get; set; }
    }
}
