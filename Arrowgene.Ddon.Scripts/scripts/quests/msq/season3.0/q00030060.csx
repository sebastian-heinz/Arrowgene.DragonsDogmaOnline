/**
 * @brief The Opposition
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ScriptedQuest));

    public override QuestType QuestType => QuestType.Main;
    public override QuestId QuestId => QuestId.TheOpposition;
    public override ushort RecommendedLevel => 82;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.AudienceChamber;
    public override QuestId NextQuestId => QuestId.PrinceNedo;

    private class EnemyGroupId
    {
        public const uint Encounter = 10;
    }

    private class NamedParamId
    {
        public const uint MasterOfTheGrotto = 1756; // Master of the Grotto
        public const uint Obstructing = 1757; // Obstructing <name>
    }

    private class QstLayoutFlag
    {
        // Fort Thines
        public const uint GurdolinLiseElliot = 5410; // Fort Thines (Gurdolin, Lise, Elliot)
        public const uint Quintus = 5411;

        // Bandit Hideout
        public const uint NpcTeam = 5402; // Sly, Meirova, Gillian, Gurdolin, Lise, Elliot
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumLevel(82));
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.SurvivorsVillage));
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
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.LakesideGrotto, 12, QuestEnemyPlacementType.Manual, new()
        {
            // Boss
            LibDdon.Enemy.CreateAuto(EnemyId.Troll, 82, 2, isBoss: true)
                .SetNamedEnemyParams(NamedParamId.MasterOfTheGrotto),
            // Adds
            LibDdon.Enemy.CreateAuto(EnemyId.LittleCrag, 82, 1)
                .SetNamedEnemyParams(NamedParamId.Obstructing),
            LibDdon.Enemy.CreateAuto(EnemyId.LittleCrag, 82, 3)
                .SetNamedEnemyParams(NamedParamId.Obstructing),
            LibDdon.Enemy.CreateAuto(EnemyId.LittleCrag, 82, 4)
                .SetNamedEnemyParams(NamedParamId.Obstructing),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTalkAndOrderBlock(Stage.AudienceChamber, NpcId.Joseph, 21467)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.AudienceChamber.TheCrewEndSeason34);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Accept, Stage.FortThines1, 1, 0, NpcId.Quintus, 21468)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.GurdolinLiseElliot)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.Quintus);
        process0.AddIsStageNoBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.LakesideGrotto);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.CheckpointAndUpdate, EnemyGroupId.Encounter + 0);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0, resetGroup: false);
        process0.AddPartyGatherBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.BerthasBanditGroupHideout, -4014, 7, -5360)
            .AddResultCmdReleaseAnnounce(ContentsRelease.None, flagInfo: QuestFlags.LakesideGrotto.GateToBandGroupHide);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.BerthasBanditGroupHideout, 0, 3);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.BerthasBanditGroupHideout, 0, 0, NpcId.Sly, 21489)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, QstLayoutFlag.GurdolinLiseElliot)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.NpcTeam);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.BerthasBanditGroupHideout, 0, 1, NpcId.Meirova0, 21490);
        process0.AddRawBlock(QuestAnnounceType.CheckpointAndUpdate)
            .AddCheckCmdIsReleaseWarpPointAnyone(73);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.AudienceChamber, NpcId.Joseph, 21491);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
