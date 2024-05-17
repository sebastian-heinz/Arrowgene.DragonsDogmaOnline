using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Shared.Entity.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model
{
    public abstract class QuestReward
    {
        public QuestRewardType RewardType { get; protected set; }

        public QuestReward(QuestRewardType rewardType)
        {
            RewardType = rewardType;
        }

        public abstract CDataRewardItem AsCDataRewardItem();

        public abstract CDataRewardBoxItem AsCDataRewardBoxItem();
    }

    public class QuestFixedRewardItem : QuestReward
    {
        public QuestFixedRewardItem() : base(QuestRewardType.Fixed)
        {

        }

        public ushort Num { get; set; }
        public uint ItemId { get; set; }

        public override CDataRewardItem AsCDataRewardItem()
        {
            return new CDataRewardItem()
            {
                ItemId = ItemId,
                Num = Num
            };
        }

        public override CDataRewardBoxItem AsCDataRewardBoxItem()
        {
            return new CDataRewardBoxItem()
            {
                ItemId = ItemId,
                Num = Num,
                UID = "",
                Type = (byte)RewardType
            };
        }
    }

    public class QuestRandomRewardItem
    {
        public uint ItemId { get; set; }
        public ushort Num { get; set; }
        public double Chance { get; set; }
    }

    public class QuestRandomReward : QuestReward
    {
        public QuestRandomReward() : base(QuestRewardType.Random)
        {

        }

        public List<QuestRandomRewardItem> LootPool { get; set; }

        public override CDataRewardItem AsCDataRewardItem()
        {
            return new CDataRewardItem()
            {
                ItemId = 0,
                Num = 1
            };
        }
        public override CDataRewardBoxItem AsCDataRewardBoxItem()
        {
            var itemIndex = Roll();
            var item = LootPool[itemIndex];

            return new CDataRewardBoxItem()
            {
                ItemId = item.ItemId,
                Num = item.Num,
                UID = "",
                Type = (byte)RewardType
            };
        }

        public int Roll()
        {
            Random rnd = new Random();
            double target = rnd.NextDouble();

            double sum = 0.0;
            for (int i = 0; i < LootPool.Count; i++)
            {
                var item = LootPool[i];
                sum += item.Chance;
                if (target <= sum)
                {
                    return i;
                }
            }

            return 0;
        }
    }

    public class QuestBoxRewards
    {
        public QuestBoxRewards()
        {
            Rewards = new List<CDataRewardBoxItem>();
        }

        public QuestId QuestId { get; set; }
        public List<CDataRewardBoxItem> Rewards { get; set; }
    }
}
