/**
 * @brief Skill Augmentation Priest Trial: II
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.SkillAugmentationPriestTrialII;
    public override ushort RecommendedLevel => 45;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.PawnCathedral;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.VocationQuest;

    public override bool ShowInAdventureGuide(GameClient client)
    {
        return client.Character.HasQuestCompleted(QuestId.SkillAugmentationPriestTrialI) &&
               client.Character.ActiveCharacterJobData.Job == JobId.Priest;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumVocationLevel(JobId.Priest, 45));
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
        public const uint FirstTest = 1143; // First Test <name>
        public const uint SecondTest = 1144; // Second Test <name>
        public const uint FinalTest = 859; // Final Test <name>
    }

    private class QstLayoutFlag
    {
        public const uint Camus = 3586;
    }

    private class MyQstFlag
    {
        public const uint CamusFSM0 = 1355;
        public const uint CamusFSM1 = 1356;
        public const uint CamusFSM2 = 1357;
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.HidellCatacombsDepths, 3, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.SwordUndead, 42, 0)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordUndead, 42, 1)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordUndead, 42, 2)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.HidellCatacombsDepths, 7, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.SkeletonKnight, 43, 0)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.SkeletonKnight, 43, 1)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.Grimwarg, 42, 2)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.Grimwarg, 42, 3)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
        });

        AddEnemies(EnemyGroupId.Encounter + 2, Stage.HidellCatacombsDepths, 9, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.EmpressGhost, 45, 0, isBoss: true)
                .SetNamedEnemyParams(NamedParamId.FinalTest)
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdPlJobEq(JobId.Priest)
            .AddCheckCmdIsTutorialQuestClear(QuestId.SkillAugmentationPriestTrialI);
        process0.AddNpcTalkAndOrderBlock(Stage.PawnCathedral, NpcId.Camus, 17613);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Accept, Stage.HidellCatacombsDepths, 1, 1, NpcId.Camus, 17615)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.Camus);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQstFlag.CamusFSM0);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0, resetGroup: false);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1, resetGroup: false);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 2);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 2, resetGroup: false);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.PawnCathedral, NpcId.Camus, 17617);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
