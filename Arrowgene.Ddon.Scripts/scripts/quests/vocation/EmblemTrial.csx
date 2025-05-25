/**
 * @brief Vocation Emblem Trial Template
 */

#load "libs.csx"

public abstract class EmblemTrial : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override ushort RecommendedLevel => 70;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.HobolicCave;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.QuestUsefulForAdventure;

    public abstract JobId QuestJobId { get; }
    public abstract uint SuraboQstLayoutFlag { get; }
    public abstract uint SuraboMsgId0 { get; }
    public abstract uint SuraboMsgId1 { get; }
    public abstract uint GilstanMsgId0 { get; }
    public abstract uint GilstanMsgId1 { get; }
    public abstract uint RentonMsgId0 { get; }

    public abstract ItemId EmblemItemId { get; }
    public abstract ContentsRelease JobEmblemContentsRelease { get; }

    public override bool ShowInAdventureGuide(GameClient client)
    {
        return client.Character.HasQuestCompleted(QuestId.TheEmblemStonesWhereabouts) &&
               client.Character.ActiveCharacterJobData.Job == QuestJobId;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumVocationLevel(QuestJobId, 70));
        AddQuestOrderCondition(QuestOrderCondition.Solo());
    }

    private class NamedParamId
    {
        public const uint Trial0 = 1827; // Trial <name>
        public const uint Trial1 = 1834; // Trial <name>
    }

    private class EnemyGroupId
    {
        public const uint Encounter = 10;
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 15350);
        AddWalletReward(WalletType.Gold, 4618);
        AddWalletReward(WalletType.RiftPoints, 600);
        AddFixedItemReward(EmblemItemId, 1);
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.TrainingChapel, 1, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.OrcBattler, 65, 4)
                .SetNamedEnemyParams(NamedParamId.Trial0),
            LibDdon.Enemy.CreateAuto(EnemyId.OrcBattler, 65, 5)
                .SetNamedEnemyParams(NamedParamId.Trial0),
            LibDdon.Enemy.CreateAuto(EnemyId.OrcSoldier0, 65, 8)
                .SetNamedEnemyParams(NamedParamId.Trial0),
            LibDdon.Enemy.CreateAuto(EnemyId.OrcSoldier0, 65, 9)
                .SetNamedEnemyParams(NamedParamId.Trial0),
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.TrainingChapel, 5, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.OrcSoldier0, 65, 4)
                .SetNamedEnemyParams(NamedParamId.Trial0),
            LibDdon.Enemy.CreateAuto(EnemyId.OrcBattler, 65, 5)
                .SetNamedEnemyParams(NamedParamId.Trial0),
            LibDdon.Enemy.CreateAuto(EnemyId.OrcSoldier0, 65, 8)
                .SetNamedEnemyParams(NamedParamId.Trial0),
            LibDdon.Enemy.CreateAuto(EnemyId.OrcBattler, 65, 9)
                .SetNamedEnemyParams(NamedParamId.Trial0),
        });

        AddEnemies(EnemyGroupId.Encounter + 2, Stage.TrainingChapel, 1, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.CaptainOrc0, 65, 14)
                .SetNamedEnemyParams(NamedParamId.Trial1),
            LibDdon.Enemy.CreateAuto(EnemyId.OrcAimer, 65, 11)
                .SetNamedEnemyParams(NamedParamId.Trial1),
            LibDdon.Enemy.CreateAuto(EnemyId.OrcBattler, 65, 10)
                .SetNamedEnemyParams(NamedParamId.Trial1),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdIsTutorialQuestClear(QuestId.TheEmblemStonesWhereabouts);
        process0.AddNpcTalkAndOrderBlock(Stage.HobolicCave, NpcId.Gilstan, GilstanMsgId0);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Accept, Stage.TrainingChapel, 0, 0, NpcId.Surabo, SuraboMsgId0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, SuraboQstLayoutFlag);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0, resetGroup: false);
        process0.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter + 1);
        process0.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter + 2);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TrainingChapel, 0, 0, NpcId.Surabo, SuraboMsgId1);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.HobolicCave, NpcId.Gilstan, GilstanMsgId1);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, NpcId.Renton0, RentonMsgId0);
        process0.AddIsStageNoBlock(QuestAnnounceType.None, Stage.TheWhiteDragonTemple0)
            .AddResultCmdReleaseAnnounce(JobEmblemContentsRelease, TutorialId.EmblemStones);
        process0.AddProcessEndBlock(true);
    }
}
