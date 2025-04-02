/**
 * @brief Resolutions and Omens
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Main;
    public override QuestId QuestId => QuestId.ResolutionsAndOmens;
    public override ushort RecommendedLevel => 1;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => false;
    public override QuestId NextQuestId => QuestId.TheSlumberingGod;
    public override bool ResetPlayerAfterQuest => true;

    private class NamedParamId
    {
        public const uint GoblinVanguard = 122; // Goblin Vanguard
        public const uint GoblinPatrol = 123; // Goblin Patrol
        public const uint OrcInvader = 125; // Orc Invader
        public const uint InvasiveKillerBee = 126; // Invasive Killer Bee
    }

    private class EnemyGroupId
    {
        public const uint Encounter = 10;
    }

    private class QstLayoutFlag
    {
        // st0101
        public const uint LeoAndIris0 = 284;
        // st0423
        public const uint AllNpcs = 1277; // Talcott, Cyrus, Iris and Leo
    }

    private class MyQstFlag
    {
        public const uint CheckForLogin = 1;
    }

    protected override void InitializeState()
    {
        AddContentsRelease(ContentsRelease.MainMenu);
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.BattlefieldOutsideGrittenFort, 1, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.Goblin, 10, 0, 0, assignDefaultDrops: false)
                .SetEnemyTargetTypesId(TargetTypesId.Normal)
                .SetNamedEnemyParams(NamedParamId.GoblinVanguard),
            LibDdon.Enemy.Create(EnemyId.Goblin, 10, 0, 1, assignDefaultDrops: false)
                .SetEnemyTargetTypesId(TargetTypesId.Normal)
                .SetNamedEnemyParams(NamedParamId.GoblinVanguard),
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.BattlefieldOutsideGrittenFort, 2, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.KillerBee, 8, 0, 0, assignDefaultDrops: false)
                .SetEnemyTargetTypesId(TargetTypesId.Normal)
                .SetNamedEnemyParams(NamedParamId.InvasiveKillerBee),
            LibDdon.Enemy.Create(EnemyId.KillerBee, 8, 0, 2, assignDefaultDrops: false)
                .SetEnemyTargetTypesId(TargetTypesId.Normal)
                .SetNamedEnemyParams(NamedParamId.InvasiveKillerBee),
            LibDdon.Enemy.Create(EnemyId.SlingGoblinRock, 10, 0, 1, assignDefaultDrops: false)
                .SetEnemyTargetTypesId(TargetTypesId.Normal)
                .SetNamedEnemyParams(NamedParamId.GoblinPatrol),
            LibDdon.Enemy.Create(EnemyId.SlingGoblinRock, 10, 0, 3, assignDefaultDrops: false)
                .SetEnemyTargetTypesId(TargetTypesId.Normal)
                .SetNamedEnemyParams(NamedParamId.GoblinPatrol),
        });

        AddEnemies(EnemyGroupId.Encounter + 2, Stage.BattlefieldOutsideGrittenFort, 3, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.OrcSoldier0, 12, 0, 0, assignDefaultDrops: false)
                .SetEnemyTargetTypesId(TargetTypesId.Normal)
                .SetNamedEnemyParams(NamedParamId.OrcInvader),
        });

        AddEnemies(EnemyGroupId.Encounter + 3, Stage.GrittenFort3, 1, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.OrcSoldier0, 12, 0, 0, assignDefaultDrops: false)
                .SetEnemyTargetTypesId(TargetTypesId.Normal)
                .SetNamedEnemyParams(NamedParamId.OrcInvader),
            LibDdon.Enemy.Create(EnemyId.OrcSoldier0, 12, 0, 1, assignDefaultDrops: false)
                .SetEnemyTargetTypesId(TargetTypesId.Normal)
                .SetNamedEnemyParams(NamedParamId.OrcInvader),
        });

        AddEnemies(EnemyGroupId.Encounter + 4, Stage.GrittenFort3, 2, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.GoblinFighter0, 10, 0, 0, assignDefaultDrops: false)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
            LibDdon.Enemy.Create(EnemyId.GoblinFighter0, 10, 0, 1, assignDefaultDrops: false)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
            LibDdon.Enemy.Create(EnemyId.GoblinFighter0, 10, 0, 2, assignDefaultDrops: false)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
        });

        AddEnemies(EnemyGroupId.Encounter + 5, Stage.GrittenFort3, 0, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.OrcSoldier0, 12, 0, 0, assignDefaultDrops: false)
                .SetNamedEnemyParams(NamedParamId.OrcInvader),
            LibDdon.Enemy.Create(EnemyId.OrcSoldier0, 12, 0, 1, assignDefaultDrops: false)
                .SetNamedEnemyParams(NamedParamId.OrcInvader),
            LibDdon.Enemy.Create(EnemyId.OrcSoldier0, 12, 0, 2, assignDefaultDrops: false)
                .SetNamedEnemyParams(NamedParamId.OrcInvader),
            LibDdon.Enemy.Create(EnemyId.OrcSoldier0, 12, 0, 3, assignDefaultDrops: false)
                .SetNamedEnemyParams(NamedParamId.OrcInvader),
        });
    }

    protected override void InitializeBlocks()
    {
        ushort processNo = 0;

        var process0 = AddNewProcess(processNo++);
        process0.AddEventAfterJumpBlock(QuestAnnounceType.None, Stage.BattlefieldOutsideGrittenFort, 10, 0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.LeoAndIris0);
        process0.AddIsStageNoBlock(QuestAnnounceType.Accept, Stage.GrittenFort3, showMarker: false)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQstFlag.CheckForLogin);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.GrittenFort3, 0, 0);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 5)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.AllNpcs);
        process0.AddEventExecBlock(QuestAnnounceType.None, Stage.GrittenFort3, 5, Stage.BattlefieldOutsideGrittenFort, 0);
        process0.AddEventExecBlock(QuestAnnounceType.None, Stage.BattlefieldOutsideGrittenFort, 5, Stage.TheWhiteDragonTemple0, 1)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Clear, MyQstFlag.CheckForLogin);
        process0.AddProcessEndBlock(true);

        // Bring player back to the stage if they are logging in
        var process1 = AddNewProcess(processNo++);
        process1.AddMyQstFlagsBlock(QuestAnnounceType.None)
            .AddCheckCmdIsLogin();
        process1.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdStageNoNotEq(Stage.BattlefieldOutsideGrittenFort)
            .AddCheckCmdStageNoNotEq(Stage.GrittenFort3);
        process1.AddRawBlock(QuestAnnounceType.None)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.CheckOn, MyQstFlag.CheckForLogin);
        process1.AddStageJumpBlock(QuestAnnounceType.None, Stage.BattlefieldOutsideGrittenFort, 0);
        process1.AddPlayEventBlock(QuestAnnounceType.None, Stage.BattlefieldOutsideGrittenFort, 10, 0);
        process1.AddProcessEndBlock(false);

        // Getting Started Tutorials
        var process2 = AddNewProcess(processNo++);
        process2.AddSceHitInBlock(QuestAnnounceType.None, Stage.BattlefieldOutsideGrittenFort, 0, false);
        process2.AddSceHitInBlock(QuestAnnounceType.None, Stage.BattlefieldOutsideGrittenFort, 1, false)
            .AddResultCmdTutorialDialog(TutorialId.WhatAreTheArisen);
        process2.AddSceHitInBlock(QuestAnnounceType.None, Stage.BattlefieldOutsideGrittenFort, 2, false)
            .AddResultCmdTutorialDialog(TutorialId.BasicAbilities);
        process2.AddIsStageNoBlock(QuestAnnounceType.None, Stage.GrittenFort3, false);
        process2.AddProcessEndBlock(false)
            .AddResultCmdTutorialDialog(TutorialId.UsingtheMinimap);

        // NPC state machine for st0101
        var process3 = AddNewProcess(processNo++);
        process3.AddIsStageNoBlock(QuestAnnounceType.None, Stage.BattlefieldOutsideGrittenFort, false);
        process3.AddSceHitInBlock(QuestAnnounceType.None, Stage.BattlefieldOutsideGrittenFort, 2, false)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, 4); // Leo and Iris waiting for action to begin
        process3.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter + 0, false)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, 11) // Iris FSM
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, 934) // Move in front of enemey
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, 935); // Move in front of enemey
        process3.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter + 1, false)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, 936) // Leo next battle
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, 937) // Iris Idle
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, 598) // Iris next battle
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, 942); // LEO
        process3.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter + 2, false)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, 946); // Move leo to next battle
        process3.AddProcessEndBlock(false)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, 18) // Move to the end
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, 599); // Move leo and iris to the gate 

        // NPC state machine for st0423
        var process4 = AddNewProcess(processNo++);
        process4.AddIsStageNoBlock(QuestAnnounceType.None, Stage.GrittenFort3, false);
        process4.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter + 3, false)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 1277)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, 20);
        process4.AddProcessEndBlock(false)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, 649);

        // Spawn Dungeon Trash
        var process5 = AddNewProcess(processNo++);
        process5.AddIsStageNoBlock(QuestAnnounceType.None, Stage.BattlefieldOutsideGrittenFort, false);
        process5.AddSpawnGroupsBlock(QuestAnnounceType.None, new List<uint>() {
            EnemyGroupId.Encounter + 0,
            EnemyGroupId.Encounter + 1,
            EnemyGroupId.Encounter + 2,
            EnemyGroupId.Encounter + 3,
            EnemyGroupId.Encounter + 4,
        });
        process5.AddProcessEndBlock(false);

        // Job Tutorials
        var tutorials = new Dictionary<JobId, List<(StageInfo StageInfo, TutorialId TutorialId, int SceNo)>>()
        {
            [JobId.Fighter] = new List<(StageInfo stageInfo, TutorialId TutorialId, int SceNo)>()
            {
                (Stage.BattlefieldOutsideGrittenFort, TutorialId.BasicTacticsFighter, 4),
                (Stage.GrittenFort3, TutorialId.AdvancedTacticsFighter, 1),
            },
            [JobId.ShieldSage] = new List<(StageInfo stageInfo, TutorialId TutorialId, int SceNo)>()
            {
                (Stage.BattlefieldOutsideGrittenFort, TutorialId.BasicTacticsShieldSage, 4),
                (Stage.GrittenFort3, TutorialId.AdvancedTacticsShieldSage, 1),
            },
            [JobId.Hunter] = new List<(StageInfo stageInfo, TutorialId TutorialId, int SceNo)>()
            {
                (Stage.BattlefieldOutsideGrittenFort, TutorialId.BasicTacticsHunter, 4),
                (Stage.GrittenFort3, TutorialId.AdvancedTacticsHunter, 1),
            },
            [JobId.Priest] = new List<(StageInfo stageInfo, TutorialId TutorialId, int SceNo)>()
            {
                (Stage.BattlefieldOutsideGrittenFort, TutorialId.BasicTacticsPriest, 4),
                (Stage.GrittenFort3, TutorialId.AdvancedTacticsPriest, 1),
            },
        };

        foreach (var jobId in tutorials.Keys)
        {
            var process = AddNewProcess(processNo++);
            process.AddRawBlock(QuestAnnounceType.None)
                .AddCheckCmdPlJobEq(jobId);
            foreach (var tutorial in tutorials[jobId])
            {
                process.AddSceHitInBlock(QuestAnnounceType.None, tutorial.StageInfo, tutorial.SceNo, showMarker: false);
                process.AddIsStageNoBlock(QuestAnnounceType.None, tutorial.StageInfo)
                    .AddResultCmdTutorialDialog(tutorial.TutorialId);
            }
            process.AddProcessEndBlock(false);
        }
    }
}

return new ScriptedQuest();
