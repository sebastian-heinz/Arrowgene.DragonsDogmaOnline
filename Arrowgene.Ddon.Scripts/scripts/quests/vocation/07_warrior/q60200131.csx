/**
 * @brief Skill Augmentation Warrior Trial: IV
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.SkillAugmentationWarriorTrialIV;
    public override ushort RecommendedLevel => 60;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.OswaldsArmorShop;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.VocationQuest;

    public override bool ShowInAdventureGuide(GameClient client)
    {
        return client.Character.HasQuestCompleted(QuestId.SkillAugmentationWarriorTrialIII) &&
               client.Character.ActiveCharacterJobData.Job == JobId.Warrior;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumVocationLevel(JobId.Warrior, 60));
        AddQuestOrderCondition(QuestOrderCondition.Solo());
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.TheNewGeneration));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 7566);
        AddWalletReward(WalletType.Gold, 3960);
        AddWalletReward(WalletType.RiftPoints, 500);

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
        public const uint Oliver = 4130;
    }

    private class MyQstFlag
    {
        public const uint OliverFSM0 = 1924;
        public const uint OliverFSM1 = 1925;
        public const uint OliverFSM2 = 1926;
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.FungalColonyCaves, 3, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.InfectedHobgoblin, 58, 10)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
            LibDdon.Enemy.CreateAuto(EnemyId.InfectedHobgoblin, 58, 4)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
            LibDdon.Enemy.CreateAuto(EnemyId.InfectedHobgoblin, 58, 5)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
            LibDdon.Enemy.CreateAuto(EnemyId.InfectedHobgoblin, 58, 13)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
            LibDdon.Enemy.CreateAuto(EnemyId.InfectedDirewolf, 58, 2)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
            LibDdon.Enemy.CreateAuto(EnemyId.InfectedDirewolf, 58, 3)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
            LibDdon.Enemy.CreateAuto(EnemyId.InfectedDirewolf, 58, 6)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
            LibDdon.Enemy.CreateAuto(EnemyId.InfectedDirewolf, 58, 11)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
            
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.FungalColonyCaves, 5, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.BoltGrimwarg, 60, 0)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.BoltGrimwarg, 60, 1)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.BoltGrimwarg, 60, 2)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.BoltGrimwarg, 57, 3)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.BoltGrimwarg, 57, 4)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.BoltGrimwarg, 57, 5)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.BoltGrimwarg, 57, 6)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
        });

        AddEnemies(EnemyGroupId.Encounter + 2, Stage.FungalColonyCaves, 6, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.InfectedBehemoth, 50, 0, isBoss: true)
                .SetInfectionType(1)
                .SetNamedEnemyParams(NamedParamId.FinalTest),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdPlJobEq(JobId.Warrior)
            .AddCheckCmdIsTutorialQuestClear(QuestId.SkillAugmentationWarriorTrialIII);
        process0.AddNpcTalkAndOrderBlock(Stage.OswaldsArmorShop, NpcId.Oliver, 19009);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Accept, Stage.FungalColonyCaves, 1, 1, NpcId.Oliver, 19011)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.Oliver);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQstFlag.OliverFSM0);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0, resetGroup: false);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1, resetGroup: false);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 2);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 2, resetGroup: false);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.OswaldsArmorShop, NpcId.Oliver, 19013);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
