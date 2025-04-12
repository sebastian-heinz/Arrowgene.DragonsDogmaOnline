/**
 * @brief Shape of Love for Someone (2) (2017)
 * @settings scripts/settings/SeasonalEventSettings.csx
 *   - EnableValentinesEvent : bool
 *   - ValentinesValidPeriod : (DateTime, DateTime)
 *   - ValentinesEventYear : uint
 * @cheats
 *   /giveitem 17107 1
 *     - Gives 1 "Special Chocolate" to the player
 *   In the quest step "Give chocolate to your favorite NPC", type one of the following in say
 *     - チｮコレート (chocolate)
 *     - ｴﾘｵｯﾄ (Elliot)
 *     - ｸﾗｳｽ (Klaus)
 *     - メイリーフ (Mayleaf)
 *     - リズ (Lise)
 *     - ガルドリン (Gurdolin)
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => (QuestId)60200025;
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
        AddFixedItemReward(ItemId.ValentineAngel, 1);
        AddPointReward(PointType.ExperiencePoints, 2140);
        AddWalletReward(WalletType.Gold, 2140);
        AddWalletReward(WalletType.RiftPoints, 2140);
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddIsQuestClearBlock(QuestAnnounceType.None, QuestType.Tutorial, (QuestId)60200024);
        process0.AddQuestNpcTalkAndOrderBlock((QuestId)60200026, Stage.TheWhiteDragonTemple0, 0, 0, NpcId.Shelly0, 22848);
        // TODO: Detect craft
        process0.AddCheckBagEventBlock(QuestAnnounceType.Accept, ItemId.SpecialChocolate, 1);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, (QuestId)60200026, Stage.TheWhiteDragonTemple0, 0, 0, NpcId.Shelly0, 22850);
        // The player has to say a message in the say 
        process0.AddCheckSayBlock(QuestAnnounceType.CheckpointAndUpdate)
            .AddResultCmdSetNpcMsg(NpcId.Gurdolin1, 23005)
            .AddResultCmdSetNpcMsg(NpcId.Elliot2, 23006)
            .AddResultCmdSetNpcMsg(NpcId.Klaus1, 23007)
            .AddResultCmdSetNpcMsg(NpcId.Mayleaf2, 23008)
            .AddResultCmdSetNpcMsg(NpcId.Lise2, 23009);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, (QuestId)60200026, Stage.TheWhiteDragonTemple0, 0, 0, NpcId.Shelly0, 22853);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
