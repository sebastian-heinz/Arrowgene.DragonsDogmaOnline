/**
 * @brief Skill Augmentation Guidance of a Renewed White Dragon
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.SkillAugmentationGuidanceOfARenewedWhiteDragon;
    public override ushort RecommendedLevel => 40;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.AudienceChamber;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.QuestUsefulForAdventure;

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.SoloWithPawns());
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.TheFateOfLestania));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 2500);
        AddWalletReward(WalletType.Gold, 600);
        AddWalletReward(WalletType.RiftPoints, 60);
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdIsMainQuestClear(QuestId.TheFateOfLestania);
        process0.AddNpcTalkAndOrderBlock(Stage.AudienceChamber, NpcId.TheWhiteDragon, 17864);
        process0.AddRawBlock(QuestAnnounceType.Accept)
            .AddCheckCmdIsTutorialQuestClear(QuestId.SkillAugmentationFighterTrialI, 0)
            .AddCheckCmdIsTutorialQuestClear(QuestId.SkillAugmentationPriestTrialI, 1)
            .AddCheckCmdIsTutorialQuestClear(QuestId.SkillAugmentationHunterTrialI, 2)
            .AddCheckCmdIsTutorialQuestClear(QuestId.SkillAugmentationShieldSageTrialI, 3)
            .AddCheckCmdIsTutorialQuestClear(QuestId.SkillAugmentationSeekerTrialI, 4)
            .AddCheckCmdIsTutorialQuestClear(QuestId.SkillAugmentationSorcererTrialI, 5)
            .AddCheckCmdIsTutorialQuestClear(QuestId.SkillAugmentationElementArcherTrialI, 6)
            .AddCheckCmdIsTutorialQuestClear(QuestId.SkillAugmentationWarriorTrialI, 7)
            .AddCheckCmdIsTutorialQuestClear(QuestId.SkillAugmentationAlchemistTrialI, 8)
            .AddCheckCmdIsTutorialQuestClear(QuestId.SkillAugmentationSpiritLancerTrialI, 9)
            .AddCheckCmdIsTutorialQuestClear(QuestId.HighScepterTacticsTrialAMagickSword, 10)
            .AddResultCmdReleaseAnnounce(ContentsRelease.WarSkillAugmentation, TutorialId.ArisensFurtherGrowthandDragonForceAugmentation);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.AudienceChamber, NpcId.TheWhiteDragon, 17866);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
