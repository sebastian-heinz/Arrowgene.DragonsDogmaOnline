/**
 * @brief Skill Augmentation Spirit Lancer Trial: I
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.SkillAugmentationSpiritLancerTrialI;
    public override ushort RecommendedLevel => 40;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.SpiritArtsHut;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.VocationQuest;

    public override bool ShowInAdventureGuide(GameClient client)
    {
        return client.Character.HasQuestCompleted(QuestId.SeekingTheMasterSpiritLancer) &&
               client.Character.ActiveCharacterJobData.Job == JobId.SpiritLancer &&
               (client.Character.CompletedQuests.ContainsKey(QuestId.SkillAugmentationGuidanceOfARenewedWhiteDragon) || client.QuestState.IsQuestAccepted(QuestId.SkillAugmentationGuidanceOfARenewedWhiteDragon));
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumVocationLevel(JobId.SpiritLancer, 40));
        AddQuestOrderCondition(QuestOrderCondition.Solo());
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.TheFateOfLestania));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 2500);
        AddWalletReward(WalletType.Gold, 600);
        AddWalletReward(WalletType.RiftPoints, 60);

        AddFixedItemReward(ItemId.CrestOfCrystallization0, 4);
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
        public const uint AdairDonnchadh = 4142;
    }

    private class MyQstFlag
    {
        public const uint AdairDonnchadhFSM0 = 1844;
        public const uint AdairDonnchadhFSM1 = 1845;
        public const uint AdairDonnchadhFSM2 = 1846;
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.PitofScreams, 1, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.SulfurSaurian, 40, 10)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
            LibDdon.Enemy.CreateAuto(EnemyId.SulfurSaurian, 40, 11)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
            LibDdon.Enemy.CreateAuto(EnemyId.SulfurSaurian, 40, 12)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.PitofScreams, 2, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.DamnedWolf, 40, 0)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.DamnedWolf, 40, 1)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.DamnedWolf, 40, 2)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.DamnedWolf, 40, 3)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.DamnedWolf, 40, 4)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
        });

        AddEnemies(EnemyGroupId.Encounter + 2, Stage.PitofScreams, 3, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.LivingArmor, 40, 2, isBoss: true)
                .SetNamedEnemyParams(NamedParamId.FinalTest),
            LibDdon.Enemy.CreateAuto(EnemyId.LivingArmor, 40, 3, isBoss: true)
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
            .AddCheckCmdPlJobEq(JobId.SpiritLancer)
            .AddCheckCmdIsTutorialQuestClear(QuestId.SeekingTheMasterSpiritLancer);
        process0.AddNpcTalkAndOrderBlock(Stage.SpiritArtsHut, NpcId.AdairDonnchadh0, 19087);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Accept, Stage.PitofScreams, 1, 1, NpcId.AdairDonnchadh0, 19089)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.AdairDonnchadh);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQstFlag.AdairDonnchadhFSM0);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0, resetGroup: false);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1, resetGroup: false);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 2);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 2, resetGroup: false);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.SpiritArtsHut, NpcId.AdairDonnchadh0, 19091);
        process0.AddIsStageNoBlock(QuestAnnounceType.None, Stage.SpiritArtsHut)
           .AddResultCmdReleaseAnnounce(ContentsRelease.SpiritLancerWarSkillAugmentation, TutorialId.InheritanceSkillsOfBattleTechniques);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
