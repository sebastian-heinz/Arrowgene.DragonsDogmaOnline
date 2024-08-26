/**
 * @brief Element Archer Tactics Trial: A Magick Bow
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ScriptedQuest));

    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.ElementArcherTacticsTrialAMagickBow;
    public override ushort RecommendedLevel => 1;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.TheWhiteDragonTemple0;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.VocationQuest;

    public override bool ShowInAdventureGuide(GameClient client)
    {
        return client.Character.ActiveCharacterJobData.Job == JobId.ElementArcher;
    }

    private class EnemyGroupId
    {
        public const uint Encounter = 10;
    }

    private class NamedParamId
    {
        public const uint Training = 47; // Training <name>
        public const uint CaptiveCyclops = 48; // Captive Cyclops
    }

    private class QstLayoutFlag
    {
        // クエスト指定メッセージOM"
        // GroupNo=2, UnitNo=1
        public const uint QuestSpecifiedMessageOM = 946;

        // Wilson: GroupNo=2, UnitNo=1
        // Arisen Corps Regimental Soldier GroupNo=3, UnitNo=1
        public const uint NpcWilson = 177;
        public const uint NpcLudovika = 954;
    }

    private class MyQstFlag
    {
        // NPC State Machine
        public const uint Unk0 = 501;
        public const uint Unk1 = 566;

        public const uint DespawnGroup0 = 1;
    }

    public override bool AcceptRequirementsMet(GameClient client)
    {
        return client.Character.ActiveCharacterJobData.Job == JobId.ElementArcher;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumVocationLevel(JobId.ElementArcher, 1));
        AddQuestOrderCondition(QuestOrderCondition.Solo());
        AddQuestOrderCondition(QuestOrderCondition.PersonalQuestCleared(QuestId.ReliableSourceOfInformation));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 480);
        AddWalletReward(WalletType.Gold, 500);
        AddWalletReward(WalletType.RiftPoints, 130);

        AddFixedItemReward(ItemId.Gnosir4, 1);
        AddFixedItemReward(ItemId.DemonsPeriapt, 1);
    }

    protected override void InitializeEnemyGroups()
    {
        // 4 goblins
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.TrainingChapel, 0, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.GoblinFighter0, 1, 0, 0)
                .SetNamedEnemyParams(NamedParamId.Training),
            LibDdon.Enemy.Create(EnemyId.GoblinFighter0, 1, 0, 1)
                .SetNamedEnemyParams(NamedParamId.Training),
            LibDdon.Enemy.Create(EnemyId.GoblinFighter0, 1, 0, 2)
                .SetNamedEnemyParams(NamedParamId.Training),
            LibDdon.Enemy.Create(EnemyId.GoblinFighter0, 1, 0, 3)
                .SetNamedEnemyParams(NamedParamId.Training),
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.TrainingChapel, 0, QuestEnemyPlacementType.Manual, new()
        {
            // Empty Group to despawn enemies
        });

        AddEnemies(EnemyGroupId.Encounter + 2, Stage.TrainingChapel, 1, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.Cyclops0, 3, 0, 0)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.CaptiveCyclops),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdPlJobEq(JobId.ElementArcher);
        process0.AddNpcTalkAndOrderBlock(Stage.TheWhiteDragonTemple0, NpcId.Renton0, 11638);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Accept, Stage.TrainingChapel, 3, 1, NpcId.Wilson, 11768)
            .AddResultCmdResetTutorialFlag()
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.NpcWilson)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.NpcLudovika);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0)
            .AddResultCmdTutorialEnemyInvincibility(true)
            .AddResultCmdTutorialDialog(TutorialId.AdvancedTacticsElementArcher)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQstFlag.Unk1);
        // process0.AddNoProgressBlock();
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddResultCmdResetTutorialFlag()
            .AddCheckCmdIsTutorialFlagOn(34); // Capture 2 enemies within Magick Eye: Seeker
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddResultCmdResetTutorialFlag()
            .AddCheckCmdIsTutorialFlagOn(35); // Attack an enemy with Seeker Arrow
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddResultCmdResetTutorialFlag()
            .AddCheckCmdIsTutorialFlagOn(68); // Get to the optimum distance away from enemies while using Magick Eye: Seeker, and attack them for more than 3 seconds.
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddResultCmdResetTutorialFlag()
            .AddCheckCmdIsTutorialFlagOn(98); // Fully charge a Seeker arrow, and loose it at an enemy.
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddResultCmdResetTutorialFlag()
            .AddCheckCmdIsTutorialFlagOn(36); // Lock onto an ally with Magick Eye: True Aid.
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddResultCmdResetTutorialFlag()
            .AddCheckCmdIsTutorialFlagOn(37); // Recover an ally's health with True Aid arrows.
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddResultCmdResetTutorialFlag()
            .AddCheckCmdIsTutorialFlagOn(69); // Recover your health with Healing Shroud.
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddResultCmdResetTutorialFlag()
            .AddCheckCmdIsTutorialFlagOn(38); // Attack an enemy with Front Kick.
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 2)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQstFlag.DespawnGroup0)
            .AddResultCmdTutorialDialog(TutorialId.FightingLargeEnemies)
            .AddResultCmdResetTutorialFlag()
            .AddCheckCmdIsTutorialFlagOn(49); // Cling to the enemy in the deeper chamber and use Invigorating Magick Arrow.
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddResultCmdResetTutorialFlag()
            .AddCheckCmdIsTutorialFlagOn(48); // While the enemy is enraged, reveal its secret core with Magick Eye: True Aid
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 2, false)
            .AddResultCmdTutorialEnemyInvincibility(false);
        process0.AddOmInteractEventBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TrainingChapel, 2, 1, OmQuestType.MyQuest, OmInteractType.Touch)
            .AddResultCmdTutorialDialog(TutorialId.TacticalRoleoftheElementArcher)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.QuestSpecifiedMessageOM);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, NpcId.Renton0, 11640)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, QstLayoutFlag.QuestSpecifiedMessageOM);
        process0.AddProcessEndBlock(true);

        var process1 = AddNewProcess(1);
        process1.AddMyQstFlagsBlock(QuestAnnounceType.None)
            .AddMyQstCheckFlag(MyQstFlag.DespawnGroup0);
        process1.AddSpawnGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter + 1, true);
        process1.AddProcessEndBlock(false);
    }
}

return new ScriptedQuest();
