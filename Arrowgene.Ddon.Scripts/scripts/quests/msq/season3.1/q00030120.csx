/**
 * @brief The Secret Entrance
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ScriptedQuest));

    public override QuestType QuestType => QuestType.Main;
    public override QuestId QuestId => QuestId.TheSecretEntrance;
    public override ushort RecommendedLevel => 87;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.AudienceChamber;
    public override QuestId NextQuestId => QuestId.ADesperateInfiltration;

    private class EnemyGroupId
    {
        public const uint Encounter = 10;
    }

    private class NamedParamId
    {
        public const uint OldDoorGuard0 = 1768; // Old Door Guard
        public const uint OldDoorGuard1 = 1769; // Old Door Guard
    }

    private class QstLayoutFlag
    {
        // Feryana Wilderness (st0132)
        public const uint FeryanaWildernessMeirova = 5596;

        // Fort Thines (st0443)
        public const uint FortThinesNpcs0 = 5594; // Gurdolin, Lise, Elliot
        public const uint FortThinesNpcs1 = 5595; // Nedo, Quintus, Bertha
        public const uint FortThinesMeirova = 5987;

        // Rothgill Traveler's Inn (st0631)
        public const uint RothgillInnGillian = 6520;

        // Royal Family's Secret Path (st1091)
        public const uint SecretPathNpcs = 5598; // Lise, Gurdolin, Elliot

    }

    private class MyQstFlag
    {
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumLevel(87));
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.DiversionaryTactics));
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
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.RoyalFamilysSecretPath, 5, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.Medusa, 87, 4, isBoss: true)
                .SetNamedEnemyParams(NamedParamId.OldDoorGuard0),
            LibDdon.Enemy.CreateAuto(EnemyId.DeathKnight, 87, 1)
                .SetIsBossGauge(true)
                .SetNamedEnemyParams(NamedParamId.OldDoorGuard1),
            LibDdon.Enemy.CreateAuto(EnemyId.DeathKnight, 87, 6)
                .SetIsBossGauge(true)
                .SetNamedEnemyParams(NamedParamId.OldDoorGuard1),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTalkAndOrderBlock(Stage.AudienceChamber, NpcId.Joseph, 21804)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.MephiteTravelersInn.Nayajiku)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.AudienceChamber.TheCrewEndSeason34);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Accept, Stage.FortThines1, 1, 0, NpcId.Nedo0, 21805)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.FortThinesNpcs0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.FortThinesNpcs1);
        process0.AddIsStageNoBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.RoyalFamilysSecretPath)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.FeryanaWildernessMeirova);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.CheckpointAndUpdate, EnemyGroupId.Encounter + 0);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0, resetGroup: false);
        process0.AddPartyGatherBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.RoyalFamilysSecretPath, 41, 1200, -59850);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.RoyalFamilysSecretPath, 0, 0, QuestJumpType.None, Stage.Invalid);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.RoyalFamilysSecretPath, 1, 0, NpcId.Lise0, 21811)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.SecretPathNpcs);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.FortThines1, 1, 0, NpcId.Nedo0, 21812);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.AudienceChamber, NpcId.Joseph, 21813);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
