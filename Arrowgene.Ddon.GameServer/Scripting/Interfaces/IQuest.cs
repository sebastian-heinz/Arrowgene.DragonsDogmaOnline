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
            MissionParams = new QuestMissionParams();
            QuestLayoutSetInfoSetList = new List<QuestLayoutFlagSetInfo>();
            OrderConditions = new List<QuestOrderCondition>();
            ServerActions = new List<QuestServerAction>();

            if (OverrideEnemySpawn == null)
            {
                OverrideEnemySpawn = (QuestType == QuestType.Main);
            }

            if (EnableCancel == null)
            {
                EnableCancel = (QuestType != QuestType.Tutorial && QuestType != QuestType.Main);
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
        public virtual bool? EnableCancel { get; } = null;
        public virtual bool ResetPlayerAfterQuest { get; } = false;

        private Dictionary<ushort, QuestProcess> Processes { get; set; }
        private List<QuestRewardItem> RewardItems { get; set; }
        private List<QuestWalletReward> WalletRewards { get; set; }
        private List<QuestPointReward> PointRewards { get; set; }
        private Dictionary<uint, QuestEnemyGroup> EnemyGroups { get; set; }
        private List<QuestLayoutFlagSetInfo> QuestLayoutSetInfoSetList { get; set; }
        protected QuestMissionParams MissionParams { get; set; }
        protected List<QuestOrderCondition> OrderConditions { get; set; }
        protected List<QuestServerAction> ServerActions { get; set; }

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

        public virtual bool AcceptRequirementsMet(GameClient client)
        {
            return Enabled;
        }

        public virtual void InitializeInstanceState(QuestState questState)
        {
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
            AddItemReward(QuestFixedRewardItem.Create(itemId, amount, isHidden));
        }

        public void AddFixedItemReward(uint itemId, ushort amount)
        {
            AddFixedItemReward((ItemId) itemId, amount);
        }

        public void AddRandomChanceItemReward(List<(ItemId ItemId, ushort Amount, double Chance)> items, bool isHidden = false)
        {
            AddItemReward(QuestRandomChanceRewardItem.Create(items, isHidden));
        }

        public void AddRandomFixedItemReward(List<(ItemId ItemId, ushort Amount)> items, bool isHidden = false)
        {
            AddItemReward(QuestRandomFixedRewardItem.Create(items, isHidden));
        }

        public void AddSelectItemReward(List<(ItemId ItemId, ushort Amount)> items, bool isHidden = false)
        {
            AddItemReward(QuestSelectRewardItem.Create(items, isHidden));
        }

        public void AddPointReward(PointType pointType, uint amount)
        {
            PointRewards.Add(QuestPointReward.Create(pointType, amount));
        }

        public void AddWalletReward(WalletType walletType, uint amount)
        {
            WalletRewards.Add(QuestWalletReward.Create(walletType, amount));
        }

        public void AddQuestOrderCondition(QuestOrderConditionType type, int param01 = 0, int param02 = 0)
        {
            AddQuestOrderCondition(new QuestOrderCondition()
            {
                Type = type,
                Param01 = param01,
                Param02 = param02,
            });
        }

        public void AddQuestOrderCondition(QuestOrderCondition orderCondition)
        {
            OrderConditions.Add(orderCondition);
        }

        protected QuestEnemyGroup GetEnemyGroup(uint enemyGroupId)
        {
            return EnemyGroups[enemyGroupId];
        }

        public void AddEnemies(uint enemyGroupId, StageInfo stageInfo, uint groupId, byte subGroupId, QuestEnemyPlacementType placementType, List<InstancedEnemy> enemies, QuestTargetType targetType = QuestTargetType.EnemyForOrder)
        {
            if (!EnemyGroups.ContainsKey(enemyGroupId))
            {
                EnemyGroups[enemyGroupId] = new QuestEnemyGroup()
                {
                    StageLayoutId = stageInfo.AsStageLayoutId(groupId),
                    SubGroupId = subGroupId,
                    GroupId = enemyGroupId,
                    PlacementType = placementType,
                    TargetType = targetType
                };
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                var enemy = enemies[i];
                enemy.StageLayoutId = stageInfo.AsStageLayoutId(groupId);
                enemy.QuestEnemyGroupId = enemyGroupId;
                enemy.QuestScheduleId = QuestScheduleId;

                if (placementType == QuestEnemyPlacementType.Automatic)
                {
                    enemy.Index = (byte) i;
                }
            }

            QuestLayoutSetInfoSetList.Add(new QuestLayoutFlagSetInfo()
            {
                FlagNo = enemyGroupId,
                GroupId = groupId,
                StageNo = stageInfo.StageNo,
            });

            EnemyGroups[enemyGroupId].Enemies.AddRange(enemies);
        }

        public void AddEnemies(uint id, StageInfo stageInfo, uint groupId, QuestEnemyPlacementType placementType, List<InstancedEnemy> enemies)
        {
            AddEnemies(id, stageInfo, groupId, 0, placementType, enemies);
        }

        public void AddServerAction(QuestSeverActionType actionType, OmInstantValueAction action, ulong key, uint value, StageInfo stageInfo, uint groupId)
        {
            ServerActions.Add(new QuestServerAction()
            {
                ActionType = actionType,
                OmInstantValueAction = action,
                Key = key,
                Value = value,
                StageLayoutId = stageInfo.AsStageLayoutId(0, groupId)
            });
        }

        private QuestProcess AddProcess(QuestProcess process)
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
            return AddProcess(new QuestProcess(processNo, QuestScheduleId)
            {
                EnemyGroups = EnemyGroups
            });
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
                EnableCancel = EnableCancel.Value,
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
                QuestLayoutSetInfoFlags = QuestLayoutSetInfoSetList,
                OrderConditions = OrderConditions,
                ServerActions = ServerActions
            };

            // TODO: Generate a ScriptedQuest instead which is more customizable
            return GenericQuest.FromAsset(server, assetData, this);
        }
    }
}
