/**
 * @brief The Battle of Lookout Castle
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ScriptedQuest));

    public override QuestType QuestType => QuestType.Main;
    public override QuestId QuestId => QuestId.TheBattleOfLookoutCastle;
    public override ushort RecommendedLevel => 88;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.AudienceChamber;
    public override QuestId NextQuestId => QuestId.None;

    private class EnemyGroupId
    {
        public const uint Encounter = 10;
    }

    private class NamedParamId
    {
        public const uint LookoutCastleGuardTroop0 = 1774; // Lookout Castle Guard Troop
        public const uint LookoutCastleGuardTroop1 = 1775; // Lookout Castle Guard Troop


        public const uint WarMasterUnit = 2516; // War Master Unit
        public const uint WarMasterPet = 2517; // War Master's Pet
        public const uint WarMaster = 2518; // War Master

        public const uint NecroFollower = 1773; // Necro Follower
    }

    private class QstLayoutFlag
    {
        // Fort Thines (st0443)
        public const uint FortThinesNpcs0 = 5600; // Gurdolin, Lise
        public const uint FortThinesNpcs1 = 5601; // Meirova, Bertha
        public const uint FortThinesNedo = 5988;
        public const uint FortThinesQuintus = 6519;
        public const uint FortThinesMeirovaNedo = 6522;

        // Lookout Castle (st0450)
        public const uint LookoutCastleRaidNpcs0 = 5605; // Gurdolin, Lise, Meirova
        public const uint LookoutCastleRaidOms = 6938; // Blockade, 

        // Lookout Castle (st0451)
        public const uint LookoutCastleNpcs0 = 5606; // Gurdolin, Lise, Nedo, Bertha, Meirova, Gillian, Elliot

        // Rothgill Traveler's Inn (st0631) 
        public const uint RothgillInnNpcs = 6522; // Gillian, Elliot

        // Royal Family's Secret Path (st1091)
        public const uint SecretPathNpcs = 5604; // Gurdolin, Lise
    }

    private class MyQstFlag
    {
        public const uint StartAdds = 1;
        public const uint EndAdds = 2;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumLevel(88));
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.ADesperateInfiltration));
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
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.LookoutCastle0, 3, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.SquadLeaderDwarfOrc, 88, 0)
                .SetNamedEnemyParams(NamedParamId.LookoutCastleGuardTroop1),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordSoldierDwarfOrc, 88, 1)
                .SetNamedEnemyParams(NamedParamId.LookoutCastleGuardTroop1),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordSoldierDwarfOrc, 88, 2)
                .SetNamedEnemyParams(NamedParamId.LookoutCastleGuardTroop1),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordSoldierDwarfOrc, 88, 3)
                .SetNamedEnemyParams(NamedParamId.LookoutCastleGuardTroop1),
            LibDdon.Enemy.CreateAuto(EnemyId.BluntSoldierDwarfOrc, 88, 4)
                .SetNamedEnemyParams(NamedParamId.LookoutCastleGuardTroop1),
            LibDdon.Enemy.CreateAuto(EnemyId.WarReadyGrimwargLightArmor, 88, 5)
                .SetInfectionType(1)
                .SetNamedEnemyParams(NamedParamId.LookoutCastleGuardTroop1),
            LibDdon.Enemy.CreateAuto(EnemyId.WarReadyGrimwargLightArmor, 88, 6)
                .SetInfectionType(1)
                .SetNamedEnemyParams(NamedParamId.LookoutCastleGuardTroop1),
            LibDdon.Enemy.CreateAuto(EnemyId.WarReadyGrimwargLightArmor, 88, 7)
                .SetInfectionType(1)
                .SetNamedEnemyParams(NamedParamId.LookoutCastleGuardTroop1),
            LibDdon.Enemy.CreateAuto(EnemyId.BluntSoldierDwarfOrc, 88, 8)
                .SetNamedEnemyParams(NamedParamId.LookoutCastleGuardTroop1),
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.LookoutCastle0, 5, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.WarReadyGrimwargLightArmor, 88, 0)
                .SetInfectionType(1)
                .SetNamedEnemyParams(NamedParamId.LookoutCastleGuardTroop1),
            LibDdon.Enemy.CreateAuto(EnemyId.WarReadyGrimwargLightArmor, 88, 1)
                .SetInfectionType(1)
                .SetNamedEnemyParams(NamedParamId.LookoutCastleGuardTroop1),
            LibDdon.Enemy.CreateAuto(EnemyId.SquadLeaderDwarfOrc, 88, 4)
                .SetNamedEnemyParams(NamedParamId.LookoutCastleGuardTroop1),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordSoldierDwarfOrc, 88, 2)
                .SetNamedEnemyParams(NamedParamId.LookoutCastleGuardTroop1),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordSoldierDwarfOrc, 88, 3)
                .SetNamedEnemyParams(NamedParamId.LookoutCastleGuardTroop1),
        });

        AddEnemies(EnemyGroupId.Encounter + 2, Stage.LookoutCastle0, 4, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.WarReadyGorecyclopsLightArmor0, 88, 0, isBoss: true)
                .SetInfectionType(1)
                .SetNamedEnemyParams(NamedParamId.LookoutCastleGuardTroop1),
            LibDdon.Enemy.CreateAuto(EnemyId.BluntSoldierDwarfOrc, 88, 1)
                .SetNamedEnemyParams(NamedParamId.LookoutCastleGuardTroop1),
            LibDdon.Enemy.CreateAuto(EnemyId.BluntSoldierDwarfOrc, 88, 2)
                .SetNamedEnemyParams(NamedParamId.LookoutCastleGuardTroop1),
            LibDdon.Enemy.CreateAuto(EnemyId.BluntSoldierDwarfOrc, 88, 3)
                .SetNamedEnemyParams(NamedParamId.LookoutCastleGuardTroop1),
        });

        AddEnemies(EnemyGroupId.Encounter + 3, Stage.LookoutCastle0, 1, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.WarMaster0, 88, 0, isBoss: true)
                .SetNamedEnemyParams(NamedParamId.WarMaster),
        });

        AddEnemies(EnemyGroupId.Encounter + 4, Stage.LookoutCastle0, 6, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.FlameSkeletonBrute, 84, 1)
                .SetEnemyTargetTypesId(1)
                .SetStartThinkTblNo(2)
                .SetIsManualSet(true)
                .SetRepopConditions(50, 60)
                .SetNamedEnemyParams(NamedParamId.NecroFollower),
            LibDdon.Enemy.CreateAuto(EnemyId.FlameSkeletonBrute, 84, 2)
                .SetEnemyTargetTypesId(1)
                .SetStartThinkTblNo(2)
                .SetIsManualSet(true)
                .SetRepopConditions(50, 60)
                .SetNamedEnemyParams(NamedParamId.NecroFollower),
            LibDdon.Enemy.CreateAuto(EnemyId.FlameSkeleton, 84, 3)
                .SetEnemyTargetTypesId(1)
                .SetStartThinkTblNo(2)
                .SetIsManualSet(true)
                .SetRepopConditions(50, 60)
                .SetNamedEnemyParams(NamedParamId.NecroFollower),
            LibDdon.Enemy.CreateAuto(EnemyId.FlameSkeleton, 84, 8)
                .SetEnemyTargetTypesId(1)
                .SetStartThinkTblNo(2)
                .SetIsManualSet(true)
                .SetRepopConditions(50, 60)
                .SetNamedEnemyParams(NamedParamId.NecroFollower),
            LibDdon.Enemy.CreateAuto(EnemyId.SkeletonSorcerer0, 84, 4)
                .SetEnemyTargetTypesId(1)
                .SetStartThinkTblNo(1)
                .SetIsManualSet(true)
                .SetRepopConditions(50, 60)
                .SetNamedEnemyParams(NamedParamId.NecroFollower),
            LibDdon.Enemy.CreateAuto(EnemyId.SkeletonSorcerer0, 84, 6)
                .SetEnemyTargetTypesId(1)
                .SetStartThinkTblNo(1)
                .SetIsManualSet(true)
                .SetRepopConditions(50, 60)
                .SetNamedEnemyParams(NamedParamId.NecroFollower),
            LibDdon.Enemy.CreateAuto(EnemyId.SkeletonSorcerer0, 84, 9)
                .SetEnemyTargetTypesId(1)
                .SetStartThinkTblNo(1)
                .SetIsManualSet(true)
                .SetRepopConditions(50, 60)
                .SetNamedEnemyParams(NamedParamId.NecroFollower),
        });

        AddEnemies(EnemyGroupId.Encounter + 5, Stage.RoyalFamilysSecretPath, 5, QuestEnemyPlacementType.Manual, new()
        {
            /* prevent enemies from spawning */
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTalkAndOrderBlock(Stage.AudienceChamber, NpcId.Joseph, 21814)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.MephiteTravelersInn.Nayajiku)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.RoyalFamilysSecretPath.LookoutCastleDoor1)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.RoyalFamilysSecretPath.LookoutCastleDoor0)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.AudienceChamber.TheCrewEndSeason34)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.LookoutCastle.NedosFurniture);
        process0.AddPartyGatherBlock(QuestAnnounceType.Accept, Stage.FortThines1, 5, 350, -3697) // Head to Fort Thines
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.FortThinesNpcs0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.FortThinesNpcs1)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.FortThinesNedo)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.FortThinesQuintus);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.FortThines1, 5, 0, QuestJumpType.None, Stage.Invalid);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.FortThines1, 2, 0, NpcId.Nedo0, 21825); // Speak with Nedo
        process0.AddIsStageNoBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.RoyalFamilysSecretPath) // Head to the shrine using the Royal Family's Hidden Passage
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.SecretPathNpcs);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.RoyalFamilysSecretPath, 0, 1, NpcId.Gurdolin3, 24204); // Rendezvous with Gurdolin before the secret door
        process0.AddIsStageNoBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.LookoutCastle0); // Storm into the Lookout Castle
        process0.AddDestroyGroupBlock(QuestAnnounceType.CheckpointAndUpdate, EnemyGroupId.Encounter + 0); // Sweep away the enemies within Lookout Castle
        process0.AddDiscoverGroupBlock(QuestAnnounceType.CheckpointAndUpdate, EnemyGroupId.Encounter + 1); // Search for the remaining enemy
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1, resetGroup: false); // Eliminate the enemy
        process0.AddDiscoverGroupBlock(QuestAnnounceType.CheckpointAndUpdate, EnemyGroupId.Encounter + 2); // Find the remaining soldiers in the upper levels
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 2, resetGroup: false); // Defeat the enemy obstructing your path
        process0.AddPartyGatherBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.LookoutCastle0, 50, 18203, -11749); // Defeat the War Master
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.LookoutCastle0, 0, 0, QuestJumpType.None, Stage.Invalid);
        process0.AddSpawnGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 3) // Defeat the leader at the Lookout Castle
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQstFlag.StartAdds)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.LookoutCastleRaidOms)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.LookoutCastleRaidNpcs0)
            .AddCheckCmdEmHpLess(Stage.LookoutCastle0, 1, 0, 65);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.LookoutCastle0, 5, 13, QuestJumpType.After, Stage.LookoutCastle1)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQstFlag.EndAdds);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.LookoutCastle1, 0, 3, NpcId.Nedo0, 21875) // Speak with Nedo
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.LookoutCastleNpcs0)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.RoyalFamilysSecretPath.LookoutCastleDoor0)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.RoyalFamilysSecretPath.LookoutCastleDoor1)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.MephiteTravelersInn.Nayajiku);
        process0.AddRawBlock(QuestAnnounceType.CheckpointAndUpdate) // Activate the Hall portcrystal
            .AddCheckCmdIsReleaseWarpPointAnyone(72);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.LookoutCastle1, 0, 3, NpcId.Nedo0, 24208); // Speak with Nedo
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.AudienceChamber, NpcId.Joseph, 21887); // Return to Lestania and report to Joseph
        process0.AddProcessEndBlock(true);

        var process1 = AddNewProcess(1);
        process1.AddMyQstFlagsBlock(QuestAnnounceType.None)
           .AddMyQstCheckFlag(MyQstFlag.StartAdds);
        process1.AddSpawnGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter + 4)
            .AddCheckCmdMyQstFlagOn(MyQstFlag.EndAdds);
        process1.AddRemoveGroupBlock(QuestAnnounceType.None, [
            EnemyGroupId.Encounter + 3,
            EnemyGroupId.Encounter + 4
        ]);
        process1.AddProcessEndBlock(false);
    }
}

return new ScriptedQuest();
