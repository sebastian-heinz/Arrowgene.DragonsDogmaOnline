/**
 * @brief Arms With the Power of the Dragon 2
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.ArmsWithThePowerOfTheDragon2;
    public override ushort RecommendedLevel => 100;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.CraftRoom;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.QuestUsefulForAdventure;

    public override bool AcceptRequirementsMet(GameClient client)
    {
        return client.Character.HasQuestCompleted(QuestId.ArmsWithThePowerOfTheDragon1);
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.BreakdownOfReason));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 105000);
        AddWalletReward(WalletType.Gold, 11000);
        AddWalletReward(WalletType.RiftPoints, 2000);
        AddFixedItemReward(ItemId.CustomMadeServiceTicket, 2);
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTalkAndOrderBlock(Stage.CraftRoom, NpcId.Craig0, 31151);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Accept, Stage.CraftRoom, NpcId.Craig0, 31152);
        process0.AddIsStageNoBlock(QuestAnnounceType.None, Stage.CraftRoom, false)
            .AddResultCmdReleaseAnnounce(ContentsRelease.SynthesisofDragonAbilities, TutorialId.None, QuestFlags.NpcFunctions.SynthesisOfDragonAbilities);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
