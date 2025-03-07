/**
 * @brief Soldiers of the Rift
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Main;
    public override QuestId QuestId => QuestId.SolidersOfTheRift;
    public override ushort RecommendedLevel => 5;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.AudienceChamber;
    public override QuestId NextQuestId => QuestId.AServantsPledge;

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumLevel(5));
        AddQuestOrderCondition(QuestOrderCondition.Solo());
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.EnvoyOfReconcilliation));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 2100);
        AddWalletReward(WalletType.Gold, 1200);
        AddWalletReward(WalletType.RiftPoints, 180);

        AddFixedItemReward(ItemId.TrouvereBand0, 1);
        AddFixedItemReward(ItemId.CombatBottoms0, 1);
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTalkAndOrderBlock(Stage.AudienceChamber, NpcId.Iris, 0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 273)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.AudienceChamber.Mysial)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.AudienceChamber.Leo)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.AudienceChamber.Iris)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.AudienceChamber.TheWhiteDragon7)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.AudienceChamber.TheWhiteDragon0)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.AudienceChamber.TheCrewEndSeason34);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.AudienceChamber, 8, 0);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Accept, Stage.BlackGrapeInn, NpcId.Alfred, 7537);
        process0.AddRawBlock(QuestAnnounceType.CheckpointAndUpdate)
            .AddResultCmdReleaseAnnounce(ContentsRelease.AreaMaster, TutorialId.AreaMasters, QuestFlags.NpcFunctions.AreaInformation)
            .AddCheckCmdOpenAreaMaster(QuestAreaId.HidellPlains);
        process0.AddIsStageNoBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.PawnCathedral)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, 1)
            .AddResultCmdTutorialDialog(TutorialId.AreaRankAndLevelingUp);
        // Don't checkpoint in case the player disconnects, that way they can get markers can help to
        // navigate back to the Pawn Cathedral when they log back in
        process0.AddTalkToNpcBlock(QuestAnnounceType.Update, Stage.PawnCathedral, NpcId.Thomason, 14773);
        process0.AddRawBlock(QuestAnnounceType.CheckpointAndUpdate)
            .AddCheckCmdIsFavoriteWarpPoint(4);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.PawnCathedral, NpcId.Alvar, 7539);
        process0.AddProcessEndBlock(true);

        var process1 = AddNewProcess(1);
        process1.AddMyQstFlagsBlock(QuestAnnounceType.None)
            .AddMyQstCheckFlag(1);
        process1.AddIsStageNoBlock(QuestAnnounceType.None, Stage.HuntersSecretPassage);
        process1.AddProcessEndBlock(false);
    }
}

return new ScriptedQuest();
