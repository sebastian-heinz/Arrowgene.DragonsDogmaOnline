using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using System.Collections.Generic;

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

    public class PointReward
    {
        public ExpType ExpType { get; set; }
        public uint ExpReward { get; set; }
    }

    public class QuestAssetData
    {
        public List<QuestProcess> Processes { get; set; }
        public QuestType Type { get; set; }
        public QuestId QuestId { get; set; }
        public QuestId NextQuestId { get; set; }
        public QuestId QuestScheduleId { get; set; }
        public QuestAreaId QuestAreaId { get; set; }
        public StageId StageId {  get; set; }
        public uint NewsImageId { get; set; }
        public ushort BaseLevel { get; set; }
        public byte MinimumItemRank { get; set; }

        public List<PointReward> PointRewards { get; set; }

        public bool Discoverable { get; set; }
        public List<QuestRewardItem> RewardItems;
        public List<QuestRewardCurrency> RewardCurrency;
        public List<QuestOrderCondition> OrderConditions;
        public bool ResetPlayerAfterQuest { get; set; }
        public List<QuestLayoutFlag> QuestLayoutFlags { get; set; }
        public List<QuestLayoutFlagSetInfo> QuestLayoutSetInfoFlags { get; set; }
        public Dictionary<uint, QuestEnemyGroup> EnemyGroups {  get; set; }
        public uint VariantId { get; set; }
        public QuestMissionParams MissionParams {  get; set; }
        public List<QuestServerAction> ServerActions { get; set; }

        public QuestAssetData()
        {
            Processes = new List<QuestProcess>();
            PointRewards = new List<PointReward>();
            RewardItems = new List<QuestRewardItem>();
            RewardCurrency = new List<QuestRewardCurrency>();
            QuestLayoutFlags = new List<QuestLayoutFlag>();
            QuestLayoutSetInfoFlags = new List<QuestLayoutFlagSetInfo>();
            EnemyGroups = new Dictionary<uint, QuestEnemyGroup>();
            OrderConditions = new List<QuestOrderCondition>();
            MissionParams = new QuestMissionParams();
            ServerActions = new List<QuestServerAction>();
        }
    }
}
