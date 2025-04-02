/**
 * @brief Shining Within a Loyal Heart
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.ShiningWithinALoyalHeart;
    public override ushort RecommendedLevel => 15;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.ArisensRoom;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.QuestUsefulForAdventure;

    private class QstLayoutFlag
    {
        // The Arisens Room (st0211)
        public const uint SpawnAlvarInTheArisensRoom = 7501; // Can't be cleared once set
        public const uint SpawnTeleportOMToTheRift = 7507; // Enables teleport to the rift from the player room

        // The Rift (st0562)
        public const uint SpawnQuestSpecifiedMessage = 7504; // Light point the player needs to touch
        public const uint SpawnAlvarInTheRift0 = 7503;
        public const uint SpawnAlvarInTheRift1 = 7506;
        public const uint SpawnQuestOMs = 7508; // Blockades and exit teleport
    }

    private class MyQstFlag
    {
        // Alvar and Nedo
        public const uint StartAlvarFsm = 4411;
    }

    private class EnemyGroupId
    {
        public const uint Encounter = 10;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.Solo());
        AddQuestOrderCondition(QuestOrderCondition.PersonalQuestCleared(QuestId.LivingWithThePartnerPawn));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 7500);
        AddWalletReward(WalletType.Gold, 7500);
        AddWalletReward(WalletType.RiftPoints, 1500);
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.TheRift1, 6, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.PawnSeeker, 15, 0),
            LibDdon.Enemy.CreateAuto(EnemyId.MistFighter, 15, 1),
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.TheRift1, 3, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.PawnMage, 15, 6),
            LibDdon.Enemy.CreateAuto(EnemyId.MistFighter, 15, 2),
            LibDdon.Enemy.CreateAuto(EnemyId.PawnMage, 15, 7),
        });

        AddEnemies(EnemyGroupId.Encounter + 2, Stage.TheRift1, 2, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.MistSorcerer, 15, 0),
        });
    }

    protected override void InitializeBlocks()
    {
        // https://www.youtube.com/watch?v=bdKRCB_R4iI

        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdIsTutorialQuestClear(QuestId.LivingWithThePartnerPawn);
        process0.AddNewNpcTalkAndOrderBlock(Stage.ArisensRoom, 0, 0, NpcId.Alvar, 28303, QuestId.ShiningWithinALoyalHeart)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.SpawnAlvarInTheArisensRoom);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Accept, Stage.ArisensRoom, 0, 0, NpcId.Alvar, 28304);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.ArisensRoom, 0, 0, NpcId.Alvar, 28308);
        process0.AddStageJumpBlock(QuestAnnounceType.None, Stage.TheRift1, 1);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheRift1, 1, 0, NpcId.Alvar, 28305)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, QstLayoutFlag.SpawnAlvarInTheArisensRoom)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.SpawnAlvarInTheRift0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.SpawnTeleportOMToTheRift)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.SpawnQuestOMs);
        // First Enemy Group
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQstFlag.StartAlvarFsm);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0, resetGroup: false);
        // Second Enemy Group
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1, resetGroup: false);
        // Third Enemy Group
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 2);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 2, resetGroup: false);
        // Touch the light
        process0.AddOmInteractEventBlock(QuestAnnounceType.Update, Stage.TheRift1, 0, 0, OmQuestType.MyQuest, OmInteractType.Touch)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.SpawnQuestSpecifiedMessage);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.TheRift1, 2, 0, NpcId.Alvar, 28306)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, QstLayoutFlag.SpawnAlvarInTheRift0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.SpawnAlvarInTheRift1);
        process0.AddStageJumpBlock(QuestAnnounceType.None, Stage.ArisensRoom, 4);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.ArisensRoom, 0, 0, NpcId.Alvar, 28307)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, QstLayoutFlag.SpawnTeleportOMToTheRift)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.SpawnAlvarInTheArisensRoom);
        process0.AddIsStageNoBlock(QuestAnnounceType.None, Stage.ArisensRoom)
            .AddResultCmdReleaseAnnounce(ContentsRelease.PawnsNewSpecialSkills, TutorialId.PawnSpecialSkills);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
