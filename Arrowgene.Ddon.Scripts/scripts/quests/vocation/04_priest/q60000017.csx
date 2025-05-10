/**
 * @brief Priest Tactics Trial: Break Attack
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ScriptedQuest));

    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.PriestTacticsTrialBreakAttack;
    public override ushort RecommendedLevel => 5;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.TheWhiteDragonTemple0;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.VocationQuest;

    public override bool ShowInAdventureGuide(GameClient client)
    {
        return client.Character.HasQuestCompleted(QuestId.PriestTacticsTrialPurification) &&
               client.Character.ActiveCharacterJobData.Job == JobId.Priest;
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
        public const uint NpcWilsonAndArisenCorps = 1328;

        // GroupNo = 3, UnitNo = 1
        public const uint QuestSpecifiedMessageOm0 = 1330;

        // GroupNo = 4, UnitNo = 1
        public const uint QuestSpecifiedMessageOm1 = 1329;

        // GroupNo = 5, UnitNo = 1
        public const uint QuestSpecifiedMessageOm2 = 1644;

        // GroupNo = 6, UnitNo = 1
        public const uint QuestSpecifiedMessageOm3 = 1645;

        // GroupNo = 7, UnitNo = 1
        public const uint QuestSpecifiedMessageOm4 = 1646;

        // GroupNo = 8, UnitNo = 1
        public const uint QuestSpecifiedMessageOm5 = 1647;

        // GroupNo = 9, UnitNo = 1
        public const uint QuestSpecifiedMessageOm6 = 1643;

        // GroupNo = 9, UnitNo = 1
        public const uint QuestSpecifiedMessageOm7 = 1715;
    }

    private class MyQstFlag
    {
        // NPC State Machine
        public const uint Unk0 = 670;
        public const uint Unk1 = 666;

        public const uint DespawnGroup0 = 1;
    }

    public override bool AcceptRequirementsMet(GameClient client)
    {
        return client.Character.ActiveCharacterJobData.Job == JobId.Priest;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumVocationLevel(JobId.Priest, 1));
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
            .AddCheckCmdPlJobEq(JobId.Priest)
            .AddCheckCmdIsTutorialQuestClear(QuestId.PriestTacticsTrialPurification);
        process0.AddNpcTalkAndOrderBlock(Stage.TheWhiteDragonTemple0, NpcId.Renton0, 14085);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Accept, Stage.TrainingChapel, 1, 1, NpcId.Wilson, 14088)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.NpcWilsonAndArisenCorps);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0)
            .AddResultCmdResetTutorialFlag()
            .AddResultCmdTutorialEnemyInvincibility(true)
            .AddResultCmdTutorialDialog(TutorialId.FightingLargeEnemies)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQstFlag.Unk1);
        // process0.AddNoProgressBlock();
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddResultCmdResetTutorialFlag()
            .AddCheckCmdIsTutorialFlagOn(70); // Cling to the enemy and successfully use Ride Holy Aura (89 and 70)
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddResultCmdResetTutorialFlag()
            .AddCheckCmdIsTutorialFlagOn(75); // Cling to the enemy and successfully use Ride Heal Aura (76 & 75)
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddResultCmdResetTutorialFlag()
            .AddCheckCmdIsTutorialFlagOn(73); // Use Resist to endure the enemy's swing (yellow icon) while climbing them
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddResultCmdResetTutorialFlag()
            .AddCheckCmdIsTutorialFlagOn(74); // While climbing the enemy, jump off during their knock-away attack (red icon)
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddResultCmdResetTutorialFlag()
            .AddCheckCmdIsTutorialFlagOn(71); // Attack and enrage the enemy or 73?
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddResultCmdResetTutorialFlag()
            .AddCheckCmdIsTutorialFlagOn(18); // Reveal the enemy's secret core during their enraged state with Heal Aura
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddResultCmdResetTutorialFlag()
            .AddCheckCmdIsTutorialFlagOn(72); // Shake the enemy while they are enraged and tire them out! (16 & 72)
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0, false)
            .AddResultCmdTutorialEnemyInvincibility(false)
            .AddResultCmdTutorialDialog(TutorialId.TacticalRoleofthePriest)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQstFlag.DespawnGroup0);
        process0.AddOmInteractEventBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TrainingChapel, 3, 1, OmQuestType.MyQuest, OmInteractType.Touch)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.QuestSpecifiedMessageOm0);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, NpcId.Renton0, 14086)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, QstLayoutFlag.QuestSpecifiedMessageOm0);
        process0.AddIsStageNoBlock(QuestAnnounceType.None, Stage.TheWhiteDragonTemple0)
            .AddResultCmdTutorialDialog(TutorialId.Clinging);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
