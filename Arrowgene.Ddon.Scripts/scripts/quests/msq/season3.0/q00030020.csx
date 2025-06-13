/**
 * @brief In Search of Hope
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ScriptedQuest));

    public override QuestType QuestType => QuestType.Main;
    public override QuestId QuestId => QuestId.InSearchOfHope;
    public override ushort RecommendedLevel => 80;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.AudienceChamber;
    public override QuestId NextQuestId => QuestId.MeirovaTheVeteranGeneral;

    private class EnemyGroupId
    {
        public const uint Encounter = 10;
    }

    private class NamedParamId
    {
        public const uint PrisonersGuard = 1751; // Prisoner's Guard
    }

    private class MyQstFlag
    {
        public const uint NpcFsm0 = 2995; // All 3 NPCs
        public const uint NpcFsm1 = 3562; // 2 NPCs
    }

    private class QstLayoutFlag
    {
        public const uint AudienceChamberNpcs = 5378; // Spawns Gurdolin and Lise in the audience chamber 
        public const uint Quintus = 5409;
        public const uint BruceDolkeyEnnis = 6526;
        public const uint GillianAndSoldiers = 5769;
        public const uint Gillian = 5770;
        public const uint Cannons = 6602;
    }

    private class GeneralAnnouncement
    {
        public const int AreaMasterUnlocked = 100271;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumLevel(80));
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.TheLandOfDespair));
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
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.RathniteFoothills, 2, QuestEnemyPlacementType.Manual, new()
        {
            // Optional Enemies
            LibDdon.Enemy.CreateAuto(EnemyId.RangedSoldierDwarfOrc, 80, 0)
                .SetIsRequired(false)
                .SetStartThinkTblNo(1)
                .SetNamedEnemyParams(NamedParamId.PrisonersGuard),
            LibDdon.Enemy.CreateAuto(EnemyId.RangedSoldierDwarfOrc, 80, 1)
                .SetIsRequired(false)
                .SetStartThinkTblNo(1)
                .SetNamedEnemyParams(NamedParamId.PrisonersGuard),
            // Required Enemies
            LibDdon.Enemy.CreateAuto(EnemyId.SquadLeaderDwarfOrc, 80, 17)
                .SetStartThinkTblNo(1)
                .SetNamedEnemyParams(NamedParamId.PrisonersGuard),
            LibDdon.Enemy.CreateAuto(EnemyId.BluntSoldierDwarfOrc, 80, 2)
                .SetNamedEnemyParams(NamedParamId.PrisonersGuard),
            LibDdon.Enemy.CreateAuto(EnemyId.BluntSoldierDwarfOrc, 80, 3)
                .SetNamedEnemyParams(NamedParamId.PrisonersGuard),
            LibDdon.Enemy.CreateAuto(EnemyId.BluntSoldierDwarfOrc, 80, 4)
                .SetNamedEnemyParams(NamedParamId.PrisonersGuard),
            LibDdon.Enemy.CreateAuto(EnemyId.BluntSoldierDwarfOrc, 80, 5)
                .SetNamedEnemyParams(NamedParamId.PrisonersGuard),
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.RathniteFoothills, 2, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.SwordSoldierDwarfOrc, 80, 18)
                .SetNamedEnemyParams(NamedParamId.PrisonersGuard),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordSoldierDwarfOrc, 80, 19)
                .SetNamedEnemyParams(NamedParamId.PrisonersGuard),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordSoldierDwarfOrc, 80, 20)
                .SetNamedEnemyParams(NamedParamId.PrisonersGuard),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTalkAndOrderBlock(Stage.AudienceChamber, NpcId.Joseph, 21247)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.PiremothTravelersInn.Endale)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.AudienceChamber.TheCrewEndSeason34)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.RathniteFoothills.OrcCampSettlementNPCs)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.RathniteFoothills.OrcCampSettlementBuildings)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.RathniteFoothills.OrcCampSettlementFixWall)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.RathniteFoothills.OrcCampSettlementFlags)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.RathniteFoothills.OrcEncampmentDebrisMSQ)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.BruceDolkeyEnnis);
        process0.AddPartyGatherBlock(QuestAnnounceType.Accept, Stage.RathniteFoothills, 834, 4614, 1370)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 5409) // Spawns Quintus
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 6602) // Spawns Cannons
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.AudienceChamberNpcs); // Spawns Gudolin and List
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.RathniteFoothills, 15, 12);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.RathniteFoothills, 4, 0, NpcId.Gillian0, 21268)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, QstLayoutFlag.AudienceChamberNpcs)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 5379) // Spawn Gurdolin, Lise and Elliot
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.Gillian);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.PiremothTravelersInn, NpcId.Endale, 24159)
            .AddCheckCmdNewTalkNpcWithoutMarker(Stage.PiremothTravelersInn, 1, 0, QuestId.Q70030001)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.FortThines.Endale);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.RathniteFoothills, 3, 0, NpcId.Gillian0, 24161)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 5379) // Spawn Gurdolin, Lise and Elliot
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, QstLayoutFlag.Gillian)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.GillianAndSoldiers) // Spawn Gillian and Liberation Soliders
            .AddResultCmdReleaseAnnounce(ContentsRelease.None, flagInfo: QuestFlags.NpcFunctions.RathniteFoothillsAreaInfo) // Area Master Unlocked
            .AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, GeneralAnnouncement.AreaMasterUnlocked);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0) // Storm into the Demon Army encampment
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQstFlag.NpcFsm0);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0, resetGroup: false); // Defeat the enemy at the campsite
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.RathniteFoothills.OrcEncampmentInnerGate);
        process0.AddPartyGatherBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.RathniteFoothills, -26812, 2987, -38468);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.RathniteFoothills, 20, 8);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.RathniteFoothills, 22, 12);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.RathniteFoothills, 4, 0, NpcId.Gillian0, 21290)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, QstLayoutFlag.GillianAndSoldiers)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.Gillian);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.AudienceChamber, NpcId.Joseph, 21291);
        process0.AddIsStageNoBlock(QuestAnnounceType.None, Stage.AudienceChamber)
            .AddResultCmdReleaseAnnounce(ContentsRelease.RathniteFoothillsWorldQuests);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
