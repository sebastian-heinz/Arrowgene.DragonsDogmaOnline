using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Scripting.Interfaces
{
    public abstract class IQuest
    {
        public IQuest()
        {
            // TODO: Apply algorithm using VariantId+QuestId
            QuestScheduleId = (uint)QuestId;

            Processes = new Dictionary<ushort, QuestProcess>();
            RewardItems = new List<QuestRewardItem>();
            WalletRewards = new List<QuestWalletReward>();
            PointRewards = new List<QuestPointReward>();
            EnemyGroups = new Dictionary<uint, QuestEnemyGroup>();

            if (OverrideEnemySpawn == null)
            {
                OverrideEnemySpawn = (QuestType == QuestType.Main);
            }
        }

        public string Path { get; set; }
        public abstract QuestType QuestType { get; }
        public abstract QuestId QuestId { get; }
        public virtual QuestId NextQuestId { get; } = QuestId.None;
        public virtual uint VariantId { get; } = 0;
        public uint QuestScheduleId { get; }
        public virtual QuestAreaId QuestAreaId { get; } = QuestAreaId.None;
        public QuestSource QuestSource { get; } = QuestSource.Script;
        public virtual bool Enabled { get; } = true;
        public virtual uint QuestOrderBackgroundImage { get; } 
        public virtual StageId StageId { get; } = StageId.Invalid;
        public virtual uint NewsImageId { get; } = 0;
        public abstract ushort RecommendedLevel { get; }
        public abstract byte MinimumItemRank { get; }
        public abstract bool IsDiscoverable { get; }
        public virtual bool? OverrideEnemySpawn { get; } = null;
        public virtual bool ResetPlayerAfterQuest { get; } = false;

        private Dictionary<ushort, QuestProcess> Processes { get; set; }
        private List<QuestRewardItem> RewardItems { get; set; }
        private List<QuestWalletReward> WalletRewards { get; set; }
        private List<QuestPointReward> PointRewards { get; set; }
        private Dictionary<uint, QuestEnemyGroup> EnemyGroups { get; set; }

        public void Initialize(string path)
        {
            Path = path;

            InitializeRewards();
            InitializeEnemyGroups();
            InitializeBlocks();
        }

        protected abstract void InitializeRewards();
        protected abstract void InitializeEnemyGroups();
        protected abstract void InitializeBlocks();

        public virtual bool AcceptRequirementsMet(DdonGameServer server, GameClient client)
        {
            return Enabled;
        }

        public void AddItemReward(QuestRewardItem reward)
        {
            if (reward != null)
            {
                RewardItems.Add(reward);
            }
        }

        public void AddFixedItemReward(uint itemId, ushort amount)
        {
            var reward = new QuestFixedRewardItem();
            reward.LootPool.Add(new FixedLootPoolItem()
            {
                ItemId = itemId,
                Num = amount
            });

            AddItemReward(reward);
        }

        public void AddFixedItemReward(ItemId itemId, ushort amount)
        {
            AddFixedItemReward((uint)itemId, amount);
        }

        public void AddPointReward(QuestPointReward reward)
        {
            if (reward != null)
            {
                PointRewards.Add(reward);
            }
        }

        public void AddPointReward(PointType pointType, uint amount)
        {
            var reward = new QuestPointReward()
            {
                PointType = pointType,
                Amount = amount
            };

            AddPointReward(reward);
        }

        public void AddWalletReward(QuestWalletReward reward)
        {
            if (reward != null)
            {
                WalletRewards.Add(reward);
            }
        }

        public void AddWalletReward(WalletType walletType, uint amount)
        {
            var reward = new QuestWalletReward()
            {
                WalletType = walletType,
                Amount = amount
            };
            AddWalletReward(reward);
        }

        public void AddEnemies(uint groupId, StageId stageId, byte subGroupId, QuestEnemyPlacementType placementType, List<InstancedEnemy> enemies)
        {
            if (!EnemyGroups.ContainsKey(groupId))
            {
                EnemyGroups[groupId] = new QuestEnemyGroup()
                {
                    StageId = stageId,
                    SubGroupId = subGroupId,
                    GroupId = groupId,
                    PlacementType = placementType
                };
            }

            foreach (var enemy in enemies)
            {
                enemy.StageId = stageId;
                enemy.IsQuestControlled = true;
            }

            EnemyGroups[groupId].Enemies.AddRange(enemies);
        }

        public void AddProcess(QuestProcess process)
        {
            if (Processes.ContainsKey(process.ProcessNo))
            {
                throw new Exception($"The process {process.ProcessNo} is defined multiple times in '{Path}'");
            }
            Processes[process.ProcessNo] = process;
        }

        private void ValidateQuestParams()
        {
            if (QuestType == QuestType.Tutorial)
            {
                if (StageId.Equals(StageId.Invalid))
                {
                    throw new Exception($"The tutorial quest '{Path}' requires a valid StageId to be assigned.");
                }
            }
            else if (QuestType == QuestType.World)
            {
                if (QuestAreaId == QuestAreaId.None)
                {
                    throw new Exception($"The world quest '{Path}' requires a valid QuestAreaId to be assigned.");
                }
            }
        }

        public Quest GenerateQuest(DdonGameServer server)
        {
            // Make sure certain required params are present based on the quest type
            ValidateQuestParams();

            var assetData = new QuestAssetData()
            {
                BaseLevel = RecommendedLevel,
                Discoverable = IsDiscoverable,
                Enabled = Enabled,
                EnemyGroups = EnemyGroups,
                NewsImageId = NewsImageId,
                MinimumItemRank = MinimumItemRank,
                NextQuestId = NextQuestId,
                OverrideEnemySpawn = OverrideEnemySpawn.Value,
                QuestAreaId = QuestAreaId,
                QuestId = QuestId,
                QuestOrderBackgroundImage = QuestOrderBackgroundImage,
                QuestSource = QuestSource,
                QuestScheduleId = QuestScheduleId,
                QuestType = QuestType,
                PointRewards = PointRewards,
                Processes = Processes.Values.ToList(),
                RewardItems = RewardItems,
                RewardCurrency = WalletRewards,
                StageId = StageId,
                ResetPlayerAfterQuest = ResetPlayerAfterQuest,
            };

            return GenericQuest.FromAsset(server, assetData, this);
        }
    }
}
