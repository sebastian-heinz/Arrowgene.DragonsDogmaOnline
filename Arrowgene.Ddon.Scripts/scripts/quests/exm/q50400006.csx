/**
 * @brief Power that Destroys Reason
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.ExtremeMission;
    public override QuestId QuestId => (QuestId)50400006;
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

        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.TheFateOfAll));
        AddQuestOrderCondition(QuestOrderCondition.MinimumLevel(100));
    }

	// TODO: Add rewards
    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 0);
        AddWalletReward(WalletType.Gold, 0);
        AddWalletReward(WalletType.RiftPoints, 0);
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.NorthLandofDarkness, 1, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.BlackDragon1stForm1, 100, 0, 0)
                .SetIsBoss(true),
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.NorthLandofDarkness, 3, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.BlackDragon2ndForm, 100, 0, 0)
                .SetStartThinkTblNo(6)
                .SetIsBoss(true),
            LibDdon.Enemy.Create(EnemyId.DragonCrystalOfDestructionShootsLaserRocks, 100, 0, 1)
				.SetStartThinkTblNo(1)
                .SetIsManualSet(true)
                .SetIsRequired(false),
            LibDdon.Enemy.Create(EnemyId.DragonCrystalOfDestructionSucksYouInWithMagic0, 100, 0, 2)
				.SetStartThinkTblNo(2)
                .SetIsManualSet(true)
                .SetIsRequired(false),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCommands([
                QuestManager.CheckCommand.IsGatherPartyInStage(3400)
            ]);
        process0.AddSpawnGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter + 0)
            .AddResultCommands([
                QuestManager.ResultCommand.StartMissionAnnounce(1),
                QuestManager.ResultCommand.AddEndContentsPurpose(0, 1),
                QuestManager.ResultCommand.SetDiePlayerReturnPos(3400, 0, 0),
                QuestManager.ResultCommand.StartContentsTimer(900),
                QuestManager.ResultCommand.GetDragonAbility(1) // Dragon protection
            ])
            .AddCheckCommands([
                QuestManager.CheckCommand.IsLinkageEnemyFlag(3400, 1, 0, 1)
            ]);
        process0.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter + 1) // BGM request fix here?
            .AddResultCommands([
                QuestManager.ResultCommand.QstLayoutFlagOn(8764)
            ]);
        process0.AddRawBlock(QuestAnnounceType.None) // Send stop timer NTC here
            .AddResultCommands([
                QuestManager.ResultCommand.EventExec(3400, 20, 0, 0)
            ])
            .AddCheckCommands([
                QuestManager.CheckCommand.EventEnd(3400, 20)
            ]);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddResultCommands([
                QuestManager.ResultCommand.UpdateAnnounceDirect(1, 3),
                QuestManager.ResultCommand.RemoveEndContentsPurpose(0),
                QuestManager.ResultCommand.AddEndContentsPurpose(1, 1),
                QuestManager.ResultCommand.QstLayoutFlagOn(8671),
                QuestManager.ResultCommand.GetDragonAbility(2), // Dragon blessing
                QuestManager.ResultCommand.Prt(3400, 760, 0, -3010)
            ])
            .AddCheckCommands([
                QuestManager.CheckCommand.Prt(3400, 760, 0, -3010)
            ]);
        process0.AddProcessEndBlock(true);

        // Phase transition
        var process1 = AddNewProcess(1);
        process1.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCommands([
                QuestManager.CheckCommand.IsMyquestLayoutFlagOn(8764)
            ]);
        process1.AddRemoveGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter + 0)
            .AddResultCommands([
                QuestManager.ResultCommand.StartTimer(1, 20)
            ])
            .AddCheckCommands([
                QuestManager.CheckCommand.IsEndTimer(1)
            ]);
        process1.AddProcessEndBlock(false)
            .AddResultCommands([
                QuestManager.ResultCommand.QstLayoutFlagOff(8764)
            ]);
    }
}

return new ScriptedQuest();
