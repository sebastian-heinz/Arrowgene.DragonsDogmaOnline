/**
 * @brief The Approaching Demon Army
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ScriptedQuest));

    public override QuestType QuestType => QuestType.Main;
    public override QuestId QuestId => QuestId.TheApproachingDemonArmy;
    public override ushort RecommendedLevel => 85;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.AudienceChamber;
    public override QuestId NextQuestId => QuestId.PortentOfDespair;

    private class EnemyGroupId
    {
        public const uint Encounter = 10;
    }

    private class NamedParamId
    {
        public const uint Raid0 = 1615; // Raid <name>
        public const uint Raid1 = 1616; // Raid <name>
        public const uint Raid2 = 1761; // Raid <name>
        public const uint Raid3 = 1762; // Raid <name>
    }

    private class QstLayoutFlag
    {
        // st0443
        public const uint Quintus = 5559; // Quintus
        public const uint FortThinesNpcs = 5563; // Meirova, Gurdolin, Elliot, Lise, Nedo, Gillian

        // st0444
        public const uint Npcs0 = 5561; // Nedo, Gurdolin, Lise, Elliot, Meirova
        public const uint Npcs1 = 7209; // Liberation Army Soldier
        public const uint Blockade = 5562;
    }

    private class MyQstFlag
    {
        public const uint NpcFSM1 = 3062;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumLevel(85));
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.TheRoyalFamilySacrament));
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
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.FortThines2, 1, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.BluntSoldierDwarfOrc, 85, 0)
                .SetNamedEnemyParams(NamedParamId.Raid0),
            LibDdon.Enemy.CreateAuto(EnemyId.BluntSoldierDwarfOrc, 85, 1)
                .SetNamedEnemyParams(NamedParamId.Raid0),
            LibDdon.Enemy.CreateAuto(EnemyId.BluntSoldierDwarfOrc, 85, 2)
                .SetNamedEnemyParams(NamedParamId.Raid0),
            LibDdon.Enemy.CreateAuto(EnemyId.SquadLeaderDwarfOrc, 85, 3)
                .SetNamedEnemyParams(NamedParamId.Raid0),
            LibDdon.Enemy.CreateAuto(EnemyId.RangedSoldierDwarfOrc, 85, 4)
                .SetNamedEnemyParams(NamedParamId.Raid0),
            LibDdon.Enemy.CreateAuto(EnemyId.BluntSoldierDwarfOrc, 85, 5)
                .SetNamedEnemyParams(NamedParamId.Raid0),
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.FortThines2, 1, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.WarReadyOgreLightArmor, 85, 6, isBoss: true)
                .SetInfectionType(1)
                .SetNamedEnemyParams(NamedParamId.Raid1),
            LibDdon.Enemy.CreateAuto(EnemyId.RangedSoldierDwarfOrc, 85, 7)
                .SetNamedEnemyParams(NamedParamId.Raid1),
            LibDdon.Enemy.CreateAuto(EnemyId.RangedSoldierDwarfOrc, 85, 8)
                .SetNamedEnemyParams(NamedParamId.Raid1),
            LibDdon.Enemy.CreateAuto(EnemyId.RangedSoldierDwarfOrc, 85, 9)
                .SetNamedEnemyParams(NamedParamId.Raid1),
            LibDdon.Enemy.CreateAuto(EnemyId.RangedSoldierDwarfOrc, 85, 10)
                .SetNamedEnemyParams(NamedParamId.Raid1),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTalkAndOrderBlock(Stage.AudienceChamber, NpcId.Joseph, 21707)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.MephiteTravelersInn.Nayajiku)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.AudienceChamber.TheCrewEndSeason34);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Accept, Stage.FortThines1, 0, 0, NpcId.Quintus, 21708)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.Quintus);
        process0.AddPartyGatherBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.FortThines1, 11, 350, -2659);
        process0.AddStageJumpBlock(QuestAnnounceType.None, Stage.FortThines2, 4);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.FortThines2, 0, 0, NpcId.Nedo0, 21709)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.Npcs0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.Npcs1);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.FortThines2, 0, 0, QuestJumpType.None, Stage.Invalid);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.Blockade);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.FortThines2, 5, 0, QuestJumpType.After, Stage.FortThines1);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.FortThines1, 1, 0, NpcId.Meirova0, 21726)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.FortThinesNpcs);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.AudienceChamber, NpcId.Joseph, 21727);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
