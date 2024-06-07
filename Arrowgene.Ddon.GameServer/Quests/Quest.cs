using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace Arrowgene.Ddon.GameServer.Quests
{
    public class QuestRewardParams
    {
        public byte ChargeRewardNum { get; set; }
        public byte ProgressBonusNum { get; set; }
        public bool IsRepeatReward { get; set; }
        public bool IsUndiscoveredReward { get; set; }
        public bool IsHelpReward { get; set; }
        public bool IsPartyBonus { get; set; }
    }

    public class QuestLocation
    {
        public StageId StageId { get; set; }
        public ushort SubGroupId { get; set; }

        public uint QuestLayoutFlag {  get; set; }

        public bool ContainsStageId(StageId stageId, ushort subGroupId)
        {
            return (stageId.Id == StageId.Id) && (stageId.GroupId == StageId.GroupId) && (stageId.LayerNo == StageId.LayerNo) && (subGroupId == SubGroupId);
        }
    }

    public class QuestDeliveryItem
    {
        public uint ItemId { get; set; }
        public uint Amount {  get; set; }
    }

    public abstract class Quest
    {
        public readonly QuestId QuestId;
        public readonly bool IsDiscoverable;
        public readonly QuestType QuestType;
        public readonly QuestId QuestScheduleId;
        public uint BaseLevel { get; set; }
        public ushort MinimumItemRank { get; set; }
        public QuestId NextQuestId {  get; protected set; }
        public bool ResetPlayerAfterQuest { get; protected set; }
        public List<QuestOrderCondition> OrderConditions {  get; protected set; }
        public QuestRewardParams RewardParams { get; protected set; }
        public List<CDataWalletPoint> WalletRewards { get; protected set; }
        public List<CDataQuestExp> ExpRewards { get; protected set; }
        public List<QuestRewardItem> ItemRewards { get; protected set; }
        public List<QuestRewardItem> SelectedableItemRewards { get; protected set; }
        public List<CDataCharacterReleaseElement> ContentsReleaseRewards { get; protected set; }
        public List<QuestLocation> Locations { get; protected set; }
        public List<QuestDeliveryItem> DeliveryItems { get; protected set; }
        public List<QuestLayoutFlagSetInfo> QuestLayoutFlagSetInfo;
        public List<QuestLayoutFlag> QuestLayoutFlags;
        public Dictionary<uint, QuestEnemyGroup> EnemyGroups { get; set; }

        public Quest(QuestId questId, QuestId questScheduleId, QuestType questType, bool isDiscoverable = false)
        {
            QuestId = questId;
            QuestType = questType;
            QuestScheduleId = questScheduleId;
            IsDiscoverable = isDiscoverable;

            OrderConditions = new List<QuestOrderCondition>();
            RewardParams = new QuestRewardParams();
            WalletRewards = new List<CDataWalletPoint>();
            ExpRewards = new List<CDataQuestExp>();
            ItemRewards = new List<QuestRewardItem>();
            SelectedableItemRewards = new List<QuestRewardItem>();
            ContentsReleaseRewards = new List<CDataCharacterReleaseElement>();
            Locations = new List<QuestLocation>();
            DeliveryItems = new List<QuestDeliveryItem>();
            QuestLayoutFlagSetInfo = new List<QuestLayoutFlagSetInfo>();
            QuestLayoutFlags = new List<QuestLayoutFlag>();
            EnemyGroups = new Dictionary<uint, QuestEnemyGroup>();
        }

        public virtual CDataQuestList ToCDataQuestList()
        {
            var quest = new CDataQuestList()
            {
                QuestId = (uint)QuestId,
                QuestScheduleId = (uint)QuestScheduleId,
                BaseLevel = BaseLevel,
                ContentJoinItemRank = MinimumItemRank,
                IsClientOrder = false,
                IsEnable = true,
                BaseExp = ExpRewards,
                BaseWalletPoints = WalletRewards,
                FixedRewardItemList = GetQuestFixedRewards(),
                FixedRewardSelectItemList = GetQuestSelectableRewards(),
                QuestOrderConditionParamList = GetQuestOrderConditions(),
            };

            foreach (var questLayoutFlag in QuestLayoutFlags)
            {
                quest.QuestLayoutFlagList.Add(questLayoutFlag.AsCDataQuestLayoutFlag());
            }

            foreach (var questLayoutFlagSet in QuestLayoutFlagSetInfo)
            {
                quest.QuestLayoutFlagSetInfoList.Add(questLayoutFlagSet.AsCDataQuestLayoutFlagSetInfo());
            }

            return quest;
        }

        public virtual CDataQuestOrderList ToCDataQuestOrderList()
        {
           var quest = new CDataQuestOrderList()
            {
                QuestId = (uint)QuestId,
                QuestScheduleId = (uint)QuestScheduleId,
                BaseLevel = BaseLevel,
                ContentJoinItemRank = MinimumItemRank,
                IsClientOrder = false,
                IsEnable = true,
                BaseExp = ExpRewards,
                BaseWalletPoints = WalletRewards,
                FixedRewardItem = GetQuestFixedRewards(),
                FixedRewardSelectItem = GetQuestSelectableRewards(),
                QuestOrderConditionParam = GetQuestOrderConditions(),
           };

            foreach (var questLayoutFlag in QuestLayoutFlags)
            {
                quest.QuestLayoutFlagList.Add(questLayoutFlag.AsCDataQuestLayoutFlag());
            }

            foreach (var questLayoutFlagSet in QuestLayoutFlagSetInfo)
            {
                quest.QuestLayoutFlagSetInfoList.Add(questLayoutFlagSet.AsCDataQuestLayoutFlagSetInfo());
            }

            return quest;
        }

        public abstract List<CDataQuestProcessState> StateMachineExecute(GameClient client, QuestProcessState processState, out QuestProgressState questProgressState);
        public abstract bool HasEnemiesInCurrentStageGroup(QuestState questState, StageId stageId, uint subGroupId);
        public abstract List<InstancedEnemy> GetEnemiesInStageGroup(StageId stageId, uint subGroupId);

        public virtual void SendProgressWorkNotices(GameClient client, StageId stageId, uint subGroupId)
        {
            client.Party.SendToAll(new S2CQuestQuestProgressWorkSaveNtc());
        }

        public virtual void ResetEnemiesForBlock(GameClient client, QuestId questId, QuestBlock questBlock)
        {
            var quest = QuestManager.GetQuest(questId);
            foreach (var groupId in questBlock.EnemyGroupIds)
            {
                var enemyGroup = quest.EnemyGroups[groupId];

                S2CInstanceEnemyGroupResetNtc resetNtc = new S2CInstanceEnemyGroupResetNtc()
                {
                    LayoutId = new CDataStageLayoutId()
                    {
                        StageId = enemyGroup.StageId.Id,
                        GroupId = enemyGroup.StageId.GroupId,
                        LayerNo = enemyGroup.StageId.LayerNo
                    }
                };

                client.Party.SendToAll(resetNtc);
            }
        }

        public List<QuestRewardItem> GetQuestRewards()
        {
            List<QuestRewardItem> rewards = new List<QuestRewardItem>();

            foreach (var reward in ItemRewards)
            {
                rewards.Add(reward);
            }

            foreach (var reward in SelectedableItemRewards)
            {
                rewards.Add(reward);
            }

            return rewards;
        }

        public byte RandomRewardNum()
        {
            byte count = 0;
            foreach (var reward in ItemRewards)
            {
                if (reward.RewardType == QuestRewardType.Random)
                {
                    count += 1;
                }
            }

            return count;
        }

        public byte FixedRewardsNum()
        {
            byte count = 0;
            foreach (var reward in ItemRewards)
            {
                switch (reward.RewardType)
                {
                    case QuestRewardType.Fixed:
                    case QuestRewardType.Select:
                        count += 1;
                        break;
                }
            }
            return count;
        }

        public static List<CDataRewardBoxItem> AsCDataRewardBoxItems(QuestBoxRewards rewards)
        {
            List<CDataRewardBoxItem> results = new List<CDataRewardBoxItem>();

            var quest = QuestManager.GetQuest(rewards.QuestId);

            foreach (var reward in quest.SelectedableItemRewards)
            {
                results.AddRange(reward.AsCDataRewardBoxItems());
            }

            List<QuestRandomRewardItem> randomRewards = new List<QuestRandomRewardItem>();
            foreach (var reward in quest.ItemRewards)
            {
                if (reward.RewardType != QuestRewardType.Random)
                {
                    results.AddRange(reward.AsCDataRewardBoxItems());
                }
                else
                {
                    randomRewards.Add((QuestRandomRewardItem) reward);
                }
            }

            foreach (var randomReward in rewards.RandomRewardIndices.Zip(randomRewards, Tuple.Create))
            {
                results.Add(randomReward.Item2.AsCDataRewardBoxItem(randomReward.Item1));
            }

            return results;
        }

        public QuestBoxRewards GenerateBoxRewards()
        {
            QuestBoxRewards obj = new QuestBoxRewards()
            {
                QuestId = QuestId
            };

            foreach (var reward in ItemRewards)
            {
                if (reward.RewardType == QuestRewardType.Random)
                {
                    var randomReward = (QuestRandomRewardItem)reward;
                    obj.RandomRewardIndices.Add(randomReward.Roll());
                }
            }
            obj.NumRandomRewards = obj.RandomRewardIndices.Count;

            return obj;
        }

        public List<CDataRewardItem> GetQuestFixedRewards()
        {
            List<CDataRewardItem> rewards = new List<CDataRewardItem>();

            foreach (var reward in ItemRewards)
            {
                rewards.AddRange(reward.AsCDataRewardItems());
            }

            return rewards;
        }

        public List<CDataRewardItem> GetQuestSelectableRewards()
        {
            List<CDataRewardItem> rewards = new List<CDataRewardItem>();

            foreach (var reward in SelectedableItemRewards)
            {
                rewards.AddRange(reward.AsCDataRewardItems());
            }

            return rewards;
        }

        public List<CDataQuestOrderConditionParam> GetQuestOrderConditions()
        {
            List<CDataQuestOrderConditionParam> orderConditions = new List<CDataQuestOrderConditionParam>();

            foreach (var orderCondition in OrderConditions)
            {
                orderConditions.Add(orderCondition.ToCDataQuestOrderConditionParam());
            }

            return orderConditions;
        }

        public bool HasRewards()
        {
            return (ItemRewards.Count > 0) || (SelectedableItemRewards.Count > 0);
        }
    }

    public class QuestDoesNotExistException : ResponseErrorException
    {
        public QuestDoesNotExistException(QuestId questId) : base(ErrorCode.ERROR_CODE_QUEST_INTERNAL_ERROR, $"The quest ${questId} does not exist")
        {
        }
    }
}
