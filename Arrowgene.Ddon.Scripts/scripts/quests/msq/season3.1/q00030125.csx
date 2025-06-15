/**
 * @brief A Desperate Infiltration
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ScriptedQuest));

    public override QuestType QuestType => QuestType.Main;
    public override QuestId QuestId => QuestId.ADesperateInfiltration;
    public override ushort RecommendedLevel => 87;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.AudienceChamber;
    public override QuestId NextQuestId => QuestId.TheBattleOfLookoutCastle;

    private class EnemyGroupId
    {
        public const uint Encounter = 10;
    }

    private class NamedParamId
    {
        public const uint PrisonersGuard0 = 1751; // Prisoner's Guard
        public const uint PrisonersGuard1 = 1771; // Prisoner's Guard
        public const uint LookoutCastleGuardTroop0 = 1774; // Lookout Castle Guard Troop
        public const uint LookoutCastleGuardTroop1 = 1775; // Lookout Castle Guard Troop
    }

    private class QstLayoutFlag
    {
        // Fort Thines (st0443)
        public const uint FortThinesNpcs0 = 6255; // Nedo, Bertha, Quintus
        public const uint FortThinesNpcs1 = 6256; // Gurdolin, Lise, Elliot, Meirova
        public const uint FortThinesNpcs2 = 6257; // Nedo, Quintus, Bertha, Meirova, Gurdolin, Lise

        // Lookout Castle (st0450)
        public const uint LookoutCastleNpcs0 = 6260; // Nedo, Bertha, Quintus,
        public const uint LookoutCastleNpcs1 = 6261; // Samuel, Isetto, Akgis, Sara, Mustafa, Aysel, Dan
        public const uint LookoutCastleNpcs2 = 6264; // Nedo, Quintus, Bertha
        public const uint LookoutCastleOMs = 6260; // Sliding door

        // Rothgill Traveler's Inn (st0631) 
        public const uint RothgillInnGillian = 6521;

        // Royal Family's Secret Path (st1091)
        public const uint SecretPathNedo = 6258; // Nedo
        public const uint BerthaQuintus = 6259; // Bertha, Quintus
        public const uint SecretPathNpcs = 6265; // Nedo, Bertha, Quintus
    }

    private class MyQstFlag
    {
        public const uint StartNedoWalk0 = 3186;
        public const uint StartNedoWalk1 = 3290;
        public const uint EndNedoWalk = 3187;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumLevel(87));
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.TheSecretEntrance));
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
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.RoyalFamilysSecretPath, 5, QuestEnemyPlacementType.Manual, new()
        {
            /* stop enemies from spawning */
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.LookoutCastle0, 2, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.WarReadyOgreLightArmor, 87, 0, isBoss: true)
                .SetInfectionType(1)
                .SetNamedEnemyParams(NamedParamId.LookoutCastleGuardTroop0),
            LibDdon.Enemy.CreateAuto(EnemyId.WarReadyGrimwargLightArmor, 87, 1)
                .SetInfectionType(1)
                .SetNamedEnemyParams(NamedParamId.PrisonersGuard0),
            LibDdon.Enemy.CreateAuto(EnemyId.WarReadyGrimwargLightArmor, 87, 2)
                .SetInfectionType(1)
                .SetNamedEnemyParams(NamedParamId.PrisonersGuard0),
            LibDdon.Enemy.CreateAuto(EnemyId.WarReadyGrimwargLightArmor, 87, 3)
                .SetInfectionType(1)
                .SetNamedEnemyParams(NamedParamId.PrisonersGuard0),
            LibDdon.Enemy.CreateAuto(EnemyId.WarReadyGrimwargLightArmor, 87, 4)
                .SetInfectionType(1)
                .SetNamedEnemyParams(NamedParamId.PrisonersGuard0),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTalkAndOrderBlock(Stage.AudienceChamber, NpcId.Joseph, 24346)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.MephiteTravelersInn.Nayajiku)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.RoyalFamilysSecretPath.LookoutCastleDoor1)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.RoyalFamilysSecretPath.LookoutCastleDoor0)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.AudienceChamber.TheCrewEndSeason34);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Accept, Stage.FortThines1, 2, 0, NpcId.Nedo0, 21805)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.FortThinesNpcs2);
        process0.AddIsStageNoBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.RoyalFamilysSecretPath)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.SecretPathNedo)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.BerthaQuintus);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.RoyalFamilysSecretPath, 0, 0, NpcId.Nedo0, 24363);
        process0.AddSceHitInBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.RoyalFamilysSecretPath, 0)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQstFlag.StartNedoWalk0);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.RoyalFamilysSecretPath, 2, 0, NpcId.Nedo0, 24366)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, QstLayoutFlag.SecretPathNedo)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.SecretPathNpcs);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQstFlag.StartNedoWalk1)
            .AddCheckCmdMyQstFlagOnFromFsm(MyQstFlag.EndNedoWalk);
        process0.AddIsStageNoBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.LookoutCastle0)
            .AddResultCmdReleaseAnnounce(ContentsRelease.None, flagInfo: QuestFlags.RoyalFamilysSecretPath.ActivateLookoutCastleWarp);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.LookoutCastle0, 0, 0, NpcId.Nedo0, 24371)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.LookoutCastleNpcs0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, QstLayoutFlag.SecretPathNpcs);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.LookoutCastle0, 0, 2, NpcId.Quintus, 24374);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1, resetGroup: false);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.LookoutCastle0, 2, 0, NpcId.Nedo0, 24377)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.LookoutCastleNpcs2)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.LookoutCastleOMs);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.FortThines1, 2, 0, NpcId.Nedo0, 24386);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.AudienceChamber, NpcId.Joseph, 24393);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
