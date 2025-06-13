/**
 * @brief Diversionary Tactics
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ScriptedQuest));

    public override QuestType QuestType => QuestType.Main;
    public override QuestId QuestId => QuestId.DiversionaryTactics;
    public override ushort RecommendedLevel => 87;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.AudienceChamber;
    public override QuestId NextQuestId => QuestId.TheSecretEntrance;

    private class EnemyGroupId
    {
        public const uint Encounter = 10;
    }

    private class NamedParamId
    {
        public const uint JifuleCastleGatekeeper0 = 1765; // Jifule Castle Gatekeeper
        public const uint JifuleCastleGatekeeper1 = 1766; // Jifule Castle Gatekeeper
        public const uint JifuleCastleGatekeeper2 = 1767; // Jifule Castle Gatekeeper
    }

    private class QstLayoutFlag
    {
        // st0132 (Feryana Wilderness)
        public const uint FeryanaWildernessNpcs1 = 5593; // Gillian, Gurdolin, Lise, Elliot
        public const uint FeryanaWildernessNpcs0 = 5984; // Gillian, Gurdolin, Lise, Elliot

        // st0443 (Fort Thines)
        public const uint FortThinesNpcs0 = 5576; // Gurdolin, Elliot, Lise
        public const uint FortThinesNpcs1 = 5578; // Meirova, Nedo, Quintus
        public const uint FortThinesGillian = 5577;
        public const uint FortThinesBertha = 5579;
        public const uint FortThinesGerhard = 6518;

        // st0631 (Rothgill Traveler's Inn)
        public const uint RothgillTravelersInnNpcs = 5591; // Gurdolin, Lise, Elliot, Gillian
        public const uint RothgillTravelersInnGerhard = 5580; // Gerhard
    }

    private class MyQstFlag
    {
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumLevel(87));
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.PortentOfDespair));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 700000);
        AddWalletReward(WalletType.Gold, 80000);
        AddWalletReward(WalletType.RiftPoints, 8000);

        AddFixedItemReward(ItemId.UnappraisedFlightTrinketGeneral, 2);
        AddFixedItemReward(ItemId.RoyalCrestMedalFeryanaDistrict, 5);
        AddFixedItemReward(ItemId.ApFeryanaWilderness, 50);
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.FeryanaWilderness, 66, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.SquadLeaderDwarfOrc, 87, 5)
                .SetNamedEnemyParams(NamedParamId.JifuleCastleGatekeeper0),
            LibDdon.Enemy.CreateAuto(EnemyId.HeavySoldierDwarfOrc, 87, 4)
                .SetNamedEnemyParams(NamedParamId.JifuleCastleGatekeeper0),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordSoldierDwarfOrc, 87, 8)
                .SetNamedEnemyParams(NamedParamId.JifuleCastleGatekeeper0),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordSoldierDwarfOrc, 87, 16)
                .SetNamedEnemyParams(NamedParamId.JifuleCastleGatekeeper0),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordSoldierDwarfOrc, 87, 15)
                .SetNamedEnemyParams(NamedParamId.JifuleCastleGatekeeper0),
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.FeryanaWilderness, 2, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.SkeletonCyclops, 87, 4, isBoss: true)
                .SetNamedEnemyParams(NamedParamId.JifuleCastleGatekeeper1),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordSoldierDwarfOrc, 87, 5)
                .SetNamedEnemyParams(NamedParamId.JifuleCastleGatekeeper1),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordSoldierDwarfOrc, 87, 6)
                .SetNamedEnemyParams(NamedParamId.JifuleCastleGatekeeper1),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordSoldierDwarfOrc, 87, 7)
                .SetNamedEnemyParams(NamedParamId.JifuleCastleGatekeeper1),
            LibDdon.Enemy.CreateAuto(EnemyId.SkeletonWarg, 87, 8)
                .SetNamedEnemyParams(NamedParamId.JifuleCastleGatekeeper1),
            LibDdon.Enemy.CreateAuto(EnemyId.SkeletonWarg, 87, 9)
                .SetNamedEnemyParams(NamedParamId.JifuleCastleGatekeeper1),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTalkAndOrderBlock(Stage.AudienceChamber, NpcId.Joseph, 21767)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.MephiteTravelersInn.Nayajiku)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.AudienceChamber.TheCrewEndSeason34);
        process0.AddPartyGatherBlock(QuestAnnounceType.Accept, Stage.FortThines1, 16, 350, -3680)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.FortThinesNpcs0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.FortThinesNpcs1)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.FortThinesBertha)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.FortThinesGillian)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.FortThinesGerhard);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.FortThines1, 0, 0, QuestJumpType.None, Stage.Invalid);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.FortThines1, 2, 1, NpcId.Nedo0, 21790)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, QstLayoutFlag.FortThinesGerhard);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.RothgillTravelersInn, 1, 0, NpcId.Gerhard, 21791)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.RothgillTravelersInnNpcs)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.RothgillTravelersInnGerhard);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.RothgillTravelersInn, 0, 3, NpcId.Gillian0, 21792);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.FeryanaWilderness, 1, 0, NpcId.Gillian0, 24196)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, QstLayoutFlag.FortThinesGillian)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.FeryanaWildernessNpcs0);
        process0.AddDestroyGroupBlock(QuestAnnounceType.CheckpointAndUpdate, EnemyGroupId.Encounter + 0)
            .AddResultCmdPlayCameraEvent(Stage.FeryanaWilderness, 90);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.CheckpointAndUpdate, EnemyGroupId.Encounter + 1)
            .AddResultCmdReleaseAnnounce(ContentsRelease.None, flagInfo: QuestFlags.FeryanaWilderness.WestFeryanaGate);
        process0.AddDestroyGroupBlock(QuestAnnounceType.CheckpointAndUpdate, EnemyGroupId.Encounter + 1, resetGroup: false);
        process0.AddPartyGatherBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.FeryanaWilderness, -166981, 15560, -112741);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.FeryanaWilderness, 10, 0, QuestJumpType.None, Stage.Invalid)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, QstLayoutFlag.FeryanaWildernessNpcs0);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.FeryanaWilderness, 0, 0, NpcId.Gillian0, 21802)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.FeryanaWildernessNpcs1);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.AudienceChamber, NpcId.Joseph, 21803);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
