/**
 * @brief Skill Augmentation Alchemist Trial: I
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.SkillAugmentationAlchemistTrialI;
    public override ushort RecommendedLevel => 40;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.TheodorsAlchemyWorkshop;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.VocationQuest;

    public override bool ShowInAdventureGuide(GameClient client)
    {
        return client.Character.HasQuestCompleted(QuestId.SeekingTheMasterAlchemist) &&
               client.Character.ActiveCharacterJobData.Job == JobId.Alchemist &&
               (client.Character.CompletedQuests.ContainsKey(QuestId.SkillAugmentationGuidanceOfARenewedWhiteDragon) || client.QuestState.IsQuestAccepted(QuestId.SkillAugmentationGuidanceOfARenewedWhiteDragon));
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumVocationLevel(JobId.Alchemist, 40));
        AddQuestOrderCondition(QuestOrderCondition.HasAreaRank(QuestAreaId.MergodaRuins, 6));
        AddQuestOrderCondition(QuestOrderCondition.Solo());
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.TheFateOfLestania));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 2500);
        AddWalletReward(WalletType.Gold, 600);
        AddWalletReward(WalletType.RiftPoints, 60);
    }

    private class EnemyGroupId
    {
        public const uint Encounter = 10;
    }

    private class NamedParamId
    {
        public const uint FirstTest = 1143; // First Test <name>
        public const uint SecondTest = 1144; // Second Test <name>
        public const uint FinalTest = 859; // Final Test <name>
    }

    private class QstLayoutFlag
    {
        public const uint Theodor = 3599;
    }

    private class MyQstFlag
    {
        public const uint TheodorFSM0 = 1394;
        public const uint TheodorFSM1 = 1395;
        public const uint TheodorFSM2 = 1396;
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.TheArk1, 1, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.Skeleton, 40, 0)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
            LibDdon.Enemy.CreateAuto(EnemyId.Skeleton, 40, 1)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
            LibDdon.Enemy.CreateAuto(EnemyId.Skeleton, 40, 2)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
            LibDdon.Enemy.CreateAuto(EnemyId.SkeletonMage0, 40, 3)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
            LibDdon.Enemy.CreateAuto(EnemyId.SkeletonMage0, 40, 4)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.TheArk1, 6, 2, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.AlchemizedSkeleton, 40, 1)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.SkeletonKnight, 40, 0)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.SkeletonKnight, 40, 2)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.SkeletonKnight, 40, 3)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.SkeletonWarrior, 40, 4)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.SkeletonWarrior, 40, 5)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
        });

        AddEnemies(EnemyGroupId.Encounter + 2, Stage.TheArk1, 10, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.AlchemizedSkeleton, 40, 0)
                .SetNamedEnemyParams(NamedParamId.FinalTest),
            LibDdon.Enemy.CreateAuto(EnemyId.AlchemizedSkeleton, 40, 1)
                .SetNamedEnemyParams(NamedParamId.FinalTest),
            LibDdon.Enemy.CreateAuto(EnemyId.AlchemizedWolf, 40, 8)
                .SetNamedEnemyParams(NamedParamId.FinalTest),
            LibDdon.Enemy.CreateAuto(EnemyId.AlchemizedWolf, 40, 9)
                .SetNamedEnemyParams(NamedParamId.FinalTest),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdIsTutorialQuestClear(QuestId.SkillAugmentationGuidanceOfARenewedWhiteDragon, 0)
            .AddCheckCommandIsTutorialQuestOrder(QuestId.SkillAugmentationGuidanceOfARenewedWhiteDragon, 1);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdPlJobEq(JobId.Alchemist)
            .AddCheckCmdIsTutorialQuestClear(QuestId.SeekingTheMasterAlchemist);
        process0.AddNpcTalkAndOrderBlock(Stage.TheodorsAlchemyWorkshop, NpcId.Theodor, 17804);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Accept, Stage.TheArk1, 1, 1, NpcId.Theodor, 17806)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.Theodor);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQstFlag.TheodorFSM0);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0, resetGroup: false);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1, resetGroup: false);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 2);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 2, resetGroup: false);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheodorsAlchemyWorkshop, NpcId.Theodor, 17808);
        process0.AddIsStageNoBlock(QuestAnnounceType.None, Stage.TheodorsAlchemyWorkshop)
           .AddResultCmdReleaseAnnounce(ContentsRelease.AlchemistWarSkillAugmentation, TutorialId.InheritanceSkillsOfBattleTechniques);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
