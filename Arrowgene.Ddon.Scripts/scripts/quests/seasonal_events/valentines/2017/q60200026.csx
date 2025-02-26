/**
 * @brief Valentines Day Season (Spawns NPCs for 2017 holiday event)
 * @settings scripts/settings/SeasonalEventSettings.csx
 *   - EnableValentinesEvent : bool
 *   - ValentinesValidPeriod : (DateTime, DateTime)
 *   - ValentinesEventYear : uint
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => (QuestId)60200026;
    public override ushort RecommendedLevel => 0;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => false;
    public override StageInfo StageInfo => Stage.TheWhiteDragonTemple0;

    public override bool AcceptRequirementsMet(GameClient client)
    {
        return SeasonalEvents.CheckConfigSettings("EnableValentinesEvent", "ValentinesEventYear", 2017, "ValentinesValidPeriod");
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNoProgressBlock();
        process0.AddReturnCheckPointBlock(0, 1);
        process0.AddProcessEndBlock(false);
    }
}

return new ScriptedQuest();
