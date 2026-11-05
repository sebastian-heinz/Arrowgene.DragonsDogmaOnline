/**
 * @brief [Hard] Blue Shadow Dancing in Revelry
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.ExtremeMission;
    public override QuestId QuestId => (QuestId)50400003;
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

        AddQuestOrderCondition(QuestOrderCondition.ExtremeMissionCleared((QuestId)50400002));
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
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.SouthLandofDarkness, 1, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.Baphomet0, 100, 0, 0)
                .SetStartThinkTblNo(1)
                .SetIsBoss(true),
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.SouthLandofDarkness, 2, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.BaphometEnraged, 100, 0, 1)
                .SetStartThinkTblNo(2)
                .SetIsBoss(true)
                .SetIsManualSet(true),
            LibDdon.Enemy.Create(EnemyId.BaphometEnraged, 100, 0, 2)
                .SetStartThinkTblNo(2)
                .SetIsBoss(true)
                .SetIsManualSet(true),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCommands([
                QuestManager.CheckCommand.IsGatherPartyInStage(3200)
            ]);
        process0.AddSpawnGroupsBlock(QuestAnnounceType.None, [EnemyGroupId.Encounter + 0, EnemyGroupId.Encounter + 1])
            .AddResultCommands([
                QuestManager.ResultCommand.StartMissionAnnounce(1),
                QuestManager.ResultCommand.AddEndContentsPurpose(0, 1),
                QuestManager.ResultCommand.StartContentsTimer(600),
                QuestManager.ResultCommand.GetDragonAbility(1)
            ])
            .AddCheckCommands([
                QuestManager.CheckCommand.DieEnemy(3200, 1, 0)
            ]);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddResultCommands([
                QuestManager.ResultCommand.EventExec(3200, 5, 0, 0),
                QuestManager.ResultCommand.StopCycleTimer()
            ])
            .AddCheckCommands([
                QuestManager.CheckCommand.EventEnd(3200, 5)
            ]);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddResultCommands([
                QuestManager.ResultCommand.UpdateAnnounceDirect(1, 3),
                QuestManager.ResultCommand.RemoveEndContentsPurpose(0),
                QuestManager.ResultCommand.AddEndContentsPurpose(1, 1),
                QuestManager.ResultCommand.QstLayoutFlagOn(8737),
                QuestManager.ResultCommand.GetDragonAbility(2),
                QuestManager.ResultCommand.Prt(3200, 2510, 10, -475)
            ])
            .AddCheckCommands([
                QuestManager.CheckCommand.Prt(3200, 2510, 10, -475)
            ]);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
