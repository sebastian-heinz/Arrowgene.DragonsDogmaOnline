/**
 * @brief Skill Augmentation Hunter Trial: I
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.SkillAugmentationHunterTrialI;
    public override ushort RecommendedLevel => 40;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.IvansLodge;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.VocationQuest;

    public override bool ShowInAdventureGuide(GameClient client)
    {
        return client.Character.HasQuestCompleted(QuestId.SeekingTheMasterHunter) &&
               client.Character.ActiveCharacterJobData.Job == JobId.Hunter &&
               (client.Character.CompletedQuests.ContainsKey(QuestId.SkillAugmentationGuidanceOfARenewedWhiteDragon) || client.QuestState.IsQuestAccepted(QuestId.SkillAugmentationGuidanceOfARenewedWhiteDragon));
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumVocationLevel(JobId.Hunter, 40));
        AddQuestOrderCondition(QuestOrderCondition.HasAreaRank(QuestAreaId.VoldenMines, 2));
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
        public const uint Ivan = 3587;
    }

    private class MyQstFlag
    {
        public const uint IvanFSM0 = 1358;
        public const uint IvanFSM1 = 1359;
        public const uint IvanFSM2 = 1360;
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.SouthernWallFissure, 12, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.SnowHarpy, 40, 0)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
            LibDdon.Enemy.CreateAuto(EnemyId.GoblinBomber, 40, 1)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
            LibDdon.Enemy.CreateAuto(EnemyId.SnowHarpy, 40, 2)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
            LibDdon.Enemy.CreateAuto(EnemyId.GoblinBomber, 40, 3)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.SouthernWallFissure, 7, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.SwordUndead, 40, 0)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordUndead, 40, 2)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordUndead, 40, 3)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordUndead, 40, 4)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
        });

        AddEnemies(EnemyGroupId.Encounter + 2, Stage.SouthernWallFissure, 16, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.DreadApe, 40, 0)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.FinalTest)
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdIsTutorialQuestClear(QuestId.SkillAugmentationGuidanceOfARenewedWhiteDragon, 0)
            .AddCheckCommandIsTutorialQuestOrder(QuestId.SkillAugmentationGuidanceOfARenewedWhiteDragon, 1);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdPlJobEq(JobId.Hunter)
            .AddCheckCmdIsTutorialQuestClear(QuestId.SeekingTheMasterHunter);
        process0.AddNpcTalkAndOrderBlock(Stage.IvansLodge, NpcId.Ivan, 17622);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Accept, Stage.SouthernWallFissure, 1, 1, NpcId.Ivan, 17624)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.Ivan);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQstFlag.IvanFSM0);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0, resetGroup: false);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1, resetGroup: false);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 2);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 2, resetGroup: false);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.IvansLodge, NpcId.Ivan, 17626);
        process0.AddIsStageNoBlock(QuestAnnounceType.None, Stage.IvansLodge)
           .AddResultCmdReleaseAnnounce(ContentsRelease.HunterWarSkillAugmentation, TutorialId.InheritanceSkillsOfBattleTechniques);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
