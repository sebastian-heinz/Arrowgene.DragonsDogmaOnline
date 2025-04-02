/**
 * @brief Custom-Made Workshop 1: Searcher's Return
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.CustomMadeWorkshop1SearchersReturn;
    public override ushort RecommendedLevel => 20;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.CraftRoom;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.QuestUsefulForAdventure;

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 10000);
        AddWalletReward(WalletType.Gold, 10000);
        AddWalletReward(WalletType.RiftPoints, 2000);
        AddFixedItemReward(ItemId.Craigblade, 1);
        AddFixedItemReward(ItemId.CustomMadeServiceTicket, 1);
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTalkAndOrderBlock(Stage.CraftRoom, NpcId.Craig0, 26391);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Accept, Stage.AudienceChamber, NpcId.Joseph, 26393);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.AudienceChamber, NpcId.Joseph, 26394);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.CraftRoom, NpcId.Craig0, 25935);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.CraftRoom, NpcId.Craig0, 25936);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.CraftRoom, NpcId.Craig0, 25936);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.CraftRoom, NpcId.Craig0, 26364);
        process0.AddIsStageNoBlock(QuestAnnounceType.None, Stage.CraftRoom, false)
            .AddResultCmdReleaseAnnounce(ContentsRelease.None, TutorialId.AboutCustomMadeWorkshop, QuestFlags.NpcFunctions.CustomMadeArmsAndEquipmentDissaembly);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
