/**
 * @brief Summer! Beach Festival <1>"
 * @settings scripts/settings/SeasonalEventSettings.csx
 *   - EnableSummerEvent : bool
 *   - SummerEventValidPeriod : (DateTime, DateTime)
 *   - SummerEventYear : uint
 * @cheats
 *   /giveitem 8708
 */

#load "C:\Users\Paul\Git\Arrowgene.DragonsDogmaOnline\Arrowgene.Ddon.Cli\bin\Debug\net9.0\Files\Assets\scripts\libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.SummerBeachFestival1;
    public override ushort RecommendedLevel => 10;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.TheWhiteDragonTemple0;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.CollaborationOrSeasonalQuest;
    public override Nullable<Boolean> EnableCancel => true;

    private class QstLayoutFlag
    {
        // White Dragon Temple
        public const uint RayAndSamantha0 = 6634;
        // Breya Coast
        public const uint RayAndSamantha1 = 6375;
    }

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

        var equipItems = new List<ItemId>
        {
            ItemId.VividGreenSwimsuitTop,
            ItemId.VividGreenSwimsuitBottom,
            ItemId.IndigoBlueSwimsuitTop,
            ItemId.IndigoBlueSwimsuitBottom,
            ItemId.BluishPurpleSwimsuitTop,
            ItemId.BluishPurpleSwimsuitBottom,
            ItemId.NightBlackSwimsuitTop,
            ItemId.NightBlackSwimsuitBottom,
            ItemId.SilverGraySwimsuitTop,
            ItemId.SilverGraySwimsuitBottom,
            ItemId.SettingSunSwimsuitTop,
            ItemId.SettingSunSwimsuitBottom,
            ItemId.AlluringSwimsuitTopGlitteringGold,
            ItemId.AlluringSwimsuitBottomGlitteringGold,
            ItemId.AlluringSwimsuitTopVividGreen,
            ItemId.AlluringSwimsuitTopIndigoBlue,
            ItemId.AlluringSwimsuitTopBluishPurple,
            ItemId.AlluringSwimsuitTopSilverGray,
            ItemId.AlluringSwimsuitTopSettingSun,
            ItemId.AlluringSwimsuitBottomVividGreen,
            ItemId.AlluringSwimsuitBottomIndigoBlue,
            ItemId.AlluringSwimsuitBottomBluishPurple,
            ItemId.AlluringSwimsuitBottomNightBlack,
            ItemId.AlluringSwimsuitBottomSilverGray,
            ItemId.AlluringSwimsuitBottomSettingSun,
            ItemId.StrongBanded0,
            ItemId.FishermansPants0,
            ItemId.CoquettishBacks0,
        };

        process0.AddNewNpcTalkAndOrderBlock(Stage.TheWhiteDragonTemple0, 1, 0, NpcId.Ray1, 25550)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.RayAndSamantha0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.RayAndSamantha1);
        var block = process0.AddRawBlock(QuestAnnounceType.Accept);
        for (var i = 0; i < equipItems.Count; i++)
        {
            block = block.AddCheckCmdIsEquip(equipItems[i], i);
        }
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, 1, 0, NpcId.Ray1, 25552);
        process0.AddIsStageNoBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.BreyaCoast);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.BreyaCoast, 1, 1, NpcId.Ray1, 25554);
        process0.AddRawBlock(QuestAnnounceType.CheckpointAndUpdate)
            .AddCheckCmdIsTakePicturesNpc(Stage.BreyaCoast, NpcId.Ray1, NpcId.Samantha);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.BreyaCoast, 1, 1, NpcId.Ray1, 25555);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
