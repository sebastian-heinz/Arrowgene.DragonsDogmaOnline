/**
 * @brief The Land of Despair
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Main;
    public override QuestId QuestId => QuestId.TheLandOfDespair;
    public override ushort RecommendedLevel => 80;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.AudienceChamber;
    public override QuestId NextQuestId => QuestId.InSearchOfHope;

    private class EnemyGroupId
    {
        public const uint Encounter = 10;
    }

    private class NamedParamId
    {
        public const uint Mysterious = 1750; // Mysterious <name>
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumLevel(80));
        AddQuestOrderCondition(QuestOrderCondition.SoloWithPawns());
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.TheNewGeneration));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 700000);
        AddWalletReward(WalletType.Gold, 70000);
        AddWalletReward(WalletType.RiftPoints, 7000);

        AddFixedItemReward(ItemId.UnappraisedSnowTrinketGeneral, 2);
        AddFixedItemReward(ItemId.RoyalCrestMedalRathniteDistrict, 5);
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.RathniteFoothills, 1, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.SwordSoldierDwarfOrc, 80, 11)
                .SetNamedEnemyParams(NamedParamId.Mysterious),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordSoldierDwarfOrc, 80, 12)
                .SetNamedEnemyParams(NamedParamId.Mysterious),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordSoldierDwarfOrc, 80, 13)
                .SetNamedEnemyParams(NamedParamId.Mysterious),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordSoldierDwarfOrc, 80, 14)
                .SetNamedEnemyParams(NamedParamId.Mysterious),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTalkAndOrderBlock(Stage.AudienceChamber, NpcId.Joseph, 23271)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.AudienceChamber.TheCrewEndSeason34);
        process0.AddRawBlock(QuestAnnounceType.Accept)
            .AddCheckCmdTouchActToNpc(Stage.TheWhiteDragonTemple0, NpcId.Seneka0);
        //  AddPlayEventBlock(this QuestProcess process, QuestAnnounceType announceType, StageInfo stageInfo, uint eventId, uint startPos, QuestJumpType jumpType, StageInfo eventStageInfo)
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.TheWhiteDragonTemple0, 20, 55, QuestJumpType.After, Stage.AudienceChamber);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.AudienceChamber.AsStageLayoutId(0), 195, 0, QuestJumpType.After, Stage.RathniteFoothills.AsStageLayoutId(13));
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.RathniteFoothills, 0, 0);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0)
            .AddResultCmdReleaseAnnounce(ContentsRelease.None, flagInfo: QuestFlags.TheWhiteDragonTemple0.Season3Warp)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 5362); // "Spawn Lise, Elliot and Gurdolin"
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.RathniteFoothills, 5, 0);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.RathniteFoothills, 1, 0, NpcId.Gurdolin3, 21215)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 5362)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 5361); // "Spawn Lise, Elliot and Gurdolin"
        process0.AddSceHitInBlock(QuestAnnounceType.Update, Stage.RathniteFoothills, 2);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.RathniteFoothills, 2, 1, NpcId.Quintus, 21216)
           .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 5361)
           .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 5365); // "Spawn Gillian and Quintus"
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.RathniteFoothills, 2, 0, NpcId.Gillian0, 21217);
        process0.AddRawBlock(QuestAnnounceType.CheckpointAndUpdate)
            .AddCheckCmdIsReleaseWarpPointAnyone(68);
        process0.AddPartyGatherBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.RathniteFoothills, 834, 4614, 1370);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.RathniteFoothills, 10, 12);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.RathniteFoothills, 3, 2, NpcId.Elliot0, 21245)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 5369); // Spawn Elliot, Lise, Gurdolin
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.AudienceChamber, NpcId.TheWhiteDragon, 21246);
        process0.AddProcessEndBlock(true)
            .AddResultCmdBgmStop();
    }
}

return new ScriptedQuest();
