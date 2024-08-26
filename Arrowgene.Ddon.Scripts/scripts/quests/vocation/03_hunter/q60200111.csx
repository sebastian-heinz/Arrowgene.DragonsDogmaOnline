/**
 * @brief Skill Augmentation Hunter Trial: IV
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.SkillAugmentationHunterTrialIV;
    public override ushort RecommendedLevel => 60;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.IvansLodge;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.VocationQuest;

    public override bool ShowInAdventureGuide(GameClient client)
    {
        return client.Character.HasQuestCompleted(QuestId.SkillAugmentationHunterTrialIII) &&
               client.Character.ActiveCharacterJobData.Job == JobId.Hunter;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumVocationLevel(JobId.Hunter, 60));
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
        public const uint Ivan = 4089;
    }

    private class MyQstFlag
    {
        public const uint IvanFSM0 = 1884;
        public const uint IvanFSM1 = 1885;
        public const uint IvanFSM2 = 1886;
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.PicturesqueGrotto, 2, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.Mudman, 58, 0)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
            LibDdon.Enemy.CreateAuto(EnemyId.Mudman, 58, 1)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
            LibDdon.Enemy.CreateAuto(EnemyId.Mudman, 58, 2)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
            LibDdon.Enemy.CreateAuto(EnemyId.Mudman, 58, 3)
                .SetNamedEnemyParams(NamedParamId.FirstTest),
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.PicturesqueGrotto, 7, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.Grimwarg, 58, 4)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.Grimwarg, 58, 6)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.Gargoyle, 58, 5)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
            LibDdon.Enemy.CreateAuto(EnemyId.Gargoyle, 58, 9)
                .SetNamedEnemyParams(NamedParamId.SecondTest),
        });

        AddEnemies(EnemyGroupId.Encounter + 2, Stage.PicturesqueGrotto, 9, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.Cockatrice, 60, 0)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.FinalTest),
            LibDdon.Enemy.CreateAuto(EnemyId.RockSaurian, 58, 1)
                .SetNamedEnemyParams(NamedParamId.FinalTest),
            LibDdon.Enemy.CreateAuto(EnemyId.RockSaurian, 58, 2)
                .SetNamedEnemyParams(NamedParamId.FinalTest),
            LibDdon.Enemy.CreateAuto(EnemyId.RockSaurian, 58, 3)
                .SetNamedEnemyParams(NamedParamId.FinalTest),
            LibDdon.Enemy.CreateAuto(EnemyId.RockSaurian, 58, 4)
                .SetNamedEnemyParams(NamedParamId.FinalTest),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdPlJobEq(JobId.Hunter)
            .AddCheckCmdIsTutorialQuestClear(QuestId.SkillAugmentationHunterTrialIII);
        process0.AddNpcTalkAndOrderBlock(Stage.IvansLodge, NpcId.Ivan, 18784);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Accept, Stage.PicturesqueGrotto, 1, 1, NpcId.Ivan, 18786)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.Ivan);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQstFlag.IvanFSM0);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0, resetGroup: false);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1, resetGroup: false);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 2);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 2, resetGroup: false);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.IvansLodge, NpcId.Ivan, 18788);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
