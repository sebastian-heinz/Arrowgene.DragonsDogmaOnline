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
    public override QuestId NextQuestId => QuestId.None;

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

    private class MyQstFlag
    {
        public const uint StartAdds = 1;
        public const uint EndAdds = 2;
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
            LibDdon.Enemy.CreateAuto(EnemyId.SquadLeaderDwarfOrc, 84, 0)
                .SetNamedEnemyParams(NamedParamId.FortGuard),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordSoldierDwarfOrc, 84, 2)
                .SetNamedEnemyParams(NamedParamId.FortGuard),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordSoldierDwarfOrc, 84, 3)
                .SetNamedEnemyParams(NamedParamId.FortGuard),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordSoldierDwarfOrc, 84, 4)
                .SetNamedEnemyParams(NamedParamId.FortGuard),
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.DacreimFortress0, 2, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.WarReadyGoremanticoreLightArmor, 84, 1, isBoss: true)
                .SetInfectionType(1)
                .SetNamedEnemyParams(NamedParamId.FortOccupyingWarReadyManticore),

            LibDdon.Enemy.CreateAuto(EnemyId.WarReadyGrimwargLightArmor, 84, 0)
                .SetInfectionType(1)
                .SetNamedEnemyParams(NamedParamId.FortOccupying1),
            LibDdon.Enemy.CreateAuto(EnemyId.WarReadyGrimwargLightArmor, 84, 2)
                .SetInfectionType(1)
                .SetNamedEnemyParams(NamedParamId.FortOccupying1),
            LibDdon.Enemy.CreateAuto(EnemyId.WarReadyGrimwargLightArmor, 84, 3)
                .SetInfectionType(1)
                .SetNamedEnemyParams(NamedParamId.FortOccupying1),
            LibDdon.Enemy.CreateAuto(EnemyId.WarReadyGrimwargLightArmor, 84, 4)
                .SetInfectionType(1)
                .SetNamedEnemyParams(NamedParamId.FortOccupying1),
        });

        AddEnemies(EnemyGroupId.Encounter + 2, Stage.DacreimFortress0, 1, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.BeastMaster0, 84, 0, isBoss: true)
                .SetNamedEnemyParams(NamedParamId.BeastCommander0),
        });

        AddEnemies(EnemyGroupId.Encounter + 3, Stage.DacreimFortress0, 4, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.Strix, 84, 1)
                .SetRepopConditions(50, 30),
            LibDdon.Enemy.CreateAuto(EnemyId.Strix, 84, 4)
                .SetRepopConditions(50, 30),
            LibDdon.Enemy.CreateAuto(EnemyId.Warg, 84, 2)
                .SetRepopConditions(50, 30),
            LibDdon.Enemy.CreateAuto(EnemyId.Warg, 84, 5)
                .SetRepopConditions(50, 30),
            LibDdon.Enemy.CreateAuto(EnemyId.Grimwarg, 84, 10)
                .SetRepopConditions(50, 30),
            LibDdon.Enemy.CreateAuto(EnemyId.Grimwarg, 84, 11)
                .SetRepopConditions(50, 30),
/*
            LibDdon.Enemy.CreateAuto(EnemyId.WarReadyGrimwargLightArmor, 84, 6)
                .SetInfectionType(1)
                .SetRepopConditions(50, 90),
            LibDdon.Enemy.CreateAuto(EnemyId.WarReadyGrimwargLightArmor, 84, 7)
                .SetInfectionType(1)
                .SetRepopConditions(50, 90),
*/

            LibDdon.Enemy.CreateAuto(EnemyId.Cragger, 84, 8),
            LibDdon.Enemy.CreateAuto(EnemyId.BlackGriffin0, 84, 9)
                .SetStartThinkTblNo(2)
                .SetNamedEnemyParams(NamedParamId.Powerful),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTalkAndOrderBlock(Stage.AudienceChamber, NpcId.Joseph, 21492)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.AudienceChamber.TheCrewEndSeason34);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Accept, Stage.BerthasBanditGroupHideout, 0, 0, NpcId.Gillian0, 21493) // Head to the bandit hideout in the Lakeside Grotto
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.BanditNpcs);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.RathniteFoothillsLakeside0, 1, 1, NpcId.Meirova0, 24166) // Head to Rothgill and speak with Meirova
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.RothgillNpcs);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.RathniteFoothillsLakeside0, 0, 0, NpcId.RothgillSoldier0, 24170) // Head to the soldier before Dacreim Fortress
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.RothgillSoldiers)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.RathniteFoothillsLakeside.FortDacriumWallBreach);
        process0.AddDestroyGroupBlock(QuestAnnounceType.CheckpointAndUpdate, EnemyGroupId.Encounter + 0); // Defeat the obstructing Orcs to storm into Dacreim Fortress            
        process0.AddDiscoverGroupBlock(QuestAnnounceType.CheckpointAndUpdate, EnemyGroupId.Encounter + 1)  // Raid Dacreim Fortress
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.RathniteFoothillsLakeside.DacriumFortressEntrance);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1, resetGroup: false); // Eliminate the War-Ready Manticore and the War-Ready Grimwarg
        process0.AddPartyGatherBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.DacreimFortress0, 44, 80, 10448) // Rescue Prince Nedo
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.Nedo);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.DacreimFortress0, 0, 4);
        process0.AddSpawnGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 2) // Eliminate the enemy
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.Allies0)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQstFlag.StartAdds)
            .AddCheckCmdEmHpLess(Stage.DacreimFortress0, 1, 0, 65)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.Blockade);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.DacreimFortress0, 5, 1)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQstFlag.EndAdds);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.DacreimFortress0, 1, 0, NpcId.Nedo0, 23223) // Speak with Nedo
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, QstLayoutFlag.Nedo)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, QstLayoutFlag.Allies0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.Allies1);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.FortThines1, 0, 0, NpcId.Nedo0, 24173) // Return to Fort Thines and speak with Nedo again
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.FortThinesNpcs)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.RathniteFoothillsLakeside.FortDacriumWallBreach)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.RathniteFoothillsLakeside.DacriumFortressEntrance);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.AudienceChamber, NpcId.TheWhiteDragon, 21568); // Return to Lestania and report to the White Dragon
        process0.AddIsStageNoBlock(QuestAnnounceType.None, Stage.AudienceChamber)
            .AddResultCmdReleaseAnnounce(ContentsRelease.SpecialSkillAugmentation, TutorialId.SkillInheritanceOfUltimateSkills, flagInfo: QuestFlags.NpcFunctions.SpecialSkillAugmentation)
            .AddResultCmdTutorialDialog(TutorialId.TheLandofDespairPrologue);
        process0.AddProcessEndBlock(true);

        // Handle spawning and erasing the groups
        var process1 = AddNewProcess(1);
        process1.AddMyQstFlagsBlock(QuestAnnounceType.None)
            .AddMyQstCheckFlag(MyQstFlag.StartAdds);
        process1.AddSpawnGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter + 3)
            .AddCheckCmdMyQstFlagOn(MyQstFlag.EndAdds);
        process1.AddRemoveGroupBlock(QuestAnnounceType.None, [
            EnemyGroupId.Encounter + 2,
            EnemyGroupId.Encounter + 3
        ]);
        process1.AddProcessEndBlock(false);
    }
}

return new ScriptedQuest();
