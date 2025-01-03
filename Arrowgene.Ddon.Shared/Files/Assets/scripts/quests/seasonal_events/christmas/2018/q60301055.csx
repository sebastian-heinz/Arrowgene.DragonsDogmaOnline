/**
 * @brief Merry Christmas with Smiles I (2018)
 * @settings scripts/settings/SeasonalEvents.csx
 *   - EnableChristmasEvent : bool
 *   - ChristmasValidPeriod : (DateTime, DateTime)
 *   - ChristmasEventYear : uint
 * @cheats
 *   /giveitem 23548 20
 *       20 "Sparkling Snow Large Crystals"
 */

#load "SeasonalEvents.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType     => QuestType.Tutorial;
    public override QuestId QuestId         => (QuestId) 60301055;
    public override ushort RecommendedLevel => 10;
    public override byte MinimumItemRank    => 0;
    public override bool IsDiscoverable     => true;
    public override StageId StageId         => StageId.WhiteDragonTemple;

    public override bool AcceptRequirementsMet(DdonGameServer server, GameClient client)
    {
        return SeasonalEvents.CheckConfigSettings(server, "EnableChristmasEvent", "ChristmasEventYear", 2018, "ChristmasValidPeriod");
    }

    protected override void InitializeRewards()
    {
        AddFixedItemReward(ItemId.SparklingSnowLargeCrystal, 50);
        AddFixedItemReward(ItemId.ChristmasOrnamentHat, 1);
        AddPointReward(PointType.ExperiencePoints, 1224);
        AddWalletReward(WalletType.Gold, 1224);
        AddWalletReward(WalletType.RiftPoints, 1224);
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(0, new StageId(1, 0, 71), 0, QuestEnemyPlacementType.Manual, new List<InstancedEnemy>()
        {
            LibDdon.CreateEnemy(EnemyId.Ent, 10, 9800, 10)
                .SetIsBoss(true)
                .SetNamedEnemyParams(LibDdon.GetNamedParam(2621)) // "Sapling"
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = new QuestProcess(0);
        process0.AddNewNpcTalkAndOrderBlock(new StageId(2, 0, 1), NpcId.Nicholas, 30843)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8245);
        process0.AddIsStageNoBlock(QuestAnnounceType.Accept, StageId.WhiteDragonTemple);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, new StageId(2, 0, 1), NpcId.Nicholas, 30847);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, StageId.WhiteDragonTemple, NpcId.Isaac, 30849);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.CheckpointAndUpdate, 0, true);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, 0, false);
        process0.AddOmInteractEventBlock(QuestAnnounceType.CheckpointAndUpdate, new StageId(1, 0, 0), OmQuestType.MyQuest, OmInteractType.Release)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8249)
                .AddAnnotation("Spawns item to collect");
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, new StageId(2, 0, 1), NpcId.Nicholas, 30851)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 8249)
                .AddAnnotation("Despawns item to collect");
        process0.AddCheckBagEventBlock(QuestAnnounceType.CheckpointAndUpdate, ItemId.SparklingSnowLargeCrystal, 1)
                .AddAnnotation("Check for snowflake (1)");
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, new StageId(2, 0, 1), NpcId.Nicholas, 30853);
        process0.AddNewDeliverItemsBlock(QuestAnnounceType.CheckpointAndUpdate, new StageId(2, 0, 1), NpcId.Nicholas, ItemId.SparklingSnowLargeCrystal, 20, 30857)
            .AddAnnotation("Deliver 20 snowflakes");
        process0.AddProcessEndBlock(true);
        AddProcess(process0);
    }
}

return new ScriptedQuest();
