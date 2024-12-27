/**
 * @brief Emergency! Not Enough Candy! (1) (Halloween Seasonal Event 2018)
 * @settings scripts/settings/SeasonalEvents.csx
 *   - EnableHalloweenEvent : bool
 *   - HalloweenValidPeriod : (DateTime, DateTime)
 *   - HalloweenEventYear : uint
 * @cheats
 *   /giveitem 23545 20
 */

#load "SeasonalEvents.csx"
#load "DropRate.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => (QuestId)60301052;
    public override ushort RecommendedLevel => 10;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageId StageId => StageId.WhiteDragonTemple;

    public override bool AcceptRequirementsMet(DdonGameServer server, GameClient client)
    {
        return SeasonalEvents.CheckConfigSettings(server, "EnableHalloweenEvent", "HalloweenEventYear", 2018, "HalloweenValidPeriod");
    }

    protected override void InitializeRewards()
    {
        AddFixedItemReward(23545, 50); // Halloween Petit Muffin
        AddFixedItemReward(23397, 1);  // Mischievous Ghost Costume
        AddPointReward(PointType.ExperiencePoints, 1031);
        AddWalletReward(WalletType.Gold, 1031);
        AddWalletReward(WalletType.RiftPoints, 1031);
    }

    protected override void InitializeEnemyGroups()
    {
        DropsTable drops = new DropsTable();
        drops.Items.Add(new GatheringItem()
        {
            ItemId = 23545,
            ItemNum = 1,
            MaxItemNum = 1,
            DropChance = DropRate.ALWAYS,
        });

        AddEnemies(0, new StageId(662, 0, 6), 0, QuestEnemyPlacementType.Manual, new List<InstancedEnemy>()
        {
            LibDdon.CreateEnemy(0x015622, 10, 136, 0) // "Grudge Ghost"
                .SetNamedEnemyParams(LibDdon.GetNamedParam(2620))
                .SetDropsTable(drops),
            LibDdon.CreateEnemy(0x015622, 10, 136, 1) // "Grudge Ghost"
                .SetNamedEnemyParams(LibDdon.GetNamedParam(2620))
                .SetDropsTable(drops),
            LibDdon.CreateEnemy(0x015622, 10, 136, 2) // "Grudge Ghost"
                .SetNamedEnemyParams(LibDdon.GetNamedParam(2620))
                .SetDropsTable(drops),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = new QuestProcess(0);
        process0.AddNewNpcTalkAndOrderBlock(new StageId(2, 1, 1), NpcId.Angelo0, 30811)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8230)
                .AddAnnotation("Spawns Shelly and Angelo");
        process0.AddCheckBagEventBlock(QuestAnnounceType.Accept, 23545, 1);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, new StageId(2, 1, 1), NpcId.Angelo0, 30813);
        process0.AddNewDeliverItemsBlock(QuestAnnounceType.CheckpointAndUpdate, new StageId(2, 1, 1), NpcId.Angelo0, 23545, 20, 30818);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, StageId.WhiteDragonTemple, NpcId.Fabio0, 30821);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, new StageId(2, 1, 1), NpcId.Angelo0, 30822);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, new StageId(2, 1, 1), NpcId.Angelo0, 30823);
        process0.AddStageJumpBlock(QuestAnnounceType.None, new StageId(662, 0, 0), 0);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, new StageId(662, 0, 0), NpcId.Angelo0, 30824)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8231)
                .AddAnnotation("Spawns Angelo");
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, 0, true);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, 0, false);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, new StageId(662, 0, 1), NpcId.Angelo0, 30826)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 8231)
                .AddAnnotation("Despawns Angelo")
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8294)
                .AddAnnotation("Spawns Angelo");
        process0.AddStageJumpBlock(QuestAnnounceType.None, StageId.WhiteDragonTemple, 0);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, new StageId(2, 1, 1), NpcId.Angelo0, 30827);
        process0.AddProcessEndBlock(true);
        AddProcess(process0);
    }
}

return new ScriptedQuest();
