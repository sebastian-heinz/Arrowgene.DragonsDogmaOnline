/**
 * @brief Recurrence of Darkness
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.ExtremeMission;
    public override QuestId QuestId => (QuestId)50204000;
    public override ushort RecommendedLevel => 80;
    public override byte MinimumItemRank => 72;
    public override bool IsDiscoverable => false;
    public override bool Enabled => false; // TODO: Remove this when quest is completed

    private static class MyQuestFlag
    {
        public const uint QuestStart = 1;
        public const uint DefeatRememberance200 = 2;
        public const uint DefeatRememberance201 = 3;
        public const uint DefeatRememberance202 = 4;
        public const uint DefeatContemplation200 = 5;
        public const uint DefeatContemplation201 = 6;
        public const uint DefeatContemplation202 = 7;
        public const uint DefeatContemplation203 = 8;
        public const uint DefeatContemplation204 = 9;
        public const uint DefeatNostalgia200 = 10;
        public const uint DefeatNostalgia201 = 11;
        public const uint DefeatNostalgia202 = 12;
        public const uint DefeatNostalgia208 = 13;
        public const uint DefeatNostalgia209 = 14;
        public const uint DefeatRemorse200 = 15;
        public const uint DefeatRemorse201 = 16;
        public const uint DefeatRemorse202 = 17;
        public const uint Defeated10Swords = 18;

        // From Client Files
        public const uint ClearRememberanceBarrier201 = 2684;
        public const uint ClearRememberanceBarrier202 = 2685;

        public const uint ClearContemplationBarrier200 = 2692;
        public const uint ClearContemplationBarrier201 = 2693;
        public const uint ClearContemplationBarrier202 = 2694;
        public const uint ClearContemplationBarrier203 = 2695;
        public const uint ClearContemplationBarrier204 = 2696;
        public const uint Contemplation0 = 2714;

        public const uint ClearNostalgiaBarrier200 = 2686;
        public const uint ClearNostalgiaBarrier201 = 2687;
        public const uint ClearNostalgiaBarrier202 = 2688;

        public const uint ClearRemorseBarrier200 = 2689;
        public const uint ClearRemorseBarrier201 = 2690;
        public const uint ClearRemorseBarrier202 = 2691;
        public const uint RemorseUnknown0 = 2713;
    }

    private static class QstLayoutFlag
    {
        public const uint InitLayout = 5096; // Spawns teleports, walls an barriers (found in st0885_adjoin.sal2.json -- Unknown2)

        // Stage Layouts and configurations
        // public const uint InitLayout0 = 5079;
        // public const uint InitLayout1 = 5080;
        // public const uint InitLayout2 = 5081;
        // public const uint InitLayout3 = 5082;
        // public const uint InitLayout4 = 5094;
        // public const uint InitLayout5 = 5095;
        // public const uint InitLayout6 = 5096;
        // public const uint InitLayout7 = 5097;

        // Branch of Rememberance
        public const uint RememberanceBlackSword200 = 5278; // GroupId=200
        public const uint RememberanceBlackSword201 = 5279; // (41,-184) GroupId=201 (HasBarrier)
        public const uint RememberanceBlackSword202 = 5280; // (50,-150) GroupId=202 (HasBarrier)

        // Branch of Contemplation
        public const uint ContemplationBlackSword200 = 5287; // (76,-80) GroupId=200 (NoBarrier)
        public const uint ContemplationBlackSword201 = 5288; // (61,-73) GroupId=201 (NoBarrier)
        public const uint ContemplationBlackSword202 = 5289; // (31,-62) GroupId=202 (NoBarrier)
        public const uint ContemplationBlackSword203 = 5290; // (46,-104) GroupId=203 (HasBarrier)
        public const uint ContemplationBlackSword204 = 5291; // GroupId=204

        // Branch of Nostalgia
        public const uint NostalgiaBlackSword200 = 5281; // (50,-108) GroupId=200
        public const uint NostalgiaBlackSword201 = 5282; // (51,-163) GroupId=201
        public const uint NostalgiaBlackSword202 = 5283; // GroupId=202
        public const uint NostalgiaBlackSword208 = 5309; // Groupid=208
        public const uint NostalgiaBlackSword209 = 5308; // GroupId=209

        // Branch of Remorse
        public const uint RemorseBlackSword200 = 5284; // (36,-67) GroupId=200
        public const uint RemorseBlackSword201 = 5285; // (48,-111) GroupId=201
        public const uint RemorseBlackSword202 = 5286; // (52,-140) GroupId=202
    }

    private static class EnemyGroupId
    {
        public const uint Phantom = 10;
        public const uint DungeonTrash = 20;
        public const uint TimeBonusGroup = 100;

        public const uint BranchOfContemplation = 200;
    }

    private static class NamedParamId
    {
        /// <summary>
        /// <b>Name Plate</b>: Time Bonus Group <br />
        /// - <b>HP</b>: 70%  <br />
        /// - <b>Attack base {Phys|Magic}</b>: 120%  <br />
        /// - <b>Ailment Damage</b>: 200%  <br />
        /// </summary>
        public const uint TimeBonusGroup0 = 1617;
        /// <summary>
        /// <b>Name Plate</b>: Time Bonus Group <br />
        /// - <b>HP</b>: 70%  <br />
        /// - <b>Attack base {Phys|Magic}</b>: 120%  <br />
        /// - <b>{Shrink|Blow|Down|Shake} endurance main</b>: 200%  <br />
        /// - <b>HP Sub</b>: 200% <br />
        /// - <b>{Shrink|Blow} endurance sub</b>: 200%  <br />
        /// </summary>
        public const uint TimeBonusGroup1 = 1618;
        /// <summary>
        /// <b>Name Plate</b>: Time Bonus Group <br />
        /// - <b>HP</b>: 70% <br />
        /// - <b>Attack base phys</b>: 120% <br />
        /// - <b>Ocd Endurance</b>: 500% <br />
        /// </summary>
        public const uint TimeBonusGroup2 = 1619;
        /// <summary>
        /// <b>Name Plate</b>: Time Bonus Group <br />
        /// - <b>HP</b>: 75% <br />
        /// - <b>Attack base {Phys|Magic}</b>: 115% <br />
        /// - <b>Defense Base {Phys|Magic}</b>: 90% <br />
        /// - <b>Ailment Damage</b>: 150% <br />
        /// </summary>
        public const uint TimeBonusGroup3 = 1692;
        /// <summary>
        /// <b>Name Plate</b>:Time Bonus Group <br />
        /// - <b>HP</b>: 75% <br />
        /// - <b>Attack base {Phys|Magic}</b>: 115% <br />
        /// - <b>Defense Base {Phys|Magic}</b>: 90% <br />
        /// - <b>Ailment Damage</b>: 150% <br />
        /// </summary>
        public const uint TimeBonusGroup4 = 1693;
    }

    private static class GeneralAnnouncements
    {
        public const int DefeatPhantomsForBonus = 100095; // Defeat the phantoms - get a chance to win bonus rewards when you clear!
        public const int PhantomDisappeared = 100096; // The phantom disappeared
        public const int PhantomAppeared = 100133; // A phantom has appeared!
        public const int PathToBlackKnightOpened = 100134; // The path to becoming the Black Knight has opened!
        public const int AllBlackSwordsInAreaDestroyed = 100135; // All of the black swords nesting in the area were destroyed!
        public const int SealWeakened = 100136; // The seal created by the black sword has weakened
        public const int BlackKnightSubordinatesAppeared = 100143; // The Black Knight's subordinates have appeared!
        public const int BlackSwordSensedAnEnemy = 100144; // The Black Sword sensed an enemy.
        public const int MonsterAppearsGuardingTheBlackSword = 100145; // A monster appears guarding the black sword!

        public const int ExUpdateTimeIncrease = 100161; // Time limit added due to participation of specific jobs
    }

    private static class Purpose
    {
        public const int DefeatPhantomKnightSwiftly = 1; // Defeat the Black Knight Phantom swiftly\nand receive a reward upon clearing.
        public const int DefeatTheEncounteredBlackKnight = 2; // Defeat the encountered Black Knight
        public const int DefeatTimeBonusGroup = 3; // Defeat the entire bonus group and gain a Time Bonus.
        public const int PursueBlackKnight = 4; // Pursue the Black Knight lurking in the deepest depths.
        public const int DestroyBlades10 = 0; // Search for the ten blades that corrupt the\n\"Core Tree\" and destroy them (10 remaining)
        public const int DestroyBlades9 = 5; // Search for the ten blades that corrupt\nthe \"Core Tree\" and destroy them (9 remaining)
        public const int DestroyBlades8 = 6; // Search for the ten blades that corrupt\nthe \"Core Tree\" and destroy them (8 remaining)
        public const int DestroyBlades7 = 7; // Search for the ten blades that corrupt\nthe \"Core Tree\" and destroy them (7 remaining)
        public const int DestroyBlades6 = 8; // Search for the ten blades that corrupt\nthe \"Core Tree\" and destroy them (6 remaining)
        public const int DestroyBlades5 = 9; // Search for the ten blades that corrupt\nthe \"Core Tree\" and destroy them (5 remaining)
        public const int DestroyBlades4 = 10; // Search for the ten blades that corrupt\nthe \"Core Tree\" and destroy them (4 remaining)
        public const int DestroyBlades3 = 11; // Search for the ten blades that corrupt\nthe \"Core Tree\" and destroy them (3 remaining)
        public const int DestroyBlades2 = 12; // Search for the ten blades that corrupt\nthe \"Core Tree\" and destroy them (2 remaining)
        public const int DestroyBlades1 = 13; // Search for the ten blades that corrupt\nthe \"Core Tree\" and destroy them (1 remaining)
    }

    private class InstanceData
    {
        /// <summary>
        /// Sets the number of swords destroyed by the party.
        /// </summary>
        /// <param name="questState">The state of the current instance of the quest being executed.</param>
        /// <param name="value">The number of swords required to complete the quest.</param>
        public static void SetSwordCount(QuestState questState, int value)
        {
            questState.InstanceVars.SetData<int>("sword_count", value);
        }

        public static int GetSwordCount(QuestState questState)
        {
            return questState.InstanceVars.GetData<int>("sword_count");
        }
    }

    public override void InitializeInstanceState(QuestState questState)
    {
        InstanceData.SetSwordCount(questState, 10);
    }

    protected override void InitializeState()
    {
        MissionParams.Group = ExtremeMissionUtils.Group.Issac;
        MissionParams.MinimumMembers = 1;
        MissionParams.MaximumMembers = 4;
        MissionParams.IsSolo = false;
        // MissionParams.PlaytimeInSeconds = 300; // 5 minutes
        MissionParams.PlaytimeInSeconds = 3600;
        MissionParams.ArmorAllowed = true;
        MissionParams.JewelryAllowed = true;
        MissionParams.MaxPawns = 3;
        MissionParams.LootDistribution = QuestLootDistribution.TimeBased;
        MissionParams.StartPos = 6;

        AddQuestOrderCondition(QuestOrderCondition.ExtremeMissionCleared((QuestId)50204002));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.JobPoints, 1000);
        AddPointReward(PointType.ExperiencePoints, 50000);
        AddFixedItemReward(ItemId.BlackKnightsThoughts, 10);
    }

    private InstancedEnemy CreateRandomPixieEnemy(ushort lv, byte index)
    {
        return LibDdon.Enemy.CreateRandom(lv, 0, index, new List<EnemyId>() {
            EnemyId.Pixie,
            EnemyId.PixieBiff,
            EnemyId.PixieDin,
            EnemyId.PixieJabber,
            EnemyId.SeverelyInfectedPixie,
            EnemyId.SeverelyInfectedPixieClub,
            EnemyId.SeverelyInfectedPixieCorruptionSeeds,
            EnemyId.HighPixieBiff,
            EnemyId.HighPixiePow,
            EnemyId.HighPixieZolda,
        });
    }

    protected override void InitializeEnemyGroups()
    {
        // Time Enemies
        AddEnemies(EnemyGroupId.TimeBonusGroup + 0, Stage.BranchofRemembrance, 1, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.Siren, 80, 0, 0)
                .SetEnemyTargetTypesId(TargetTypesId.Normal)
                .SetNamedEnemyParams(NamedParamId.TimeBonusGroup0),
            LibDdon.Enemy.Create(EnemyId.DarkSkeleton, 80, 0, 1)
                .SetStartThinkTblNo(2)
                .SetEnemyTargetTypesId(TargetTypesId.Normal)
                .SetNamedEnemyParams(NamedParamId.TimeBonusGroup0),
            LibDdon.Enemy.Create(EnemyId.Siren, 80, 0, 0)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
            LibDdon.Enemy.Create(EnemyId.Siren, 80, 0, 0)
                .SetEnemyTargetTypesId(TargetTypesId.Normal)
                .SetNamedEnemyParams(NamedParamId.TimeBonusGroup0),
        });

        // Black Knight Phantom Enemies
        DropsTable phantomTable = new DropsTable()
            .AddDrop(ItemId.BlackKnightsThoughts, 1, 3, DropRate.UNCOMMON);

        AddEnemies(EnemyGroupId.Phantom + 0, Stage.BranchofRemembrance, 6, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.BlackKnightPhantomClear, 80, 0, 1)
                .SetIsBoss(true)
                .SetStartThinkTblNo(2)
                .SetDropsTable(phantomTable)
                .SetNamedEnemyParams(NamedParamId.TimeBonusGroup0),
        });

        AddEnemies(EnemyGroupId.Phantom + 1, Stage.BranchofRemembrance, 5, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.BlackKnightPhantomClear, 80, 0, 1)
                .SetIsBoss(true)
                .SetStartThinkTblNo(2)
                .SetDropsTable(phantomTable)
                .SetNamedEnemyParams(NamedParamId.TimeBonusGroup0),
        });

        AddEnemies(EnemyGroupId.Phantom + 2, Stage.BranchofNostalgia, 2, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.BlackKnightPhantomClear, 80, 0, 1)
                .SetIsBoss(true)
                .SetStartThinkTblNo(2)
                .SetDropsTable(phantomTable)
                .SetNamedEnemyParams(NamedParamId.TimeBonusGroup0),
        });

        // Dungeon Trash
        AddEnemies(EnemyGroupId.DungeonTrash + 0, Stage.BranchofRemembrance, 8, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.FlameSkeletonBrute, 80, 0, 2)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
            LibDdon.Enemy.Create(EnemyId.FlameSkeletonBrute, 80, 0, 3)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
        });

        AddEnemies(EnemyGroupId.DungeonTrash + 1, Stage.BranchofRemembrance, 9, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.SkeletonBrute, 80, 0, 0)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
            LibDdon.Enemy.Create(EnemyId.Eliminator, 80, 0, 1)
                .SetIsBoss(true)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
        });

        AddEnemies(EnemyGroupId.DungeonTrash + 2, Stage.BranchofNostalgia, 3, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.Eliminator, 80, 0, 0)
                .SetIsBoss(true)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
            LibDdon.Enemy.Create(EnemyId.Eliminator, 80, 0, 1)
                .SetIsBoss(true)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
        });

        // Branch of Contemplation
        // Sword 203 guardians
        AddEnemies(EnemyGroupId.BranchOfContemplation + 0, Stage.BranchofContemplation, 18, QuestEnemyPlacementType.Manual, new()
        {
            CreateRandomPixieEnemy(80, 0)
                .SetNamedEnemyParams(NamedParamId.TimeBonusGroup0),
            CreateRandomPixieEnemy(80, 1)
                .SetNamedEnemyParams(NamedParamId.TimeBonusGroup0),
            CreateRandomPixieEnemy(80, 2)
                .SetNamedEnemyParams(NamedParamId.TimeBonusGroup0),
            CreateRandomPixieEnemy(80, 3)
                .SetNamedEnemyParams(NamedParamId.TimeBonusGroup0),
            CreateRandomPixieEnemy(80, 4)
                .SetNamedEnemyParams(NamedParamId.TimeBonusGroup0),
        });

        AddEnemies(EnemyGroupId.BranchOfContemplation + 1, Stage.BranchofContemplation, 6, QuestEnemyPlacementType.Manual, new()
        {
            CreateRandomPixieEnemy(80, 0)
                .SetNamedEnemyParams(NamedParamId.TimeBonusGroup0),
            CreateRandomPixieEnemy(80, 1)
                .SetNamedEnemyParams(NamedParamId.TimeBonusGroup0),
            CreateRandomPixieEnemy(80, 2)
                .SetNamedEnemyParams(NamedParamId.TimeBonusGroup0),
            CreateRandomPixieEnemy(80, 3)
                .SetNamedEnemyParams(NamedParamId.TimeBonusGroup0),
        });

        // Time Extension enemies when first entering
        AddEnemies(EnemyGroupId.BranchOfContemplation + 3, Stage.BranchofContemplation, 1, QuestEnemyPlacementType.Manual, new()
        {
            CreateRandomPixieEnemy(80, 0)
                .SetNamedEnemyParams(NamedParamId.TimeBonusGroup0),
            CreateRandomPixieEnemy(80, 1)
                .SetNamedEnemyParams(NamedParamId.TimeBonusGroup0),
            CreateRandomPixieEnemy(80, 2)
                .SetNamedEnemyParams(NamedParamId.TimeBonusGroup0),
            CreateRandomPixieEnemy(80, 3)
                .SetNamedEnemyParams(NamedParamId.TimeBonusGroup0),
        });
    }

    private static readonly Dictionary<int, int> SwordUpdateIdMap = new Dictionary<int, int>()
    {
        [1] = Purpose.DestroyBlades1,
        [2] = Purpose.DestroyBlades2,
        [3] = Purpose.DestroyBlades3,
        [4] = Purpose.DestroyBlades4,
        [5] = Purpose.DestroyBlades5,
        [6] = Purpose.DestroyBlades6,
        [7] = Purpose.DestroyBlades7,
        [8] = Purpose.DestroyBlades8,
        [9] = Purpose.DestroyBlades9,
        [10] = Purpose.DestroyBlades10,
    };

    private void UpdateSwordsCb(QuestCallbackParam param)
    {
        var count = InstanceData.GetSwordCount(param.QuestState);
        if ((count - 1) <= 0)
        {
            InstanceData.SetSwordCount(param.QuestState, 0);
            param.ResultCommands.AddResultCmdMyQstFlagOn(MyQuestFlag.Defeated10Swords);
            return;
        }

        if (!SwordUpdateIdMap.ContainsKey(count) || !SwordUpdateIdMap.ContainsKey(count - 1))
        {
            return;
        }

        var prevAnnounceNo = SwordUpdateIdMap[count];
        var newAnnounceNo = SwordUpdateIdMap[count - 1];

        // We need to remove all current annoucements and then add them back
        param.ResultCommands.AddResultCmdRemoveEndContentsPurpose(prevAnnounceNo);
        param.ResultCommands.AddResultCmdRemoveEndContentsPurpose(Purpose.DefeatTimeBonusGroup);

        // Add back new annoucements in the proper order
        param.ResultCommands.AddResultCmdEndContentsPurpose(newAnnounceNo, QuestEndContentsAnnounceType.QuestAnnounce);
        param.ResultCommands.AddResultCmdEndContentsPurpose(Purpose.DefeatTimeBonusGroup, QuestEndContentsAnnounceType.Purpose);

        InstanceData.SetSwordCount(param.QuestState, count - 1);
    }

    private void CheckForNonDpsJobs(QuestCallbackParam param)
    {
        uint timeAmount = 0;
        if (param.Client.Party.ContainsBlueJob())
        {
            timeAmount += 90;
        }

        if (param.Client.Party.ContainsGreenJob())
        {
            timeAmount += 120;
        }

        if (timeAmount > 0)
        {
            LibDdon.Quest.ApplyTimeExtension(param.Client, timeAmount);
            param.ResultCommands.AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, GeneralAnnouncements.ExUpdateTimeIncrease);
        }
    }

    private class SwordEntry
    {
        public StageInfo StageInfo { get; }
        public int GroupId { get; }
        public int SetNo { get; }
        public uint QstLayoutFlagSpawnSword { get; }
        public uint MyQstFlagClearBarrier { get; }
        public uint MyQstFlagDefeatSword { get; }

        public SwordEntry(StageInfo stageInfo, int groupId, int setNo, uint qstLayoutFlagSpawnSword, uint myQstFlagClearBarrier, uint myQstFlagDefeatSword)
        {
            StageInfo = stageInfo;
            GroupId = groupId;
            SetNo = setNo;
            QstLayoutFlagSpawnSword = qstLayoutFlagSpawnSword;
            MyQstFlagClearBarrier = myQstFlagClearBarrier;
            MyQstFlagDefeatSword = myQstFlagDefeatSword;
        }
    }

    protected override void InitializeBlocks()
    {
        ushort processNo = 0;

        var mainProcess = AddNewProcess(processNo++);
        mainProcess.AddIsGatherPartyInStageBlock(QuestAnnounceType.None, Stage.HollowofBeginnings2);
        mainProcess.AddWaitForEventEndBlock(QuestAnnounceType.None, Stage.HollowofBeginnings2, 0);
        mainProcess.AddMyQstFlagsBlock(QuestAnnounceType.Start)
            // TODO: This dungeon has many layouts (this one matches videos found)
            .AddResultCmdEndContentsPurpose(Purpose.DestroyBlades10, QuestEndContentsAnnounceType.QuestAnnounce)
            .AddResultCmdEndContentsPurpose(Purpose.DefeatTimeBonusGroup, QuestEndContentsAnnounceType.Purpose)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.InitLayout)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQuestFlag.QuestStart)
            .AddMyQstCheckFlag(MyQuestFlag.Defeated10Swords);
        mainProcess.AddProcessEndBlock(true);

        // Handles time Extension for special jobs
        var processSpecial = AddNewProcess(processNo++);
        processSpecial.AddMyQstFlagsBlock(QuestAnnounceType.None)
            .AddMyQstCheckFlag(MyQuestFlag.QuestStart);
        processSpecial.AddNoProgressBlock()
            .AddCallback(CheckForNonDpsJobs);
        processSpecial.AddProcessEndBlock(false);

        var swords = new Dictionary<uint, SwordEntry>()
        {
            // Branch Of Rememberance Swords
            [QstLayoutFlag.RememberanceBlackSword200] = new SwordEntry(Stage.BranchofRemembrance, 200, 0, QstLayoutFlag.RememberanceBlackSword200, 0, MyQuestFlag.DefeatRememberance200),
            [QstLayoutFlag.RememberanceBlackSword201] = new SwordEntry(Stage.BranchofRemembrance, 201, 0, QstLayoutFlag.RememberanceBlackSword201, MyQuestFlag.ClearRememberanceBarrier201, MyQuestFlag.DefeatRememberance201),
            [QstLayoutFlag.RememberanceBlackSword202] = new SwordEntry(Stage.BranchofRemembrance, 202, 0, QstLayoutFlag.RememberanceBlackSword202, MyQuestFlag.ClearRememberanceBarrier202, MyQuestFlag.DefeatRememberance202),
            // Branch of Contemplation Swords
            [QstLayoutFlag.ContemplationBlackSword200] = new SwordEntry(Stage.BranchofContemplation, 200, 0, QstLayoutFlag.ContemplationBlackSword200, MyQuestFlag.ClearContemplationBarrier200, MyQuestFlag.DefeatContemplation200),
            [QstLayoutFlag.ContemplationBlackSword201] = new SwordEntry(Stage.BranchofContemplation, 201, 0, QstLayoutFlag.ContemplationBlackSword201, MyQuestFlag.ClearContemplationBarrier201, MyQuestFlag.DefeatContemplation201),
            [QstLayoutFlag.ContemplationBlackSword202] = new SwordEntry(Stage.BranchofContemplation, 202, 0, QstLayoutFlag.ContemplationBlackSword202, MyQuestFlag.ClearContemplationBarrier202, MyQuestFlag.DefeatContemplation202),
            [QstLayoutFlag.ContemplationBlackSword203] = new SwordEntry(Stage.BranchofContemplation, 203, 0, QstLayoutFlag.ContemplationBlackSword203, MyQuestFlag.ClearContemplationBarrier203, MyQuestFlag.DefeatContemplation203),
            [QstLayoutFlag.ContemplationBlackSword204] = new SwordEntry(Stage.BranchofContemplation, 204, 0, QstLayoutFlag.ContemplationBlackSword204, MyQuestFlag.ClearContemplationBarrier204, MyQuestFlag.DefeatContemplation204),
            // Branch of Nostalgia
            [QstLayoutFlag.NostalgiaBlackSword200] = new SwordEntry(Stage.BranchofNostalgia, 200, 0, QstLayoutFlag.NostalgiaBlackSword200, MyQuestFlag.ClearNostalgiaBarrier200, MyQuestFlag.DefeatNostalgia200),
            [QstLayoutFlag.NostalgiaBlackSword201] = new SwordEntry(Stage.BranchofNostalgia, 201, 0, QstLayoutFlag.NostalgiaBlackSword201, MyQuestFlag.ClearNostalgiaBarrier201, MyQuestFlag.DefeatNostalgia201),
            [QstLayoutFlag.NostalgiaBlackSword202] = new SwordEntry(Stage.BranchofNostalgia, 202, 0, QstLayoutFlag.NostalgiaBlackSword202, MyQuestFlag.ClearNostalgiaBarrier202, MyQuestFlag.DefeatNostalgia202),
            [QstLayoutFlag.NostalgiaBlackSword208] = new SwordEntry(Stage.BranchofNostalgia, 208, 0, QstLayoutFlag.NostalgiaBlackSword208, 0, MyQuestFlag.DefeatNostalgia208),
            [QstLayoutFlag.NostalgiaBlackSword209] = new SwordEntry(Stage.BranchofNostalgia, 209, 0, QstLayoutFlag.NostalgiaBlackSword209, 0, MyQuestFlag.DefeatNostalgia209),
            // Branch of Remorse
            [QstLayoutFlag.RemorseBlackSword200] = new SwordEntry(Stage.BranchofRemorse, 200, 0, QstLayoutFlag.RemorseBlackSword200, MyQuestFlag.ClearRemorseBarrier200, MyQuestFlag.DefeatRemorse200),
            [QstLayoutFlag.RemorseBlackSword201] = new SwordEntry(Stage.BranchofRemorse, 201, 0, QstLayoutFlag.RemorseBlackSword201, MyQuestFlag.ClearRemorseBarrier201, MyQuestFlag.DefeatRemorse201),
            [QstLayoutFlag.RemorseBlackSword202] = new SwordEntry(Stage.BranchofRemorse, 202, 0, QstLayoutFlag.RemorseBlackSword202, MyQuestFlag.ClearRemorseBarrier202, MyQuestFlag.DefeatRemorse202),
        };

        var adds = new Dictionary<uint, Func<ushort, QuestProcess>>()
        {
            [QstLayoutFlag.ContemplationBlackSword203] = (processNo) => {
                var process = AddNewProcess(processNo);
                process.AddSceHitInBlock(QuestAnnounceType.None, Stage.BranchofContemplation, 3, false);
                process.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.BranchOfContemplation + 0)
                    .AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, GeneralAnnouncements.MonsterAppearsGuardingTheBlackSword);
                process.AddProcessEndBlock(false)
                    .SetTimeAmount(25);
                return process;
            },
            [QstLayoutFlag.ContemplationBlackSword204] = (processNo) => {
                var process = AddNewProcess(processNo);
                process.AddSceHitInBlock(QuestAnnounceType.None, Stage.BranchofContemplation, 4, false);
                process.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.BranchOfContemplation + 1)
                    .AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, GeneralAnnouncements.BlackSwordSensedAnEnemy)
                    .AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, GeneralAnnouncements.MonsterAppearsGuardingTheBlackSword);
                process.AddProcessEndBlock(false)
                    .SetTimeAmount(25);
                return process;
            }
        };

        foreach (var (key, sword) in swords)
        {
            var process = AddNewProcess(processNo++);
            process.AddIsBrokenLayoutBlock(QuestAnnounceType.None, sword.StageInfo, sword.GroupId, sword.SetNo, false)
                .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, sword.QstLayoutFlagSpawnSword);
            process.AddProcessEndBlock(false)
                .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, sword.MyQstFlagClearBarrier)
                .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, sword.MyQstFlagDefeatSword)
                .AddCallback(UpdateSwordsCb)
                .AddCallback((param) => {
                    Console.WriteLine($"GroupId={sword.GroupId}, ClearBarrier={sword.MyQstFlagClearBarrier}, DefeatSword={sword.MyQstFlagDefeatSword}");
                });

            if (adds.ContainsKey(sword.QstLayoutFlagSpawnSword))
            {
                adds[sword.QstLayoutFlagSpawnSword](processNo++);
            }
        }

        var dungeonTrashGroups = new List<(uint GroupId, uint TimeBonusInSec)>()
        {
            (EnemyGroupId.BranchOfContemplation + 3, 25)
        };

        foreach (var enemyGroup in dungeonTrashGroups)
        {
            var process = AddNewProcess(processNo++);
            process.AddDiscoverGroupBlock(QuestAnnounceType.None, enemyGroup.GroupId, showMarker: false);
            process.AddDestroyGroupBlock(QuestAnnounceType.None, enemyGroup.GroupId, resetGroup: false);
            process.AddProcessEndBlock(false)
                .SetTimeAmount(enemyGroup.TimeBonusInSec);
        }
    }
}

return new ScriptedQuest();
