/**
 * @brief Survivors Village
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ScriptedQuest));

    public override QuestType QuestType => QuestType.Main;
    public override QuestId QuestId => QuestId.SurvivorsVillage;
    public override ushort RecommendedLevel => 82;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.AudienceChamber;
    public override QuestId NextQuestId => QuestId.TheOpposition;

    private class EnemyGroupId
    {
        public const uint Encounter = 10;
    }

    private class NamedParamId
    {
        public const uint Gatekeeper = 1865; // Gatekeeper <name>
        public const uint OrcAimingForRothgill = 1755;
        public const uint ManticoreAimingForRothgill = 1899;
    }

    private class MyQstFlag
    {
        // public const uint = ;
    }

    private class QstLayoutFlag
    {
        public const uint CampNPCs0 = 5395; // Fort Thines (Meirova, Gillian, Quintus, Gurdolin, Lise, Elliot)
        public const uint CampNPCs1 = 5399; // st0131
        public const uint CampNPCs2 = 5401; // st0131
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumLevel(82));
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.ThePrincesWhereabouts));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 700000);
        AddWalletReward(WalletType.Gold, 70000);
        AddWalletReward(WalletType.RiftPoints, 7000);

        AddFixedItemReward(ItemId.UnappraisedSnowTrinketGeneral, 2);
        AddFixedItemReward(ItemId.RoyalCrestMedalRathniteDistrict, 5);
        AddFixedItemReward(ItemId.ApRathniteFoothills, 100);
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.RathniteFoothills, 4, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.SquadLeaderDwarfOrc, 82, 5)
                .SetNamedEnemyParams(NamedParamId.Gatekeeper),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordSoldierDwarfOrc, 82, 1)
                .SetNamedEnemyParams(NamedParamId.Gatekeeper),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordSoldierDwarfOrc, 82, 2)
                .SetNamedEnemyParams(NamedParamId.Gatekeeper),
            LibDdon.Enemy.CreateAuto(EnemyId.RangedSoldierDwarfOrc, 82, 3)
                .SetNamedEnemyParams(NamedParamId.Gatekeeper),
            LibDdon.Enemy.CreateAuto(EnemyId.RangedSoldierDwarfOrc, 82, 4)
                .SetNamedEnemyParams(NamedParamId.Gatekeeper),
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.RathniteFoothillsLakeside0, 17, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.WarReadyGoremanticoreLightArmor, 82, 1, isBoss: true)
                .SetInfectionType(1)
                .SetNamedEnemyParams(NamedParamId.ManticoreAimingForRothgill),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordSoldierDwarfOrc, 82, 2)
                .SetNamedEnemyParams(NamedParamId.OrcAimingForRothgill),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordSoldierDwarfOrc, 82, 0)
                .SetNamedEnemyParams(NamedParamId.OrcAimingForRothgill),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTalkAndOrderBlock(Stage.AudienceChamber, NpcId.Joseph, 21441)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.RathniteFoothills.FortThinesFixWall)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.RathniteFoothills.FortThinesNpcs)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.RathniteFoothills.FortThinesBuildings)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.RathniteFoothills.FortThines0)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.RathniteFoothills.FortThines1)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.AudienceChamber.TheCrewEndSeason34);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Accept, Stage.FortThines1, 0, 0, NpcId.Meirova0, 21442)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.CampNPCs0)
            .AddResultCmdReleaseAnnounce(ContentsRelease.None, flagInfo: QuestFlags.RathniteFoothills.FortThinesGate);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.CheckpointAndUpdate, EnemyGroupId.Encounter + 0);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0, resetGroup: false);
        process0.AddPartyGatherBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.RathniteFoothillsLakeside0, -93878, 7935, -39097);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.RathniteFoothillsLakeside0, 0, 14);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.RathniteFoothillsLakeside0, 0, 0, NpcId.Meirova0, 21463)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, QstLayoutFlag.CampNPCs0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.CampNPCs1);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.CheckpointAndUpdate, EnemyGroupId.Encounter + 1);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1, resetGroup: false);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.RathniteFoothillsLakeside0, 1, 0, NpcId.Gerhard, 21464)
            .SetBgmStop(true)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, QstLayoutFlag.CampNPCs1)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.CampNPCs2)
            .AddResultCmdReleaseAnnounce(ContentsRelease.None, flagInfo: QuestFlags.RathniteFoothillsLakeside.RothgillFrontGate);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.RathniteFoothillsLakeside0, 1, 1, NpcId.Meirova0, 21465);
        process0.AddRawBlock(QuestAnnounceType.CheckpointAndUpdate)
            .AddCheckCmdIsReleaseWarpPointAnyone(70);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.AudienceChamber, NpcId.Joseph, 21466);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
