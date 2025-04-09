/**
 * @brief Custom-Made Workshop 2: Limit Break
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.CustomMadeWorkshop2LimitBreak;
    public override ushort RecommendedLevel => 20;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.CraftRoom;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.QuestUsefulForAdventure;

    public override bool AcceptRequirementsMet(GameClient client)
    {
        return client.Character.HasQuestCompleted(QuestId.CustomMadeWorkshop1SearchersReturn);
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 10000);
        AddWalletReward(WalletType.Gold, 10000);
        AddWalletReward(WalletType.RiftPoints, 2000);
        AddFixedItemReward(ItemId.CustomMadeServiceTicket, 1);
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTalkAndOrderBlock(Stage.CraftRoom, NpcId.Craig0, 26368);
        process0.AddIsStageNoBlock(QuestAnnounceType.Accept, Stage.CraftRoom, false)
            .AddResultCmdTutorialDialog(TutorialId.LimitBreakingArms)
            .AddAnnotation("TODO: Detect weapon has been limit broken");
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.CraftRoom, NpcId.Craig0, 26370);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
