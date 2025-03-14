/**
 * @brief Battle for Gritten Fort: Recapture Battle
 *
 * @note Capcom used the same quest ID to implement multiple versions of the quest.
 * https://h1g.jp/dd-on/?%E3%82%B0%E3%83%AA%E3%83%83%E3%83%86%E3%83%B3%E7%A0%A6%E6%94%BB%E9%98%B2%E6%88%A6
 * 
 * @cheats
 * - First Phase
 *   /group destroy 71.0.25
 * - Second Phase
 *   /group destroy 71.0.26
 *   /group destroy 71.0.27
 *   /group destroy 71.0.2
 *   /group destroy 71.0.7
 *   /group destroy 71.0.21
 *   /group destroy 71.0.0
 *   /group destroy 71.0.22
 *   /group destroy 71.0.4
 *   /group destroy 71.0.9
 *   /group destroy 71.0.14
 *   /group destroy 71.0.28
 * - Third Phase
 *   /group destroy 71.0.1
 */

#load "libs.csx"

using Arrowgene.Ddon.GameServer.Scripting.Interfaces;

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.ExtremeMission;
    public override QuestId QuestId => (QuestId)50300004;
    public override ushort RecommendedLevel => 90;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => false;

    private class NamedParamId
    {
        public const uint CursedEmpress = 300; // Cursed Empress
        public const uint Stray = 403; // Stray <name>
        public const uint Invader = 406; // Invader <name>
        public const uint Elite = 407; // Elite <name>
        public const uint SupremeCommander = 408; // Supreme Commander <name>
        public const uint AssaultSoldier = 410; // Assault Soldier <name>
        public const uint CommandingOfficer = 416; // Commanding Officer <name>
    }

    protected override void InitializeState()
    {
        MissionParams.Group = ExtremeMissionUtils.Group.Alan;
        MissionParams.MinimumMembers = 1;
        MissionParams.MaximumMembers = 8;
        MissionParams.IsSolo = false;
        MissionParams.PlaytimeInSeconds = 1800;
        MissionParams.ArmorAllowed = true;
        MissionParams.JewelryAllowed = true;
        MissionParams.MaxPawns = 7;
        MissionParams.LootDistribution = QuestLootDistribution.TimeBased;
    }

    protected override void InitializeRewards()
    {
        if (RecommendedLevel >= 90)
        {
            // Bonus for Assisting
            // TODO: Support adding reward for helping other players
            AddFixedItemReward(ItemId.HighOrb1Ho, 100);
            AddFixedItemReward(ItemId.BloodOrb1Bo, 1000);

            // First reward for clearing per day
            // TODO: Support first clear only rewards
            AddRandomFixedItemReward(new() {
                (ItemId.UnidentifiedDragonTrinketAlchemist,     1),
                (ItemId.UnidentifiedDragonTrinketElementArcher, 1),
                (ItemId.UnidentifiedDragonTrinketFighter,       1),
                (ItemId.UnidentifiedDragonTrinketHighScepter,   1),
                (ItemId.UnidentifiedDragonTrinketHunter,        1),
                (ItemId.UnidentifiedDragonTrinketPriest,        1),
                (ItemId.UnidentifiedDragonTrinketSeeker,        1),
                (ItemId.UnidentifiedDragonTrinketShieldSage,    1),
                (ItemId.UnidentifiedDragonTrinketSorcerer,      1),
                (ItemId.UnidentifiedDragonTrinketSpiritLancer,  1),
                (ItemId.UnidentifiedDragonTrinketWarrior,       1),
            }, isHidden: true);

            // Bonus for multiple clears
            // TODO: Support rewards based on clear count
            AddRandomFixedItemReward(new() {
                (ItemId.BonusDungeonTicketG,  1), // x1 clears
                (ItemId.BonusDungeonTicketR,  1), // x5 clears
                (ItemId.BonusDungeonTicketBo, 1), // x10 clears
            }, isHidden: true);

            // Bonus for destroying all enemies
            // TODO: Support rewards based on all enemies killed
            AddRandomFixedItemReward(new() {
                (ItemId.UnappraisedFlightTrinketKing, 1),
                (ItemId.UnappraisedSnowTrinketKing,   1),
                (ItemId.SuperiorGalaExtract,          3),
                (ItemId.SuperiorHealingPotion,        3)
            }, isHidden: true);
        }
        else
        {
            AddRandomFixedItemReward(new() {
                (ItemId.MeritMedalLion, 10),
                (ItemId.MeritMedalLion, 15),
                (ItemId.MeritMedalLion, 20),
                (ItemId.MeritMedalLion, 25),
                (ItemId.MeritMedalLion, 30),
                (ItemId.MeritMedalLion, 35),
                (ItemId.MeritMedalLion, 40),
                (ItemId.MeritMedalLion, 45),
                (ItemId.MeritMedalLion, 50),
            }, isHidden: true);

            AddRandomFixedItemReward(new() {
                (ItemId.UnappraisedMoonTrinketSoldier, 1),
                (ItemId.UnappraisedMoonTrinketSoldier, 2),
                (ItemId.UnappraisedMoonTrinketSoldier, 3),
                (ItemId.UnappraisedMoonTrinketSoldier, 4),
                (ItemId.UnappraisedMoonTrinketGeneral, 1),
            }, isHidden: true);

            AddRandomFixedItemReward(new() {
                (ItemId.SilverTicket, 60),
                (ItemId.SilverTicket, 90),
                (ItemId.SilverTicket, 120),
            }, isHidden: true);

            AddRandomFixedItemReward(new() {
                (ItemId.RedDye,                         1),
                (ItemId.RedDye,                         2),
                (ItemId.RedDye,                         3),
                (ItemId.QualityDefenseUpgradeRock,      1),
                (ItemId.QualityDefenseUpgradeRock,      2),
                (ItemId.QualityDefenseUpgradeRock,      3),
                (ItemId.QualityDefenseUpgradeRock,      4),
                (ItemId.WhiteDragonDefenseUpgradeRock,  1),
                (ItemId.WhiteDragonDefenseUpgradeRock,  2),
                (ItemId.WhiteWingsRadiantCrystal,      10),
                (ItemId.WhiteWingsRadiantCrystal,      20),
                (ItemId.WhiteWingsRadiantCrystal,      30),
                (ItemId.WhiteWingsRadiantCrystal,      60),
                (ItemId.BlessedLestalite,               1),
                (ItemId.BlessedLestalite,               3),
            }, isHidden: true);
        }
    }

    protected override void InitializeEnemyGroups()
    {
        ushort EnemyLevel = (ushort)(RecommendedLevel + 2);
        ushort BossLevel = (ushort)(RecommendedLevel + 3);

        // TODO: Stage.GrittenFort1

        // 1: Goal (1F)
        AddEnemies(1, Stage.GrittenFort1, 25, 0, QuestEnemyPlacementType.Manual, new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.Cyclops0, EnemyLevel, 0, 0)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.AssaultSoldier)
                .SetRaidPoints(850),
            LibDdon.Enemy.Create(EnemyId.ArmoredCyclopsClub, EnemyLevel, 0, 1)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.AssaultSoldier)
                .SetRaidPoints(850),
            LibDdon.Enemy.Create(EnemyId.OrcBattler, EnemyLevel, 0, 2)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(50),
            LibDdon.Enemy.Create(EnemyId.OrcBringer, EnemyLevel, 0, 3)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(50),
            LibDdon.Enemy.Create(EnemyId.OrcTrooper, EnemyLevel, 0, 4)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(50),
            LibDdon.Enemy.Create(EnemyId.OrcSoldier0, EnemyLevel, 0, 5)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(50),
            LibDdon.Enemy.Create(EnemyId.OrcBanger, EnemyLevel, 0, 6)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(50),
            LibDdon.Enemy.Create(EnemyId.OrcAimer, EnemyLevel, 0, 7)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(50),
        });

        // 2: Target (1F northeast passage)
        AddEnemies(2, Stage.GrittenFort1, 26, 0, QuestEnemyPlacementType.Manual, new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.SlingRedcap, EnemyLevel, 0, 0)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(100),
            LibDdon.Enemy.Create(EnemyId.RedcapFighter, EnemyLevel, 0, 1)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(100),
            LibDdon.Enemy.Create(EnemyId.ForestGoblin, EnemyLevel, 0, 2)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(100),
            LibDdon.Enemy.Create(EnemyId.Redcap, EnemyLevel, 0, 3)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(100),
            LibDdon.Enemy.Create(EnemyId.ForestGoblinFighter, EnemyLevel, 0, 4)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(100),
            // LibDdon.Enemy.Create(EnemyId.SlingForestGoblin, EnemyLevel, 0, 5)
            //    .SetNamedEnemyParams(NamedParamId.Elite)
            //    .SetRaidPoints(100),
        });

        // 3: Target (1F Northeast)
        AddEnemies(3, Stage.GrittenFort1, 27, 0, QuestEnemyPlacementType.Manual, new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.Sludgeman, EnemyLevel, 0, 0)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(100),
            LibDdon.Enemy.Create(EnemyId.Sludgeman, EnemyLevel, 0, 1)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(100),
            LibDdon.Enemy.Create(EnemyId.Sludgeman, EnemyLevel, 0, 2)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(100),
            LibDdon.Enemy.Create(EnemyId.Wight0, EnemyLevel, 0, 3)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.AssaultSoldier)
                .SetRaidPoints(800),
            LibDdon.Enemy.Create(EnemyId.CaptainOrc0, EnemyLevel, 0, 4)
                .SetNamedEnemyParams(NamedParamId.CommandingOfficer)
                .SetRaidPoints(300),
        });

        // 4: Target (1F East)
        AddEnemies(4, Stage.GrittenFort1, 2, 0, QuestEnemyPlacementType.Manual, new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.CaptainOrc0, EnemyLevel, 0, 0)
                .SetNamedEnemyParams(NamedParamId.CommandingOfficer)
                .SetRaidPoints(300),
            LibDdon.Enemy.Create(EnemyId.Witch, EnemyLevel, 0, 1)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.AssaultSoldier)
                .SetRaidPoints(800),
            LibDdon.Enemy.Create(EnemyId.Mudman, EnemyLevel, 0, 2)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(100),
        });

        // 5: Target (1F, just before the east stairs)
        AddEnemies(5, Stage.GrittenFort1, 7, 0, QuestEnemyPlacementType.Manual, new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.HobgoblinFighter, EnemyLevel, 0, 0)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(100),
            LibDdon.Enemy.Create(EnemyId.GoblinFighter0, EnemyLevel, 0, 1)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(100),
            LibDdon.Enemy.Create(EnemyId.ShieldGoblin, EnemyLevel, 0, 2)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(100),
            LibDdon.Enemy.Create(EnemyId.Harpy, EnemyLevel, 0, 3)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(100),
            LibDdon.Enemy.Create(EnemyId.SnowHarpy, EnemyLevel, 0, 4)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(100),
        });

        // 6: Target (1F center)
        // TODO: Wiki says next set of enemies can be the following enemies. Unsure of the exact conditions.
        // - Shadow Wolves
        // - Shadow Goblin
        // - Shadow Goblin Fighter
        // - Shadow Sling Goblin
        // - Shadow Harpies
        AddEnemies(6, Stage.GrittenFort1, 21, 0, QuestEnemyPlacementType.Manual, new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.EmpressGhost, EnemyLevel, 0, 0)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.CursedEmpress)
                .SetRaidPoints(850),
            LibDdon.Enemy.Create(EnemyId.ShadowWolf, EnemyLevel, 0, 1)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(100),
            LibDdon.Enemy.Create(EnemyId.ShadowGoblin, EnemyLevel, 0, 2)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(100),
            LibDdon.Enemy.Create(EnemyId.ShadowGoblinFighter, EnemyLevel, 0, 3)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(100),
            LibDdon.Enemy.Create(EnemyId.ShadowSlingGoblin, EnemyLevel, 0, 4)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(100),
            LibDdon.Enemy.Create(EnemyId.ShadowHarpy, EnemyLevel, 0, 5)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(100),
            LibDdon.Enemy.Create(EnemyId.ShadowGoblinLeader, EnemyLevel, 0, 6)
                .SetNamedEnemyParams(NamedParamId.CommandingOfficer)
                .SetRaidPoints(300),
        });

        // 7: Target (1F South)
        AddEnemies(7, Stage.GrittenFort1, 0, 0, QuestEnemyPlacementType.Manual, new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.DreadApe, EnemyLevel, 0, 0)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.AssaultSoldier)
                .SetRaidPoints(1750),
            LibDdon.Enemy.Create(EnemyId.Ghoul0, EnemyLevel, 0, 1)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.AssaultSoldier)
                .SetRaidPoints(1750),
            LibDdon.Enemy.Create(EnemyId.Nightmare, EnemyLevel, 0, 2)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.AssaultSoldier)
                .SetRaidPoints(1750),
        });

        // 8: Target (1F Southwest)
        AddEnemies(8, Stage.GrittenFort1, 22, 0, QuestEnemyPlacementType.Manual, new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.GiantSulfurSaurian, EnemyLevel, 0, 0)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(300),
            LibDdon.Enemy.Create(EnemyId.GiantSaurian, EnemyLevel, 0, 1)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(300),
            LibDdon.Enemy.Create(EnemyId.SulfurSaurian, EnemyLevel, 0, 2)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(100),
            LibDdon.Enemy.Create(EnemyId.Saurian, EnemyLevel, 0, 3)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(100),
        });

        // 9: Target (1F West)
        AddEnemies(9, Stage.GrittenFort1, 4, 0, QuestEnemyPlacementType.Manual, new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.ShadowChimera, EnemyLevel, 0, 0)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.AssaultSoldier)
                .SetRaidPoints(1750),
            LibDdon.Enemy.Create(EnemyId.Chimera0, EnemyLevel, 0, 1)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.AssaultSoldier)
                .SetRaidPoints(1750),
            LibDdon.Enemy.Create(EnemyId.Golem, EnemyLevel, 0, 2)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.AssaultSoldier)
                .SetRaidPoints(1750),
        });

        // 10: Target (1F Northwest Passage)
        AddEnemies(10, Stage.GrittenFort1, 9, 0, QuestEnemyPlacementType.Manual, new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.Hobgoblin, EnemyLevel, 0, 0)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(100),
            LibDdon.Enemy.Create(EnemyId.Goblin, EnemyLevel, 0, 1)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(100),
            LibDdon.Enemy.Create(EnemyId.GoblinLeader, EnemyLevel, 0, 2)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(300),
            LibDdon.Enemy.Create(EnemyId.HobgoblinLeader, EnemyLevel, 0, 3)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(300),
            LibDdon.Enemy.Create(EnemyId.SlingGoblinRock, EnemyLevel, 0, 4)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(100),
            LibDdon.Enemy.Create(EnemyId.SlingHobgoblinOilFlask, EnemyLevel, 0, 5)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(100),
        });

        // 11: Target (2F East-West Passage)
        AddEnemies(11, Stage.GrittenFort1, 14, 0, QuestEnemyPlacementType.Manual, new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.Wolf, EnemyLevel, 0, 0)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(100),
            LibDdon.Enemy.Create(EnemyId.Grimwarg, EnemyLevel, 0, 1)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(100),
            LibDdon.Enemy.Create(EnemyId.Direwolf, EnemyLevel, 0, 2)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(100),
        });

        // 12: Target (2F South)
        AddEnemies(12, Stage.GrittenFort1, 28, 0, QuestEnemyPlacementType.Manual, new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.Ogre, EnemyLevel, 0, 0)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.AssaultSoldier)
                .SetRaidPoints(1750),
            LibDdon.Enemy.Create(EnemyId.SilverRoar, EnemyLevel, 0, 1)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.AssaultSoldier)
                .SetRaidPoints(1750),
            LibDdon.Enemy.Create(EnemyId.GeoGolem, EnemyLevel, 0, 2)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.AssaultSoldier)
                .SetRaidPoints(1750),
        });

        // 13: Assault (2F South)
        AddEnemies(13, Stage.GrittenFort1, 5, 0, QuestEnemyPlacementType.Manual, new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.Ent, EnemyLevel, 0, 0)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.AssaultSoldier)
                .SetRaidPoints(1000),
            LibDdon.Enemy.Create(EnemyId.GeneralOrc, EnemyLevel, 0, 1)
                .SetNamedEnemyParams(NamedParamId.CommandingOfficer)
                .SetRaidPoints(100),
            LibDdon.Enemy.Create(EnemyId.WhiteChimera0, EnemyLevel, 0, 2)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.AssaultSoldier)
                .SetRaidPoints(1000),
        }, QuestTargetType.ExmSub);

        // 14: Assault (2F East-West Passage)
        AddEnemies(14, Stage.GrittenFort1, 29, 0, QuestEnemyPlacementType.Manual, new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.BruteApe, EnemyLevel, 0, 0)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(50),
            LibDdon.Enemy.Create(EnemyId.OrcBattler, EnemyLevel, 0, 1)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(50),
            LibDdon.Enemy.Create(EnemyId.OrcBringer, EnemyLevel, 0, 2)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(50),
            LibDdon.Enemy.Create(EnemyId.OrcTrooper, EnemyLevel, 0, 3)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(50),
            LibDdon.Enemy.Create(EnemyId.OrcSoldier0, EnemyLevel, 0, 4)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(50),
            LibDdon.Enemy.Create(EnemyId.OrcBanger, EnemyLevel, 0, 5)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(50),
            LibDdon.Enemy.Create(EnemyId.OrcAimer, EnemyLevel, 0, 6)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(50),
        }, QuestTargetType.ExmSub);

        // 15: Assault (1F Northeast Passage)
        AddEnemies(15, Stage.GrittenFort1, 8, 0, QuestEnemyPlacementType.Manual, new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.SwordUndead, EnemyLevel, 0, 0)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(50),
            LibDdon.Enemy.Create(EnemyId.SkeletonWarrior, EnemyLevel, 0, 1)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(50),
            LibDdon.Enemy.Create(EnemyId.WarriorUndead, EnemyLevel, 0, 2)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(50),
            LibDdon.Enemy.Create(EnemyId.UndeadMale, EnemyLevel, 0, 3)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(50),
            LibDdon.Enemy.Create(EnemyId.Skeleton, EnemyLevel, 0, 4)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(50),
            LibDdon.Enemy.Create(EnemyId.SkeletonKnight, EnemyLevel, 0, 5)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(50),
            // LibDdon.Enemy.Create(EnemyId.SkeletonSorcerer0, EnemyLevel, 0, 6)
            //    .SetNamedEnemyParams(NamedParamId.Elite)
            //    .SetRaidPoints(50),
        }, QuestTargetType.ExmSub);

        // 16: Assault (1F Northeast)
        AddEnemies(16, Stage.GrittenFort1, 13, 0, QuestEnemyPlacementType.Manual, new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.GeneralOrc, EnemyLevel, 0, 0)
                .SetNamedEnemyParams(NamedParamId.CommandingOfficer)
                .SetRaidPoints(100),
            LibDdon.Enemy.Create(EnemyId.SkullLord, EnemyLevel, 0, 1)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(200),
        }, QuestTargetType.ExmSub);

        // 17: Assault (1F East)
        AddEnemies(17, Stage.GrittenFort1, 2, 0, QuestEnemyPlacementType.Manual, new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.LivingArmor, EnemyLevel, 0, 3)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(200),
            LibDdon.Enemy.Create(EnemyId.SkullLord, EnemyLevel, 0, 4)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(200),
            LibDdon.Enemy.Create(EnemyId.GeneralOrc, EnemyLevel, 0, 5)
                .SetNamedEnemyParams(NamedParamId.CommandingOfficer)
                .SetRaidPoints(100),
        }, QuestTargetType.ExmSub);

        // 18: Assault (1F, just before the east stairs)
        AddEnemies(18, Stage.GrittenFort1, 23, 0, QuestEnemyPlacementType.Manual, new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.RockSaurianSpinel, EnemyLevel, 0, 0)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(50),
            LibDdon.Enemy.Create(EnemyId.RockSaurian, EnemyLevel, 0, 1)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(50),
            LibDdon.Enemy.Create(EnemyId.GoblinBomber, EnemyLevel, 0, 2)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(50),
            LibDdon.Enemy.Create(EnemyId.StoutUndead, EnemyLevel, 0, 3)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(50),
        }, QuestTargetType.ExmSub);

        // 19: Assault (1F center)
        AddEnemies(19, Stage.GrittenFort1, 6, 0, QuestEnemyPlacementType.Manual, new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.GeneralOrc, EnemyLevel, 0, 0)
                .SetNamedEnemyParams(NamedParamId.CommandingOfficer)
                .SetRaidPoints(100),
            LibDdon.Enemy.Create(EnemyId.GhostMail, EnemyLevel, 0, 1)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(200),
            LibDdon.Enemy.Create(EnemyId.SkullLord, EnemyLevel, 0, 2)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(200),
        }, QuestTargetType.ExmSub);

        // 20: Assault (1F South)
        AddEnemies(20, Stage.GrittenFort1, 0, 0, QuestEnemyPlacementType.Manual, new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.GeneralOrc, EnemyLevel, 0, 7)
                .SetNamedEnemyParams(NamedParamId.CommandingOfficer)
                .SetRaidPoints(100),
            LibDdon.Enemy.Create(EnemyId.Troll, EnemyLevel, 0, 8)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.AssaultSoldier)
                .SetRaidPoints(1000),
            LibDdon.Enemy.Create(EnemyId.Griffin0, EnemyLevel, 0, 9)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.AssaultSoldier)
                .SetRaidPoints(1000),
            LibDdon.Enemy.Create(EnemyId.Sphinx0, EnemyLevel, 0, 10)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.AssaultSoldier)
                .SetRaidPoints(1000),
        }, QuestTargetType.ExmSub);

        // 21: Assault (1F Southwest Passage)
        AddEnemies(21, Stage.GrittenFort1, 10, 0, QuestEnemyPlacementType.Manual, new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.OrcAimer, EnemyLevel, 0, 0)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(50),
            LibDdon.Enemy.Create(EnemyId.OrcTrooper, EnemyLevel, 0, 1)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(50),
            LibDdon.Enemy.Create(EnemyId.OrcBringer, EnemyLevel, 0, 2)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(50),
            LibDdon.Enemy.Create(EnemyId.BruteApe, EnemyLevel, 0, 3)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(50),
        }, QuestTargetType.ExmSub);

        // 22: Assault (1F West)
        AddEnemies(22, Stage.GrittenFort1, 30, 0, QuestEnemyPlacementType.Manual, new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.Colossus0, EnemyLevel, 0, 0)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.AssaultSoldier)
                .SetRaidPoints(1000),
            LibDdon.Enemy.Create(EnemyId.MoleTroll0, EnemyLevel, 0, 1)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.AssaultSoldier)
                .SetRaidPoints(1000),
            LibDdon.Enemy.Create(EnemyId.BlackGriffin0, EnemyLevel, 0, 2)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.AssaultSoldier)
                .SetRaidPoints(1000),
            LibDdon.Enemy.Create(EnemyId.Cockatrice, EnemyLevel, 0, 3)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.AssaultSoldier)
                .SetRaidPoints(1000),
        }, QuestTargetType.ExmSub);

        // 23: Assault (1F Northwest Passage)
        AddEnemies(23, Stage.GrittenFort1, 24, 0, QuestEnemyPlacementType.Manual, new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.FrostCorpsePunisher, EnemyLevel, 0, 0)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(50),
            LibDdon.Enemy.Create(EnemyId.FrostCorpseTorturer, EnemyLevel, 0, 1)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(50),
            LibDdon.Enemy.Create(EnemyId.BlueNewt, EnemyLevel, 0, 2)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(50),
            LibDdon.Enemy.Create(EnemyId.SkeletonMage0, EnemyLevel, 0, 3)
                .SetNamedEnemyParams(NamedParamId.Elite)
                .SetRaidPoints(50),
        }, QuestTargetType.ExmSub);

        // 24: Target (1F North)
        AddEnemies(24, Stage.GrittenFort1, 1, 0, QuestEnemyPlacementType.Manual, new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.Zuhl0, EnemyLevel, 0, 0)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.Invader)
                .SetRaidPoints(7500),
            LibDdon.Enemy.Create(EnemyId.Mogok, BossLevel, 0, 1)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.SupremeCommander)
                .SetRaidPoints(7500),
        });

        // Random (After defeating all raids?)
        // @note Strays drop treasure key?
        // Stray Glutton Ooze (100pt; 1F Center)
        // Stray Spider (100pt; 1F Southeast Passage)
        // Stray Ruby Eye (100pt; 1F Northwest Passage)
        // Stray Crystal Eye (100pt; 1F Northeast Passage)
        // Stray Deep Slime (100pt; 1F West)
        // Stray Worm (100pt; )
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddIsGatherPartyInStageBlock(QuestAnnounceType.None, new StageLayoutId(71, 0, 0))
            .AddCheckCommand(QuestCheckCommand.EventEnd, 405, 0);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Start, 1);
        process0.AddMyQstFlagsBlock(QuestAnnounceType.Update)
            .AddMyQstSetFlag(1)
            .AddMyQstCheckFlags(new List<uint>() {
                2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12
            });
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, 24);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, 24, false)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, 25);
        process0.AddProcessEndBlock(true);

        // 11 Target groups required, generate processes for them.
        var requiredGroups = new List<uint>()
        {
            2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12
        };

        ushort i = 1;
        foreach (var groupId in requiredGroups)
        {
            var process = AddNewProcess(i++);
            process.AddMyQstFlagsBlock(QuestAnnounceType.None)
                .AddMyQstCheckFlag(1);
            process.AddDestroyGroupBlock(QuestAnnounceType.None, groupId);
            process.AddMyQstFlagsBlock(QuestAnnounceType.None)
                .AddMyQstSetFlag(groupId);
            process.AddProcessEndBlock(false);
        }

        // 11 Target groups required, generate processes for them.
        var optionalGroups = new List<uint>()
        {
            13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23
        };

        foreach (var groupId in optionalGroups)
        {
            var process = AddNewProcess(i++);
            process.AddMyQstFlagsBlock(QuestAnnounceType.None)
                .AddMyQstCheckFlag(25);
            process.AddDiscoverGroupBlock(QuestAnnounceType.None, groupId);
            process.AddDestroyGroupBlock(QuestAnnounceType.None, groupId, false);
            process.AddMyQstFlagsBlock(QuestAnnounceType.None)
                .AddMyQstSetFlag(groupId);
            process.AddExtendTimeBlock(QuestAnnounceType.None, 180);
            process.AddProcessEndBlock(false);
        }
    }
}

return new ScriptedQuest();
