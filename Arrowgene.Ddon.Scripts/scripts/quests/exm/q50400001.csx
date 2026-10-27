/**
 * @brief [Hard] Gatekeeper of the Dark World
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.ExtremeMission;
    public override QuestId QuestId => (QuestId)50400001;
    public override ushort RecommendedLevel => 100;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => false;

    private class EnemyGroupId
    {
        public const uint Encounter = 10;
    }

    protected override void InitializeState()
    {
        MissionParams.Group = ExtremeMissionUtils.Group.Travers;
        MissionParams.MinimumMembers = 1;
        MissionParams.MaximumMembers = 4;
        MissionParams.IsSolo = false;
        MissionParams.PlaytimeInSeconds = 900;
        MissionParams.ArmorAllowed = true;
        MissionParams.JewelryAllowed = true;
        MissionParams.MaxPawns = 3;

        AddQuestOrderCondition(QuestOrderCondition.ExtremeMissionCleared((QuestId)50400000));
        AddQuestOrderCondition(QuestOrderCondition.MinimumLevel(100));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 0);
        AddWalletReward(WalletType.Gold, 0);
        AddWalletReward(WalletType.RiftPoints, 0);

        AddFixedItemReward(ItemId.KeystoneToRuin, 10, QuestRewardBucket.FirstClear);
        AddFixedItemReward(ItemId.KeystoneToRuin, 2, QuestRewardBucket.PeriodFirstClear);
        AddFixedItemReward(ItemId.CoinPouch10000G, 1, QuestRewardBucket.RepeatClear);
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.Encounter, Stage.WestLandofDarkness, 1, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.Abaddon0, 100, 0, 0)
                .SetIsBoss(true),
            LibDdon.Enemy.Create((EnemyId)0x30111, 100, 0, 1)
                .SetIsRequired(false),
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.WestLandofDarkness, 3, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.AlbeParasiticPoison, 100, 0, 0)
                .SetRepopCount(50)
                .SetStartThinkTblNo(6)
				.SetEnemyTargetTypesId(1)
                .SetIsManualSet(true),
            LibDdon.Enemy.Create(EnemyId.AlbeParasiticPoison, 100, 0, 1)
                .SetRepopCount(50)
                .SetStartThinkTblNo(6)
				.SetEnemyTargetTypesId(1)
                .SetIsManualSet(true),
            LibDdon.Enemy.Create(EnemyId.AlbeParasiticPoison, 100, 0, 2)
                .SetRepopCount(50)
                .SetStartThinkTblNo(6)
				.SetEnemyTargetTypesId(1)
                .SetIsManualSet(true),
            LibDdon.Enemy.Create(EnemyId.AlbeParasiticPoison, 100, 0, 3)
                .SetRepopCount(50)
                .SetStartThinkTblNo(6)
				.SetEnemyTargetTypesId(1)
                .SetIsManualSet(true),
            LibDdon.Enemy.Create(EnemyId.AlbeParasiticPoison, 100, 0, 4)
                .SetRepopCount(50)
                .SetStartThinkTblNo(6)
				.SetEnemyTargetTypesId(1)
                .SetIsManualSet(true),
            LibDdon.Enemy.Create(EnemyId.AlbeParasiticPoison, 100, 0, 5)
                .SetRepopCount(50)
                .SetStartThinkTblNo(6)
				.SetEnemyTargetTypesId(1)
                .SetIsManualSet(true),
            LibDdon.Enemy.Create(EnemyId.AlbeParasiticPoison, 100, 0, 6)
                .SetRepopCount(50)
                .SetStartThinkTblNo(6)
				.SetEnemyTargetTypesId(1)
                .SetIsManualSet(true),
            LibDdon.Enemy.Create(EnemyId.AlbeParasiticPoison, 100, 0, 7)
                .SetRepopCount(50)
                .SetStartThinkTblNo(6)
				.SetEnemyTargetTypesId(1)
                .SetIsManualSet(true),
            LibDdon.Enemy.Create(EnemyId.AlbeParasiticPoison, 100, 0, 8)
                .SetRepopCount(50)
                .SetStartThinkTblNo(6)
				.SetEnemyTargetTypesId(1)
                .SetIsManualSet(true),
            LibDdon.Enemy.Create(EnemyId.AlbeParasiticPoison, 100, 0, 9)
                .SetRepopCount(50)
                .SetStartThinkTblNo(6)
				.SetEnemyTargetTypesId(1)
                .SetIsManualSet(true),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCommands([
                QuestManager.CheckCommand.IsGatherPartyInStage(3100)
            ]);
        process0.AddSpawnGroupsBlock(QuestAnnounceType.None, [EnemyGroupId.Encounter, EnemyGroupId.Encounter + 1])
            .AddResultCommands([
                QuestManager.ResultCommand.StartMissionAnnounce(1),
                QuestManager.ResultCommand.AddEndContentsPurpose(0, 1),
                QuestManager.ResultCommand.StartContentsTimer(600),
                QuestManager.ResultCommand.GetDragonAbility(1)
            ])
            .AddCheckCommands([
                QuestManager.CheckCommand.DieEnemy(3100, 1, 0)
            ]);
        process0.AddRemoveGroupBlock(QuestAnnounceType.None, [EnemyGroupId.Encounter, EnemyGroupId.Encounter + 1])
            .AddResultCommands([
                QuestManager.ResultCommand.EventExec(3100, 5, 0, 0),
                QuestManager.ResultCommand.StopCycleTimer()
            ])
            .AddCheckCommands([
                QuestManager.CheckCommand.EventEnd(3100, 5)
            ]);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddResultCommands([
                QuestManager.ResultCommand.UpdateAnnounceDirect(1, 3),
                QuestManager.ResultCommand.RemoveEndContentsPurpose(0),
                QuestManager.ResultCommand.AddEndContentsPurpose(1, 1),
                QuestManager.ResultCommand.QstLayoutFlagOn(8742),
                QuestManager.ResultCommand.GetDragonAbility(2),
                QuestManager.ResultCommand.Prt(3100, 2510, 10, -475)
            ])
            .AddCheckCommands([
                QuestManager.CheckCommand.Prt(3100, 2510, 10, -475)
            ]);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
