using System.Collections.Generic;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

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
        public List<QuestBlock> Blocks { get; set; }
        public QuestType Type { get; set; }
        public QuestId QuestId { get; set; }
        public QuestId NextQuestId { get; set; }
        public ushort BaseLevel { get; set; }
        public byte MinimumItemRank { get; set; }
        public uint ExpReward { get; set; }
        public bool Discoverable { get; set; }
        public List<QuestRewardItem> RewardItems;
        public List<QuestRewardCurrency> RewardCurrency;

        public List<QuestLayoutFlag> QuestLayoutFlags { get; set; }
        public List<QuestLayoutFlagSetInfo> QuestLayoutSetInfoFlags { get; set; }

        public QuestAssetData()
        {
            Blocks = new List<QuestBlock>();
            RewardItems = new List<QuestRewardItem>();
            RewardCurrency = new List<QuestRewardCurrency>();
            QuestLayoutFlags = new List<QuestLayoutFlag>();
            QuestLayoutSetInfoFlags = new List<QuestLayoutFlagSetInfo>();
        }
    }
}
