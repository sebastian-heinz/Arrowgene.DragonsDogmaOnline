/**
 * @brief Seeking the Master Priest
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.SeekingTheMasterPriest;
    public override ushort RecommendedLevel => 18;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.TheWhiteDragonTemple0;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.VocationQuest;

    public override bool ShowInAdventureGuide(GameClient client)
    {
        return client.Character.HasQuestCompleted(QuestId.PriestTacticsTrialBreakAttack) &&
               client.Character.ActiveCharacterJobData.Job == JobId.Priest;
    }

    public override bool AcceptRequirementsMet(GameClient client)
    {
        return client.Character.ActiveCharacterJobData.Job == JobId.Priest;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumVocationLevel(JobId.Priest, 18));
        AddQuestOrderCondition(QuestOrderCondition.PersonalQuestCleared(QuestId.PriestTacticsTrialBreakAttack));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 2500);
        AddWalletReward(WalletType.Gold, 1000);
        AddWalletReward(WalletType.RiftPoints, 300);

        AddFixedItemReward(ItemId.SpritesAmulet, 1);
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdPlJobEq(JobId.Priest)
            .AddCheckCmdIsTutorialQuestClear(QuestId.PriestTacticsTrialBreakAttack);
        process0.AddNpcTalkAndOrderBlock(Stage.TheWhiteDragonTemple0, NpcId.Renton0, 14015);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Accept, Stage.PawnCathedral, NpcId.Camus, 14017);
        process0.AddIsStageNoBlock(QuestAnnounceType.None, Stage.PawnCathedral)
            .AddResultCmdReleaseAnnounce(ContentsRelease.PriestJobTraining, TutorialId.JobTrainingLog);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
