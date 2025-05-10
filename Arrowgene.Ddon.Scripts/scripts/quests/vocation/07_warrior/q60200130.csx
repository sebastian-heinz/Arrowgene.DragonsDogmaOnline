/**
 * @brief Skill Augmentation Warrior Trial: III
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.SkillAugmentationWarriorTrialIII;
    public override ushort RecommendedLevel => 50;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.OswaldsArmorShop;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.VocationQuest;

    public override bool ShowInAdventureGuide(GameClient client)
    {
        return client.Character.HasQuestCompleted(QuestId.SkillAugmentationWarriorTrialII) &&
               client.Character.ActiveCharacterJobData.Job == JobId.Warrior;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumVocationLevel(JobId.Warrior, 50));
        AddQuestOrderCondition(QuestOrderCondition.Solo());
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.TheDarknessOfTheHeart));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 3400);
        AddWalletReward(WalletType.Gold, 820);
        AddWalletReward(WalletType.RiftPoints, 100);

        AddFixedItemReward(ItemId.CrestOfHerculeanPower0, 4);
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
        public const uint Oliver = 4126;
    }

    private class MyQstFlag
    {
        public const uint OliverFSM0 = 1828;
        public const uint OliverFSM1 = 1829;
        public const uint OliverFSM2 = 1830;
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.MysteriousMausoleum, 4, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.Ghost, 50, 0)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
            LibDdon.Enemy.CreateAuto(EnemyId.Ghost, 50, 1)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
            LibDdon.Enemy.CreateAuto(EnemyId.Ghost, 50, 2)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.MysteriousMausoleum, 7, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.DarkCorpseTorturer, 50, 12)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.DarkCorpseTorturer, 50, 13)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.DarkCorpseTorturer, 50, 14)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.DarkSkeletonBrute, 50, 2)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.DarkSkeletonBrute, 50, 3)
                .SetNamedEnemyParams(NamedParamId.SecondTest),

        });

        AddEnemies(EnemyGroupId.Encounter + 2, Stage.MysteriousMausoleum, 9, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.DeathKnight, 50, 4, isBoss: true)
                .SetNamedEnemyParams(NamedParamId.FinalTest),
            LibDdon.Enemy.CreateAuto(EnemyId.Ghost, 50, 0)
                .SetNamedEnemyParams(NamedParamId.FinalTest),
            LibDdon.Enemy.CreateAuto(EnemyId.Ghost, 50, 1)
                .SetNamedEnemyParams(NamedParamId.FinalTest),
            LibDdon.Enemy.CreateAuto(EnemyId.Ghost, 50, 2)
                .SetNamedEnemyParams(NamedParamId.FinalTest),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdPlJobEq(JobId.Warrior)
            .AddCheckCmdIsTutorialQuestClear(QuestId.SkillAugmentationWarriorTrialII);
        process0.AddNpcTalkAndOrderBlock(Stage.OswaldsArmorShop, NpcId.Oliver, 18987);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Accept, Stage.MysteriousMausoleum, 1, 1, NpcId.Oliver, 18989)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.Oliver);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQstFlag.OliverFSM0);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0, resetGroup: false);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1, resetGroup: false);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 2);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 2, resetGroup: false);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.OswaldsArmorShop, NpcId.Oliver, 18991);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
