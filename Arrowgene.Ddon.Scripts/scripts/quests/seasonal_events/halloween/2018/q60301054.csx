/**
 * @brief Halloween Season (Spawns NPCs for 2018 holiday event)
 * @settings scripts/settings/SeasonalEventSettings.csx
 *   - EnableHalloweenEvent : bool
 *   - HalloweenValidPeriod : (DateTime, DateTime)
 *   - HalloweenEventYear : uint
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => (QuestId)60301054;
    public override ushort RecommendedLevel => 0;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => false;
    public override StageInfo StageInfo => Stage.TheWhiteDragonTemple0;

    public override bool AcceptRequirementsMet(GameClient client)
    {
        return SeasonalEvents.CheckConfigSettings("EnableHalloweenEvent", "HalloweenEventYear", 2018, "HalloweenValidPeriod");
    }

    protected override void InitializeRewards()
    {
        // This quest has no rewards
    }

    protected override void InitializeEnemyGroups()
    {
        // This quest has no enemy groups
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddIsQuestClearBlock(QuestAnnounceType.None, QuestType.Tutorial, (QuestId)60301053);
        process0.AddIsQuestClearBlock(QuestAnnounceType.None, QuestType.Tutorial, (QuestId)60301054)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8236)
                .AddAnnotation("Spawns decorations and warp")
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8228)
                .AddAnnotation("Spawns Angelo and Shelly so they remain for the event");
        process0.AddReturnCheckPointBlock(0, 2)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8236)
                .AddAnnotation("Spawns decorations and warp")
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8228)
                .AddAnnotation("Spawns Angelo and Shelly so they remain for the event");
        process0.AddProcessEndBlock(false);
    }
}

return new ScriptedQuest();
