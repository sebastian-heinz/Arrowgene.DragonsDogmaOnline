/**
 * @brief Living with the Partner Pawn
 * @cheats
 *     /giveitem 10133 2
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.LivingWithThePartnerPawn;
    public override ushort RecommendedLevel => 8;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.TheWhiteDragonTemple0;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.QuestUsefulForAdventure;

    private class WorldManageQstFlag
    {
        public const uint StartJuliaWalk = 1511;
        public const uint EndJuliaWalk = 1512;
    }

    private class QstFlagLayout
    {
        public const uint SpawnJuliaInArisensRoom = 3807;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.SoloWithPawns());
        AddQuestOrderCondition(QuestOrderCondition.PersonalQuestCleared(QuestId.CraftedTokenOfTheHeart));
    }

    protected override void InitializeRewards()
    {
        AddFixedItemReward(ItemId.RiftstoneShard, 3);
        AddPointReward(PointType.ExperiencePoints, 650);
        AddWalletReward(WalletType.Gold, 400);
        AddWalletReward(WalletType.RiftPoints, 40);
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdIsMainQuestClear(QuestId.AServantsPledge);
        process0.AddNewNpcTalkAndOrderBlock(Stage.TheWhiteDragonTemple0, 0, 0, NpcId.Julia, 17815, QuestId.Q70020001)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.TheWhiteDragonTemple0.Julia0)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.TheWhiteDragonTemple0.Julia1);
        process0.AddRawBlock(QuestAnnounceType.Accept)
            .AddQuestFlag(QuestFlagType.WorldManageQuest, QuestFlagAction.Set, WorldManageQstFlag.StartJuliaWalk, QuestId.Q70020001)
            .AddCheckCmdMyQstFlagOnFromFsm(WorldManageQstFlag.EndJuliaWalk);
        process0.AddIsStageNoBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.ArisensRoom)
            .AddResultCmdReleaseAnnounce(ContentsRelease.MyRoom, TutorialId.ArisensRoom, QuestFlags.NpcFunctions.ArisensRoom)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.TheWhiteDragonTemple0.Julia1);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.ArisensRoom, 1, 1, NpcId.Julia, 17817)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstFlagLayout.SpawnJuliaInArisensRoom)
            .AddResultCmdPlayCameraEvent(Stage.ArisensRoom, 90);
        process0.AddRawBlock(QuestAnnounceType.CheckpointAndUpdate)
            .AddCheckCmdIsSetPartnerPawn();
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.ArisensRoom, 1, 1, NpcId.Julia, 17818);
        process0.AddRawBlock(QuestAnnounceType.CheckpointAndUpdate)
            .AddCheckCmdIsPresentPartnerPawn();
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.ArisensRoom, 1, 1, NpcId.Julia, 18357);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
