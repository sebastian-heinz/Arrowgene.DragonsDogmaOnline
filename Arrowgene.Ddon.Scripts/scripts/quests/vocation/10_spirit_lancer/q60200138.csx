/**
 * @brief Skill Augmentation Spirit Lancer Trial: III
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.SkillAugmentationSpiritLancerTrialIII;
    public override ushort RecommendedLevel => 50;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.SpiritArtsHut;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.VocationQuest;

    public override bool ShowInAdventureGuide(GameClient client)
    {
        return client.Character.HasQuestCompleted(QuestId.SkillAugmentationSpiritLancerTrialII) &&
               client.Character.ActiveCharacterJobData.Job == JobId.SpiritLancer;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumVocationLevel(JobId.SpiritLancer, 50));
        AddQuestOrderCondition(QuestOrderCondition.Solo());
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.TheDarknessOfTheHeart));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 3400);
        AddWalletReward(WalletType.Gold, 820);
        AddWalletReward(WalletType.RiftPoints, 100);

        AddFixedItemReward(ItemId.CrestOfDwindledDefense0, 4);
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
        public const uint AdairDonnchadh = 4150;
    }

    private class MyQstFlag
    {
        public const uint AdairDonnchadhFSM0 = 1860;
        public const uint AdairDonnchadhFSM1 = 1861;
        public const uint AdairDonnchadhFSM2 = 1862;
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.SunlitUndergroundCave, 1, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.BoltGrimwarg, 50, 3)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
            LibDdon.Enemy.CreateAuto(EnemyId.BoltGrimwarg, 50, 4)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
            LibDdon.Enemy.CreateAuto(EnemyId.BoltGrimwarg, 50, 5)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
            LibDdon.Enemy.CreateAuto(EnemyId.BoltGrimwarg, 50, 6)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
            LibDdon.Enemy.CreateAuto(EnemyId.BoltGrimwarg, 50, 7)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.SunlitUndergroundCave, 6, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.InfectedDirewolf, 50, 0)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.InfectedDirewolf, 50, 1)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.InfectedDirewolf, 50, 2)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.InfectedDirewolf, 50, 3)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.InfectedDirewolf, 50, 4)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
        });

        AddEnemies(EnemyGroupId.Encounter + 2, Stage.SunlitUndergroundCave, 2, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.InfectedHobgoblin, 50, 0)
                .SetInfectionType(2)
                .SetNamedEnemyParams(NamedParamId.FinalTest),
            LibDdon.Enemy.CreateAuto(EnemyId.BoltGrimwarg, 50, 3)
                .SetInfectionType(2)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
            LibDdon.Enemy.CreateAuto(EnemyId.BoltGrimwarg, 50, 4)
                .SetInfectionType(2)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
            LibDdon.Enemy.CreateAuto(EnemyId.InfectedSnowHarpy, 50, 5)
                .SetInfectionType(2)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
            LibDdon.Enemy.CreateAuto(EnemyId.InfectedSnowHarpy, 50, 6)
                .SetInfectionType(2)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
            
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdPlJobEq(JobId.SpiritLancer)
            .AddCheckCmdIsTutorialQuestClear(QuestId.SkillAugmentationSpiritLancerTrialII);
        process0.AddNpcTalkAndOrderBlock(Stage.SpiritArtsHut, NpcId.AdairDonnchadh0, 19131);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Accept, Stage.SunlitUndergroundCave, 1, 1, NpcId.AdairDonnchadh0, 19133)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.AdairDonnchadh);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQstFlag.AdairDonnchadhFSM0);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0, resetGroup: false);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1, resetGroup: false);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 2);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 2, resetGroup: false);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.SpiritArtsHut, NpcId.AdairDonnchadh0, 19135);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
