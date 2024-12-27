/**
 * @brief The Darkness of Halloween (Halloween Seasonal Event 2017)
 * @settings scripts/settings/SeasonalEvents.csx
 *   - EnableHalloweenEvent : bool
 *   - HalloweenValidPeriod : (DateTime, DateTime)
 *   - HalloweenEventYear : uint
 * @cheats
 *   /giveitem 21182 5
 */

#load "SeasonalEvents.csx"
#load "DropRate.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => (QuestId)60301000;
    public override ushort RecommendedLevel => 10;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageId StageId => StageId.WhiteDragonTemple;

    public override bool AcceptRequirementsMet(DdonGameServer server, GameClient client)
    {
        return SeasonalEvents.CheckConfigSettings(server, "EnableHalloweenEvent", "HalloweenEventYear", 2017, "HalloweenValidPeriod");
    }

    protected override void InitializeRewards()
    {
        AddFixedItemReward(21182, 100); // Ghost Marshmallow
        AddFixedItemReward(19500, 1);  // Magical Night Jack-o'-Helm
        AddPointReward(PointType.ExperiencePoints, 1031);
        AddWalletReward(WalletType.Gold, 1031);
        AddWalletReward(WalletType.RiftPoints, 1031);
    }

    protected override void InitializeEnemyGroups()
    {
        DropsTable slimeTable = new DropsTable();
        slimeTable.Items.Add(new GatheringItem()
        {
            ItemId = 21182,
            ItemNum = 1,
            MaxItemNum = 1,
            DropChance = DropRate.UNCOMMON,
        });

        AddEnemies(0, new StageId(126, 0, 0), 0, QuestEnemyPlacementType.Manual, new List<InstancedEnemy>()
        {
            LibDdon.CreateEnemy(0x010900, 3, 24, 0, false) // Slime
                .SetDropsTable(slimeTable),
            LibDdon.CreateEnemy(0x010900, 3, 24, 1, false) // Slime
                .SetDropsTable(slimeTable),
            LibDdon.CreateEnemy(0x010900, 3, 24, 2, false) // Slime
                .SetDropsTable(slimeTable),
            LibDdon.CreateEnemy(0x010900, 3, 24, 3, false) // Slime
                .SetDropsTable(slimeTable),
            LibDdon.CreateEnemy(0x010900, 3, 24, 4, false) // Slime
                .SetDropsTable(slimeTable),
            LibDdon.CreateEnemy(0x010900, 3, 24, 5, false) // Slime
                .SetDropsTable(slimeTable),
            LibDdon.CreateEnemy(0x010900, 3, 24, 6, false) // Slime
                .SetDropsTable(slimeTable),
            LibDdon.CreateEnemy(0x010900, 3, 24, 8, false) // Slime
                .SetDropsTable(slimeTable),
        });

        AddEnemies(1, new StageId(126, 0, 0), 0, QuestEnemyPlacementType.Manual, new List<InstancedEnemy>()
        {
            LibDdon.CreateEnemy(0x015604, 10, 1500, 7, false) // Candy Loving Witch
                .SetIsBoss(true)
                .SetNamedEnemyParams(LibDdon.GetNamedParam(2108))
                .AddDrop(21182, 1, 10, DropRate.ALWAYS)
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = new QuestProcess(0);
        process0.AddQuestNpcTalkAndOrderBlock((QuestId)60301001, new StageId(2, 0, 0), NpcId.Angelo0, 27113);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Accept, (QuestId)60301001, new StageId(2, 0, 1), NpcId.Shelly0, 27115);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, (QuestId)60301001, new StageId(2, 0, 0), NpcId.Angelo0, 27116);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, StageId.WhiteDragonTemple, NpcId.Gregory0, 27117);
        process0.AddCheckBagEventBlock(QuestAnnounceType.CheckpointAndUpdate, 20435, 1)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, 1)
                .AddAnnotation("Progresses the process 1 state machine to spawn enemies in the withered well");
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, (QuestId)60301001, new StageId(2, 0, 0), NpcId.Angelo0, 27118)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, 2)
                .AddAnnotation("Stop spawning slimes in process 1");
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, StageId.Lestania, NpcId.Norman, 27142);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, new StageId(126, 0, 0), NpcId.Shelly0, 27121)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 6989)
                .AddAnnotation("Spawns fake Shelly");
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, 1, true)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 6989)
                .AddAnnotation("Despawns fake shelly");
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, new StageId(126, 0, 1), NpcId.Shelly0, 27124)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 6990)
                .AddAnnotation("Spawns Real Shelly");
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, (QuestId)60301001, new StageId(2, 0, 0), NpcId.Angelo0, 27128);
        process0.AddProcessEndBlock(true);
        AddProcess(process0);

        var process1 = new QuestProcess(1);
        process1.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCommand(QuestManager.CheckCommand.MyQstFlagOn(1));
        process1.AddSpawnGroupBlock(QuestAnnounceType.None, 0)
            .AddCheckCommand(QuestManager.CheckCommand.MyQstFlagOn(2));
        process1.AddProcessEndBlock(false);
        // process1.AddDestroyGroupBlock(QuestAnnounceType.None, 0);
        AddProcess(process1);
    }
}

return new ScriptedQuest();
