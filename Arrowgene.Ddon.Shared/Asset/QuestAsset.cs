using Arrowgene.Ddon.Shared.Entity.Structure;
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

    public class QuestPointReward
    {
        public PointType PointType { get; set; }
        public uint Amount { get; set; }

        public static QuestPointReward Create(PointType type, uint amount)
        {
            return new QuestPointReward()
            {
                PointType = type,
                Amount = amount
            };
        }

        public CDataQuestExp AsCDataQuestExp()
        {
            return new CDataQuestExp()
            {
                Type = PointType,
                Reward = Amount,
            };
        }
    }

    public class QuestAssetData
    {
        public List<QuestProcess> Processes { get; set; }
        public QuestType QuestType { get; set; }
        public QuestId QuestId { get; set; }
        public QuestId NextQuestId { get; set; }
        public uint QuestScheduleId { get; set; }
        public QuestAreaId QuestAreaId { get; set; }
        public uint QuestOrderBackgroundImage { get; set; }
        public bool IsImportant { get; set; }
        public QuestAdventureGuideCategory AdventureGuideCategory { get; set; }

        public StageLayoutId StageLayoutId {  get; set; }
        public uint NewsImageId { get; set; }
        public ushort BaseLevel { get; set; }
        public byte MinimumItemRank { get; set; }
        public bool Enabled { get; set; }
        public bool OverrideEnemySpawn { get; set; }
        public bool EnableCancel { get; set; }
        public QuestSource QuestSource { get; set; }

        public List<QuestPointReward> PointRewards { get; set; }

        public bool Discoverable { get; set; }
        public List<QuestRewardItem> RewardItems;
        public List<QuestWalletReward> RewardCurrency;
        public List<QuestOrderCondition> OrderConditions;
        public bool ResetPlayerAfterQuest { get; set; }
        public List<QuestLayoutFlag> QuestLayoutFlags { get; set; }
        public List<QuestLayoutFlagSetInfo> QuestLayoutSetInfoFlags { get; set; }
        public Dictionary<uint, QuestEnemyGroup> EnemyGroups {  get; set; }
        public QuestMissionParams MissionParams {  get; set; }
        public CDataLightQuestDetail LightQuestDetail { get; set; }
        public List<QuestServerAction> ServerActions { get; set; }
        public HashSet<QuestUnlock> ContentsReleased { get; set; }
        public Dictionary<QuestId, List<QuestFlagInfo>> WorldManageUnlocks { get; set; }
        public List<QuestProgressWork> QuestProgressWork { get; set; }

        public QuestAssetData()
        {
            Processes = new List<QuestProcess>();
            PointRewards = new List<QuestPointReward>();
            RewardItems = new List<QuestRewardItem>();
            RewardCurrency = new List<QuestWalletReward>();
            QuestLayoutFlags = new List<QuestLayoutFlag>();
            QuestLayoutSetInfoFlags = new List<QuestLayoutFlagSetInfo>();
            EnemyGroups = new Dictionary<uint, QuestEnemyGroup>();
            OrderConditions = new List<QuestOrderCondition>();
            MissionParams = new QuestMissionParams();
            ServerActions = new List<QuestServerAction>();
            LightQuestDetail = new CDataLightQuestDetail();
            ContentsReleased = new HashSet<QuestUnlock>();
            WorldManageUnlocks = new Dictionary<QuestId, List<QuestFlagInfo>>();
            QuestProgressWork = new List<QuestProgressWork>();
        }
    }
}
