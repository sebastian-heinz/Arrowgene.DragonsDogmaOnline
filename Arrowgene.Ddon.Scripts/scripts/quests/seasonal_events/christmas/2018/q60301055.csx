/**
 * @brief Merry Christmas with Smiles I (2018)
 * @settings scripts/settings/SeasonalEventSettings.csx
 *   - EnableChristmasEvent : bool
 *   - ChristmasValidPeriod : (DateTime, DateTime)
 *   - ChristmasEventYear : uint
 * @cheats
 *   /giveitem 23548 20
 *       20 "Sparkling Snow Large Crystals"
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => (QuestId)60301055;
    public override ushort RecommendedLevel => 10;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.TheWhiteDragonTemple0;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.CollaborationOrSeasonalQuest;

    public override bool AcceptRequirementsMet(GameClient client)
    {
        return SeasonalEvents.CheckConfigSettings("EnableChristmasEvent", "ChristmasEventYear", 2018, "ChristmasValidPeriod");
    }

    protected override void InitializeRewards()
    {
        AddFixedItemReward(ItemId.SparklingSnowLargeCrystal, 50);
        AddFixedItemReward(ItemId.ChristmasOrnamentHat, 1);
        AddPointReward(PointType.ExperiencePoints, 1224);
        AddWalletReward(WalletType.Gold, 1224);
        AddWalletReward(WalletType.RiftPoints, 1224);
    }

    private static class NamedParamId
    {
        public const uint Sapling = 2611; // Sapling
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(0, Stage.Lestania, 71, 0, QuestEnemyPlacementType.Manual, new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.Ent, 10, 9800, 10)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.Sapling)
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNewNpcTalkAndOrderBlock(Stage.TheWhiteDragonTemple0, 1, 0, NpcId.Nicholas, 30843)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8245);
        process0.AddIsStageNoBlock(QuestAnnounceType.Accept, Stage.TheWhiteDragonTemple0);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, 1, 0, NpcId.Nicholas, 30847);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, NpcId.Isaac, 30849);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.CheckpointAndUpdate, 0, true);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, 0, false);
        process0.AddOmInteractEventBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.Lestania, 0, 0, OmQuestType.MyQuest, OmInteractType.Release)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8249)
                .AddAnnotation("Spawns item to collect");
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, 1, 0, NpcId.Nicholas, 30851)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 8249)
                .AddAnnotation("Despawns item to collect");
        process0.AddCheckBagEventBlock(QuestAnnounceType.CheckpointAndUpdate, ItemId.SparklingSnowLargeCrystal, 1)
                .AddAnnotation("Check for snowflake (1)");
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, 1, 0, NpcId.Nicholas, 30853);
        process0.AddNewDeliverItemsBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, 1, 0, NpcId.Nicholas, ItemId.SparklingSnowLargeCrystal, 20, 30857)
            .AddAnnotation("Deliver 20 snowflakes");
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
