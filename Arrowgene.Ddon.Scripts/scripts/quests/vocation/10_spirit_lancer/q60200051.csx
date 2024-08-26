/**
 * @brief Seeking the Master Spirit Lancer
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.SeekingTheMasterSpiritLancer;
    public override ushort RecommendedLevel => 18;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.DanaCentrum;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.VocationQuest;

    public override bool ShowInAdventureGuide(GameClient client)
    {
        return client.Character.HasQuestCompleted(QuestId.SpiritLancerTacticsTrialASpiritLance) &&
               client.Character.ActiveCharacterJobData.Job == JobId.SpiritLancer;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumVocationLevel(JobId.SpiritLancer, 18));
        AddQuestOrderCondition(QuestOrderCondition.PersonalQuestCleared(QuestId.SpiritLancerTacticsTrialASpiritLance));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 2500);
        AddWalletReward(WalletType.Gold, 1000);
        AddWalletReward(WalletType.RiftPoints, 300);

        AddFixedItemReward(ItemId.DragonSpirits, 5);
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdPlJobEq(JobId.SpiritLancer)
            .AddCheckCmdIsTutorialQuestClear(QuestId.SpiritLancerTacticsTrialASpiritLance);
        process0.AddNpcTalkAndOrderBlock(Stage.DanaCentrum, NpcId.Razanailt, 19699);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Accept, Stage.SpiritArtsHut, NpcId.AdairDonnchadh0, 19701);
        process0.AddIsStageNoBlock(QuestAnnounceType.None, Stage.SpiritArtsHut)
            .AddResultCmdReleaseAnnounce(ContentsRelease.SpiritLancerJobTraining, TutorialId.JobTrainingLog);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
