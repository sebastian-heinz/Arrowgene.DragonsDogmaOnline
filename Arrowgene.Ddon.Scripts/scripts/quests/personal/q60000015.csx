/**
 * @brief Lestania's Best Dressed
 * @cheats
 *    /giveitem 8005 5
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.LestaniasBestDressed;
    public override ushort RecommendedLevel => 1;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.TheWhiteDragonTemple0;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.QuestUsefulForAdventure;

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.TheGirlInTheForest));
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdIsMainQuestClear(QuestId.AServantsPledge);
        process0.AddNpcTalkAndOrderBlock(Stage.TheWhiteDragonTemple0, NpcId.Ophelia, 14120);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Accept, Stage.TheWhiteDragonTemple0, NpcId.Mel, 14122);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.CraftRoom, NpcId.Sonia, 14124);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, NpcId.Cornelia0, 14126);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.LoneHouseintheForest, NpcId.Emerada0, 14128);
        process0.AddRawBlock(QuestAnnounceType.CheckpointAndUpdate)
            .AddCheckCmdHaveItemAllBag(ItemId.ScrollOfSorcery, 5);
        process0.AddDeliverItemsBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.LoneHouseintheForest, NpcId.Emerada0, ItemId.ScrollOfSorcery, 5, 14130);
        process0.AddIsStageNoBlock(QuestAnnounceType.None, Stage.LoneHouseintheForest)
            .AddResultCmdReleaseAnnounce(ContentsRelease.DressEquipment, TutorialId.DressEquipment);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
