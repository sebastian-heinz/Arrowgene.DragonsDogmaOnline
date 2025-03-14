/**
 * @brief For Christmas event NPC set (2018).
 * @settings scripts/settings/SeasonalEventSettings.csx
 *   - EnableChristmasEvent : bool
 *   - ChristmasValidPeriod : (DateTime, DateTime)
 *   - ChristmasEventYear : uint
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => (QuestId)60301057;
    public override ushort RecommendedLevel => 0;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => false;
    public override StageInfo StageInfo => Stage.TheWhiteDragonTemple0;

    public override bool AcceptRequirementsMet(GameClient client)
    {
        return SeasonalEvents.CheckConfigSettings("EnableChristmasEvent", "ChristmasEventYear", 2018, "ChristmasValidPeriod");
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
        process0.AddIsQuestClearBlock(QuestAnnounceType.None, QuestType.Tutorial, (QuestId)60301055);
        process0.AddIsQuestClearBlock(QuestAnnounceType.None, QuestType.Tutorial, (QuestId)60301056)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8296)
                .AddAnnotation("Clan Base Christmas Tree");
        process0.AddNoProgressBlock()
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8296)
                .AddAnnotation("Clan Base Christmas Tree")
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8229)
                .AddAnnotation("Spawns Nicholas, Marco and Mia and decorations");
        // These decoration tutorial quests have some strange behavior where they appear to progress
        // even when undesired. It might be related to the fact that we are not "accepting" the quest
        // but unsure. This block just re-applies the flags and then jumps back to the previous block creating
        // an infinite loop
        process0.AddReturnCheckPointBlock(0, 3)
            .AddAnnotation("Jump back to block 3 incase we progressed this far")
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8296)
                .AddAnnotation("Clan Base Christmas Tree")
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8229)
                .AddAnnotation("Spawns Nicholas, Marco and Mia and decorations");
        process0.AddProcessEndBlock(false);
    }
}

return new ScriptedQuest();
