/**
 * @brief Portent of Despair
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ScriptedQuest));

    public override QuestType QuestType => QuestType.Main;
    public override QuestId QuestId => QuestId.PortentOfDespair;
    public override ushort RecommendedLevel => 85;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.AudienceChamber;
    public override QuestId NextQuestId => QuestId.DiversionaryTactics;

    private class EnemyGroupId
    {
        public const uint Encounter = 10;
    }

    private class NamedParamId
    {
        public const uint RothgillRaidForce = 1825; // Rothgill Raid Force
        public const uint Marching0 = 1763; // Marching <name>
        public const uint Marching1 = 1764; // Marching <name>
    }

    private class QstLayoutFlag
    {
        // st0131 (Rathnite Foothills Lakeside)
        public const uint RothgillNpcs = 6104; // Gerhard, Rothgill Soldier, Gurdolin, Elliot

        // st0132 (Feryana)
        public const uint Npcs0 = 5575; // Nedo, Gurdolin, Lise, Elliot
        public const uint BanditGroup = 5687; // Bertha's Bandit Group
        public const uint Blockade = 6313;

        // st0443 (Fort Thines)
        public const uint Nedo = 5571; // Nedo
        public const uint FortThinesNpcs = 5570; // Quintus, Meirova, Gillian
        public const uint Lise = 6102;
        public const uint GurdolinElliot = 6103; // Gurdolin, Elliot

        // st1001
        public const uint Bertha = 5572;
        public const uint BanditCampNpcs = 5573; // Gurdolin, Elliot, Lise, Nedo
    }

    private class MyQstFlag
    {
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumLevel(85));
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.TheApproachingDemonArmy));
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
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.RathniteFoothillsLakeside0, 18, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.HeavySoldierDwarfOrc, 85, 11)
                .SetNamedEnemyParams(NamedParamId.RothgillRaidForce),
            LibDdon.Enemy.CreateAuto(EnemyId.SquadLeaderDwarfOrc, 85, 12)
                .SetNamedEnemyParams(NamedParamId.RothgillRaidForce),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordSoldierDwarfOrc, 85, 13)
                .SetNamedEnemyParams(NamedParamId.RothgillRaidForce),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordSoldierDwarfOrc, 85, 14)
                .SetNamedEnemyParams(NamedParamId.RothgillRaidForce),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordSoldierDwarfOrc, 85, 15)
                .SetNamedEnemyParams(NamedParamId.RothgillRaidForce),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordSoldierDwarfOrc, 85, 16)
                .SetNamedEnemyParams(NamedParamId.RothgillRaidForce),
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.FeryanaWilderness, 1, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.Catoblepas, 85, 6, isBoss: true)
                .SetNamedEnemyParams(NamedParamId.Marching1),
            LibDdon.Enemy.CreateAuto(EnemyId.BluntSoldierDwarfOrc, 85, 4)
                .SetNamedEnemyParams(NamedParamId.Marching0),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordSoldierDwarfOrc, 85, 5)
                .SetNamedEnemyParams(NamedParamId.Marching0),
            LibDdon.Enemy.CreateAuto(EnemyId.BluntSoldierDwarfOrc, 85, 7)
                .SetNamedEnemyParams(NamedParamId.Marching0),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTalkAndOrderBlock(Stage.AudienceChamber, NpcId.Joseph, 21728)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.MephiteTravelersInn.Nayajiku)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.BerthasBanditGroupHideout.Bertha)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.AudienceChamber.TheCrewEndSeason34);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Accept, Stage.FortThines1, 1, 1, NpcId.Meirova0, 21729)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.Nedo)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.Lise)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.GurdolinElliot)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.FortThinesNpcs);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.FortThines1, 1, 2, NpcId.Gillian0, 21730);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.RathniteFoothillsLakeside0, 0, 0, NpcId.Gerhard, 24184)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.RothgillNpcs);
        process0.AddDestroyGroupBlock(QuestAnnounceType.CheckpointAndUpdate, EnemyGroupId.Encounter + 0);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.RathniteFoothillsLakeside0, 0, 0, NpcId.Gerhard, 24189);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.BerthasBanditGroupHideout, 0, 0, NpcId.Bertha, 21731)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.Bertha)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.BanditCampNpcs);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.BerthasBanditGroupHideout, 1, 3, NpcId.Nedo0, 21732);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.FeryanaWilderness, 1, 0, NpcId.BerthasBanditGroup0, 24194)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.BanditGroup);
        process0.AddPartyGatherBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.FeryanaWilderness, -130869, 17625, -102357);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.FeryanaWilderness, 0, 0, QuestJumpType.None, Stage.Invalid);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.Blockade);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.FeryanaWilderness, 5, 0, QuestJumpType.None, Stage.Invalid);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.FeryanaWilderness, 0, 0, NpcId.Nedo0, 21764)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, QstLayoutFlag.Blockade)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.Npcs0);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.BerthasBanditGroupHideout, 0, 0, NpcId.Bertha, 21765);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.AudienceChamber, NpcId.TheWhiteDragon, 21766);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
