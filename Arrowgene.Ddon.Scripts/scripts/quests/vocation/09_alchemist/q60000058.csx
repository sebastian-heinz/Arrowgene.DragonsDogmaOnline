/**
 * @brief Seeking the Master Alchemist
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.SeekingTheMasterAlchemist;
    public override ushort RecommendedLevel => 18;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.TheWhiteDragonTemple0;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.VocationQuest;

    public override bool ShowInAdventureGuide(GameClient client)
    {
        return client.Character.HasQuestCompleted(QuestId.AlchemistTacticsTrialAlchemy) &&
               client.Character.ActiveCharacterJobData.Job == JobId.Alchemist;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumVocationLevel(JobId.Alchemist, 18));
        AddQuestOrderCondition(QuestOrderCondition.PersonalQuestCleared(QuestId.AlchemistTacticsTrialAlchemy));
        AddQuestOrderCondition(QuestOrderCondition.HasAreaRank(QuestAreaId.MergodaRuins, 6));
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
            .AddCheckCmdPlJobEq(JobId.Alchemist)
            .AddCheckCmdIsTutorialQuestClear(QuestId.AlchemistTacticsTrialAlchemy);
        process0.AddNpcTalkAndOrderBlock(Stage.TheWhiteDragonTemple0, NpcId.Renton0, 14037);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Accept, Stage.TheodorsAlchemyWorkshop, NpcId.Theodor, 14039);
        process0.AddIsStageNoBlock(QuestAnnounceType.None, Stage.TheodorsAlchemyWorkshop)
            .AddResultCmdReleaseAnnounce(ContentsRelease.AlchemistJobTraining, TutorialId.JobTrainingLog);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
