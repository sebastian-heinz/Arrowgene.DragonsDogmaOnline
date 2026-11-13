/**
 * @brief Feryana Wilderness Trial: Investigate Sealed Evil
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => (QuestId)60319010; // Schedule ID: 1677100288
    public override ushort RecommendedLevel => 88;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => false;
    public override StageInfo StageInfo => Stage.LookoutCastle1;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.AreaTrialOrMission;

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.SoloWithPawns());
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.TheBattleOfLookoutCastle));
        AddQuestOrderCondition(QuestOrderCondition.RequiredItemRank(94));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 105000);
        AddWalletReward(WalletType.Gold, 11000);
        AddWalletReward(WalletType.RiftPoints, 2000);

        AddFixedItemReward(ItemId.RoyalCrestMedalFeryanaDistrict, 3);
        AddFixedItemReward(ItemId.UnderworldDrop, 2);
        AddFixedItemReward(ItemId.MisleadingTwig, 2);
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdCheckAreaRank(QuestAreaId.FeryanaWilderness, 9);
        process0.AddNpcTalkAndOrderBlock(Stage.LookoutCastle1, NpcId.Nayajiku, 25706);
        process0.AddRawBlock(QuestAnnounceType.Accept)
			.AddCheckCmdTouchActToNpc(Stage.LookoutCastle1, NpcId.Nayajiku);
        process0.AddEventAfterJumpContinueBlock(QuestAnnounceType.None, Stage.OldTekiaGrotto0, 0, 1);
        process0.AddStageJumpBlock(QuestAnnounceType.None, Stage.LookoutCastle1, 17);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Update, Stage.LookoutCastle1, NpcId.Nayajiku, 23892);
		process0.AddRawBlock(QuestAnnounceType.CheckpointAndUpdate)
            .AddResultCmdQstTalkChg(NpcId.Nayajiku, 23893)
            .AddResultCmdQstTalkChg(NpcId.Shekel, 23894)
			.AddCheckCommands([
				QuestManager.CheckCommand.MyQstFlagOn(0)
			])
			.AddCheckCommands([
				QuestManager.CheckCommand.TalkNpc(635, NpcId.Shekel),
				QuestManager.CheckCommand.DummyNotProgress()
			]);
        process0.AddRawBlock(QuestAnnounceType.None)
			.AddCheckCommands([
				QuestManager.CheckCommand.MyQstFlagOn(1)
			]);
        process0.AddTalkToNpcBlock(QuestAnnounceType.None, Stage.LookoutCastle1, NpcId.Nayajiku, 25704)
			.AddResultCommands([
				QuestManager.ResultCommand.UpdateAnnounceDirect(9, 3)
			]);
        process0.AddProcessEndBlock(true)
			.AddResultCommands([
				QuestManager.ResultCommand.WorldManageLayoutFlagOn(1216, 70000001),
				QuestManager.ResultCommand.CallGreatPurpose(6, 5)
			]);

		// Branch 1 - Gerald
		var process1 = AddNewProcess(1);
		process1.AddRawBlock(QuestAnnounceType.None)
			.AddCheckCommands([
				QuestManager.CheckCommand.TalkNpcChoice(635, NpcId.Shekel, 0)
			]);
		process1.AddRawBlock(QuestAnnounceType.None)
			.AddResultCommands([
				QuestManager.ResultCommand.MyQstFlagOn(0),
				QuestManager.ResultCommand.UpdateAnnounceDirect(6, 3),
				QuestManager.ResultCommand.QstTalkChg(NpcId.Shekel, 23895),
				QuestManager.ResultCommand.QstTalkChg(NpcId.Gerald, 25698)
			])
			.AddCheckCommands([
				QuestManager.CheckCommand.TalkNpc(1001, NpcId.Gerald)
			]);
		process1.AddDeliverItemsBlock(QuestAnnounceType.Update, Stage.BerthasBanditGroupHideout, NpcId.Gerald, ItemId.GiantApophyllite, 3, 25699)
			.AddResultCommands([
				QuestManager.ResultCommand.QstTalkChg(NpcId.Gerald, 25700)
			]);
		process1.AddRawBlock(QuestAnnounceType.Update)
			.AddCheckCommands([
				QuestManager.CheckCommand.NewTalkNpc(132, 0, 0, 60319010)
			])
			.AddResultCommands([
				QuestManager.ResultCommand.QstLayoutFlagOn(6370),
				QuestManager.ResultCommand.QstTalkChg(NpcId.Gerald, 25692),
				QuestManager.ResultCommand.QstTalkChg(NpcId.Dudley, 25702)
			]);
		process1.AddRawBlock(QuestAnnounceType.None)
			.AddResultCommands([
				QuestManager.ResultCommand.MyQstFlagOn(1),
				QuestManager.ResultCommand.QstTalkChg(NpcId.Dudley, 25703)
			]);

		// Branch 2 - Klaus
		var process2 = AddNewProcess(2);
		process2.AddRawBlock(QuestAnnounceType.None)
			.AddCheckCommands([
				QuestManager.CheckCommand.TalkNpcChoice(635, NpcId.Shekel, 1)
			]);
        process2.AddTalkToNpcBlock(QuestAnnounceType.Update, Stage.AudienceChamber, NpcId.Klaus0, 25694)
			.AddResultCommands([
				QuestManager.ResultCommand.MyQstFlagOn(0),
				QuestManager.ResultCommand.QstTalkChg(NpcId.Shekel, 23896)
			]);
        process2.AddDelayBlock(QuestAnnounceType.None, 1, 180)
			.AddResultCommands([
				QuestManager.ResultCommand.QstLayoutFlagOn(5612),
				QuestManager.ResultCommand.UpdateAnnounceDirect(4, 3)
			]);
		process2.AddRawBlock(QuestAnnounceType.None)
			.AddResultCommands([
				QuestManager.ResultCommand.UpdateAnnounceDirect(5, 3)
			])
			.AddCheckCommands([
				QuestManager.CheckCommand.StageNo(635)
			]);
        process2.AddNewTalkToNpcBlock(QuestAnnounceType.None, Stage.MephiteTravelersInn, 0, 0, NpcId.Klaus0, 60319010)
			.AddResultCommands([
				QuestManager.ResultCommand.MyQstFlagOn(3366),
				QuestManager.ResultCommand.QstTalkChg(NpcId.Klaus0, 25696)
			]);
		process2.AddRawBlock(QuestAnnounceType.None)
			.AddResultCommands([
				QuestManager.ResultCommand.MyQstFlagOn(3420),
				QuestManager.ResultCommand.MyQstFlagOn(1),
				QuestManager.ResultCommand.QstTalkChg(NpcId.Klaus0, 25697)
			]);

		var process3 = AddNewProcess(3);
		process3.AddRawBlock(QuestAnnounceType.None)
			.AddCheckCommands([
				QuestManager.CheckCommand.IsMyquestLayoutFlagOn(5612)
			]);
		process3.AddRawBlock(QuestAnnounceType.None)
			.AddCheckCommands([
				QuestManager.CheckCommand.StageNoNotEq(201)
			]);
		process3.AddRawBlock(QuestAnnounceType.None)
			.AddResultCommands([
				QuestManager.ResultCommand.QstTalkChg(NpcId.Klaus0, 25695),
				QuestManager.ResultCommand.WorldManageLayoutFlagOff(1216, 70000001)
			]);

    }
}
return new ScriptedQuest();
