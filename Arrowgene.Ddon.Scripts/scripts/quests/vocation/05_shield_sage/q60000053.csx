/**
 * @brief Seeking the Master Shield Sage
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.SeekingTheMasterShieldSage;
    public override ushort RecommendedLevel => 18;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.TheWhiteDragonTemple0;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.VocationQuest;

    public override bool ShowInAdventureGuide(GameClient client)
    {
        return client.Character.HasQuestCompleted(QuestId.ShieldSageTacticsTrialBreakAttack) &&
               client.Character.ActiveCharacterJobData.Job == JobId.ShieldSage;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumVocationLevel(JobId.ShieldSage, 18));
        AddQuestOrderCondition(QuestOrderCondition.PersonalQuestCleared(QuestId.ShieldSageTacticsTrialBreakAttack));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 2500);
        AddWalletReward(WalletType.Gold, 1000);
        AddWalletReward(WalletType.RiftPoints, 300);

        AddFixedItemReward(ItemId.AngelsAmulet, 1);
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdPlJobEq(JobId.ShieldSage)
            .AddCheckCmdIsTutorialQuestClear(QuestId.ShieldSageTacticsTrialBreakAttack);
        process0.AddNpcTalkAndOrderBlock(Stage.TheWhiteDragonTemple0, NpcId.Renton0, 14022);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Accept, Stage.LighthouseKeepersHouse, NpcId.Rudolfo, 14024);
        process0.AddIsStageNoBlock(QuestAnnounceType.None, Stage.LighthouseKeepersHouse)
            .AddResultCmdReleaseAnnounce(ContentsRelease.ShieldSageJobTraining, TutorialId.JobTrainingLog);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
