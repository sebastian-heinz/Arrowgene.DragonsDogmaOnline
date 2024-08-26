/**
 * @brief Seeking the Master Element Archer
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.SeekingTheMasterElementArcher;
    public override ushort RecommendedLevel => 18;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.TheWhiteDragonTemple0;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.VocationQuest;

    public override bool ShowInAdventureGuide(GameClient client)
    {
        return client.Character.HasQuestCompleted(QuestId.ElementArcherTacticsTrialAMagickBow) &&
               client.Character.ActiveCharacterJobData.Job == JobId.ElementArcher;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumVocationLevel(JobId.ElementArcher, 18));
        AddQuestOrderCondition(QuestOrderCondition.PersonalQuestCleared(QuestId.ElementArcherTacticsTrialAMagickBow));
        AddQuestOrderCondition(QuestOrderCondition.HasAreaRank(QuestAreaId.DeenanWoods, 5));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 2500);
        AddWalletReward(WalletType.Gold, 1000);
        AddWalletReward(WalletType.RiftPoints, 300);

        AddFixedItemReward(ItemId.DemonsAmulet, 1);
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdPlJobEq(JobId.ElementArcher)
            .AddCheckCmdIsTutorialQuestClear(QuestId.ElementArcherTacticsTrialAMagickBow);
        process0.AddNpcTalkAndOrderBlock(Stage.TheWhiteDragonTemple0, NpcId.Renton0, 14031);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Accept, Stage.SecretBowmakersHome, NpcId.Ringdeel0, 14033);
        process0.AddIsStageNoBlock(QuestAnnounceType.None, Stage.SecretBowmakersHome)
            .AddResultCmdReleaseAnnounce(ContentsRelease.ElementArcherJobTraining, TutorialId.JobTrainingLog);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
