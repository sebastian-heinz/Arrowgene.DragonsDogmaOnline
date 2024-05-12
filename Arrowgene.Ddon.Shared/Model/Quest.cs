using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;


namespace Arrowgene.Ddon.GameServer.Quests
{
    public class QuestRewardParams
    {
        public QuestRewardParams()
        {

        }
        public byte RandomRewardNum { get; set; }
        public byte ChargeRewardNum { get; set; }
        public byte ProgressBonusNum { get; set; }
        public bool IsRepeatReward { get; set; }
        public bool IsUndiscoveredReward { get; set; }
        public bool IsHelpReward { get; set; }
        public bool IsPartyBonus { get; set; }
    }

    public abstract class Quest
    {
        public readonly bool IsDiscoverable;
        public readonly QuestType QuestType;
        virtual public QuestRewardParams RewardParams { get { return new QuestRewardParams(); } }
        virtual public List<CDataWalletPoint> WalletRewards { get { return new List<CDataWalletPoint>(); } }
        virtual public List<CDataRewardItem> FixedItemRewards { get { return new List<CDataRewardItem>(); } }

        virtual public List<CDataQuestExp> ExpRewards { get { return new List<CDataQuestExp>(); } }

        public Quest(QuestType questType, bool isDiscoverable = false)
        {
            QuestType = questType;
            IsDiscoverable = isDiscoverable;
        }

        public abstract CDataQuestList ToCDataQuestList();
        public abstract List<CDataQuestProcessState> StateMachineExecute(uint processNo, uint reqNo, out QuestState questState);
        public abstract List<S2CQuestQuestProgressWorkSaveNtc> GetProgressWorkNotices(uint stageNo, uint groupId, uint subGroupId);
        public abstract bool HasEnemiesInCurrentStageGroup(uint stageNo, uint groupId, uint subGroupId);
    }
}
