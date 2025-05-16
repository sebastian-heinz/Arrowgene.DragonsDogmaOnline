/**
 * @brief Skill Augmentation Spirit Lancer Trial: IV
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.SkillAugmentationSpiritLancerTrialIV;
    public override ushort RecommendedLevel => 60;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.SpiritArtsHut;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.VocationQuest;

    public override bool ShowInAdventureGuide(GameClient client)
    {
        return client.Character.HasQuestCompleted(QuestId.SkillAugmentationSpiritLancerTrialIII) &&
               client.Character.ActiveCharacterJobData.Job == JobId.SpiritLancer;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumVocationLevel(JobId.SpiritLancer, 60));
        AddQuestOrderCondition(QuestOrderCondition.Solo());
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.TheNewGeneration));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 7566);
        AddWalletReward(WalletType.Gold, 3960);
        AddWalletReward(WalletType.RiftPoints, 500);

        AddFixedItemReward(ItemId.CrestOfDwindledMagickDefense0, 4);
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
        public const uint AdairDonnchadh = 4154;
    }

    private class MyQstFlag
    {
        public const uint AdairDonnchadhFSM0 = 1940;
        public const uint AdairDonnchadhFSM1 = 1941;
        public const uint AdairDonnchadhFSM2 = 1942;
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.InfectedDenDepths, 1, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.InfectedHobgoblinFighter, 60, 0)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
            LibDdon.Enemy.CreateAuto(EnemyId.InfectedHobgoblinFighter, 60, 1)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
            LibDdon.Enemy.CreateAuto(EnemyId.InfectedSnowHarpy, 60, 3)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
            LibDdon.Enemy.CreateAuto(EnemyId.InfectedSnowHarpy, 60, 4)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.InfectedDenDepths, 9, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.GiantSaurian, 60, 2)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.GiantSaurian, 60, 6)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.SaurianSage, 60, 3)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.SaurianSage, 60, 5)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
        });

        AddEnemies(EnemyGroupId.Encounter + 2, Stage.InfectedDenDepths, 11, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.InfectedBehemoth, 60, 2)
                .SetInfectionType(1)
                .SetNamedEnemyParams(NamedParamId.FinalTest),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdPlJobEq(JobId.SpiritLancer)
            .AddCheckCmdIsTutorialQuestClear(QuestId.SkillAugmentationSpiritLancerTrialIII);
        process0.AddNpcTalkAndOrderBlock(Stage.SpiritArtsHut, NpcId.AdairDonnchadh0, 19153);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Accept, Stage.InfectedDenDepths, 1, 1, NpcId.AdairDonnchadh0, 19155)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.AdairDonnchadh);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQstFlag.AdairDonnchadhFSM0);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0, resetGroup: false);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1, resetGroup: false);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 2);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 2, resetGroup: false);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.SpiritArtsHut, NpcId.AdairDonnchadh0, 19157);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
