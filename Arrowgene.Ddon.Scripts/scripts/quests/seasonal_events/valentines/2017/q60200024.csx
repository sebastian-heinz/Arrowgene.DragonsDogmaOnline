/**
 * @brief Shape of Love for Someone (1) (2017)
 * @settings scripts/settings/SeasonalEventSettings.csx
 *   - EnableValentinesEvent : bool
 *   - ValentinesValidPeriod : (DateTime, DateTime)
 *   - ValentinesEventYear : uint
 * @cheats
 *   /giveitem 17106 1
 *     - Gives 1 "Handpicked Caocao" to the player
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => (QuestId)60200024;
    public override ushort RecommendedLevel => 10;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.TheWhiteDragonTemple0;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.CollaborationOrSeasonalQuest;

    public override bool AcceptRequirementsMet(GameClient client)
    {
        return SeasonalEvents.CheckConfigSettings("EnableValentinesEvent", "ValentinesEventYear", 2017, "ValentinesValidPeriod");
    }

    protected override void InitializeRewards()
    {
        AddFixedItemReward(ItemId.HandpickedCacao, 1);
        AddFixedItemReward(ItemId.SpecialChocolate, 1);
        AddFixedItemReward(ItemId.HealingPotion, 2);
        AddPointReward(PointType.ExperiencePoints, 2140);
        AddWalletReward(WalletType.Gold, 2140);
        AddWalletReward(WalletType.RiftPoints, 2140);
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddQuestNpcTalkAndOrderBlock((QuestId)60200026, Stage.TheWhiteDragonTemple0, 0, 0, NpcId.Shelly0, 22845);
        process0.AddCheckBagEventBlock(QuestAnnounceType.Accept, ItemId.HandpickedCacao, 1);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, (QuestId)60200026, Stage.TheWhiteDragonTemple0, 0, 0, NpcId.Shelly0, 22847);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
