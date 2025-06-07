/**
 * @brief Summer! Beach Festival <2>"
 * @settings scripts/settings/SeasonalEventSettings.csx
 *   - EnableSummerEvent : bool
 *   - SummerEventValidPeriod : (DateTime, DateTime)
 *   - SummerEventYear : uint
 * @cheats
 *   /giveitem 8708
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.SummerBeachFestival2;
    public override ushort RecommendedLevel => 10;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.TheWhiteDragonTemple0;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.CollaborationOrSeasonalQuest;
    public override Nullable<Boolean> EnableCancel => true;

    public override bool AcceptRequirementsMet(GameClient client)
    {
        return SeasonalEvents.CheckConfigSettings("EnableSummerEvent", "SummerEventYear", 2018, "SummerEventValidPeriod");
    }

    protected override void InitializeRewards()
    {
        AddFixedItemReward(ItemId.GoldenPalmFiber, 5);
        AddPointReward(PointType.ExperiencePoints, 8000);
        AddWalletReward(WalletType.Gold, 8000);
        AddWalletReward(WalletType.RiftPoints, 1500);
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdIsTutorialQuestClear(QuestId.SummerBeachFestival1);
        process0.AddNewNpcTalkAndOrderBlock(Stage.TheWhiteDragonTemple0, 0, 1, NpcId.Samantha, 25557, QuestId.SummerBeachFestivalDecorations);
        process0.AddRawBlock(QuestAnnounceType.Accept)
            .AddCheckCmdHaveItemAllBag(ItemId.GoldenPalmFiber, 1);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, QuestId.SummerBeachFestivalDecorations, Stage.TheWhiteDragonTemple0, 0, 1, NpcId.Samantha, 25559);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, NpcId.Gregory0, 25561);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, QuestId.SummerBeachFestivalDecorations, Stage.TheWhiteDragonTemple0, 0, 1, NpcId.Samantha, 25562);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
