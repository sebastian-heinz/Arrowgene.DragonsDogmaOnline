/**
 * @brief The Darkness of Halloween (Halloween Seasonal Event 2017)
 * @settings scripts/settings/SeasonalEventSettings.csx
 *   - EnableHalloweenEvent : bool
 *   - HalloweenValidPeriod : (DateTime, DateTime)
 *   - HalloweenEventYear : uint
 * @cheats
 *   /giveitem 21182 5
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => (QuestId)60301000;
    public override ushort RecommendedLevel => 10;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.TheWhiteDragonTemple0;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.CollaborationOrSeasonalQuest;

    private static class NamedParamId
    {
        public const uint CandyLovingWitch = 2108; // Candy Loving Witch
    }

    public override bool AcceptRequirementsMet(GameClient client)
    {
        return SeasonalEvents.CheckConfigSettings("EnableHalloweenEvent", "HalloweenEventYear", 2017, "HalloweenValidPeriod");
    }

    protected override void InitializeRewards()
    {
        AddFixedItemReward(ItemId.GhostMarshmallow, 100);
        AddFixedItemReward(ItemId.MagicalNightJackOHelm, 1);
        AddPointReward(PointType.ExperiencePoints, 1031);
        AddWalletReward(WalletType.Gold, 1031);
        AddWalletReward(WalletType.RiftPoints, 1031);
    }

    protected override void InitializeEnemyGroups()
    {
        DropsTable slimeTable = new DropsTable()
            .AddDrop(ItemId.GhostMarshmallow, 1, 1, DropRate.UNCOMMON);

        AddEnemies(0, Stage.TelsWitheredWell, 0, 0, QuestEnemyPlacementType.Manual, new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.Slime, 3, 24, 0, false)
                .SetDropsTable(slimeTable),
            LibDdon.Enemy.Create(EnemyId.Slime, 3, 24, 1, false)
                .SetDropsTable(slimeTable),
            LibDdon.Enemy.Create(EnemyId.Slime, 3, 24, 2, false)
                .SetDropsTable(slimeTable),
            LibDdon.Enemy.Create(EnemyId.Slime, 3, 24, 3, false)
                .SetDropsTable(slimeTable),
            LibDdon.Enemy.Create(EnemyId.Slime, 3, 24, 4, false)
                .SetDropsTable(slimeTable),
            LibDdon.Enemy.Create(EnemyId.Slime, 3, 24, 5, false)
                .SetDropsTable(slimeTable),
            LibDdon.Enemy.Create(EnemyId.Slime, 3, 24, 6, false)
                .SetDropsTable(slimeTable),
            LibDdon.Enemy.Create(EnemyId.Slime, 3, 24, 8, false)
                .SetDropsTable(slimeTable),
        });

        AddEnemies(1, Stage.TelsWitheredWell, 0, 0, QuestEnemyPlacementType.Manual, new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.Witch, 10, 1500, 7, false)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.CandyLovingWitch)
                .AddDrop(ItemId.GhostMarshmallow, 1, 10, DropRate.ALWAYS)
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddQuestNpcTalkAndOrderBlock((QuestId)60301001, Stage.TheWhiteDragonTemple0, 0, 0, NpcId.Angelo0, 27113);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Accept, (QuestId)60301001, Stage.TheWhiteDragonTemple0, 1, 0, NpcId.Shelly0, 27115);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, (QuestId)60301001, Stage.TheWhiteDragonTemple0, 0, 0, NpcId.Angelo0, 27116);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, NpcId.Gregory0, 27117);
        process0.AddCheckBagEventBlock(QuestAnnounceType.CheckpointAndUpdate, ItemId.GhostLantern, 1)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, 1)
                .AddAnnotation("Progresses the process 1 state machine to spawn enemies in the withered well");
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, (QuestId)60301001, Stage.TheWhiteDragonTemple0, 0, 0, NpcId.Angelo0, 27118)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, 2)
                .AddAnnotation("Stop spawning slimes in process 1");
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.Lestania, NpcId.Norman, 27142);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TelsWitheredWell, 0, 0, NpcId.Shelly0, 27121)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 6989)
                .AddAnnotation("Spawns fake Shelly");
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, 1, true)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 6989)
                .AddAnnotation("Despawns fake shelly");
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TelsWitheredWell, 1, 0, NpcId.Shelly0, 27124)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 6990)
                .AddAnnotation("Spawns Real Shelly");
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, (QuestId)60301001, Stage.TheWhiteDragonTemple0, 0, 0, NpcId.Angelo0, 27128);
        process0.AddProcessEndBlock(true);

        var process1 = AddNewProcess(1);
        process1.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCommand(QuestManager.CheckCommand.MyQstFlagOn(1));
        process1.AddSpawnGroupBlock(QuestAnnounceType.None, 0)
            .AddCheckCommand(QuestManager.CheckCommand.MyQstFlagOn(2));
        process1.AddProcessEndBlock(false);
        // process1.AddDestroyGroupBlock(QuestAnnounceType.None, 0);
    }
}

return new ScriptedQuest();
