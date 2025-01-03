/**
 * @brief Merry Christmas with Smiles 2 (2018)
 * @settings scripts/settings/SeasonalEvents.csx
 *   - EnableChristmasEvent : bool
 *   - ChristmasValidPeriod : (DateTime, DateTime)
 *   - ChristmasEventYear : uint
 */

#load "SeasonalEvents.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType     => QuestType.Tutorial;
    public override QuestId QuestId         => (QuestId)60301056;
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
        AddFixedItemReward(ItemId.SparklingSnowLargeCrystal, 70);
        AddFixedItemReward(ItemId.ChristmasPuppetMandragora, 1);
        AddPointReward(PointType.ExperiencePoints, 1225);
        AddWalletReward(WalletType.Gold, 1225);
        AddWalletReward(WalletType.RiftPoints, 1225);
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(0, new StageId(1, 0, 71), 0, QuestEnemyPlacementType.Manual, new List<InstancedEnemy>()
        {
            LibDdon.CreateEnemy(EnemyId.GreenGuardian, 10, 980, 7)
                .SetNamedEnemyParams(LibDdon.GetNamedParam(2657)),
            LibDdon.CreateEnemy(EnemyId.GreenGuardian, 10, 980, 8)
                .SetNamedEnemyParams(LibDdon.GetNamedParam(2657)),
            LibDdon.CreateEnemy(EnemyId.GreenGuardian, 10, 980, 9)
                .SetNamedEnemyParams(LibDdon.GetNamedParam(2657)),
            LibDdon.CreateEnemy(EnemyId.Ent, 10, 9800, 10)
                .SetIsBoss(true)
                .SetNamedEnemyParams(LibDdon.GetNamedParam(2622)), // "Angry Ent Chieftan"
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = new QuestProcess(0);
        process0.AddIsQuestClearBlock(QuestAnnounceType.None, QuestType.Tutorial, (QuestId)60301055);
        process0.AddNewNpcTalkAndOrderBlock(new StageId(2, 2, 1), NpcId.Mia, 30858)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8246)
                .AddAnnotation("Spawns Event NPCs");
        process0.AddTalkToNpcBlock(QuestAnnounceType.Accept, StageId.WhiteDragonTemple, NpcId.Cornelia0, 30860);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, StageId.PawnCathedral, NpcId.Alvar, 30862);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.CheckpointAndUpdate, 0, true);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, 0, false);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, StageId.PawnCathedral, NpcId.Alvar, 30864);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, new StageId(2, 2, 1), NpcId.Mia, 30866);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, new StageId(2, 1, 1), NpcId.Marco, 30867);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, new StageId(2, 0, 1), NpcId.Nicholas, 30868);
        process0.AddProcessEndBlock(true);
        AddProcess(process0);
    }
}

return new ScriptedQuest();
