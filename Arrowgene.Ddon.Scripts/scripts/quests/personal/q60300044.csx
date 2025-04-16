/**
 * @brief The High Scepter's Heir
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.TheHighSceptersHeir;
    public override ushort RecommendedLevel => 10;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.TheWhiteDragonTemple0;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.VocationQuest;

    private class QstLayoutFlag
    {
        // st0503
        public const uint Barris0 = 7411;
        public const uint Barris1 = 7735;

    }

    private class EnemyGroupId
    {
        public const uint Encounter = 10;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.PersonalQuestCleared(QuestId.TheArisensAbilities));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 10000);
        AddWalletReward(WalletType.Gold, 10000);
        AddWalletReward(WalletType.RiftPoints, 3000);

        AddFixedItemReward(ItemId.Scimitar0, 1);
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.SmallCaveTombs, 1, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.WarriorUndead, 10, 0),
            LibDdon.Enemy.CreateAuto(EnemyId.UndeadMale, 10, 1),
            LibDdon.Enemy.CreateAuto(EnemyId.WarriorUndead, 10, 2),
            LibDdon.Enemy.CreateAuto(EnemyId.UndeadMale, 10, 3),
            LibDdon.Enemy.CreateAuto(EnemyId.WarriorUndead, 10, 4),
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.SmallCaveTombs, 1, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.WarriorUndead, 10, 5),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordUndead, 15, 6),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordUndead, 15, 7),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordUndead, 15, 8),
            LibDdon.Enemy.CreateAuto(EnemyId.WarriorUndead, 15, 9),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTalkAndOrderBlock(Stage.TheWhiteDragonTemple0, NpcId.Renton0, 29199);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Accept, Stage.CraftRoom, NpcId.Craig0, 27144); // Hear what Craig has to say in the Craft Room
        process0.AddIsStageNoBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.SmallCaveTombs); // Head to "Small Cave Tombs" and stand-in for Craig
        process0.AddDiscoverGroupBlock(QuestAnnounceType.CheckpointAndUpdate, EnemyGroupId.Encounter + 0); // Investigate a mysterious person
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0, false); // Defeat the enemy encountered
        process0.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter + 1)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.Barris0);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.SmallCaveTombs, 1, 0, NpcId.Barris, 29148) // Speak to the person who assisted
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, QstLayoutFlag.Barris0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.Barris1);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.CraftRoom, NpcId.Craig0, 27146); // Return to the Craft Room and report to Craig
        process0.AddIsStageNoBlock(QuestAnnounceType.None, Stage.CraftRoom, false)
            .AddResultCmdReleaseAnnounce(ContentsRelease.ChangetoHighScepter, TutorialId.BasicTacticsHighScepter)
            .AddResultCmdReleaseAnnounce(ContentsRelease.HighScepterJobTraining, TutorialId.JobTrainingLog);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
