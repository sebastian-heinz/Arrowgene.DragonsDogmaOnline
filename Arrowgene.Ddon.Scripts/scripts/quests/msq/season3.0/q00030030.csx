/**
 * @brief Meirova The Veteran General
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ScriptedQuest));

    public override QuestType QuestType => QuestType.Main;
    public override QuestId QuestId => QuestId.MeirovaTheVeteranGeneral;
    public override ushort RecommendedLevel => 80;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.AudienceChamber;
    public override QuestId NextQuestId => QuestId.ThePrincesWhereabouts;

    private class EnemyGroupId
    {
        public const uint Encounter = 10;
    }

    private class NamedParamId
    {
        public const uint VillageRaiding = 1752; // Village Raiding <name>
    }

    private class QstLayoutFlag
    {
        public const uint CampNPCs0 = 5381; // Gillian, Quintus, Gurdolin, Lise, Elliot
        public const uint CampNPCs1 = 5384; // Gillian, Quintus, Gurdolin, Lise, Elliot
        public const uint GillianMeirova0 = 5385; // In Combat
        public const uint GillianMeirova1 = 5386; // After Combat
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumLevel(80));
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.InSearchOfHope));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 700000);
        AddWalletReward(WalletType.Gold, 70000);
        AddWalletReward(WalletType.RiftPoints, 7000);

        AddFixedItemReward(ItemId.UnappraisedSnowTrinketGeneral, 2);
        AddFixedItemReward(ItemId.RoyalCrestMedalRathniteDistrict, 5);
        AddFixedItemReward(ItemId.ApRathniteFoothills, 50);
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.RathniteFoothills, 8, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.WarReadyGrimwargLightArmor, 80, 0)
                .SetInfectionType(1)
                .SetNamedEnemyParams(NamedParamId.VillageRaiding),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordSoldierDwarfOrc, 80, 1)
                .SetNamedEnemyParams(NamedParamId.VillageRaiding),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordSoldierDwarfOrc, 80, 2)
                .SetNamedEnemyParams(NamedParamId.VillageRaiding),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordSoldierDwarfOrc, 80, 3)
                .SetNamedEnemyParams(NamedParamId.VillageRaiding),
        });

        // Create an empty group around the NPCs for the quest
        AddEnemies(EnemyGroupId.Encounter + 1, Stage.RathniteFoothills, 38, QuestEnemyPlacementType.Manual, new()
        {
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTalkAndOrderBlock(Stage.AudienceChamber, NpcId.Joseph, 21292)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.AudienceChamber.TheCrewEndSeason34)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.RathniteFoothills.OrcCampSettlementBuildings)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.RathniteFoothills.OrcCampSettlementNPCs)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.RathniteFoothills.OrcCampSettlementFixWall)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.RathniteFoothills.OrcCampSettlementFlags)
            .AddResultCmdReleaseAnnounce(ContentsRelease.None, flagInfo: QuestFlags.RathniteFoothills.OrcEncampmentGate);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Accept, Stage.RathniteFoothills, 0, 0, NpcId.Gillian0, 21293)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.CampNPCs0);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.RathniteFoothills, 1, 0, NpcId.Gillian0, 21294)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, QstLayoutFlag.CampNPCs0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.CampNPCs1);
        process0.AddPartyGatherBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.RathniteFoothills, -2974, 5010, -39111);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.RathniteFoothills, 25, 9);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, QstLayoutFlag.CampNPCs1)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.GillianMeirova0);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.RathniteFoothills, 30, 10);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.RathniteFoothills, 3, 0, NpcId.Meirova0, 21411)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, QstLayoutFlag.GillianMeirova0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.GillianMeirova1);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.AudienceChamber, NpcId.Joseph, 21412);
        process0.AddProcessEndBlock(true);

        var process1 = AddNewProcess(1);
        process1.AddSpawnGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter + 1);
        process1.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
