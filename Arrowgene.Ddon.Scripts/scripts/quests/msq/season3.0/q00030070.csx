/**
 * @brief Prince Nedo
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ScriptedQuest));

    public override QuestType QuestType => QuestType.Main;
    public override QuestId QuestId => QuestId.PrinceNedo;
    public override ushort RecommendedLevel => 84;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.AudienceChamber;
    public override QuestId NextQuestId => QuestId.TheRoyalFamilySacrament;

    private class EnemyGroupId
    {
        public const uint Encounter = 10;
    }

    private class NamedParamId
    {
        public const uint FortGuard = 1901; // Fort Guard <name>
        public const uint FortOccupying0 = 1754; // Fort Occupying <name>
        public const uint FortOccupying1 = 1759; // Fort Occupying <name>
        public const uint FortOccupyingWarReadyManticore = 1900;

        public const uint BeastCommander0 = 1758;
        public const uint BeastCommander1 = 1983;

        public const uint Powerful = 1947; // Powerful <name>
    }

    private class QstLayoutFlag
    {
        // Bandit Hideout
        public const uint BanditNpcs = 5403; // Gillian, Gurdolin, Lise, Elliot, Sly

        // Fort Thines
        public const uint FortThinesNpcs = 5796; // Nedo, Gillian, Meirova, Quintus, Gurdolin, Lise, Elliot

        // Dacreim Fortress
        public const uint Allies0 = 5405; // Gillian, Lise, Gurdolin, Elliot
        public const uint Allies1 = 5412; // Nedo, Meirova, Gillian, Gurdolin, Lise, Elliot, Bertha
        public const uint Nedo = 6532; // Nedo,
        public const uint Blockade = 5643;

        // Rathnite Foothills Lakeside
        public const uint RothgillSoldiers = 5789; // Rothgill Soldier
        public const uint RothgillNpcs = 5791; // Gerhard, Meirova, Liberation Army Soldier
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumLevel(84));
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.TheOpposition));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 900000);
        AddWalletReward(WalletType.Gold, 70000);
        AddWalletReward(WalletType.RiftPoints, 7000);

        AddFixedItemReward(ItemId.UnappraisedSnowTrinketGeneral, 2);
        AddFixedItemReward(ItemId.RoyalCrestMedalRathniteDistrict, 10);
        AddFixedItemReward(ItemId.ApRathniteFoothills, 100);
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.RathniteFoothillsLakeside0, 25, QuestEnemyPlacementType.Manual, new()
        {
            // 0, 2, 3, 4
            LibDdon.Enemy.Create(EnemyId.SquadLeaderDwarfOrc, 84, 4200, 0)
                .SetNamedEnemyParams(NamedParamId.FortGuard),
            LibDdon.Enemy.Create(EnemyId.SwordSoldierDwarfOrc, 84, 4200, 2)
                .SetNamedEnemyParams(NamedParamId.FortGuard),
            LibDdon.Enemy.Create(EnemyId.SwordSoldierDwarfOrc, 84, 4200, 3)
                .SetNamedEnemyParams(NamedParamId.FortGuard),
            LibDdon.Enemy.Create(EnemyId.SwordSoldierDwarfOrc, 84, 4200, 4)
                .SetNamedEnemyParams(NamedParamId.FortGuard),
            LibDdon.Enemy.Create(EnemyId.WarReadyGrimwargLightArmor, 84, 4200, 1)
                .SetEnemyTargetTypesId(TargetTypesId.Normal)
                .SetInfectionType(1)
                .SetIsRequired(false),
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.DacreimFortress0, 2, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.WarReadyGoremanticoreLightArmor, 84, 105000, 1)
                .SetIsBoss(true)
                .SetInfectionType(1)
                .SetNamedEnemyParams(NamedParamId.FortOccupyingWarReadyManticore),
            LibDdon.Enemy.Create(EnemyId.WarReadyGrimwargLightArmor, 84, 4200, 0)
                .SetInfectionType(1)
                .SetNamedEnemyParams(NamedParamId.FortOccupying1),
            LibDdon.Enemy.Create(EnemyId.WarReadyGrimwargLightArmor, 84, 4200, 2)
                .SetInfectionType(1)
                .SetNamedEnemyParams(NamedParamId.FortOccupying1),
            LibDdon.Enemy.Create(EnemyId.WarReadyGrimwargLightArmor, 84, 4200, 3)
                .SetInfectionType(1)
                .SetNamedEnemyParams(NamedParamId.FortOccupying1),
            LibDdon.Enemy.Create(EnemyId.WarReadyGrimwargLightArmor, 84, 4200, 4)
                .SetInfectionType(1)
                .SetNamedEnemyParams(NamedParamId.FortOccupying1),
            LibDdon.Enemy.Create(EnemyId.RangedSoldierDwarfOrc, 84, 4200, 5)
                .SetSetType(3)
                .SetEnemyTargetTypesId(TargetTypesId.Normal)
                .SetNamedEnemyParams(NamedParamId.FortOccupying0)
                .SetIsRequired(false),
        });

        AddEnemies(EnemyGroupId.Encounter + 2, Stage.DacreimFortress0, 1, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.BeastMaster0, 84, 0, isBoss: true)
                .SetNamedEnemyParams(NamedParamId.BeastCommander0),
        });

        AddEnemies(EnemyGroupId.Encounter + 3, Stage.DacreimFortress0, 4, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.Strix, 84, 0, 0)
                .SetStartThinkTblNo(9)
                .SetIsManualSet(true),
            LibDdon.Enemy.Create(EnemyId.Strix, 84, 0, 1)
                .SetStartThinkTblNo(9)
                .SetIsManualSet(true),
            LibDdon.Enemy.Create(EnemyId.Strix, 84, 0, 2)
                .SetStartThinkTblNo(9)
                .SetIsManualSet(true),
            LibDdon.Enemy.Create(EnemyId.Strix, 84, 0, 3)
                .SetStartThinkTblNo(9)
                .SetIsManualSet(true),
            LibDdon.Enemy.Create(EnemyId.Warg, 84, 0, 4)
                .SetStartThinkTblNo(9)
                .SetIsManualSet(true),
            LibDdon.Enemy.Create(EnemyId.Warg, 84, 0, 5)
                .SetStartThinkTblNo(9)
                .SetIsManualSet(true),
            LibDdon.Enemy.Create(EnemyId.Warg, 84, 0, 6)
                .SetStartThinkTblNo(9)
                .SetIsManualSet(true),
            LibDdon.Enemy.Create(EnemyId.Warg, 84, 0, 7)
                .SetStartThinkTblNo(9)
                .SetIsManualSet(true),
        });

        AddEnemies(EnemyGroupId.Encounter + 4, Stage.DacreimFortress0, 4, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.Grimwarg, 84, 0, 4)
                .SetStartThinkTblNo(9)
                .SetIsManualSet(true),
            LibDdon.Enemy.Create(EnemyId.Grimwarg, 84, 0, 5)
                .SetStartThinkTblNo(9)
                .SetIsManualSet(true),
            LibDdon.Enemy.Create(EnemyId.Grimwarg, 84, 0, 6)
                .SetStartThinkTblNo(9)
                .SetIsManualSet(true),
            LibDdon.Enemy.Create(EnemyId.Grimwarg, 84, 0, 7)
                .SetStartThinkTblNo(9)
                .SetIsManualSet(true),
            LibDdon.Enemy.Create(EnemyId.Strix, 84, 0, 8)
                .SetStartThinkTblNo(9)
                .SetIsManualSet(true),
            LibDdon.Enemy.Create(EnemyId.Strix, 84, 0, 9)
                .SetStartThinkTblNo(9)
                .SetIsManualSet(true),
            LibDdon.Enemy.Create(EnemyId.Grimwarg, 84, 0, 10)
                .SetStartThinkTblNo(9)
                .SetIsManualSet(true),
            LibDdon.Enemy.Create(EnemyId.Strix, 84, 0, 11)
                .SetStartThinkTblNo(9)
                .SetIsManualSet(true),
        });

        AddEnemies(EnemyGroupId.Encounter + 5, Stage.DacreimFortress0, 4, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.WarReadyGrimwargLightArmor, 84, 0, 4)
                .SetNamedEnemyParams(NamedParamId.Powerful)
                .SetInfectionType(1)
                .SetStartThinkTblNo(9)
                .SetIsManualSet(true),
            LibDdon.Enemy.Create(EnemyId.WarReadyGrimwargLightArmor, 84, 0, 5)
                .SetNamedEnemyParams(NamedParamId.Powerful)
                .SetInfectionType(1)
                .SetStartThinkTblNo(9)
                .SetIsManualSet(true),
            LibDdon.Enemy.Create(EnemyId.WarReadyGrimwargLightArmor, 84, 0, 6)
                .SetNamedEnemyParams(NamedParamId.Powerful)
                .SetInfectionType(1)
                .SetStartThinkTblNo(9)
                .SetIsManualSet(true),
            LibDdon.Enemy.Create(EnemyId.WarReadyGrimwargLightArmor, 84, 0, 7)
                .SetNamedEnemyParams(NamedParamId.Powerful)
                .SetInfectionType(1)
                .SetStartThinkTblNo(9)
                .SetIsManualSet(true),
            LibDdon.Enemy.Create(EnemyId.WarReadyGrimwargLightArmor, 84, 0, 8)
                .SetNamedEnemyParams(NamedParamId.Powerful)
                .SetInfectionType(1)
                .SetStartThinkTblNo(9)
                .SetIsManualSet(true),
            LibDdon.Enemy.Create(EnemyId.WarReadyGrimwargLightArmor, 84, 0, 9)
                .SetNamedEnemyParams(NamedParamId.Powerful)
                .SetInfectionType(1)
                .SetStartThinkTblNo(9)
                .SetIsManualSet(true),
            LibDdon.Enemy.Create(EnemyId.Cragger, 84, 0, 11)
                .SetNamedEnemyParams(NamedParamId.Powerful)
                .SetStartThinkTblNo(9)
                .SetIsBoss(true)
                .SetIsManualSet(true),
        });

        AddEnemies(EnemyGroupId.Encounter + 6, Stage.DacreimFortress0, 4, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.Grimwarg, 84, 0, 5)
                .SetNamedEnemyParams(NamedParamId.Powerful)
                .SetStartThinkTblNo(9)
                .SetIsManualSet(true),
            LibDdon.Enemy.Create(EnemyId.Grimwarg, 84, 0, 6)
                .SetNamedEnemyParams(NamedParamId.Powerful)
                .SetStartThinkTblNo(9)
                .SetIsManualSet(true),
            LibDdon.Enemy.Create(EnemyId.Grimwarg, 84, 0, 7)
                .SetNamedEnemyParams(NamedParamId.Powerful)
                .SetStartThinkTblNo(9)
                .SetIsManualSet(true),
            LibDdon.Enemy.Create(EnemyId.Grimwarg, 84, 0, 8)
                .SetNamedEnemyParams(NamedParamId.Powerful)
                .SetStartThinkTblNo(9)
                .SetIsManualSet(true),
            LibDdon.Enemy.Create(EnemyId.BlackGriffin0, 84, 0, 10)
                .SetNamedEnemyParams(NamedParamId.Powerful)
                .SetStartThinkTblNo(9)
                .SetIsBoss(true)
                .SetIsManualSet(true),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTalkAndOrderBlock(Stage.AudienceChamber, NpcId.Joseph, 21492)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.AudienceChamber.TheCrewEndSeason34);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Accept, Stage.BerthasBanditGroupHideout, 0, 0, NpcId.Gillian0, 21493) // Head to the bandit hideout in the Lakeside Grotto
            .AddResultCmdQstTalkChg(NpcId.Joseph, 23214)
            .AddResultCmdQstTalkChg(NpcId.Klaus0, 23215)
            .AddResultCmdQstTalkChg(NpcId.Meirova0, 23217)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 23218)
            .AddResultCmdQstTalkChg(NpcId.Elliot0, 23219)
            .AddResultCmdQstTalkChg(NpcId.Lise0, 23220)
            .AddResultCmdQstTalkChg(NpcId.Sly, 23221)
            .AddResultCmdQstTalkChg(NpcId.Raven, 23222)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.BanditNpcs);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.RathniteFoothillsLakeside0, 1, 1, NpcId.Meirova0, 24166) // Head to Rothgill and speak with Meirova
            .AddResultCmdQstTalkChg(NpcId.Gillian0, 23216)
            .AddResultCmdQstTalkChg(NpcId.Gerhard, 24165)
            .AddResultCmdQstTalkChg(NpcId.LiberationArmySoldier3, 24167)
            .AddResultCmdQstTalkChg(NpcId.LiberationArmySoldier4, 24168)
            .AddResultCmdQstTalkChg(NpcId.LiberationArmySoldier5, 24169)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.RothgillNpcs);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.RathniteFoothillsLakeside0, 0, 0, NpcId.RothgillSoldier0, 24170) // Head to the soldier before Dacreim Fortress
            .AddResultCmdQstTalkChg(NpcId.RothgillSoldier1, 24171)
            .AddResultCmdQstTalkChg(NpcId.RothgillSoldier2, 24172)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.RothgillSoldiers)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.RathniteFoothillsLakeside.FortDacriumWallBreach);
        process0.AddDestroyGroupBlock(QuestAnnounceType.CheckpointAndUpdate, EnemyGroupId.Encounter + 0); // Defeat the obstructing Orcs to storm into Dacreim Fortress
        process0.AddIsStageNoBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.DacreimFortress0)  // Raid Dacreim Fortress
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.RathniteFoothillsLakeside.DacriumFortressEntrance);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1)  // Eliminate the War-Ready Manticore and the War-Ready Grimwarg
            .AddResultCmdQstLayoutFlagOn(6527)
            .AddResultCmdQstLayoutFlagOn(6528);
        process0.AddPartyGatherBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.DacreimFortress0, 44, 80, 10448) // Rescue Prince Nedo
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.Nedo);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.DacreimFortress0, 0, 4);
        process0.AddSpawnGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 2) // Eliminate the enemy
            .AddResultCmdPlayMessage(21503, 10)
            .AddResultCmdPlayMessage(21522, 10)
            .AddResultCmdPlayMessage(21515, 10)
            .AddResultCmdPlayMessage(21528, 10)
            .AddResultCmdPlayMessage(21535, 10)
            .AddResultCmdPlayMessage(21504, 10)
            .AddResultCmdPlayMessage(21516, 10)
            .AddResultCmdPlayMessage(21529, 10)
            .AddResultCmdPlayMessage(21536, 10)
            .AddResultCmdPlayMessage(21505, 10)
            .AddResultCmdPlayMessage(21523, 10)
            .AddResultCmdPlayMessage(21537, 10)
            .AddResultCmdPlayMessage(21506, 10)
            .AddResultCmdPlayMessage(21530, 10)
            .AddResultCmdPlayMessage(21517, 10)
            .AddResultCmdPlayMessage(21507, 10)
            .AddResultCmdPlayMessage(21538, 10)
            .AddResultCmdPlayMessage(21524, 10)
            .AddResultCmdPlayMessage(21508, 10)
            .AddResultCmdPlayMessage(21518, 10)
            .AddResultCmdPlayMessage(21531, 10)
            .AddResultCmdPlayMessage(21509, 10)
            .AddResultCmdPlayMessage(21519, 10)
            .AddResultCmdPlayMessage(21525, 10)
            .AddResultCmdPlayMessage(21510, 10)
            .AddResultCmdPlayMessage(21532, 10)
            .AddResultCmdPlayMessage(21511, 10)
            .AddResultCmdPlayMessage(21526, 10)
            .AddResultCmdPlayMessage(21539, 10)
            .AddResultCmdPlayMessage(21533, 10)
            .AddResultCmdPlayMessage(21512, 10)
            .AddResultCmdPlayMessage(21527, 10)
            .AddResultCmdPlayMessage(21520, 10)
            .AddResultCmdPlayMessage(21540, 10)
            .AddResultCmdPlayMessage(21503, 10)
            .AddResultCmdPlayMessage(21514, 10)
            //.AddResultCmdSetDiePlayerReturnPos(Stage.DacreimFortress0, 4, 0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.Allies0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.Blockade)
            .AddCheckCmdEmHpLess(Stage.DacreimFortress0, 1, 0, 65);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.DacreimFortress0, 5, 1)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, QstLayoutFlag.Blockade)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, QstLayoutFlag.Nedo)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, QstLayoutFlag.Allies0)
            .AddResultCmdStopMessage();
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.DacreimFortress0, 1, 0, NpcId.Nedo0, 23223) // Speak with Nedo
            .AddResultCmdMyQstFlagOn(3605)
            .AddResultCmdQstTalkChg(NpcId.Meirova0, 23224)
            .AddResultCmdQstTalkChg(NpcId.Gillian0, 23225)
            .AddResultCmdQstTalkChg(NpcId.Bertha, 23226)
            .AddResultCmdQstTalkChg(NpcId.Elliot0, 23227)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 23228)
            .AddResultCmdQstTalkChg(NpcId.Lise0, 23229)
            //.AddResultCmdResetDiePlayerReturnPos(Stage.Invalid, 0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.Allies1);
        process0.AddIsStageNoBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.FortThines1) // Return to Fort Thines and speak with Nedo again
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.RathniteFoothillsLakeside.FortDacriumWallBreach)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.RathniteFoothillsLakeside.DacriumFortressEntrance);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.None, Stage.FortThines1, 0, 0, NpcId.Nedo0, 24173)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.FortThinesNpcs)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, QstLayoutFlag.Allies1)
            .AddResultCmdQstTalkChg(NpcId.Meirova0, 24174)
            .AddResultCmdQstTalkChg(NpcId.Gillian0, 24175)
            .AddResultCmdQstTalkChg(NpcId.Quintus, 24176)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 24177)
            .AddResultCmdQstTalkChg(NpcId.Lise0, 24178)
            .AddResultCmdQstTalkChg(NpcId.Elliot0, 24179);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.AudienceChamber, NpcId.TheWhiteDragon, 21568) // Return to Lestania and report to the White Dragon
            .AddResultCmdQstTalkChg(NpcId.Klaus0, 23230)
            .AddResultCmdQstTalkChg(NpcId.Joseph, 23231);
        process0.AddProcessEndBlock(true)
            .AddResultCmdTutorialDialog(TutorialId.TheLandofDespairPrologue);

        // First group
        var process1 = AddNewProcess(1);
        process1.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdIsLinkageEnemyFlag(Stage.DacreimFortress0, 1, 0, 2);
        process1.AddSpawnGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter + 3)
            .AddCheckCmdIsLinkageEnemyFlag(Stage.DacreimFortress0, 1, 0, 4);
        process1.AddRawBlock(QuestAnnounceType.None)
            .AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, 100316)
            .AddCheckCmdIsKilledTargetEnemySetGroup2NoMarker(13);
        process1.AddProcessEndBlock(false)
            .AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, 100315);

        // Second group
        var process2 = AddNewProcess(2);
        process2.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdEmHpLess(Stage.DacreimFortress0, 1, 0, 90);
        process2.AddSpawnGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter + 4)
            .AddCheckCmdIsLinkageEnemyFlag(Stage.DacreimFortress0, 1, 0, 4);
        process2.AddRawBlock(QuestAnnounceType.None)
            .AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, 100316)
            .AddCheckCmdIsKilledTargetEnemySetGroup2NoMarker(14);
        process2.AddProcessEndBlock(false)
            .AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, 100315);

        // Third group
        var process3 = AddNewProcess(3);
        process3.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdEmHpLess(Stage.DacreimFortress0, 1, 0, 80);
        process3.AddSpawnGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter + 5)
            .AddCheckCmdIsLinkageEnemyFlag(Stage.DacreimFortress0, 1, 0, 4);
        process3.AddRawBlock(QuestAnnounceType.None)
            .AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, 100317)
            .AddCheckCmdIsKilledTargetEnemySetGroup2NoMarker(15);
        process3.AddProcessEndBlock(false)
            .AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, 100315);

        // Fourth group
        var process4 = AddNewProcess(4);
        process4.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdEmHpLess(Stage.DacreimFortress0, 1, 0, 70);
        process4.AddSpawnGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter + 6)
            .AddCheckCmdIsLinkageEnemyFlag(Stage.DacreimFortress0, 1, 0, 4);
        process4.AddRawBlock(QuestAnnounceType.None)
            .AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, 100317)
            .AddCheckCmdIsKilledTargetEnemySetGroup2NoMarker(16);
        process4.AddProcessEndBlock(false)
            .AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, 100315);

        // Clean up groups
        var process5 = AddNewProcess(5);
        process5.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdEmHpLess(Stage.DacreimFortress0, 1, 0, 65);
        process5.AddRemoveGroupBlock(QuestAnnounceType.None, [EnemyGroupId.Encounter + 2, EnemyGroupId.Encounter + 6]);
        process5.AddProcessEndBlock(false);
    }
}

return new ScriptedQuest();
