/**
 * @brief Summer! Beach Festival"
 * @settings scripts/settings/SeasonalEventSettings.csx
 *   - EnableSummerEvent : bool
 *   - SummerEventValidPeriod : (DateTime, DateTime)
 *   - SummerEventYear : uint
 */

#load "C:\Users\Paul\Git\Arrowgene.DragonsDogmaOnline\Arrowgene.Ddon.Cli\bin\Debug\net9.0\Files\Assets\scripts\libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.SummerBeachFestivalDecorations;
    public override ushort RecommendedLevel => 0;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => false;
    public override StageInfo StageInfo => Stage.TheWhiteDragonTemple0;

    private class QstLayoutFlag
    {
        public const uint RayAndSam = 6338;
        public const uint BreyaCoastWarp = 6376;
        public const uint BreyaCoastGlitterPoints = 6376; 
    }

    public override bool AcceptRequirementsMet(GameClient client)
    {
        return SeasonalEvents.CheckConfigSettings("EnableSummerEvent", "SummerEventYear", 2018, "SummerEventValidPeriod");
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNoProgressBlock()
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.BreyaCoastWarp)
            .AddAnnotation("Spawns warp");
        process0.AddReturnCheckPointBlock(0, 1);
        process0.AddNoProgressBlock();
        process0.AddProcessEndBlock(true);

        var process1 = AddNewProcess(1);
        process1.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdIsTutorialQuestClear(QuestId.SummerBeachFestival1);
        process1.AddNoProgressBlock()
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.RayAndSam)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.BreyaCoastGlitterPoints);
        process1.AddProcessEndBlock(false);
    }
}

return new ScriptedQuest();
