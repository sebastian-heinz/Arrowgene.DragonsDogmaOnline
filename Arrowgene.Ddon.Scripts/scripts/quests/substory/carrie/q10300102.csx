/**
 * @brief Carrie the Cook Despairs
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Substory;
    public override QuestId QuestId => QuestId.CarrieTheCookDespairs;
    public override ushort RecommendedLevel => 82;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.FortThinesGreatDiningHall;

    private class QstLayoutFlag
    {
        public const uint Carrie2StNo443 = 6745;
    }

    private class EnemyGroupId
    {
        public const uint Encounter = 10;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.SoloWithPawns(1));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 42000);
        AddWalletReward(WalletType.Gold, 11000);
        AddWalletReward(WalletType.RiftPoints, 2000);

        AddSelectItemReward(new()
        {
            (ItemId.RoyalCrestMedalRathniteDistrict, 3),
            (ItemId.CarriesSpecialSandwich, 1),
        });
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.Encounter, Stage.RathniteFoothills, 34, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.GrimGoblin, 80, 4200, 16),
            LibDdon.Enemy.Create(EnemyId.GrimGoblin, 80, 4200, 17),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdNpcPreTalkAndOrderUi(Stage.FortThinesGreatDiningHall, NpcId.Carrie2, 26274, 0)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.FortThinesGreatDiningHall.Carrie)
            .AddResultCommands([
                QuestManager.ResultCommand.QstTalkChg(NpcId.Carrie2, 26157)
            ]);
        process0.AddStageJumpBlock(QuestAnnounceType.Accept, Stage.FortThines1, 16);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Start, Stage.FortThines1, 0, 0, NpcId.Carrie2, 26207)
            .SetIsCheckpoint(true)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.Carrie2StNo443);
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddResultCmdQstTalkChg(NpcId.Carrie2, 26278)
            .AddCheckCommands([
                QuestManager.CheckCommand.TalkNpcChoice(443, NpcId.Carrie2, 0)
            ])
            .AddCheckCommands([
                QuestManager.CheckCommand.TalkNpc(443, NpcId.Carrie2),
                QuestManager.CheckCommand.DummyNotProgress()
            ]);
        process0.AddStageJumpBlock(QuestAnnounceType.None, Stage.RathniteFoothills, 23);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.RathniteFoothills, 1, 0, NpcId.Carrie2, 26281)
            .AddResultCmdTutorialDialog(TutorialId.NPCEscortsFoodGauge)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 6746);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddResultCmdEnableSubstoryGauge()
            .AddCheckCmdMyQstFlagOnFromFsm(3646)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, 3645);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.RathniteFoothills, 1, 0, NpcId.Carrie2, 26282);
        process0.AddStageJumpBlock(QuestAnnounceType.None, Stage.RathniteFoothills, 24)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 6747)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 6746);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.RathniteFoothills, 2, 0, NpcId.Carrie2, 26175);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdMyQstFlagOnFromFsm(3650)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, 3647);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter);
        process0.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter, resetGroup: false);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.RathniteFoothills, 2, 0, NpcId.Carrie2, 26283)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, 3649);
        process0.AddStageJumpBlock(QuestAnnounceType.None, Stage.FortThinesGreatDiningHall, 2);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Update, Stage.FortThinesGreatDiningHall, NpcId.Carrie2, 26158)
            .AddResultCmdDisableSubstoryGauge()
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 6747);
        process0.AddNoProgressBlock();
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
