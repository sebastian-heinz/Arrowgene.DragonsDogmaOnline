/**
 * @brief Halloween Season (Spawns NPCs for 2017 holiday event)
 * @settings scripts/settings/SeasonalEventSettings.csx
 *   - EnableHalloweenEvent : bool
 *   - HalloweenValidPeriod : (DateTime, DateTime)
 *   - HalloweenEventYear : uint
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => (QuestId)60301001;
    public override ushort RecommendedLevel => 0;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => false;
    public override StageInfo StageInfo => Stage.TheWhiteDragonTemple0;

    public override bool AcceptRequirementsMet(GameClient client)
    {
        return SeasonalEvents.CheckConfigSettings("EnableHalloweenEvent", "HalloweenEventYear", 2017, "HalloweenValidPeriod");
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
        process0.AddIsQuestClearBlock(QuestAnnounceType.None, QuestType.Tutorial, (QuestId)60301000)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 6985)
                .AddAnnotation("Spawns Angelo")
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 6986)
                .AddAnnotation("Spawns Pumpin Head Shelly");
        process0.AddNoProgressBlock()
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 6985)
                .AddAnnotation("Spawns Angelo")
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 6986)
                .AddAnnotation("Despawns punmpkin head shelly")
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 6987)
                .AddAnnotation("Spawns Normal Shelly");
        // These decoration tutorial quests have some strange behavior where they appear to progress
        // even when undesired. It might be related to the fact that we are not "accepting" the quest
        // but unsure. This block just re-applies the flags and then jumps back to the previous block creating
        // an infinite loop
        process0.AddReturnCheckPointBlock(0, 2)
            .AddAnnotation("Jump back to block 2 incase we progressed this far")
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 6985)
                .AddAnnotation("Spawns Angelo")
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 6987)
                .AddAnnotation("Spawns Normal Shelly");
        process0.AddProcessEndBlock(false);
    }
}

return new ScriptedQuest();
