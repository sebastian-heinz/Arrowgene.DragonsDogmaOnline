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
        public QuestRewardParams()
        {

        }
        public byte ChargeRewardNum { get; set; }
        public byte ProgressBonusNum { get; set; }
        public bool IsRepeatReward { get; set; }
        public bool IsUndiscoveredReward { get; set; }
        public bool IsHelpReward { get; set; }
        public bool IsPartyBonus { get; set; }
    }

    public abstract class Quest
    {
        public readonly QuestId QuestId;
        public readonly bool IsDiscoverable;
        public readonly QuestType QuestType;
        virtual public QuestRewardParams RewardParams { get { return new QuestRewardParams(); } }
        virtual public List<CDataWalletPoint> WalletRewards { get { return new List<CDataWalletPoint>(); } }
        virtual public List<CDataQuestExp> ExpRewards { get { return new List<CDataQuestExp>(); } }

        virtual public List<QuestReward> ItemRewards { get { return new List<QuestReward>(); } }
        virtual public List<QuestReward> SelectedableItemRewards { get { return new List<QuestReward>(); } }
        virtual public List<CDataCharacterReleaseElement> ContentsReleaseReards { get { return new List<CDataCharacterReleaseElement>(); } }

        public Quest(QuestId questId, QuestType questType, bool isDiscoverable = false)
        {
            QuestId = questId;
            QuestType = questType;
            IsDiscoverable = isDiscoverable;
        }

        public abstract CDataQuestList ToCDataQuestList();
        public abstract List<CDataQuestProcessState> StateMachineExecute(uint processNo, uint reqNo, out QuestState questState);
        public abstract List<S2CQuestQuestProgressWorkSaveNtc> GetProgressWorkNotices(uint stageNo, uint groupId, uint subGroupId);
        public abstract bool HasEnemiesInCurrentStageGroup(uint stageNo, uint groupId, uint subGroupId);

        public List<QuestReward> GetQuestRewards()
        {
            List<QuestReward> rewards = new List<QuestReward>();

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

        public QuestBoxRewards GetBoxRewards()
        {
            QuestBoxRewards result = new QuestBoxRewards()
            {
                QuestId = QuestId
            };

            foreach (var reward in ItemRewards)
            {
                result.Rewards.Add(reward.AsCDataRewardBoxItem());
            }

            foreach (var reward in SelectedableItemRewards)
            {
                result.Rewards.Add(reward.AsCDataRewardBoxItem());
            }

            return result;
        }

        public List<CDataRewardItem> GetQuestFixedRewards()
        {
            List<CDataRewardItem> rewards = new List<CDataRewardItem>();

            foreach (var reward in ItemRewards)
            {
                rewards.Add(reward.AsCDataRewardItem());
            }

            return rewards;
        }

        public List<CDataRewardItem> GetQuestSelectableRewards()
        {
            List<CDataRewardItem> rewards = new List<CDataRewardItem>();

            foreach (var reward in SelectedableItemRewards)
            {
                rewards.Add(reward.AsCDataRewardItem());
            }

            return rewards;
        }

        public bool HasRewards()
        {
            return (ItemRewards.Count > 0) || (SelectedableItemRewards.Count > 0);
        }
    }
}
