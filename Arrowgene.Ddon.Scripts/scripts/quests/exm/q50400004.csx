/**
 * @brief Arisen of the Black Dragon
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.ExtremeMission;
    public override QuestId QuestId => (QuestId)50400004;
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
        MissionParams.PlaytimeInSeconds = 600;
        MissionParams.ArmorAllowed = true;
        MissionParams.JewelryAllowed = true;
        MissionParams.MaxPawns = 3;

        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.TheWhiteDragonsArisen));
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
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.EastLandofDarkness, 1, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.ArisenOfTheBlackDragon, 100, 0, 0)
                .SetStartThinkTblNo(1)
                .SetIsBoss(true),
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.EastLandofDarkness, 4, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.BlackSword1, 100, 0, 0)
                .SetRepopCount(50)
                .SetStartThinkTblNo(1)
                .SetEnemyTargetTypesId(1)
                .SetIsManualSet(true),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCommands([
                QuestManager.CheckCommand.IsGatherPartyInStage(3300)
            ]);
        process0.AddSpawnGroupsBlock(QuestAnnounceType.None, [EnemyGroupId.Encounter + 0, EnemyGroupId.Encounter + 1])
            .AddResultCommands([
                QuestManager.ResultCommand.StartMissionAnnounce(1),
                QuestManager.ResultCommand.AddEndContentsPurpose(0, 1),
                QuestManager.ResultCommand.StartContentsTimer(600),
                QuestManager.ResultCommand.GetDragonAbility(1) // Dragon protection
            ])
            .AddCheckCommands([
                QuestManager.CheckCommand.DieEnemy(3300, 1, 0)
            ]);
        process0.AddRemoveGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter + 1)
            .AddResultCommands([
                QuestManager.ResultCommand.EventExec(3300, 15, 0, 0),
                QuestManager.ResultCommand.StopCycleTimer()
            ])
            .AddCheckCommands([
                QuestManager.CheckCommand.EventEnd(3300, 15)
            ]);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddResultCommands([
                QuestManager.ResultCommand.UpdateAnnounceDirect(1, 3),
                QuestManager.ResultCommand.RemoveEndContentsPurpose(0),
                QuestManager.ResultCommand.AddEndContentsPurpose(1, 1),
                QuestManager.ResultCommand.QstLayoutFlagOn(8670),
                QuestManager.ResultCommand.GetDragonAbility(2), // Dragon blessing
                QuestManager.ResultCommand.Prt(3300, 2530, 10, -480)
            ])
            .AddCheckCommands([
                QuestManager.CheckCommand.Prt(3300, 2530, 10, -480)
            ]);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
