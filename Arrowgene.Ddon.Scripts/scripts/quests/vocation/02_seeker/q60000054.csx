/**
 * @brief Seeking the Master Seeker
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.SeekingTheMasterSeeker;
    public override ushort RecommendedLevel => 18;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.TheWhiteDragonTemple0;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.VocationQuest;

    public override bool ShowInAdventureGuide(GameClient client)
    {
        return client.Character.HasQuestCompleted(QuestId.SeekerTacticsTrialAWhirlwind) &&
               client.Character.ActiveCharacterJobData.Job == JobId.Seeker;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumVocationLevel(JobId.Seeker, 18));
        AddQuestOrderCondition(QuestOrderCondition.PersonalQuestCleared(QuestId.SeekerTacticsTrialAWhirlwind));
        AddQuestOrderCondition(QuestOrderCondition.HasAreaRank(QuestAreaId.DoweValley, 5));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 2500);
        AddWalletReward(WalletType.Gold, 1000);
        AddWalletReward(WalletType.RiftPoints, 300);

        AddFixedItemReward(ItemId.ConquerorsAmulet, 1);
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdPlJobEq(JobId.Seeker)
            .AddCheckCmdIsTutorialQuestClear(QuestId.SeekerTacticsTrialAWhirlwind);
        process0.AddNpcTalkAndOrderBlock(Stage.TheWhiteDragonTemple0, NpcId.Renton0, 14025);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Accept, Stage.LoneHouseintheValley, NpcId.Chester0, 14027);
        process0.AddIsStageNoBlock(QuestAnnounceType.None, Stage.LoneHouseintheValley)
            .AddResultCmdReleaseAnnounce(ContentsRelease.SeekerJobTraining, TutorialId.JobTrainingLog);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
