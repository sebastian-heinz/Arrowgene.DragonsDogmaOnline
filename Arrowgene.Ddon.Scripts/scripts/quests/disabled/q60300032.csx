/**
 * @brief Unmasker of Dark Deeds
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => (QuestId)60300032; // Schedule ID: 1673531136
    public override ushort RecommendedLevel => 94;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.FortressCityMegadoResidentialLevel1;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.QuestUsefulForAdventure;
    public override bool Enabled => false;

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 105000);
        AddWalletReward(WalletType.Gold, 11000);
        AddWalletReward(WalletType.RiftPoints, 2000);

        AddFixedItemReward(ItemId.RoyalCrestMedalMegadosysDistrict, 3);
        AddFixedItemReward(ItemId.FlameMushroom, 1);
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.Solo());
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.TheFinalBattleOfTheRoyalCapital));
    }

    private class EnemyGroupId
    {
        public const uint Set7943 = 7943;
        public const uint Set7690 = 7690;
        public const uint Set7691 = 7691;

    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.Set7943, Stage.MarquiseKurtsresidence1, 3, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.SwordSoldierDwarfOrc, 94, 4200, 0),
            LibDdon.Enemy.Create(EnemyId.SwordSoldierDwarfOrc, 94, 4200, 1),
        });

        AddEnemies(EnemyGroupId.Set7690, Stage.MarquiseKurtsresidence1, 10, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.SwordSoldierDwarfOrc, 94, 4200, 0),
            LibDdon.Enemy.Create(EnemyId.SwordSoldierDwarfOrc, 94, 4200, 1),
        });

        AddEnemies(EnemyGroupId.Set7691, Stage.MarquiseKurtsresidence1, 11, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.HeavySoldierDwarfOrc, 94, 4200, 0),
            LibDdon.Enemy.Create(EnemyId.RangedSoldierDwarfOrc, 94, 4200, 1),
            LibDdon.Enemy.Create(EnemyId.RangedSoldierDwarfOrc, 94, 4200, 2),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdIsMainQuestClear(QuestId.TheFinalBattleOfTheRoyalCapital);
        process0.AddNewNpcTalkAndOrderBlock(Stage.FortressCityMegadoResidentialLevel1, 0, 0, NpcId.Ennis, 28054)
			.AddResultCmdQstLayoutFlagOn(7415);
        process0.AddRawBlock(QuestAnnounceType.Accept)
			.AddResultCmdQstTalkChg(NpcId.Ennis, 28055)
            .AddCheckCommands([
                QuestManager.CheckCommand.TalkNpcChoice(461, NpcId.Ennis, 0)
            ])
            .AddCheckCommands([
                QuestManager.CheckCommand.NewTalkNpc(461, 0, 0, 60300032),
                QuestManager.CheckCommand.DummyNotProgress()
            ]);
        process0.AddStageJumpBlock(QuestAnnounceType.None, Stage.FortressCityMegadoResidentialLevel2, 37);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.FortressCityMegadoResidentialLevel2, 0, 0, NpcId.Navid, 28723)
			.AddResultCmdSetCustomWeather(12, 1)
			.AddResultCmdQstTalkChg(NpcId.Ennis, 28722)
			.AddResultCmdQstLayoutFlagOn(7612);
        process0.AddStageJumpBlock(QuestAnnounceType.None, Stage.MarquiseKurtsresidence1, 0);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.None, Stage.MarquiseKurtsresidence1, 0, 0, NpcId.Navid, 28730)
            .AddQuestFlag(QuestFlagType.WorldManageLayout, QuestFlagAction.Set, 7894, QuestId.Q70032001)
			.AddResultCmdQstLayoutFlagOff(7612)
			.AddResultCmdQstLayoutFlagOn(7613);
        process0.AddStageJumpBlock(QuestAnnounceType.None, Stage.MarquiseKurtsresidence1, 3);
        process0.AddRawBlock(QuestAnnounceType.Update)
			.AddResultCmdSetCustomWeather(21, 1)
			.AddResultCmdQstLayoutFlagOff(7613)
			.AddResultCmdQstLayoutFlagOn(7741)
			.AddResultCmdQstLayoutFlagOn(7742)
			.AddResultCmdMyQstFlagOn(4570)
			.AddCheckCmdMyQstFlagOn(4571)
			.AddCheckCmdMyQstFlagOn(4572)
			.AddCheckCmdMyQstFlagOn(4573);
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddQuestFlag(QuestFlagType.WorldManageLayout, QuestFlagAction.Set, 7743, QuestId.Q70032001)
			.AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, 100617)
			.AddResultCmdQstLayoutFlagOff(7741)
			.AddResultCmdMyQstFlagOff(4570)
			.AddCheckCmdSceHitIn(Stage.MarquiseKurtsresidence1, 4);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Set7943)
			.AddResultCmdQstLayoutFlagOff(7742)
			.AddResultCmdQstLayoutFlagOn(7628)
			.AddResultCmdQstLayoutFlagOn(7629)
			.AddResultCmdQstLayoutFlagOn(7943)
			.AddResultCmdPlayMessage(28879, 0);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.None, Stage.MarquiseKurtsresidence1, 2, 0, NpcId.Mephis1, 28738)
			.AddResultCmdMyQstFlagOn(4861);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Set7690)
			.AddResultCmdMyQstFlagOn(4607)
			.AddResultCmdQstLayoutFlagOn(7690);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Set7691);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Set7691, resetGroup: false);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.MarquiseKurtsresidence1, 4, 0, NpcId.Mephis1, 28739)
			.AddResultCmdQstLayoutFlagOff(7629)
			.AddResultCmdQstLayoutFlagOn(7707);
        process0.AddStageJumpBlock(QuestAnnounceType.None, Stage.FortressCityMegadoResidentialLevel3, 40);
        process0.AddRawBlock(QuestAnnounceType.Update)
			.AddResultCmdSetCustomWeather(21, 1)
			.AddResultCmdQstLayoutFlagOff(7707)
			.AddResultCmdQstLayoutFlagOn(7744)
			.AddCheckCmdSceHitIn(Stage.FortressCityMegadoResidentialLevel3, 15);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.FortressCityMegadoResidentialLevel3, 4, 0, NpcId.Mephis1, 28740)
			.AddResultCmdQstLayoutFlagOff(7744)
			.AddResultCmdQstLayoutFlagOn(7854);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.FortressCityMegadoResidentialLevel3, 1, 0, NpcId.Shem, 28880)
			.AddResultCmdQstTalkChg(NpcId.Mephis1, 29488)
			.AddResultCmdQstLayoutFlagOn(7701);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.FortressCityMegadoResidentialLevel3, 2, 0, NpcId.Mephis1, 29485)
			.AddResultCmdQstTalkChg(NpcId.Basit, 29487)
			.AddResultCmdQstLayoutFlagOff(7854)
			.AddResultCmdQstLayoutFlagOn(7745);
        process0.AddStageJumpBlock(QuestAnnounceType.None, Stage.FortressCityMegadoResidentialLevel1, 18);
        process0.AddProcessEndBlock(true)
            .AddResultCmdReleaseAnnounce(ContentsRelease.CooperatorsoftheRoyalFamily, TutorialId.ExposetheDarkSidetotheOneOperatingbehindtheScenesMephis)
			.AddResultCmdCallGreatPurpose(6, 11);

        var process1 = AddNewProcess(1);
        process1.AddRawBlock(QuestAnnounceType.None)
			.AddCheckCmdMyQstFlagOn(4570);
        process1.AddRawBlock(QuestAnnounceType.None)
			.AddCheckCmdQuestOmReleaseTouch(Stage.MarquiseKurtsresidence1, 5, 0);
        process1.AddProcessEndBlock(false)
			.AddResultCmdMyQstFlagOn(4571);

        var process2 = AddNewProcess(2);
        process2.AddRawBlock(QuestAnnounceType.None)
			.AddCheckCmdMyQstFlagOn(4570);
        process2.AddRawBlock(QuestAnnounceType.None)
			.AddCheckCmdQuestOmReleaseTouch(Stage.MarquiseKurtsresidence1, 5, 1);
        process2.AddProcessEndBlock(false)
			.AddResultCmdMyQstFlagOn(4572);

        var process3 = AddNewProcess(3);
        process3.AddRawBlock(QuestAnnounceType.None)
			.AddCheckCmdMyQstFlagOn(4570);
        process3.AddRawBlock(QuestAnnounceType.None)
			.AddCheckCmdQuestOmReleaseTouch(Stage.MarquiseKurtsresidence1, 5, 2);
        process3.AddProcessEndBlock(false)
			.AddResultCmdMyQstFlagOn(4573);
    }
}

return new ScriptedQuest();
