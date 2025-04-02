/**
 * @brief Merry Christmas with Smiles 2 (2018)
 * @settings scripts/settings/SeasonalEventSettings.csx
 *   - EnableChristmasEvent : bool
 *   - ChristmasValidPeriod : (DateTime, DateTime)
 *   - ChristmasEventYear : uint
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => (QuestId)60301056;
    public override ushort RecommendedLevel => 10;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.TheWhiteDragonTemple0;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.CollaborationOrSeasonalQuest;

    private static class NamedParamId
    {
        public const uint AngryEntChieftan = 2622; // Angry Ent Chieftan
        public const uint FleetFooted = 2657; // Fleet-footed <named>
    }

    public override bool AcceptRequirementsMet(GameClient client)
    {
        return SeasonalEvents.CheckConfigSettings("EnableChristmasEvent", "ChristmasEventYear", 2018, "ChristmasValidPeriod");
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
        AddEnemies(0, Stage.Lestania, 71, 0, QuestEnemyPlacementType.Manual, new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.GreenGuardian, 10, 980, 7)
                .SetNamedEnemyParams(NamedParamId.FleetFooted),
            LibDdon.Enemy.Create(EnemyId.GreenGuardian, 10, 980, 8)
                .SetNamedEnemyParams(NamedParamId.FleetFooted),
            LibDdon.Enemy.Create(EnemyId.GreenGuardian, 10, 980, 9)
                .SetNamedEnemyParams(NamedParamId.FleetFooted),
            LibDdon.Enemy.Create(EnemyId.Ent, 10, 9800, 10)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.AngryEntChieftan),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddIsQuestClearBlock(QuestAnnounceType.None, QuestType.Tutorial, (QuestId)60301055);
        process0.AddNewNpcTalkAndOrderBlock(Stage.TheWhiteDragonTemple0, 1, 2, NpcId.Mia, 30858)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8246)
                .AddAnnotation("Spawns Event NPCs");
        process0.AddTalkToNpcBlock(QuestAnnounceType.Accept, Stage.TheWhiteDragonTemple0, NpcId.Cornelia0, 30860);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.PawnCathedral, NpcId.Alvar, 30862);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.CheckpointAndUpdate, 0, true);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, 0, false);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.PawnCathedral, NpcId.Alvar, 30864);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, 1, 2, NpcId.Mia, 30866);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, 1, 1, NpcId.Marco, 30867);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, 1, 0, NpcId.Nicholas, 30868);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
