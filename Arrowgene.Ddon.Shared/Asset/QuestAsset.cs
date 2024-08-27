using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;

namespace Arrowgene.Ddon.Shared.Asset
{
    public class QuestAsset
    {
        public QuestAsset()
        {
            Quests = new List<QuestAssetData>();
        }

        public List<QuestAssetData> Quests { get; set; }
    }

    public class QuestAssetData
    {
        public List<QuestProcess> Processes { get; set; }
        public QuestType Type { get; set; }
        public QuestId QuestId { get; set; }
        public QuestId NextQuestId { get; set; }
        public QuestId QuestScheduleId { get; set; }
        public QuestAreaId QuestAreaId { get; set; }
        public uint NewsImageId { get; set; }
        public ushort BaseLevel { get; set; }
        public byte MinimumItemRank { get; set; }
        public ExpType ExpType { get; set; }
        public uint ExpReward { get; set; }
        public bool Discoverable { get; set; }
        public List<QuestRewardItem> RewardItems;
        public List<QuestRewardCurrency> RewardCurrency;
        public List<QuestOrderCondition> OrderConditions;
        public bool ResetPlayerAfterQuest { get; set; }
        public List<QuestLayoutFlag> QuestLayoutFlags { get; set; }
        public List<QuestLayoutFlagSetInfo> QuestLayoutSetInfoFlags { get; set; }
        public Dictionary<uint, QuestEnemyGroup> EnemyGroups {  get; set; }

        public QuestAssetData()
        {
            Processes = new List<QuestProcess>();
            RewardItems = new List<QuestRewardItem>();
            RewardCurrency = new List<QuestRewardCurrency>();
            QuestLayoutFlags = new List<QuestLayoutFlag>();
            QuestLayoutSetInfoFlags = new List<QuestLayoutFlagSetInfo>();
            EnemyGroups = new Dictionary<uint, QuestEnemyGroup>();
            OrderConditions = new List<QuestOrderCondition>();
        }
    }
}
