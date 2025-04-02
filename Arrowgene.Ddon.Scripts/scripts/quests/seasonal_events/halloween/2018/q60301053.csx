/**
 * @brief Emergency! Not Enough Candy! (2) (Halloween Seasonal Event 2018)
 * @settings scripts/settings/SeasonalEventSettings.csx
 *   - EnableHalloweenEvent : bool
 *   - HalloweenValidPeriod : (DateTime, DateTime)
 *   - HalloweenEventYear : uint
 * @cheats
 *   /giveitem 23545 10
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => (QuestId)60301053;
    public override ushort RecommendedLevel => 10;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.TheWhiteDragonTemple0;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.CollaborationOrSeasonalQuest;

    public override bool AcceptRequirementsMet(GameClient client)
    {
        return SeasonalEvents.CheckConfigSettings("EnableHalloweenEvent", "HalloweenEventYear", 2018, "HalloweenValidPeriod");
    }

    protected override void InitializeRewards()
    {
        AddFixedItemReward(ItemId.HalloweenPetitMuffin, 70);
        AddFixedItemReward(ItemId.HorrorNightPlush, 1);
        AddPointReward(PointType.ExperiencePoints, 1031);
        AddWalletReward(WalletType.Gold, 1031);
        AddWalletReward(WalletType.RiftPoints, 1031);
    }

    protected override void InitializeEnemyGroups()
    {
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddIsQuestClearBlock(QuestAnnounceType.None, QuestType.Tutorial, (QuestId)60301052)
            .AddAnnotation("Wait for part (1) to be completed");
        process0.AddNewNpcTalkAndOrderBlock(Stage.TheWhiteDragonTemple0, 1, 0, NpcId.Shelly1, 30829)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8238)
                .AddAnnotation("Spawns Shelly and Angelo");
        process0.AddTalkToNpcBlock(QuestAnnounceType.Accept, Stage.AudienceChamber, NpcId.Joseph, 30831);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, NpcId.Milburn, 30832);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, NpcId.Charleston, 30833);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, 1, 0, NpcId.Shelly1, 30834);
        process0.AddPartyGatherBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, 3644, 12495, 17320)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8239)
                .AddAnnotation("Spawns Ghost (1)");
        process0.AddPartyGatherBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, -5768, 11065, 21829)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 8239)
                .AddAnnotation("Despawns Ghost (1)")
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8240)
                .AddAnnotation("Spawns Ghost (2)");
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, 3, 0, NpcId.MysteriousGhost, 30836)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 8240)
                .AddAnnotation("Despawns Ghost (2)")
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8241)
                .AddAnnotation("Spawns Ghost (3)");
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, 1, 0, NpcId.Shelly1, 30837);
        process0.AddNewDeliverItemsBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, 3, 0, NpcId.MysteriousGhost, ItemId.HalloweenPetitMuffin, 10, 30838);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, 1, 0, NpcId.Shelly1, 30840);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
