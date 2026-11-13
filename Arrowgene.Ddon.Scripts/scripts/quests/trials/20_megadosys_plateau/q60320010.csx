/**
 * @brief Megadosys Plateau Trial：Shadow of Heresy
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.MegadosysPlateauTrialShadowOfHeresy; // Schedule ID: 1677099008
    public override ushort RecommendedLevel => 95;
    public override byte MinimumItemRank => 104;
    public override bool IsDiscoverable => false;
    public override bool? EnableCancel => true;
    public override StageInfo StageInfo => Stage.FortressCityMegadoResidentialLevel1;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.AreaTrialOrMission;

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.TheFinalBattleOfTheRoyalCapital));
        AddQuestOrderCondition(QuestOrderCondition.SoloWithPawns());
        AddQuestOrderCondition(QuestOrderCondition.RequiredItemRank(104));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 42000);
        AddWalletReward(WalletType.Gold, 11000);
        AddWalletReward(WalletType.RiftPoints, 2000);

        AddFixedItemReward(ItemId.RoyalCrestMedalMegadosysDistrict, 3);
        AddFixedItemReward(ItemId.Phlogopite, 1);
        AddFixedItemReward(ItemId.HighlandAkadama, 1);
    }

    private class EnemyGroupId
    {
        public const uint Set7546 = 7546;
        public const uint Set7547 = 7547;
        public const uint Set7548 = 7548;
        public const uint Set7485 = 7485;
    }

    // Investigate how to make the harpies fall like boulders later
    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.Set7546, Stage.MegadosysPlateau, 53, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.BlazeHarpy, 95, 4200, 4)
                .SetStartThinkTblNo(1),
            LibDdon.Enemy.Create(EnemyId.BlazeHarpy, 95, 4200, 5)
                .SetStartThinkTblNo(1),
            LibDdon.Enemy.Create(EnemyId.BlazeHarpy, 95, 4200, 6)
                .SetStartThinkTblNo(3),
            LibDdon.Enemy.Create(EnemyId.BlazeHarpy, 95, 4200, 7)
                .SetStartThinkTblNo(3),
            LibDdon.Enemy.Create(EnemyId.BlazeHarpy, 95, 4200, 8)
                .SetStartThinkTblNo(3),
        });

        AddEnemies(EnemyGroupId.Set7547, Stage.MegadosysPlateau, 58, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.BlazeHarpy, 95, 4200, 5)
                .SetStartThinkTblNo(1),
            LibDdon.Enemy.Create(EnemyId.BlazeHarpy, 95, 4200, 6)
                .SetStartThinkTblNo(1),
            LibDdon.Enemy.Create(EnemyId.BlazeHarpy, 95, 4200, 7)
                .SetStartThinkTblNo(1),
            LibDdon.Enemy.Create(EnemyId.BlazeHarpy, 95, 4200, 8)
                .SetStartThinkTblNo(1),
            LibDdon.Enemy.Create(EnemyId.BlazeHarpy, 95, 4200, 9)
                .SetStartThinkTblNo(1),
        });

        AddEnemies(EnemyGroupId.Set7548, Stage.MegadosysPlateau, 60, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.BlazeHarpy, 95, 4200, 3)
                .SetStartThinkTblNo(1),
            LibDdon.Enemy.Create(EnemyId.BlazeHarpy, 95, 4200, 4)
                .SetStartThinkTblNo(1),
            LibDdon.Enemy.Create(EnemyId.BlazeHarpy, 95, 4200, 5)
                .SetStartThinkTblNo(1),
            LibDdon.Enemy.Create(EnemyId.BlazeHarpy, 95, 4200, 6)
                .SetStartThinkTblNo(1),
        });

        AddEnemies(EnemyGroupId.Set7485, Stage.MegadosysPlateau, 100, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.BlazeHarpy, 95, 4200, 0),
            LibDdon.Enemy.Create(EnemyId.BlazeHarpy, 95, 4200, 1),
            LibDdon.Enemy.Create(EnemyId.BlazeHarpy, 95, 4200, 2),
            LibDdon.Enemy.Create(EnemyId.BlazeHarpy, 95, 4200, 3),
            LibDdon.Enemy.Create(EnemyId.BlazeHarpy, 95, 4200, 4),
            LibDdon.Enemy.Create(EnemyId.BlazeHarpy, 95, 4200, 5),
            LibDdon.Enemy.Create(EnemyId.BlazeHarpy, 95, 4200, 6)
                .SetStartThinkTblNo(1),
            LibDdon.Enemy.Create(EnemyId.BlazeHarpy, 95, 4200, 7)
                .SetStartThinkTblNo(1),
            LibDdon.Enemy.Create(EnemyId.BlazeHarpy, 95, 4200, 8)
                .SetStartThinkTblNo(1),
            LibDdon.Enemy.Create(EnemyId.BlazeHarpy, 95, 4200, 9)
                .SetStartThinkTblNo(1),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdCheckAreaRank(QuestAreaId.MegadosysPlateau, 9);
        process0.AddNpcTalkAndOrderBlock(Stage.FortressCityMegadoResidentialLevel1, NpcId.Doris, 28276);
        process0.AddRawBlock(QuestAnnounceType.Accept)
            .AddCheckCmdTouchActToNpc(Stage.FortressCityMegadoResidentialLevel1, NpcId.Doris);
        process0.AddEventAfterJumpContinueBlock(QuestAnnounceType.None, Stage.TheKingsRoomofConcealment0, 0, 0);
        process0.AddStageJumpBlock(QuestAnnounceType.None, Stage.FortressCityMegadoResidentialLevel1, 36);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.FortressCityMegadoResidentialLevel1, NpcId.Doris, 28277);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.CheckpointAndUpdate, EnemyGroupId.Set7546)
            .AddResultCmdQstTalkChg(NpcId.Doris, 28278);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Set7546, resetGroup: false);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Set7547)
            .AddResultCmdTutorialDialog(TutorialId.AboutFlameEnemies);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Set7547, resetGroup: false);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Set7548);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Set7548, resetGroup: false);
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7484)
            .AddCheckCmdQuestOmReleaseTouch(Stage.MegadosysPlateau, 0, 0);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Set7485)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7484);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.FortressCityMegadoResidentialLevel1, NpcId.Doris, 28279);
        process0.AddProcessEndBlock(true)
            .AddResultCmdCallGreatPurpose(6, 9)
            .AddWorldManageUnlock(QuestFlags.MegadosysPlateau.FlamesOfDarkness);
    }
}

return new ScriptedQuest();
