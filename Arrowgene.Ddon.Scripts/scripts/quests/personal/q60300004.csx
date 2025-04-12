/**
 * @brief Custom-Made Workshop 3: Ultimate Arms Synthesis
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.CustomMadeWorkshop3UltimateArmsSynthesis;
    public override ushort RecommendedLevel => 20;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.CraftRoom;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.QuestUsefulForAdventure;

    public override bool AcceptRequirementsMet(GameClient client)
    {
        return client.Character.HasQuestCompleted(QuestId.CustomMadeWorkshop2LimitBreak);
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 10000);
        AddWalletReward(WalletType.Gold, 10000);
        AddWalletReward(WalletType.RiftPoints, 2000);
        AddFixedItemReward(ItemId.CraigsWornOutPants, 1);
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTalkAndOrderBlock(Stage.CraftRoom, NpcId.Craig0, 26396);
        process0.AddIsStageNoBlock(QuestAnnounceType.Accept, Stage.CraftRoom, false)
            .AddResultCmdTutorialDialog(TutorialId.UltimateSynthesisofArms)
            .AddAnnotation("TODO: Detect an ultimate synthesis has occurred");
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.CraftRoom, NpcId.Craig0, 26398);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
