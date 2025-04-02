/**
 * @brief Arms With the Power of the Dragon I
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.ArmsWithThePowerOfTheDragon1;
    public override ushort RecommendedLevel => 100;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.CraftRoom;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.QuestUsefulForAdventure;

    public override bool AcceptRequirementsMet(GameClient client)
    {
        return client.Character.HasQuestCompleted(QuestId.CustomMadeWorkshop3UltimateArmsSynthesis);
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
        process0.AddNpcTalkAndOrderBlock(Stage.CraftRoom, NpcId.Craig0, 31147);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Accept, Stage.CraftRoom, NpcId.Craig0, 31149);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.CraftRoom, NpcId.Sonia, 31272)
            .AddResultCmdReleaseAnnounce(ContentsRelease.DismantlingofDragonArms, TutorialId.DragonEquipmentDecomposition, QuestFlags.NpcFunctions.DragonArmorAppraisal)
            .AddResultCmdReleaseAnnounce(ContentsRelease.AppraisalExchangeofDragonArmor);
        process0.AddIsStageNoBlock(QuestAnnounceType.None, Stage.CraftRoom, false)
            .AddResultCmdTutorialDialog(TutorialId.EnhancementofDragonAbilities);
        process0.AddProcessEndBlock(true);        
    }
}

return new ScriptedQuest();
