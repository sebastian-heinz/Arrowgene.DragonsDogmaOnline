/**
 * @brief The Princes Whereabouts
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ScriptedQuest));

    public override QuestType QuestType => QuestType.Main;
    public override QuestId QuestId => QuestId.ThePrincesWhereabouts;
    public override ushort RecommendedLevel => 82;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.AudienceChamber;
    public override QuestId NextQuestId => QuestId.SurvivorsVillage;

    private class EnemyGroupId
    {
        public const uint Encounter = 10;
    }

    private class NamedParamId
    {
        public const uint OccupationArmy = 1898; // Occupation Army <name>
        public const uint FortOccupying0 = 1754; // Fort Occupying <name> (80% hp)
        public const uint FortOccupying1 = 1759; // Fort Occupying <name> (70% hp)
    }

    private class MyQstFlag
    {
        public const uint SpawnCannonMarkers = 1;
        public const uint CannonsDestroyed = 2;
    }

    private class QstLayoutFlag
    {
        public const uint CampNPCs0 = 5387; // Meirova, Gillian, Quintus, Gurdolin, Lise, Elliot
        public const uint CampNPCs1 = 5389; // Meirova, Gillian, Quintus, Gurdolin, Lise, Elliot

        public const uint Cannon0 = 5777; // 50, 0
        public const uint Cannon1 = 6448; // 51, 0
        public const uint Cannon2 = 6449; // 52, 0
        public const uint Cannon3 = 6450; // 53, 0
        public const uint Cannon4 = 6451; // 54, 0

        public const uint AlliedNPC = 5392; // Gillian
        public const uint BossBlockade = 5644;
        public const uint FloorMountedHeavyLever = 5778;
        public const uint LiberationArmySoldierAndGillian = 6393;
        public const uint FortSlidingDoor = 6793;
        public const uint PostBossNpcs = 5393;
    }

    private class InstanceData
    {
        public static void SetCannonCount(QuestState questState, int value)
        {
            questState.InstanceVars.SetData<int>("cannon_count", value);
        }

        public static int GetCannonCount(QuestState questState)
        {
            return questState.InstanceVars.GetData<int>("cannon_count");
        }
    }

    public override void InitializeInstanceState(QuestState questState)
    {
        InstanceData.SetCannonCount(questState, 5);
    }

    private void UpdateCannonsCb(QuestCallbackParam param)
    {
        lock (param.QuestState)
        {
            var count = InstanceData.GetCannonCount(param.QuestState);
            if (count <= 0)
            {
                Logger.Error("Cannon callback is already 0 but set again");
                return;
            }

            var amountLeft = count - 1;
            InstanceData.SetCannonCount(param.QuestState, amountLeft);
            if (amountLeft == 0)
            {
                param.ResultCommands.AddResultCmdMyQstFlagOn(MyQstFlag.CannonsDestroyed);
            }
        }
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumLevel(82));
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.MeirovaTheVeteranGeneral));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 700000);
        AddWalletReward(WalletType.Gold, 70000);
        AddWalletReward(WalletType.RiftPoints, 7000);

        AddFixedItemReward(ItemId.UnappraisedSnowTrinketGeneral, 2);
        AddFixedItemReward(ItemId.RoyalCrestMedalRathniteDistrict, 5);
        AddFixedItemReward(ItemId.ApRathniteFoothills, 100);
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.FortThines0, 3, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.WarReadyGorecyclopsLightArmor0, 82, 0, isBoss: true)
                .SetInfectionType(1)
                .SetNamedEnemyParams(NamedParamId.OccupationArmy),
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.FortThines0, 1, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.SquadLeaderDwarfOrc, 82, 1)
                .SetNamedEnemyParams(NamedParamId.OccupationArmy),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordSoldierDwarfOrc, 82, 2)
                .SetNamedEnemyParams(NamedParamId.FortOccupying0),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordSoldierDwarfOrc, 82, 3)
                .SetNamedEnemyParams(NamedParamId.FortOccupying0),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordSoldierDwarfOrc, 82, 4)
                .SetNamedEnemyParams(NamedParamId.FortOccupying0),
        });

        AddEnemies(EnemyGroupId.Encounter + 2, Stage.RathniteFoothills, 3, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.RangedSoldierDwarfOrc, 82, 0)
                .SetStartThinkTblNo(1)
                .SetEnemyTargetTypesId(1),
            LibDdon.Enemy.CreateAuto(EnemyId.RangedSoldierDwarfOrc, 82, 1)
                .SetStartThinkTblNo(1)
                .SetEnemyTargetTypesId(1),
            LibDdon.Enemy.CreateAuto(EnemyId.RangedSoldierDwarfOrc, 82, 2)
                .SetStartThinkTblNo(1)
                .SetEnemyTargetTypesId(1),
            LibDdon.Enemy.CreateAuto(EnemyId.SquadLeaderDwarfOrc, 82, 3)
                .SetStartThinkTblNo(1)
                .SetEnemyTargetTypesId(1),
            LibDdon.Enemy.CreateAuto(EnemyId.RangedSoldierDwarfOrc, 82, 4)
                .SetStartThinkTblNo(1)
                .SetEnemyTargetTypesId(1),
            LibDdon.Enemy.CreateAuto(EnemyId.WarReadyGrimwargLightArmor, 82, 5)
                .SetInfectionType(1)
                .SetEnemyTargetTypesId(1),
            LibDdon.Enemy.CreateAuto(EnemyId.WarReadyGrimwargLightArmor, 82, 6)
                .SetInfectionType(1)
                .SetEnemyTargetTypesId(1),
        });
    }

    protected override void InitializeBlocks()
    {
        ushort processNo = 0;

        var process0 = AddNewProcess(processNo++);
        process0.AddNpcTalkAndOrderBlock(Stage.AudienceChamber, NpcId.Joseph, 21413)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.RathniteFoothills.FortThinesFixWall)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.RathniteFoothills.FortThinesNpcs)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.RathniteFoothills.FortThinesBuildings)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.RathniteFoothills.FortThines0)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.RathniteFoothills.FortThines1)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.RathniteFoothills.FortThinesDebrisMSQ)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.AudienceChamber.TheCrewEndSeason34);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Accept, Stage.RathniteFoothills, 0, 0, NpcId.Meirova0, 21414)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.CampNPCs0);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.RathniteFoothills, 0, 1, NpcId.Gillian0, 21415);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.RathniteFoothills, 2, 0, NpcId.Gillian0, 21416)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, QstLayoutFlag.CampNPCs0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.CampNPCs1);
        process0.AddMyQstFlagsBlock(QuestAnnounceType.Update)
            .AddMyQstSetFlag(MyQstFlag.SpawnCannonMarkers)
            .AddMyQstCheckFlag(MyQstFlag.CannonsDestroyed);
        process0.AddIsStageNoBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.FortThines0)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.RathniteFoothills.FortThines0);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.LiberationArmySoldierAndGillian)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.FloorMountedHeavyLever);
        process0.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter + 0, resetGroup: false);
        process0.AddOmInteractEventBlock(QuestAnnounceType.Update, Stage.FortThines0, 2, 0, OmQuestType.MyQuest, OmInteractType.Release);
        process0.AddPartyGatherBlock(QuestAnnounceType.Update, Stage.FortThines0, 62, 350, -2050)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, QstLayoutFlag.LiberationArmySoldierAndGillian);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.FortThines0, 0, 3);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.AlliedNPC)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.BossBlockade)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.FortSlidingDoor);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.FortThines0, 5, 6, QuestJumpType.After, Stage.FortThines1)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, QstLayoutFlag.BossBlockade);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.FortThines1, 0, 0, NpcId.Meirova0, 21438)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.RathniteFoothills.FortThines0)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.RathniteFoothills.FortThines1)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.PostBossNpcs);
        process0.AddRawBlock(QuestAnnounceType.CheckpointAndUpdate)
            .AddCheckCmdIsReleaseWarpPointAnyone(69);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.AudienceChamber, NpcId.Joseph, 21440);
        process0.AddProcessEndBlock(true);

        var process1 = AddNewProcess(processNo++);
        process1.AddSpawnGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter + 2);
        process1.AddNoProgressBlock();
        process1.AddProcessEndBlock(false);

        var cannons = new List<(uint FlagNo, int GroupNo)>
        {
            (QstLayoutFlag.Cannon0, 50),
            (QstLayoutFlag.Cannon1, 51),
            (QstLayoutFlag.Cannon2, 52),
            (QstLayoutFlag.Cannon3, 53),
            (QstLayoutFlag.Cannon4, 54),
        };

        foreach (var cannon in cannons)
        {
            var process = AddNewProcess(processNo++);
            process.AddMyQstFlagsBlock(QuestAnnounceType.None)
                .AddMyQstCheckFlag(MyQstFlag.SpawnCannonMarkers);
            process.AddIsBrokenLayoutBlock(QuestAnnounceType.None, Stage.RathniteFoothills, cannon.GroupNo, 0)
                .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, cannon.FlagNo);
            process.AddProcessEndBlock(false)
                .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, cannon.FlagNo)
                .AddCallback(UpdateCannonsCb);
        }
    }
}

return new ScriptedQuest();
