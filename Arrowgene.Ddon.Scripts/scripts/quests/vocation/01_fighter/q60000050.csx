/**
 * @brief Seeking the Master Fighter
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.SeekingTheMasterFighter;
    public override ushort RecommendedLevel => 18;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.TheWhiteDragonTemple0;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.VocationQuest;

    public override bool ShowInAdventureGuide(GameClient client)
    {
        return client.Character.HasQuestCompleted(QuestId.FighterTacticsTrialBreakAttack) &&
               client.Character.ActiveCharacterJobData.Job == JobId.Fighter;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumVocationLevel(JobId.Fighter, 18));
        AddQuestOrderCondition(QuestOrderCondition.PersonalQuestCleared(QuestId.FighterTacticsTrialBreakAttack));
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
            .AddCheckCmdPlJobEq(JobId.Fighter)
            .AddCheckCmdIsTutorialQuestClear(QuestId.FighterTacticsTrialBreakAttack);
        process0.AddNpcTalkAndOrderBlock(Stage.TheWhiteDragonTemple0, NpcId.Renton0, 14011);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Accept, Stage.GrittenFort0, NpcId.Vanessa0, 14014);
        process0.AddIsStageNoBlock(QuestAnnounceType.None, Stage.GrittenFort0)
            .AddResultCmdReleaseAnnounce(ContentsRelease.FighterJobTraining, TutorialId.JobTrainingLog);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
