/**
 * @brief Skill Augmentation Sorcerer Trial: III
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.SkillAugmentationSorcererTrialIII;
    public override ushort RecommendedLevel => 50;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.LoneHouseintheForest;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.VocationQuest;

    public override bool ShowInAdventureGuide(GameClient client)
    {
        return client.Character.HasQuestCompleted(QuestId.SkillAugmentationSorcererTrialII) &&
               client.Character.ActiveCharacterJobData.Job == JobId.Sorcerer;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumVocationLevel(JobId.Sorcerer, 50));
        AddQuestOrderCondition(QuestOrderCondition.Solo());
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.TheDarknessOfTheHeart));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 3400);
        AddWalletReward(WalletType.Gold, 820);
        AddWalletReward(WalletType.RiftPoints, 100);

        AddFixedItemReward(ItemId.CrestOfPermafrost0, 1);
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
        public const uint Emerada = 4110;
    }

    private class MyQstFlag
    {
        public const uint EmeradaFSM0 = 1812;
        public const uint EmeradaFSM1 = 1813;
        public const uint EmeradaFSM2 = 1814;
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.AncientBurialMound, 7, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.Eliminator, 50, 2)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
            LibDdon.Enemy.CreateAuto(EnemyId.Eliminator, 50, 5)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.AncientBurialMound, 8, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.FrostCorpsePunisher, 50, 0)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.FrostCorpsePunisher, 50, 1)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.FrostCorpsePunisher, 50, 3)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
        });

        AddEnemies(EnemyGroupId.Encounter + 2, Stage.AncientBurialMound, 11, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.SkeletonMage0, 50, 1)
                .SetNamedEnemyParams(NamedParamId.FinalTest),
            LibDdon.Enemy.CreateAuto(EnemyId.SkeletonBrute, 50, 0)
                .SetNamedEnemyParams(NamedParamId.FinalTest),
            LibDdon.Enemy.CreateAuto(EnemyId.SkeletonBrute, 50, 2)
                .SetNamedEnemyParams(NamedParamId.FinalTest),
            LibDdon.Enemy.CreateAuto(EnemyId.SkeletonBrute, 50, 3)
                .SetNamedEnemyParams(NamedParamId.FinalTest),
            LibDdon.Enemy.CreateAuto(EnemyId.SkeletonBrute, 50, 4)
                .SetNamedEnemyParams(NamedParamId.FinalTest),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdPlJobEq(JobId.Sorcerer)
            .AddCheckCmdIsTutorialQuestClear(QuestId.SkillAugmentationSorcererTrialII);
        process0.AddNpcTalkAndOrderBlock(Stage.LoneHouseintheForest, NpcId.Emerada0, 18899);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Accept, Stage.AncientBurialMound, 1, 1, NpcId.Emerada0, 18901)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.Emerada);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQstFlag.EmeradaFSM0);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0, resetGroup: false);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1, resetGroup: false);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 2);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 2, resetGroup: false);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.LoneHouseintheForest, NpcId.Emerada0, 18903);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
