/**
 * @brief Skill Augmentation Seeker Trial: II
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.SkillAugmentationSeekerTrialII;
    public override ushort RecommendedLevel => 45;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.LoneHouseintheValley;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.VocationQuest;

    public override bool ShowInAdventureGuide(GameClient client)
    {
        return client.Character.HasQuestCompleted(QuestId.SkillAugmentationSeekerTrialI) &&
               client.Character.ActiveCharacterJobData.Job == JobId.Seeker;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumVocationLevel(JobId.Seeker, 45));
        AddQuestOrderCondition(QuestOrderCondition.Solo());
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.StrayingPower));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 3200);
        AddWalletReward(WalletType.Gold, 750);
        AddWalletReward(WalletType.RiftPoints, 80);
    }

    private class EnemyGroupId
    {
        public const uint Encounter = 10;
    }

    private class NamedParamId
    {
        public const uint FirstTest = 1143; // First Test
        public const uint SecondTest = 1144; // Second Test
        public const uint FinalTest = 859; // Final Test <name>
    }

    private class QstLayoutFlag
    {
        public const uint Chester = 3592;
    }

    private class MyQstFlag
    {
        public const uint ChesterFSM0 = 1373;
        public const uint ChesterFSM1 = 1374;
        public const uint ChesterFSM2 = 1375;
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.ForestPeoplesTunnel, 1, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.ForestGoblinFighter, 42, 0)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
            LibDdon.Enemy.CreateAuto(EnemyId.ForestGoblinFighter, 42, 1)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
            LibDdon.Enemy.CreateAuto(EnemyId.Mudman, 43, 2)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.ForestPeoplesTunnel, 4, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.Wolf, 42, 2)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.Wolf, 42, 3)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.Mudman, 43, 6)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.Wolf, 42, 7)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.Wolf, 42, 9)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
        });

        AddEnemies(EnemyGroupId.Encounter + 2, Stage.ForestPeoplesTunnel, 6, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.SilverRoar, 45, 0)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.FinalTest)
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdPlJobEq(JobId.Seeker)
            .AddCheckCmdIsTutorialQuestClear(QuestId.SkillAugmentationSeekerTrialI);
        process0.AddNpcTalkAndOrderBlock(Stage.LoneHouseintheValley, NpcId.Chester0, 17667);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Accept, Stage.ForestPeoplesTunnel, 1, 1, NpcId.Chester0, 17669)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.Chester);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQstFlag.ChesterFSM0);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0, resetGroup: false);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1, resetGroup: false);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 2);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 2, resetGroup: false);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.LoneHouseintheValley, NpcId.Chester0, 17671);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
