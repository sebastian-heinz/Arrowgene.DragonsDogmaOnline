using Arrowgene.Ddon.GameServer.Quests.Extensions;
using Arrowgene.Ddon.GameServer.Scripting.Interfaces;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Quests.LightQuests
{
    public abstract class LightQuestQuest : IQuest
    {
        public override QuestType QuestType => QuestType.Light;
        public override QuestId QuestId => QuestRecord?.QuestId ?? 0;
        public override QuestAreaId QuestAreaId => QuestRecord?.QuestInfo.AreaId ?? QuestAreaId.None;
        public override ushort RecommendedLevel => QuestRecord.Level;
        public override byte MinimumItemRank => 0;
        public override bool IsDiscoverable => false;
        public override uint QuestScheduleId => Shared.Model.Quest.QuestScheduleId.GenerateRotatingId(
            (byte)Shared.Model.Quest.QuestScheduleId.ScheduleIdType.Board, 
            VariantId);

        public LightQuestRecord QuestRecord { get; set; }

        public LightQuestQuest(LightQuestRecord record)
        {
            QuestRecord = record;

            AddPointReward(PointType.ExperiencePoints, record.RewardXP);
            AddPointReward(PointType.AreaPoints, record.RewardAP);
            AddWalletReward(WalletType.Gold, record.RewardG);
            AddWalletReward(WalletType.RiftPoints, record.RewardR);
        }

        new public Quest GenerateQuest(DdonGameServer server)
        {
            InitializeBlocks();

            LightQuestInfo info = LightQuestId.FromQuestId(QuestId);
            CDataLightQuestDetail lightQuestDetail = new()
            {
                AreaId = (uint)info.AreaId,
                BoardId = (uint)info.Board,
                GetAp = QuestRecord.RewardAP,
                BoardType = 1,
            };

            var assetData = new QuestAssetData()
            {
                BaseLevel = RecommendedLevel,
                Discoverable = IsDiscoverable,
                Enabled = Enabled,
                NewsImageId = NewsImageId,
                MinimumItemRank = MinimumItemRank,
                NextQuestId = NextQuestId,
                OverrideEnemySpawn = OverrideEnemySpawn.Value,
                EnableCancel = EnableCancel.Value,
                QuestAreaId = QuestAreaId,
                QuestId = QuestId,
                QuestOrderBackgroundImage = QuestOrderBackgroundImage,
                IsImportant = IsImportant.Value,
                AdventureGuideCategory = AdventureGuideCategory.Value,
                QuestSource = QuestSource,
                VariantIndex = QuestRecord.VariantIndex,
                QuestType = QuestType,
                PointRewards = PointRewards,
                Processes = Processes.Values.ToList(),
                RewardCurrency = WalletRewards,
                StageLayoutId = StageInfo.AsStageLayoutId(0, 0),
                ResetPlayerAfterQuest = ResetPlayerAfterQuest,
                MissionParams = MissionParams,
                OrderConditions = OrderConditions,
                ServerActions = ServerActions,
                ContentsReleased = ContentsReleased,
                WorldManageUnlocks = WorldManageUnlocks,
                QuestProgressWork = QuestProgressWork,
                LightQuestDetail = lightQuestDetail,
                DistributionStart = QuestRecord.DistributionStart,
                DistributionEnd = QuestRecord.DistributionEnd,
            };

            return GenericQuest.FromAsset(server, assetData, this);
        }

        public abstract CDataQuestProgressWork StepAsWork(uint step);
    }

    public class LightQuestHuntQuest(LightQuestRecord record) : LightQuestQuest(record)
    {
        protected override void InitializeBlocks()
        {
            var process0 = AddNewProcess(0);

            process0.AddRawBlock(QuestAnnounceType.Accept)
                .AddCheckCmdIsOrderLightQuest();

            process0.AddKillTargetEnemiesBlock(QuestAnnounceType.Checkpoint, (EnemyUIId)QuestRecord.Target, QuestRecord.Level, (uint)QuestRecord.Count);

            process0.AddProcessEndBlock(true);
        }

        public override CDataQuestProgressWork StepAsWork(uint step)
        {
            return new CDataQuestProgressWork()
            {
                CommandNo = (uint)QuestNotifyCommand.KilledEnemyLight,
                Work01 = QuestRecord.Target,
                Work02 = QuestRecord.Level,
                Work03 = (int)(step - 1),
                Work04 = 0,
            };
        }
    }

    public class LightQuestDeliveryQuest(LightQuestRecord record) : LightQuestQuest(record)
    {
        protected override void InitializeBlocks()
        {
            var process0 = AddNewProcess(0);

            process0.AddRawBlock(QuestAnnounceType.Accept)
                .AddCheckCmdIsOrderLightQuest();

            process0.AddGatherItemsLightBlock(QuestAnnounceType.Checkpoint, (ItemId)QuestRecord.Target, (uint)QuestRecord.Count);

            process0.AddDeliverItemsLightBlock(QuestAnnounceType.CheckpointAndUpdate, (ItemId)QuestRecord.Target, (uint)QuestRecord.Count);

            process0.AddProcessEndBlock(true);
        }

        public override CDataQuestProgressWork StepAsWork(uint step)
        {
            return null;
        }
    }
}
