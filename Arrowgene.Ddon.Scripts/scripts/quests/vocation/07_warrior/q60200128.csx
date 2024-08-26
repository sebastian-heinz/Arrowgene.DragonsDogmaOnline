/**
 * @brief Skill Augmentation Warrior Trial: I
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.SkillAugmentationWarriorTrialI;
    public override ushort RecommendedLevel => 40;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.OswaldsArmorShop;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.VocationQuest;

    public override bool ShowInAdventureGuide(GameClient client)
    {
        return client.Character.HasQuestCompleted(QuestId.SeekingTheMasterWarrior) &&
               client.Character.ActiveCharacterJobData.Job == JobId.Warrior &&
               (client.Character.CompletedQuests.ContainsKey(QuestId.SkillAugmentationGuidanceOfARenewedWhiteDragon) || client.QuestState.IsQuestAccepted(QuestId.SkillAugmentationGuidanceOfARenewedWhiteDragon));
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumVocationLevel(JobId.Warrior, 40));
        AddQuestOrderCondition(QuestOrderCondition.HasAreaRank(QuestAreaId.EasternZandora, 3));
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
        public const uint Oliver = 3597;
    }

    private class MyQstFlag
    {
        public const uint OliverFSM0 = 1388;
        public const uint OliverFSM1 = 1389;
        public const uint OliverFSM2 = 1390;
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.AbandonedUndergroundPassage, 11, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.SkeletonKnight, 40, 1)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
            LibDdon.Enemy.CreateAuto(EnemyId.SkeletonMage0, 40, 2)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
            LibDdon.Enemy.CreateAuto(EnemyId.SkeletonKnight, 40, 0)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
            LibDdon.Enemy.CreateAuto(EnemyId.SkeletonKnight, 40, 3)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.AbandonedUndergroundPassage, 8, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.UndeadMale, 40, 1)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.SkeletonKnight, 40, 2)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.StoutUndead, 40, 3)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.SkeletonKnight, 40, 4)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.UndeadFemale, 40, 5)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
        });

        AddEnemies(EnemyGroupId.Encounter + 2, Stage.AbandonedUndergroundPassage, 5, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.GhostMail, 40, 1, isBoss: true)
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
            .AddCheckCmdPlJobEq(JobId.Warrior)
            .AddCheckCmdIsTutorialQuestClear(QuestId.SeekingTheMasterWarrior);
        process0.AddNpcTalkAndOrderBlock(Stage.OswaldsArmorShop, NpcId.Oliver, 17789);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Accept, Stage.AbandonedUndergroundPassage, 1, 1, NpcId.Oliver, 17791)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.Oliver);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQstFlag.OliverFSM0);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0, resetGroup: false);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1, resetGroup: false);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 2);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 2, resetGroup: false);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.OswaldsArmorShop, NpcId.Oliver, 17793);
        process0.AddIsStageNoBlock(QuestAnnounceType.None, Stage.OswaldsArmorShop)
           .AddResultCmdReleaseAnnounce(ContentsRelease.WarriorWarSkillAugmentation, TutorialId.InheritanceSkillsOfBattleTechniques);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
