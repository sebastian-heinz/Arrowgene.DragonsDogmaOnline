/**
 * @brief Extend Garden
 * @cheats
 *  /giveitem 7968 15
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.ExtendGarden;
    public override ushort RecommendedLevel => 20;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.TheWhiteDragonTemple0;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.QuestUsefulForAdventure;

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.SoloWithPawns());
        AddQuestOrderCondition(QuestOrderCondition.PersonalQuestCleared(QuestId.LivingWithThePartnerPawn));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 10000);
        AddWalletReward(WalletType.Gold, 10000);
        AddWalletReward(WalletType.RiftPoints, 2000);
        AddFixedItemReward(ItemId.RiftstoneShard, 5);
    }

    protected override void InitializeBlocks()
    {
        // https://www.youtube.com/watch?v=Dr2MRw2Vm1E

        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdIsTutorialQuestClear(QuestId.LivingWithThePartnerPawn);
        process0.AddNpcTalkAndOrderBlock(Stage.TheWhiteDragonTemple0, NpcId.Barton, 24006);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Accept, Stage.TheWhiteDragonTemple0, NpcId.Barton, 24007);
        process0.AddDeliverItemsBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, NpcId.Barton, ItemId.PineLumber, 15, 25003)
            .AddCallback((param) => {
                LibDdon.ChatMgr.SendMessageToPlayer(param.Client, LobbyChatMsgType.ManagementGuideC, "Quest menu decisions not implemented, selecting first choice");
            });
        // TODO: Wait 3 minutes...
        // This announce will misfire if the player logs out here
        // Should investigate how to resume when updating this way
        process0.AddTalkToNpcBlock(QuestAnnounceType.None, Stage.TheWhiteDragonTemple0, NpcId.Barton, 25002)
            .SetIsCheckpoint(true)
            .AddResultCmdUpdateAnnounceDirect(8, QuestAnnounceType.Update);
        process0.AddIsStageNoBlock(QuestAnnounceType.None, Stage.TheWhiteDragonTemple0)
            .AddResultCmdReleaseAnnounce(ContentsRelease.YourRoomsTerrace);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
