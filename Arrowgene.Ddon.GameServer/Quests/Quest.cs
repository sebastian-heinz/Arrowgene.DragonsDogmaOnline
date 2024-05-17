using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;


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
    }

    public abstract class Quest
    {
        public readonly QuestId QuestId;
        public readonly bool IsDiscoverable;
        public readonly QuestType QuestType;

        public uint BaseLevel { get; set; }
        public ushort MinimumItemRank { get; set; }

        public QuestRewardParams RewardParams { get; protected set; }
        public List<CDataWalletPoint> WalletRewards { get; protected set; }
        public List<CDataQuestExp> ExpRewards { get; protected set; }
        public List<QuestRewardItem> ItemRewards { get; protected set; }
        public List<QuestRewardItem> SelectedableItemRewards { get; protected set; }
        public List<CDataCharacterReleaseElement> ContentsReleaseRewards { get; protected set; }
        public List<QuestLocation> Locations { get; protected set; }

        public Quest(QuestId questId, QuestType questType, bool isDiscoverable = false)
        {
            QuestId = questId;
            QuestType = questType;
            IsDiscoverable = isDiscoverable;

            RewardParams = new QuestRewardParams();
            WalletRewards = new List<CDataWalletPoint>();
            ExpRewards = new List<CDataQuestExp>();
            ItemRewards = new List<QuestRewardItem>();
            SelectedableItemRewards = new List<QuestRewardItem>();
            ContentsReleaseRewards = new List<CDataCharacterReleaseElement>();
            Locations = new List<QuestLocation>();
        }

        public virtual CDataQuestList ToCDataQuestList()
        {
            return new CDataQuestList()
            {
                QuestId = (uint)QuestId,
                QuestScheduleId = (uint)QuestId,
                BaseLevel = BaseLevel,
                ContentJoinItemRank = MinimumItemRank,
                IsClientOrder = false,
                IsEnable = true,
                BaseExp = ExpRewards,
                BaseWalletPoints = WalletRewards,
                FixedRewardItemList = GetQuestFixedRewards(),
                FixedRewardSelectItemList = GetQuestSelectableRewards(),
            };
        }

        public virtual CDataQuestOrderList ToCDataQuestOrderList()
        {
            return new CDataQuestOrderList()
            {
                QuestId = (uint)QuestId,
                QuestScheduleId = (uint)QuestId,
                BaseLevel = BaseLevel,
                ContentJoinItemRank = MinimumItemRank,
                IsClientOrder = false,
                IsEnable = true,
                BaseExp = ExpRewards,
                BaseWalletPoints = WalletRewards,
                FixedRewardItem = GetQuestFixedRewards(),
                FixedRewardSelectItem = GetQuestSelectableRewards(),
            };
        }

        public abstract List<CDataQuestProcessState> StateMachineExecute(QuestProcessState processState, out QuestProgressState questProgressState);
        public abstract bool HasEnemiesInCurrentStageGroup(QuestState questState, StageId stageId, uint subGroupId);
        public abstract List<InstancedEnemy> GetEnemiesInStageGroup(StageId stageId, uint subGroupId);

        public virtual void SendProgressWorkNotices(GameClient client, StageId stageId, uint subGroupId)
        {
            client.Party.SendToAll(new S2CQuestQuestProgressWorkSaveNtc());
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

        public QuestBoxRewards GetBoxRewards()
        {
            QuestBoxRewards result = new QuestBoxRewards()
            {
                QuestId = QuestId
            };

            foreach (var reward in ItemRewards)
            {
                var cdataRewards = reward.AsCDataRewardBoxItems();
                foreach (var cdataReward in cdataRewards)
                {
                    result.Rewards[cdataReward.UID] = cdataReward;
                }
            }

            foreach (var reward in SelectedableItemRewards)
            {
                var cdataRewards = reward.AsCDataRewardBoxItems();
                foreach (var cdataReward in cdataRewards)
                {
                    result.Rewards[cdataReward.UID] = cdataReward;
                }
            }

            return result;
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

        public bool HasRewards()
        {
            return (ItemRewards.Count > 0) || (SelectedableItemRewards.Count > 0);
        }
    }
}
