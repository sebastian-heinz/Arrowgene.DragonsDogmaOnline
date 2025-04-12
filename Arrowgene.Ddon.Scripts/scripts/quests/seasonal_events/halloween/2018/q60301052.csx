/**
 * @brief Emergency! Not Enough Candy! (1) (Halloween Seasonal Event 2018)
 * @settings scripts/settings/SeasonalEventSettings.csx
 *   - EnableHalloweenEvent : bool
 *   - HalloweenValidPeriod : (DateTime, DateTime)
 *   - HalloweenEventYear : uint
 * @cheats
 *   /giveitem 23545 20
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => (QuestId)60301052;
    public override ushort RecommendedLevel => 10;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.TheWhiteDragonTemple0;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.CollaborationOrSeasonalQuest;

    private static class NamedParamId
    {
        public const uint SweetTooth = 2620; // Sweet Tooth
    }

    public override bool AcceptRequirementsMet(GameClient client)
    {
        return SeasonalEvents.CheckConfigSettings("EnableHalloweenEvent", "HalloweenEventYear", 2018, "HalloweenValidPeriod");
    }

    protected override void InitializeRewards()
    {
        AddFixedItemReward(ItemId.HalloweenPetitMuffin, 50);
        AddFixedItemReward(ItemId.MischievousGhostCostume, 1);
        AddPointReward(PointType.ExperiencePoints, 1031);
        AddWalletReward(WalletType.Gold, 1031);
        AddWalletReward(WalletType.RiftPoints, 1031);
    }

    protected override void InitializeEnemyGroups()
    {
        DropsTable drops = new DropsTable()
            .AddDrop(ItemId.HalloweenPetitMuffin, 1, 1, DropRate.ALWAYS);

        AddEnemies(0, Stage.HidellCatacombs1, 6, 0, QuestEnemyPlacementType.Manual, new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.GrudgeGhost, 10, 136, 0)
                .SetNamedEnemyParams(NamedParamId.SweetTooth)
                .SetDropsTable(drops),
            LibDdon.Enemy.Create(EnemyId.GrudgeGhost, 10, 136, 1)
                .SetNamedEnemyParams(NamedParamId.SweetTooth)
                .SetDropsTable(drops),
            LibDdon.Enemy.Create(EnemyId.GrudgeGhost, 10, 136, 2)
                .SetNamedEnemyParams(NamedParamId.SweetTooth)
                .SetDropsTable(drops),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNewNpcTalkAndOrderBlock(Stage.TheWhiteDragonTemple0, 1, 1, NpcId.Angelo0, 30811)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8230)
                .AddAnnotation("Spawns Shelly and Angelo");
        process0.AddCheckBagEventBlock(QuestAnnounceType.Accept, ItemId.HalloweenPetitMuffin, 1);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, 1, 1, NpcId.Angelo0, 30813);
        process0.AddNewDeliverItemsBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, 1, 1, NpcId.Angelo0, ItemId.HalloweenPetitMuffin, 20, 30818);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, NpcId.Fabio0, 30821);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, 1, 1, NpcId.Angelo0, 30822);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, 1, 1, NpcId.Angelo0, 30823);
        process0.AddStageJumpBlock(QuestAnnounceType.None, Stage.HidellCatacombs1, 0);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.HidellCatacombs1, 0, 0, NpcId.Angelo0, 30824)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8231)
                .AddAnnotation("Spawns Angelo");
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, 0, true);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, 0, false);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.HidellCatacombs1, 1, 0, NpcId.Angelo0, 30826)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 8231)
                .AddAnnotation("Despawns Angelo")
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8294)
                .AddAnnotation("Spawns Angelo");
        process0.AddStageJumpBlock(QuestAnnounceType.None, Stage.TheWhiteDragonTemple0, 0);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, 1, 1, NpcId.Angelo0, 30827);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
