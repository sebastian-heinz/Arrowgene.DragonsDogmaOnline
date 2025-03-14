/**
 * @brief The Crucible of Demons
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.ExtremeMission;
    public override QuestId QuestId => (QuestId)50300005;
    public override ushort RecommendedLevel => 60;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => false;

    private class GroupId
    {
        public const uint WaterfallGreatCavern = 0; // Initial spawn
        public const uint HermitsHollow = 2; // Initial spawn (final boss spawns here)
        public const uint GateOfTheWavingCave = 5; // Delayed spawn after first dragon kin killed
    }

    private class EnemyGroupId
    {
        public const uint WaterfallGreatCavern = 10;
        public const uint HermitsHollow = 20;
        public const uint GateOfTheWavingCave = 30;
        public const uint DungeonTrash = 40;
        public const uint TimeBonus = 100;
    }

    private class LayoutSetInfoFlag
    {
        public const uint TimeBonus = 7000;
    }

    private class MyQuestFlag
    {
        public const uint SpawnStartingGroups = 1;
        public const uint DetectAnyDragonKinDeath = 2;
        public const uint RubyEyeDead = 3;
        public const uint EmeraldEyeDead = 4;
        public const uint LapisEyeDead = 5;
        public const uint SpawnTimeExtGroup = 6;
    }

    private class NamedParamId
    {
        public const uint TimeBonusEnemy = 462; // Time Bonus Magic-Shell <name>
        public const uint Blaze = 575; // <name> (Blaze)
    }

    private class GeneralAnnouncements
    {
        public const int EliminateTimeExtEnemy = 100182; // Eliminate the Timekeeper
        public const int ElderDragonDefeated = 100183; // Successfully defeated the Elder Dragon
        public const int TimeShiftPresence = 100184; // I sense the presence of the "Time Shift"--
        public const int DiscoverTimeExtEnemy = 100185; // Discover "Time Guard"! Defeat it to get time bonus
        public const int ElderDragonAppeared = 100186; // The Elder Dragon has appeared!
        public const int NewDragonKin = 100187; // A new Dragonkin has appeared!
        public const int TimeBonus5min = 100188; // Time bonus "5 minutes" added!
        public const int AllGemsCollected = 100189; // All three types of gems have been collected!
        public const int ElderDragonBlazeAppeared = 100191; // Enchanted by the three gems, Elder Dragon (Blaze) has appeared!
        public const int DefatedTheEmeraldEye = 100192; // Defeated Emerald Eye
        public const int DefeatedTheLapisEye = 100193; // Defeated Lapis Eye
        public const int DefeatedTheRubyEye = 100194; // Defeated Ruby Eye
        public const int DefeatedTheCrystalEye = 100195; // Defeated Crystal Eye
    }

    private class Purpose
    {
        public const int DefeatDragonKin = 0; // Defeat the Dragonkin.
        public const int FindAndDefeatTimeBonus = 1; // Find and defeat \"Time Bonus\" enemies\nto earn a time bonus.
        public const int FindAndDefeatEyes = 2; // Find and defeat the three types of gems.
    }

    protected override void InitializeState()
    {
        MissionParams.Group = ExtremeMissionUtils.Group.Alan;
        MissionParams.MinimumMembers = 1;
        MissionParams.MaximumMembers = 8;
        MissionParams.IsSolo = false;
        MissionParams.PlaytimeInSeconds = 1200; // 20 minutes
        MissionParams.ArmorAllowed = true;
        MissionParams.JewelryAllowed = true;
        MissionParams.MaxPawns = 7;
        MissionParams.LootDistribution = QuestLootDistribution.TimeBased;
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.JobPoints, 2250);

        // TODO: OG has 5 loot pools but we only have 4 random slots
        AddRandomFixedItemReward(new() {
            (ItemId.HealingElixir, 4),
            (ItemId.BlueDye, 4),
            (ItemId.SuperiorQualityGalaExtract, 3),
            (ItemId.DragonBone, 2),
            (ItemId.LargeDragonBone, 1),
            (ItemId.ScaleDust, 4),
        }, isHidden: true);

        AddRandomFixedItemReward(new() {
            (ItemId.DragonBone, 13),
            (ItemId.LargeDragonBone, 13),
            (ItemId.ShiningDragonBone, 2),
            (ItemId.ScaleDust, 20),
            (ItemId.LargeGrainedSand, 7),
            (ItemId.AncientScaleDust, 3),
            (ItemId.Lestalite, 2),
        }, isHidden: true);

        AddRandomFixedItemReward(new() {
            (ItemId.DragonBone, 13),
            (ItemId.LargeDragonBone, 13),
            (ItemId.ShiningDragonBone, 1),
            (ItemId.ScaleDust, 20),
            (ItemId.LargeGrainedSand, 7),
            (ItemId.AncientScaleDust, 3),
            (ItemId.Lestalite, 3),
            (ItemId.HighLestalite, 1),
        }, isHidden: true);

        AddRandomFixedItemReward(new() {
            (ItemId.DragonBone, 13),
            (ItemId.LargeDragonBone, 13),
            (ItemId.ShiningDragonBone, 1),
            (ItemId.ScaleDust, 20),
            (ItemId.LargeGrainedSand, 18),
            (ItemId.AncientScaleDust, 3),
            (ItemId.ThickDragonscale, 1),
            (ItemId.SacredTreeRing, 1),
        }, isHidden: true);
    }

    private InstancedEnemy CreateRandomFlyingEnemy(ushort lv, byte index)
    {
        return LibDdon.Enemy.CreateRandom(lv, 0, index, new List<EnemyId>() {
            EnemyId.Harpy,
            EnemyId.SnowHarpy
        }).SetIsRequired(false).SetEnemyTargetTypesId(TargetTypesId.Normal);
    }

    private InstancedEnemy CreateRandomGroundEnemy(ushort lv, byte index)
    {
        return LibDdon.Enemy.CreateRandom(lv, 0, index, new List<EnemyId>() {
            EnemyId.Wolf,
            EnemyId.Grimwarg,
            EnemyId.StoutUndead,
            EnemyId.SwordUndead,
            EnemyId.SkeletonKnight,
            EnemyId.SkeletonMage0,
        }).SetIsRequired(false).SetEnemyTargetTypesId(TargetTypesId.Normal);
    }

    private InstancedEnemy CreateRandomDragonKin(ushort lv, byte index)
    {
        return LibDdon.Enemy.CreateRandom(lv, 0, index, new List<EnemyId>() {
            EnemyId.Behemoth0,
            EnemyId.Lindwurm0,
            EnemyId.Angules0,
        }).SetIsBoss(true);
    }

    private ushort SelectRandomLevel()
    {
        return new ushort[] { 55, 60 }[Random.Shared.Next(0, 2)];
    }

    protected override void InitializeEnemyGroups()
    {
        ushort level = 0;

        #region "WaterfallGreatCavern Enemies"
        var waterfallBossIndex = new List<byte>() { 0, 1, 2 };
        uint waterfallIndex = EnemyGroupId.WaterfallGreatCavern;
        foreach (var bossIndex in waterfallBossIndex)
        {
            level = SelectRandomLevel();
            AddEnemies(waterfallIndex++, Stage.DragonsNest, GroupId.WaterfallGreatCavern, 0, QuestEnemyPlacementType.Manual, new()
            {
                CreateRandomDragonKin(level, bossIndex),
                CreateRandomGroundEnemy(level, 3),
                CreateRandomGroundEnemy(level, 4),
                CreateRandomGroundEnemy(level, 5),
                CreateRandomFlyingEnemy(level, 7),
                CreateRandomFlyingEnemy(level, 8),
            }, QuestTargetType.ExmSub);
        }

        level = SelectRandomLevel();
        AddEnemies(waterfallIndex, Stage.DragonsNest, GroupId.WaterfallGreatCavern, 0, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.EmeraldEye, level, 0, 6)
        }, QuestTargetType.ExmSub);
        #endregion

        #region "HermitsHollow Enemies"
        var hermitsHollowBossIndex = new List<byte>() { 0, 1 };
        uint hermitsHollowIndex = EnemyGroupId.HermitsHollow;
        foreach (var bossIndex in hermitsHollowBossIndex)
        {
            level = SelectRandomLevel();
            AddEnemies(hermitsHollowIndex++, Stage.DragonsNest, GroupId.HermitsHollow, 0, QuestEnemyPlacementType.Manual, new()
            {
                CreateRandomDragonKin(level, bossIndex),
                CreateRandomGroundEnemy(level, 3),
                CreateRandomGroundEnemy(level, 4),
                CreateRandomGroundEnemy(level, 5),
                CreateRandomFlyingEnemy(level, 6),
                CreateRandomFlyingEnemy(level, 8),
            }, QuestTargetType.ExmSub);
        }

        level = SelectRandomLevel();
        AddEnemies(hermitsHollowIndex++, Stage.DragonsNest, GroupId.HermitsHollow, 0, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.EmeraldEye, level, 0, 7)
        }, QuestTargetType.ExmSub);

        AddEnemies(hermitsHollowIndex, Stage.DragonsNest, GroupId.HermitsHollow, 0, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.ElderDragon0, 60, 0, 1)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.Blaze),
        });
        #endregion

        #region "GateOfTheWavingCave Enemies"
        var wavingCaveBossIndex = new List<byte>() { 0, 1, 2 };
        uint wavingCaveIndex = EnemyGroupId.GateOfTheWavingCave;
        foreach (var bossIndex in wavingCaveBossIndex)
        {
            level = SelectRandomLevel();
            AddEnemies(wavingCaveIndex++, Stage.DragonsNest, GroupId.GateOfTheWavingCave, 0, QuestEnemyPlacementType.Manual, new()
            {
                CreateRandomDragonKin(level, bossIndex),
                CreateRandomGroundEnemy(level, 3),
                CreateRandomGroundEnemy(level, 4),
                CreateRandomGroundEnemy(level, 5),
                CreateRandomGroundEnemy(level, 6),
                CreateRandomFlyingEnemy(level, 8),
            }, QuestTargetType.ExmSub);
        }

        level = SelectRandomLevel();
        AddEnemies(wavingCaveIndex, Stage.DragonsNest, GroupId.GateOfTheWavingCave, 0, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.EmeraldEye, level, 0, 7)
        }, QuestTargetType.ExmSub);
        #endregion

        #region "Time Bonus Enemies"
        AddEnemies(EnemyGroupId.TimeBonus, Stage.DragonsNest, 9, 0, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.Golem, 55, 0, 0)
                .SetNamedEnemyParams(NamedParamId.TimeBonusEnemy),
            LibDdon.Enemy.Create(EnemyId.RockSaurian, 56, 0, 1)
                .SetIsRequired(false)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
            LibDdon.Enemy.Create(EnemyId.RockSaurian, 56, 0, 2)
                .SetIsRequired(false)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
        }, QuestTargetType.ExmSub);
        #endregion

        #region "Dungeon Trash

        AddEnemies(EnemyGroupId.DungeonTrash + 0, Stage.DragonsNest, 4, 0, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.GiantSaurian, RecommendedLevel, 0, 0)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
            LibDdon.Enemy.Create(EnemyId.RockSaurian, RecommendedLevel, 0, 1)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
            LibDdon.Enemy.Create(EnemyId.RockSaurian, RecommendedLevel, 0, 2)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
            LibDdon.Enemy.Create(EnemyId.RockSaurian, RecommendedLevel, 0, 3)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
            LibDdon.Enemy.Create(EnemyId.RockSaurian, RecommendedLevel, 0, 4)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
        });

        AddEnemies(EnemyGroupId.DungeonTrash + 1, Stage.DragonsNest, 6, 0, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.Wolf, RecommendedLevel, 0, 0)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
            LibDdon.Enemy.Create(EnemyId.SwordUndead, RecommendedLevel, 0, 1)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
            LibDdon.Enemy.Create(EnemyId.StoutUndead, RecommendedLevel, 0, 2)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
            LibDdon.Enemy.Create(EnemyId.StoutUndead, RecommendedLevel, 0, 3)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
            LibDdon.Enemy.Create(EnemyId.SwordUndead, RecommendedLevel, 0, 4)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
            LibDdon.Enemy.Create(EnemyId.Wolf, RecommendedLevel, 0, 5)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
        });

        AddEnemies(EnemyGroupId.DungeonTrash + 2, Stage.DragonsNest, 7, 0, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.RockSaurian, RecommendedLevel, 0, 0)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
            LibDdon.Enemy.Create(EnemyId.RockSaurian, RecommendedLevel, 0, 1)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
        });

        AddEnemies(EnemyGroupId.DungeonTrash + 3, Stage.DragonsNest, 8, 0, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.SkeletonKnight, RecommendedLevel, 0, 0)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
            LibDdon.Enemy.Create(EnemyId.SkeletonKnight, RecommendedLevel, 0, 1)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
            LibDdon.Enemy.Create(EnemyId.SkeletonKnight, RecommendedLevel, 0, 2)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
            LibDdon.Enemy.Create(EnemyId.SkeletonMage0, RecommendedLevel, 0, 3)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
        });

        AddEnemies(EnemyGroupId.DungeonTrash + 4, Stage.DragonsNest, 10, 0, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.SkeletonMage0, RecommendedLevel, 0, 0)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
            LibDdon.Enemy.Create(EnemyId.SkeletonKnight, RecommendedLevel, 0, 1)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
            LibDdon.Enemy.Create(EnemyId.SkeletonKnight, RecommendedLevel, 0, 2)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
            LibDdon.Enemy.Create(EnemyId.SkeletonKnight, RecommendedLevel, 0, 3)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
            LibDdon.Enemy.Create(EnemyId.SkeletonMage0, RecommendedLevel, 0, 4)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
        });

        #endregion
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddIsGatherPartyInStageBlock(QuestAnnounceType.None, Stage.DragonsNest)
            .AddResultCmdEndContentsPurpose(Purpose.DefeatDragonKin);
        process0.AddMyQstFlagsBlock(QuestAnnounceType.Start)
            .AddMyQstSetFlag(MyQuestFlag.SpawnStartingGroups)
            .AddMyQstCheckFlag(MyQuestFlag.DetectAnyDragonKinDeath);
        process0.AddMyQstFlagsBlock(QuestAnnounceType.ExUpdate)
            .AddMyQstCheckFlags(new() { MyQuestFlag.RubyEyeDead, MyQuestFlag.EmeraldEyeDead, MyQuestFlag.LapisEyeDead })
            .AddResultCmdEndContentsPurpose(Purpose.FindAndDefeatEyes);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.None, EnemyGroupId.HermitsHollow + 3)
            .AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, GeneralAnnouncements.AllGemsCollected);
        process0.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.HermitsHollow + 3, false)
            .AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, GeneralAnnouncements.ElderDragonBlazeAppeared);
        process0.AddProcessEndBlock(true);

        // Waterfall Great Cavern Dragonkin
        var process1 = AddNewProcess(1);
        process1.AddMyQstFlagsBlock(QuestAnnounceType.None)
            .AddMyQstCheckFlag(MyQuestFlag.SpawnStartingGroups);
        // First Wave
        process1.AddDiscoverGroupBlock(QuestAnnounceType.None, EnemyGroupId.WaterfallGreatCavern);
        process1.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.WaterfallGreatCavern, false);
        // Second Wave
        process1.AddDiscoverGroupBlock(QuestAnnounceType.None, EnemyGroupId.WaterfallGreatCavern + 1)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQuestFlag.DetectAnyDragonKinDeath)
            .AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, GeneralAnnouncements.NewDragonKin);
        process1.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.WaterfallGreatCavern + 1, false);
        // Third Wave
        process1.AddDiscoverGroupBlock(QuestAnnounceType.None, EnemyGroupId.WaterfallGreatCavern + 2)
            .AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, GeneralAnnouncements.NewDragonKin);
        process1.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.WaterfallGreatCavern + 2, false);
        // Spawn Emerald Eye
        process1.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.WaterfallGreatCavern + 3);
        process1.AddProcessEndBlock(false)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQuestFlag.EmeraldEyeDead)
            .AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, GeneralAnnouncements.DefatedTheEmeraldEye);

        // Hermit's Hollow Dragon Kin
        var process2 = AddNewProcess(2);
        process2.AddMyQstFlagsBlock(QuestAnnounceType.None)
            .AddMyQstCheckFlag(1);
        // First Wave
        process2.AddDiscoverGroupBlock(QuestAnnounceType.None, EnemyGroupId.HermitsHollow);
        process2.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.HermitsHollow, false);
        // Second Wave
        process2.AddDiscoverGroupBlock(QuestAnnounceType.None, EnemyGroupId.HermitsHollow + 1)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQuestFlag.DetectAnyDragonKinDeath)
            .AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, GeneralAnnouncements.NewDragonKin);
        process2.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.HermitsHollow + 1, false);
        // Spawn Ruby Eye
        process2.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.HermitsHollow + 2);
        process2.AddProcessEndBlock(false)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQuestFlag.RubyEyeDead)
            .AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, GeneralAnnouncements.DefeatedTheRubyEye);

        // Gate of the Waving Cave Dragon Kin
        var process3 = AddNewProcess(3);
        process3.AddMyQstFlagsBlock(QuestAnnounceType.None)
            .AddMyQstCheckFlag(MyQuestFlag.DetectAnyDragonKinDeath);
        // First Wave
        process3.AddDiscoverGroupBlock(QuestAnnounceType.None, EnemyGroupId.GateOfTheWavingCave);
        process3.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.GateOfTheWavingCave, false);
        // Second Wave
        process3.AddDiscoverGroupBlock(QuestAnnounceType.None, EnemyGroupId.GateOfTheWavingCave + 1)
            .AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, GeneralAnnouncements.NewDragonKin);
        process3.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.GateOfTheWavingCave + 1, false);
        // Third Wave
        process3.AddDiscoverGroupBlock(QuestAnnounceType.None, EnemyGroupId.GateOfTheWavingCave + 2)
            .AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, GeneralAnnouncements.NewDragonKin);
        process3.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.GateOfTheWavingCave + 2, false);
        // Spawn Lapis Eye
        process3.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.GateOfTheWavingCave + 3);
        process3.AddProcessEndBlock(false)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQuestFlag.LapisEyeDead)
            .AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, GeneralAnnouncements.DefeatedTheLapisEye);

        // Spawn dungeon trash
        var trashGroupIds = new List<uint>()
        {
            EnemyGroupId.DungeonTrash + 0,
            EnemyGroupId.DungeonTrash + 1,
            EnemyGroupId.DungeonTrash + 2,
            EnemyGroupId.DungeonTrash + 3,
            EnemyGroupId.DungeonTrash + 4,
        };

        var timespawnEnemyGroupId = trashGroupIds[Random.Shared.Next(0, trashGroupIds.Count)];
        // TODO: Figure out how to assign layout flags to enemy groups
        // AddQuestLayoutSetInfoFlag(LayoutSetInfoFlag.TimeBonus, Stage.DragonsNest, timespawnEnemyGroupId);

        // Spawn Trash + assign timespawn
        ushort processIndex = 4;
        foreach (var enemyGroupId in trashGroupIds)
        {
            var process = AddNewProcess(processIndex++);
            if (enemyGroupId == timespawnEnemyGroupId)
            {
                // TODO: Figure out how to use layout flags for detecting a group is dead
                // process.AddDestroyGroupBlock(QuestAnnounceType.None, LayoutSetInfoFlag.TimeBonus, enemyGroupId, showMarker: false);
                process.AddDestroyGroupBlock(QuestAnnounceType.None, enemyGroupId);
                process.AddProcessEndBlock(false)
                    .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQuestFlag.SpawnTimeExtGroup)
                    .AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, GeneralAnnouncements.DiscoverTimeExtEnemy);
            }
            else
            {
                process.AddSpawnGroupBlock(QuestAnnounceType.None, enemyGroupId);
                process.AddProcessEndBlock(false);
            }
        }

        // Handle spawning timespan group
        var processN = AddNewProcess(processIndex);
        processN.AddMyQstFlagsBlock(QuestAnnounceType.None)
            .AddMyQstCheckFlag(MyQuestFlag.SpawnTimeExtGroup);
        processN.AddDiscoverGroupBlock(QuestAnnounceType.None, EnemyGroupId.TimeBonus, showMarker: false)
            .AddResultCmdEndContentsPurpose(Purpose.FindAndDefeatTimeBonus);
        processN.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.TimeBonus, false);
        processN.AddProcessEndBlock(false)
            .SetTimeAmount(300)
            .AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, GeneralAnnouncements.TimeBonus5min);
    }
}

return new ScriptedQuest();
