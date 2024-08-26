/**
 * @brief Hunter Tactics Trial: An Ambushing Bow
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ScriptedQuest));

    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.HunterTacticsTrialAnAmbushingBow;
    public override ushort RecommendedLevel => 1;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.TheWhiteDragonTemple0;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.VocationQuest;

    public override bool ShowInAdventureGuide(GameClient client)
    {
        return client.Character.ActiveCharacterJobData.Job == JobId.Hunter;
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
        // Lestania
        // GroupNo=1, UnitNo=1
        public const uint WhiteKnight1 = 934;

        // Training Chapel
        // Wilson: GroupNo=3, UnitNo=1
        public const uint NpcWilson = 173;
        public const uint ArisenCorpsRegimentalSoldier = 952;

        // GroupNo = 1, UnitNo = 1
        public const uint QuestSpecifiedMessageOm0 = 931;

        // GroupNo = 2, UnitNo = 1
        public const uint QuestSpecifiedMessageOm1 = 944;
    }

    private class MyQstFlag
    {
        // NPC State Machine
        public const uint Unk0 = 499;
        public const uint Unk1 = 564;

        public const uint DespawnGroup0 = 1;
    }

    public override bool AcceptRequirementsMet(GameClient client)
    {
        return client.Character.ActiveCharacterJobData.Job == JobId.Hunter;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumVocationLevel(JobId.Hunter, 1));
        AddQuestOrderCondition(QuestOrderCondition.Solo());
        AddQuestOrderCondition(QuestOrderCondition.PersonalQuestCleared(QuestId.ReliableSourceOfInformation));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 480);
        AddWalletReward(WalletType.Gold, 500);
        AddWalletReward(WalletType.RiftPoints, 130);

        AddFixedItemReward(ItemId.StalkingBow4, 1);
        AddFixedItemReward(ItemId.ConquerorsPeriapt, 1);
    }

    protected override void InitializeEnemyGroups()
    {
        // 4 goblins
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.TrainingChapel, 0, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.GoblinFighter0, 1, 0, 0, assignDefaultDrops: false)
                .SetNamedEnemyParams(NamedParamId.Training),
            LibDdon.Enemy.Create(EnemyId.GoblinFighter0, 1, 0, 1, assignDefaultDrops: false)
                .SetNamedEnemyParams(NamedParamId.Training),
            LibDdon.Enemy.Create(EnemyId.GoblinFighter0, 1, 0, 2, assignDefaultDrops: false)
                .SetNamedEnemyParams(NamedParamId.Training),
            LibDdon.Enemy.Create(EnemyId.GoblinFighter0, 1, 0, 3, assignDefaultDrops: false)
                .SetNamedEnemyParams(NamedParamId.Training),
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.TrainingChapel, 0, QuestEnemyPlacementType.Manual, new()
        {
            // Despawn previous group
        });

        // 5 Goblins
        AddEnemies(EnemyGroupId.Encounter + 2, Stage.TrainingChapel, 1, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.GoblinFighter0, 1, 0, 1, assignDefaultDrops: false)
                .SetNamedEnemyParams(NamedParamId.Training),
            LibDdon.Enemy.Create(EnemyId.GoblinFighter0, 1, 0, 2, assignDefaultDrops: false)
                .SetNamedEnemyParams(NamedParamId.Training),
            LibDdon.Enemy.Create(EnemyId.GoblinFighter0, 1, 0, 3, assignDefaultDrops: false)
                .SetNamedEnemyParams(NamedParamId.Training),
            LibDdon.Enemy.Create(EnemyId.GoblinFighter0, 1, 0, 4, assignDefaultDrops: false)
                .SetNamedEnemyParams(NamedParamId.Training),
            LibDdon.Enemy.Create(EnemyId.GoblinFighter0, 1, 0, 5, assignDefaultDrops: false)
                .SetNamedEnemyParams(NamedParamId.Training),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdPlJobEq(JobId.Hunter);
        process0.AddNpcTalkAndOrderBlock(Stage.TheWhiteDragonTemple0, NpcId.Renton0, 11631);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Accept, Stage.Lestania, 1, 1, NpcId.WhiteKnight1, 11632)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.WhiteKnight1);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TrainingChapel, 3, 1, NpcId.Wilson, 11762)
            .AddResultCmdResetTutorialFlag()
            .AddResultCmdPlayCameraEvent(Stage.Lestania, 97)
            .AddResultCmdTutorialDialog(TutorialId.Climbing)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.NpcWilson)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.ArisenCorpsRegimentalSoldier);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0)
            .AddResultCmdTutorialEnemyInvincibility(true)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQstFlag.Unk1);
        // process0.AddNoProgressBlock();
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddResultCmdResetTutorialFlag()
            .AddCheckCmdIsTutorialFlagOn(11); // Attack an enemy with Loose
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddResultCmdResetTutorialFlag()
            .AddCheckCmdIsTutorialFlagOn(58); // Attack an enemy with Loose from the optimum distance
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddResultCmdResetTutorialFlag()
            .AddCheckCmdIsTutorialFlagOn(12); // Attack an enemy with Explosive Arrow
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddResultCmdResetTutorialFlag()
            .AddCheckCmdIsTutorialFlagOn(13); // Attack the enemy by detonating the explosive arrow
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddResultCmdResetTutorialFlag()
            .AddCheckCmdIsTutorialFlagOn(14); // Attack an enemy with Front Kick
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddResultCmdResetTutorialFlag()
            .AddCheckCmdIsTutorialFlagOn(59); // Reload arrows successfully
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 2, false)
            .AddResultCmdTutorialEnemyInvincibility(false)
            .AddResultCmdTutorialDialog(TutorialId.TacticalRoleoftheHunter)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQstFlag.DespawnGroup0);
        process0.AddOmInteractEventBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TrainingChapel, 2, 1, OmQuestType.MyQuest, OmInteractType.Touch)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.QuestSpecifiedMessageOm1);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, NpcId.Renton0, 11633)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, QstLayoutFlag.QuestSpecifiedMessageOm1);
        process0.AddProcessEndBlock(true);

        var process1 = AddNewProcess(1);
        process1.AddMyQstFlagsBlock(QuestAnnounceType.None)
            .AddMyQstCheckFlag(MyQstFlag.DespawnGroup0);
        process1.AddSpawnGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter + 1, true);
        process1.AddProcessEndBlock(false);
    }
}

return new ScriptedQuest();
