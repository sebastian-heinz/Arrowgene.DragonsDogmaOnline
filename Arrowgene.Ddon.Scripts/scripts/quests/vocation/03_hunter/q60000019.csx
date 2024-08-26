/**
 * @brief Hunter Tactics Trial: Break Attack
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ScriptedQuest));

    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.HunterTacticsTrialBreakAttack;
    public override ushort RecommendedLevel => 5;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.TheWhiteDragonTemple0;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.VocationQuest;

    public override bool ShowInAdventureGuide(GameClient client)
    {
        return client.Character.HasQuestCompleted(QuestId.HunterTacticsTrialAnAmbushingBow) &&
               client.Character.ActiveCharacterJobData.Job == JobId.Hunter;
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
        // Training Chapel
        // Wilson: GroupNo=1, UnitNo=1
        public const uint NpcWilsonAndArisenCorps = 1338;

        // GroupNo = 3, UnitNo = 1
        public const uint QuestSpecifiedMessageOm0 = 1325;

        // GroupNo = 4, UnitNo = 1
        public const uint QuestSpecifiedMessageOm1 = 1339;
    }

    private class MyQstFlag
    {
        // NPC State Machine
        public const uint Unk0 = 691;
        public const uint Unk1 = 687;

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
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 650);
        AddWalletReward(WalletType.Gold, 800);
        AddWalletReward(WalletType.RiftPoints, 150);

        AddFixedItemReward(ItemId.SuperiorHealingPotion, 3);
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.TrainingChapel, 1, QuestEnemyPlacementType.Manual, new()
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
            .AddCheckCmdPlJobEq(JobId.Hunter)
            .AddCheckCmdIsTutorialQuestClear(QuestId.HunterTacticsTrialAnAmbushingBow);
        process0.AddNpcTalkAndOrderBlock(Stage.TheWhiteDragonTemple0, NpcId.Renton0, 14102);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Accept, Stage.TrainingChapel, 1, 1, NpcId.Wilson, 14104)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.NpcWilsonAndArisenCorps);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0)
            .AddResultCmdResetTutorialFlag()
            .AddResultCmdTutorialEnemyInvincibility(true)
            .AddResultCmdTutorialDialog(TutorialId.FightingLargeEnemies)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQstFlag.Unk1);
        // process0.AddNoProgressBlock();
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddResultCmdResetTutorialFlag()
            .AddCheckCmdIsTutorialFlagOn(65); // Cling to the enemy, and hit them with Arrowhead Strike
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddResultCmdResetTutorialFlag()
            .AddCheckCmdIsTutorialFlagOn(63); // Use Resist to endure the enemy's swing (yellow icon) while climbing them
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddResultCmdResetTutorialFlag()
            .AddCheckCmdIsTutorialFlagOn(64); // While climbing the enemy, jump off during their knock-away attack (red icon)
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddResultCmdResetTutorialFlag()
            .AddCheckCmdIsTutorialFlagOn(61); // Attack and enrage the enemy
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddResultCmdResetTutorialFlag()
            .AddCheckCmdIsTutorialFlagOn(62); // Shake the enemy while they are enraged and tire them out!
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0, false)
            .AddResultCmdTutorialEnemyInvincibility(false)
            .AddResultCmdTutorialDialog(TutorialId.TacticalRoleoftheHunter)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQstFlag.DespawnGroup0);
        process0.AddOmInteractEventBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TrainingChapel, 3, 1, OmQuestType.MyQuest, OmInteractType.Touch)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.QuestSpecifiedMessageOm0);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, NpcId.Renton0, 14103)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, QstLayoutFlag.QuestSpecifiedMessageOm0);
        process0.AddIsStageNoBlock(QuestAnnounceType.None, Stage.TheWhiteDragonTemple0)
            .AddResultCmdTutorialDialog(TutorialId.Clinging);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
