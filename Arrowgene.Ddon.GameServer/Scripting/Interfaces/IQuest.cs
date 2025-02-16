using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity.Structure;
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
            MissionParams = new QuestMissionParams();
            QuestLayoutSetInfoFlags = new List<QuestLayoutFlagSetInfo>();

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
        public virtual StageInfo StageInfo { get; } = Stage.Invalid;
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
        private List<QuestLayoutFlagSetInfo> QuestLayoutSetInfoFlags { get; set; }
        protected QuestMissionParams MissionParams { get; set; }

        public void Initialize(string path)
        {
            Path = path;

            InitializeState();
            InitializeRewards();
            InitializeEnemyGroups();
            InitializeBlocks();
        }

        protected virtual void InitializeState()
        {
        }

        protected virtual void InitializeRewards()
        {
        }

        protected virtual void InitializeEnemyGroups()
        {
        }

        protected virtual void InitializeBlocks()
        {
        }

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

        public void AddFixedItemReward(ItemId itemId, ushort amount, bool isHidden = false)
        {
            var reward = new QuestFixedRewardItem(isHidden);
            reward.LootPool.Add(new FixedLootPoolItem()
            {
                ItemId = itemId,
                Num = amount,
            });

            AddItemReward(reward);
        }

        public void AddFixedItemReward(uint itemId, ushort amount)
        {
            AddFixedItemReward((ItemId) itemId, amount);
        }

        public void AddRandomChanceItemReward(List<(ItemId ItemId, ushort Amount, double Chance)> items, bool isHidden = false)
        {
            var reward = new QuestRandomChanceRewardItem(isHidden);
            foreach (var item in items)
            {
                reward.LootPool.Add(new ChanceLootPoolItem()
                {
                    ItemId = item.ItemId,
                    Num = item.Amount,
                    Chance = item.Chance
                });
            }
            AddItemReward(reward);
        }

        public void AddRandomFixedItemReward(List<(ItemId ItemId, ushort Amount)> items, bool isHidden = false)
        {
            var reward = new QuestRandomFixedRewardItem(isHidden);
            foreach (var item in items)
            {
                reward.LootPool.Add(new FixedLootPoolItem
                {
                    ItemId = item.ItemId,
                    Num = item.Amount,
                });
            }
            AddItemReward(reward);
        }

        public void AddSelectItemReward(List<(ItemId ItemId, ushort Amount)> items, bool isHidden = false)
        {
            var reward = new QuestSelectRewardItem(isHidden);
            foreach (var item in items)
            {
                reward.LootPool.Add(new SelectLootPoolItem
                {
                    ItemId = item.ItemId,
                    Num = item.Amount,
                });
            }
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

        public void AddQuestLayoutSetInfoFlag(uint flagNo, StageInfo stageInfo, uint enemyGroupId)
        {
            var enemyGroup = GetEnemyGroup(enemyGroupId);
            QuestLayoutSetInfoFlags.Add(new QuestLayoutFlagSetInfo()
            {
                FlagNo = flagNo,
                StageNo = stageInfo.StageNo,
                GroupId = enemyGroup.StageLayoutId.GroupId
            });
        }

        protected QuestEnemyGroup GetEnemyGroup(uint enemyGroupId)
        {
            return EnemyGroups[enemyGroupId];
        }

        public void AddEnemies(uint id, StageLayoutId stageLayoutId, byte subGroupId, QuestEnemyPlacementType placementType, List<InstancedEnemy> enemies, QuestTargetType targetType = QuestTargetType.EnemyForOrder)
        {
            if (!EnemyGroups.ContainsKey(id))
            {
                EnemyGroups[id] = new QuestEnemyGroup()
                {
                    StageLayoutId = stageLayoutId,
                    SubGroupId = subGroupId,
                    GroupId = id,
                    PlacementType = placementType,
                    TargetType = targetType
                };
            }

            foreach (var enemy in enemies)
            {
                enemy.StageId = stageLayoutId;
                enemy.IsQuestControlled = true;
            }

            EnemyGroups[id].Enemies.AddRange(enemies);
        }

        public void AddEnemies(uint id, StageInfo stage, uint groupId, byte subGroupId, QuestEnemyPlacementType placementType, List<InstancedEnemy> enemies, QuestTargetType targetType = QuestTargetType.EnemyForOrder)
        {
            AddEnemies(id, stage.AsStageLayoutId(groupId), subGroupId, placementType, enemies, targetType);
        }

        public void AddEnemies(uint id, StageInfo stage, uint groupId, QuestEnemyPlacementType placementType, List<InstancedEnemy> enemies, QuestTargetType targetType = QuestTargetType.EnemyForOrder)
        {
            AddEnemies(id, stage.AsStageLayoutId(groupId), 0, placementType, enemies, targetType);
        }

        public QuestProcess AddProcess(QuestProcess process)
        {
            if (Processes.ContainsKey(process.ProcessNo))
            {
                throw new Exception($"The process {process.ProcessNo} is defined multiple times in '{Path}'");
            }
            Processes[process.ProcessNo] = process;
            return process;
        }

        public QuestProcess AddNewProcess(ushort processNo)
        {
            return AddProcess(new QuestProcess(processNo));
        }

        private void ValidateQuestParams()
        {
            if (QuestType == QuestType.Tutorial)
            {
                if (StageInfo == Stage.Invalid)
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
                StageLayoutId = StageInfo.AsStageLayoutId(0, 0),
                ResetPlayerAfterQuest = ResetPlayerAfterQuest,
                MissionParams = MissionParams,
                QuestLayoutSetInfoFlags = QuestLayoutSetInfoFlags,
            };

            // TODO: Generate a ScriptedQuest instead which is more customizable
            return GenericQuest.FromAsset(server, assetData, this);
        }
    }
}
