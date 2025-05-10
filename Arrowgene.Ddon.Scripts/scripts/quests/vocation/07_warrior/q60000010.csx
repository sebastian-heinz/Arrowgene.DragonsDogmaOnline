/**
 * @brief Warrior Tactics Trial: A Strong Sword
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ScriptedQuest));

    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.WarriorTacticsTrialAStrongSword;
    public override ushort RecommendedLevel => 1;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.TheWhiteDragonTemple0;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.VocationQuest;

    public override bool ShowInAdventureGuide(GameClient client)
    {
        return client.Character.ActiveCharacterJobData.Job == JobId.Warrior;
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
        public const uint QuestSpecifiedMessageOM = 938;

        // Wilson: GroupNo=2, UnitNo=1
        // Arisen Corps Regimental Soldier GroupNo=3, UnitNo=1
        public const uint NpcWilson = 179;
        public const uint NpcArisenCorpsRegimentalSoldier = 955;
    }

    private class MyQstFlag
    {
        // NPC State Machine
        public const uint Unk0 = 493;
        public const uint Unk1 = 558;

        public const uint DespawnGroup0 = 1;
    }

    public override bool AcceptRequirementsMet(GameClient client)
    {
        return client.Character.ActiveCharacterJobData.Job == JobId.Warrior;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumVocationLevel(JobId.Warrior, 1));
        AddQuestOrderCondition(QuestOrderCondition.Solo());
        AddQuestOrderCondition(QuestOrderCondition.PersonalQuestCleared(QuestId.ReliableSourceOfInformation));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 480);
        AddWalletReward(WalletType.Gold, 500);
        AddWalletReward(WalletType.RiftPoints, 130);

        AddFixedItemReward(ItemId.BattleSword4, 1);
        AddFixedItemReward(ItemId.ConquerorsPeriapt, 1);
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
            LibDdon.Enemy.Create(EnemyId.Cyclops0, 3, 0, 0, assignDefaultDrops: false)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.CaptiveCyclops),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdPlJobEq(JobId.Warrior);
        process0.AddNpcTalkAndOrderBlock(Stage.TheWhiteDragonTemple0, NpcId.Renton0, 11643);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Accept, Stage.TrainingChapel, 3, 1, NpcId.Wilson, 11774)
            .AddResultCmdResetTutorialFlag()
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.NpcWilson)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.NpcArisenCorpsRegimentalSoldier);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0)
            .AddResultCmdTutorialEnemyInvincibility(true)
            .AddResultCmdTutorialDialog(TutorialId.AdvancedTacticsWarrior)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQstFlag.Unk1);
        // process0.AddNoProgressBlock();
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddResultCmdResetTutorialFlag()
            .AddCheckCmdIsTutorialFlagOn(29); // Attack an enemy with Hack
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddResultCmdResetTutorialFlag()
            .AddCheckCmdIsTutorialFlagOn(97); // Attack an enemy with Aerial Hack
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddResultCmdResetTutorialFlag()
            .AddCheckCmdIsTutorialFlagOn(30); // Attack an enemy with Savage Lash
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddResultCmdResetTutorialFlag()
            .AddCheckCmdIsTutorialFlagOn(32); // Charge the revenge gauge with Brace.
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddResultCmdResetTutorialFlag()
            .AddCheckCmdIsTutorialFlagOn(33); // Attack an enemy with Savage Lash with a full Revenge Gauge
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 2)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQstFlag.DespawnGroup0)
            .AddResultCmdTutorialDialog(TutorialId.FightingLargeEnemies)
            .AddResultCmdResetTutorialFlag()
            .AddCheckCmdIsTutorialFlagOn(52); // Cling to the Cyclops in the deeper chamber and use Inverse Slash.
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 2, false)
            .AddResultCmdTutorialEnemyInvincibility(false);
        process0.AddOmInteractEventBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TrainingChapel, 2, 1, OmQuestType.MyQuest, OmInteractType.Touch)
            .AddResultCmdTutorialDialog(TutorialId.TacticalRoleoftheWarrior)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.QuestSpecifiedMessageOM);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, NpcId.Renton0, 11645)
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
