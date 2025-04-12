/**
 * @brief The Temple's Tradespeople
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.TheTemplesTradespeople;
    public override ushort RecommendedLevel => 2;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.TheWhiteDragonTemple0;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.QuestUsefulForAdventure;

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.Solo());
        AddQuestOrderCondition(QuestOrderCondition.PersonalQuestCleared(QuestId.TheArisensAbilities));
    }

    protected override void InitializeRewards()
    {
        AddFixedItemReward(ItemId.Pickaxe, 3);
        AddFixedItemReward(ItemId.LumberKnife, 3);
        AddFixedItemReward(ItemId.Lockpick, 3);

        AddPointReward(PointType.ExperiencePoints, 600);
        AddWalletReward(WalletType.Gold, 1000);
        AddWalletReward(WalletType.RiftPoints, 100);
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdIsMainQuestClear(QuestId.AServantsPledge);
        process0.AddNpcTalkAndOrderBlock(Stage.TheWhiteDragonTemple0, NpcId.Cameron0, 11181);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Accept, Stage.TheWhiteDragonTemple0, NpcId.Ferguson, 14131)
            .AddResultCmdTutorialDialog(TutorialId.TreasureLot);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, NpcId.Kittredge, 14132)
            .AddResultCmdTutorialDialog(TutorialId.Equipment);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, NpcId.Ophelia, 14134)
            .AddResultCmdTutorialDialog(TutorialId.TradingGoods);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, NpcId.Barton, 14143)
            .AddResultCmdTutorialDialog(TutorialId.RecoveringRevivals);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, NpcId.Mel, 14523)
            .AddResultCmdTutorialDialog(TutorialId.Inn);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, NpcId.Kittredge, 14527)
            .AddResultCmdTutorialDialog(TutorialId.Bazaar)
            .AddCheckCmdHaveItemAllBag(ItemId.BeastSteak, 1);
        process0.AddDeliverItemsBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, NpcId.Barton, ItemId.BeastSteak, 1, 14140)
            .AddResultCmdTutorialDialog(TutorialId.UsingItems);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, NpcId.Cameron0, 14139);
        process0.AddIsStageNoBlock(QuestAnnounceType.None, Stage.TheWhiteDragonTemple0)
            .AddResultCmdTutorialDialog(TutorialId.NPCs);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
