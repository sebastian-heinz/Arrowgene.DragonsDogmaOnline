/**
 * @brief The Royal Family Sacrament
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ScriptedQuest));

    public override QuestType QuestType => QuestType.Main;
    public override QuestId QuestId => QuestId.TheRoyalFamilySacrament;
    public override ushort RecommendedLevel => 85;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.AudienceChamber;
    public override QuestId NextQuestId => QuestId.TheApproachingDemonArmy;

    private class GeneralAnnouncement
    {
        public const int AreaMasterUnlocked = 100272;
    }

    private class EnemyGroupId
    {
        public const uint Encounter = 10;
    }

    private class NamedParamId
    {
        public const uint GuardianOfTheSpring = 1802;
    }

    private class QstLayoutFlag
    {
        // st0443
        public const uint FortThinesNpcs0 = 5549; // Gurdolin, Elliot, Lise
        public const uint FortThinesNpcs1 = 5548; // Gillian, Quintus, Meirova, Nedo

        // st0131
        public const uint Gerhard = 5555; // Gerhard
        public const uint RothgilNpcs = 5645; // Gurdolin, Lise, Elliot, Meirova, Nedo

        // st1021
        public const uint Gurdolin = 5551;

        // st0631
        public const uint RothgilInnNpcs0 = 5554;
        public const uint RothgilInnNpcs1 = 5556;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumLevel(85));
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.PrinceNedo));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 700000);
        AddWalletReward(WalletType.Gold, 80000);
        AddWalletReward(WalletType.RiftPoints, 8000);

        AddFixedItemReward(ItemId.UnappraisedFlightTrinketGeneral, 2);
        AddFixedItemReward(ItemId.RoyalCrestMedalRathniteDistrict, 5);
        AddFixedItemReward(ItemId.ApRathniteFoothills, 50);
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.BeforetheSecretSpring, 1, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.Angules0, 85, 1, isBoss: true)
                .SetNamedEnemyParams(NamedParamId.GuardianOfTheSpring),
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.BeforetheSecretSpring, 2, QuestEnemyPlacementType.Manual, new()
        {
            /* prevent enemies from spawning */
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTouchAndOrderBlock(Stage.AudienceChamber, NpcId.Joseph, 0)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.MephiteTravelersInn.Nayajiku)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.BeforetheSecretSpring.DragonSpringOn)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.AudienceChamber.TheCrewEndSeason34)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.BeforetheSecretSpring.DragonSpringPresent)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.BeforetheSecretSpring.KeyMonsterGate);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.AudienceChamber, 200, 6);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Accept, Stage.FortThines1, 2, 0, NpcId.Meirova0, 21576)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.FortThinesNpcs0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.FortThinesNpcs1);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.FortThines1, 2, 2, NpcId.Nedo0, 21577);
        process0.AddPartyGatherBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.RathniteFoothillsLakeside0, -96548, 8404, -41579)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.Gerhard);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.RathniteFoothillsLakeside0, 5, 18);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.RathniteFoothillsLakeside0, 0, 0, NpcId.Gerhard, 21668)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, QstLayoutFlag.FortThinesNpcs0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, QstLayoutFlag.FortThinesNpcs1)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.RothgilNpcs);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.MephiteTravelersInn, NpcId.Nayajiku, 24182)
            .AddCheckCmdNewTalkNpcWithoutMarker(Stage.MephiteTravelersInn, 1, 0, QuestId.Q70030001);
        process0.AddRawBlock(QuestAnnounceType.CheckpointAndUpdate)
            .AddCheckCmdIsReleaseWarpPointAnyone(71)
            .AddResultCmdReleaseAnnounce(ContentsRelease.None, flagInfo: QuestFlags.NpcFunctions.FeryanaWildernessAreaInfo)
            .AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, GeneralAnnouncement.AreaMasterUnlocked);
        process0.AddIsStageNoBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.DarkPathtotheSecretSpring)
            .AddResultCmdReleaseAnnounce(ContentsRelease.None, flagInfo: QuestFlags.FeryanaWilderness.DarkPathToTheSecretSpring);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0, resetGroup: false);
        process0.AddPartyGatherBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.BeforetheSecretSpring, -20, 1601, -13383)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.BeforetheSecretSpring.KeyMonsterGate);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.BeforetheSecretSpring, 0, 0, QuestJumpType.None, Stage.Invalid);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.BeforetheSecretSpring, 0, 0, NpcId.Gurdolin3, 21690)
           .AddQuestFlag(QuestFlagAction.Set, QuestFlags.BeforetheSecretSpring.DragonSpringOn)
           .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.Gurdolin);
        process0.AddPartyGatherBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.RothgillTravelersInn, 167, -4, -312)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, QstLayoutFlag.Gerhard)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, QstLayoutFlag.RothgilNpcs)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.RothgilInnNpcs0);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.RothgillTravelersInn, 0, 0);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.AudienceChamber, NpcId.Joseph, 21706)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.RothgilInnNpcs1);
        process0.AddIsStageNoBlock(QuestAnnounceType.None, Stage.AudienceChamber)
            .AddResultCmdReleaseAnnounce(ContentsRelease.FeryanaWildernessWorldQuests);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
