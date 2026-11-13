/**
 * @brief Rathnite Foothills Trial: Detect the Demon Army's Attack
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => (QuestId)60318010;
    public override ushort RecommendedLevel => 83;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => false;
    public override StageInfo StageInfo => Stage.FortThines1;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.AreaTrialOrMission;

    private class EnemyGroupId
    {
        public const uint Encounter0 = 10;
        public const uint Encounter1 = 11;
        public const uint Encounter2 = 12;
    }

    private class NamedParamId
    {
        public const uint AideDeCamp = 1808;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.SoloWithPawns());
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.PrinceNedo));
        AddQuestOrderCondition(QuestOrderCondition.RequiredItemRank(84));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 105000);
        AddWalletReward(WalletType.Gold, 11000);
        AddWalletReward(WalletType.RiftPoints, 2000);

        AddFixedItemReward(ItemId.RoyalCrestMedalRathniteDistrict, 3);
        AddFixedItemReward(ItemId.PyroclasticRock, 1);
        AddFixedItemReward(ItemId.VolcanicGlass, 1);
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.Encounter0, Stage.RathniteFoothillsLakeside0, 40, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.GrimGoblinLeader, 83, 21000, 0)
                .SetNamedEnemyParams(NamedParamId.AideDeCamp),
        });

        AddEnemies(EnemyGroupId.Encounter1, Stage.RathniteFoothillsLakeside0, 41, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.GrimGoblinLeader, 83, 21000, 0)
                .SetNamedEnemyParams(NamedParamId.AideDeCamp),
        });

        AddEnemies(EnemyGroupId.Encounter2, Stage.RathniteFoothillsLakeside0, 42, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.GrimGoblinLeader, 83, 21000, 0)
                .SetNamedEnemyParams(NamedParamId.AideDeCamp),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdCheckAreaRank(QuestAreaId.RathniteFoothills, 9);
        process0.AddNpcTalkAndOrderBlock(Stage.FortThines1, NpcId.Endale, 25366);
        process0.AddRawBlock(QuestAnnounceType.Accept)
			.AddCheckCmdTouchActToNpc(Stage.FortThines1, NpcId.Endale);
        process0.AddEventAfterJumpContinueBlock(QuestAnnounceType.None, Stage.RathniteFoothillsLakeside0, 10, 0);
        process0.AddStageJumpBlock(QuestAnnounceType.None, Stage.FortThines1, 7);
		process0.AddRawBlock(QuestAnnounceType.CheckpointAndUpdate)
            .AddResultCmdQstTalkChg(NpcId.Endale, 23663)
			.AddCheckCommands([
				QuestManager.CheckCommand.TalkNpcChoice(443, NpcId.Endale, 0)
			])
			.AddCheckCommands([
				QuestManager.CheckCommand.TalkNpcChoice(443, NpcId.Endale, 1)
			])
			.AddCheckCommands([
				QuestManager.CheckCommand.TalkNpc(443, NpcId.Endale),
				QuestManager.CheckCommand.DummyNotProgress()
			]);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Update, Stage.BerthasBanditGroupHideout, NpcId.Joaquim, 23666)
            .AddResultCmdQstTalkChg(NpcId.Endale, 26224);
		process0.AddDeliverItemsBlock(QuestAnnounceType.Update, Stage.BerthasBanditGroupHideout, NpcId.Joaquim, ItemId.MarkOfBruteCourage, 3, 23667)
            .AddResultCmdQstTalkChg(NpcId.Joaquim, 25359);
		process0.AddRawBlock(QuestAnnounceType.CheckpointAndUpdate)
            .AddResultCmdQstTalkChg(NpcId.Joaquim, 25360)
			.AddCheckCommands([
				QuestManager.CheckCommand.TalkNpcChoice(1001, NpcId.Joaquim, 0)
			])
			.AddCheckCommands([
				QuestManager.CheckCommand.TalkNpcChoice(1001, NpcId.Joaquim, 1)
			])
			.AddCheckCommands([
				QuestManager.CheckCommand.TalkNpc(1001, NpcId.Joaquim),
				QuestManager.CheckCommand.DummyNotProgress()
			]);
        process0.AddRawBlock(QuestAnnounceType.Update)
			.AddResultCommands([
				QuestManager.ResultCommand.QstTalkChg(NpcId.Joaquim, 25364),
				QuestManager.ResultCommand.SetRandom(1, 1, 3, 1),
				QuestManager.ResultCommand.MyQstFlagOn(0)
			])
			.AddCheckCommands([
				QuestManager.CheckCommand.MyQstFlagOn(2)
			]);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Update, Stage.BerthasBanditGroupHideout, NpcId.Joaquim, 25363);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.RathniteFoothillsLakeside0, NpcId.Victor, 25365);
        process0.AddProcessEndBlock(true)
			.AddResultCmdCallGreatPurpose(6, 1);

		var process1 = AddNewProcess(1);
		process1.AddRawBlock(QuestAnnounceType.None)
			.AddCheckCommands([
				QuestManager.CheckCommand.MyQstFlagOn(0)
			]);
		process1.AddRawBlock(QuestAnnounceType.None)
			.AddCheckCommands([
				QuestManager.CheckCommand.IsEnemyFoundRadius(131, 40, -1, 1)
			])
			.AddCheckCommands([
				QuestManager.CheckCommand.MyQstFlagOn(1)
			]);
		process1.AddRawBlock(QuestAnnounceType.None)
			.AddResultCommands([
				QuestManager.ResultCommand.MyQstFlagOn(10)
			])
			.AddCheckCommands([
				QuestManager.CheckCommand.MyQstFlagOff(40),
				QuestManager.CheckCommand.MyQstFlagOff(1)
			]);
		process1.AddRawBlock(QuestAnnounceType.None)
			.AddResultCommands([
				QuestManager.ResultCommand.CallGeneralAnnounce(1, 100250)
			]);

		var process2 = AddNewProcess(2);
		process2.AddRawBlock(QuestAnnounceType.None)
			.AddCheckCommands([
				QuestManager.CheckCommand.MyQstFlagOn(0)
			]);
		process2.AddRawBlock(QuestAnnounceType.None)
			.AddCheckCommands([
				QuestManager.CheckCommand.IsEnemyFoundRadius(131, 41, -1, 1)
			])
			.AddCheckCommands([
				QuestManager.CheckCommand.MyQstFlagOn(1)
			]);
		process2.AddRawBlock(QuestAnnounceType.None)
			.AddResultCommands([
				QuestManager.ResultCommand.MyQstFlagOn(11)
			])
			.AddCheckCommands([
				QuestManager.CheckCommand.MyQstFlagOff(41),
				QuestManager.CheckCommand.MyQstFlagOff(1)
			]);
		process2.AddRawBlock(QuestAnnounceType.None)
			.AddResultCommands([
				QuestManager.ResultCommand.CallGeneralAnnounce(1, 100250)
			]);

		var process3 = AddNewProcess(3);
		process3.AddRawBlock(QuestAnnounceType.None)
			.AddCheckCommands([
				QuestManager.CheckCommand.MyQstFlagOn(0)
			]);
		process3.AddRawBlock(QuestAnnounceType.None)
			.AddCheckCommands([
				QuestManager.CheckCommand.IsEnemyFoundRadius(131, 42, -1, 1)
			])
			.AddCheckCommands([
				QuestManager.CheckCommand.MyQstFlagOn(1)
			]);
		process3.AddRawBlock(QuestAnnounceType.None)
			.AddResultCommands([
				QuestManager.ResultCommand.MyQstFlagOn(12)
			])
			.AddCheckCommands([
				QuestManager.CheckCommand.MyQstFlagOff(42),
				QuestManager.CheckCommand.MyQstFlagOff(1)
			]);
		process3.AddRawBlock(QuestAnnounceType.None)
			.AddResultCommands([
				QuestManager.ResultCommand.CallGeneralAnnounce(1, 100250)
			]);

		var process4 = AddNewProcess(4);
		process4.AddRawBlock(QuestAnnounceType.None)
			.AddCheckCommands([
				QuestManager.CheckCommand.RandomEq(1, 1)
			]);
        process4.AddSpawnGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter0)
			.AddResultCommands([
				QuestManager.ResultCommand.MyQstFlagOn(40)
			])
			.AddCheckCommands([
				QuestManager.CheckCommand.MyQstFlagOn(10)
			]);
        process4.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter0)
			.AddResultCommands([
				QuestManager.ResultCommand.CallGeneralAnnounce(1, 100251),
				QuestManager.ResultCommand.MyQstFlagOn(1)
			]);
		process4.AddRawBlock(QuestAnnounceType.Update)
			.AddResultCommands([
				QuestManager.ResultCommand.QstLayoutFlagOn(6335),
				QuestManager.ResultCommand.CallGeneralAnnounce(1, 100253)
			])
			.AddCheckCommands([
				QuestManager.CheckCommand.QuestOmSetTouch(131, 0, 0)
			]);
		process4.AddRawBlock(QuestAnnounceType.None)
			.AddResultCommands([
				QuestManager.ResultCommand.MyQstFlagOn(2)
			]);

		var process5 = AddNewProcess(5);
		process5.AddRawBlock(QuestAnnounceType.None)
			.AddCheckCommands([
				QuestManager.CheckCommand.RandomEq(1, 2)
			]);
        process5.AddSpawnGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter1)
			.AddResultCommands([
				QuestManager.ResultCommand.MyQstFlagOn(41)
			])
			.AddCheckCommands([
				QuestManager.CheckCommand.MyQstFlagOn(11)
			]);
        process5.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter1)
			.AddResultCommands([
				QuestManager.ResultCommand.CallGeneralAnnounce(1, 100251),
				QuestManager.ResultCommand.MyQstFlagOn(1)
			]);
		process5.AddRawBlock(QuestAnnounceType.Update)
			.AddResultCommands([
				QuestManager.ResultCommand.QstLayoutFlagOn(6336),
				QuestManager.ResultCommand.CallGeneralAnnounce(1, 100253)
			])
			.AddCheckCommands([
				QuestManager.CheckCommand.QuestOmSetTouch(131, 1, 0)
			]);
		process5.AddRawBlock(QuestAnnounceType.None)
			.AddResultCommands([
				QuestManager.ResultCommand.MyQstFlagOn(2)
			]);

		var process6 = AddNewProcess(6);
		process6.AddRawBlock(QuestAnnounceType.None)
			.AddCheckCommands([
				QuestManager.CheckCommand.RandomEq(1, 3)
			]);
        process6.AddSpawnGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter2)
			.AddResultCommands([
				QuestManager.ResultCommand.MyQstFlagOn(42)
			])
			.AddCheckCommands([
				QuestManager.CheckCommand.MyQstFlagOn(12)
			]);
        process6.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter2)
			.AddResultCommands([
				QuestManager.ResultCommand.CallGeneralAnnounce(1, 100251),
				QuestManager.ResultCommand.MyQstFlagOn(1)
			]);
		process6.AddRawBlock(QuestAnnounceType.Update)
			.AddResultCommands([
				QuestManager.ResultCommand.QstLayoutFlagOn(6337),
				QuestManager.ResultCommand.CallGeneralAnnounce(1, 100253)
			])
			.AddCheckCommands([
				QuestManager.CheckCommand.QuestOmSetTouch(131, 2, 0)
			]);
		process6.AddRawBlock(QuestAnnounceType.None)
			.AddResultCommands([
				QuestManager.ResultCommand.MyQstFlagOn(2)
			]);
    }
}

return new ScriptedQuest();
