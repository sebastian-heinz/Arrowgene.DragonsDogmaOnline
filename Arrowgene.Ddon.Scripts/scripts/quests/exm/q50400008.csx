/**
 * @brief The Great Dragon Crystal War: The Resisting Land
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.ExtremeMission;
    public override QuestId QuestId => (QuestId)50400008;
    public override ushort RecommendedLevel => 100;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => false;
    // Quest enemy info list: AlteredZuhl, LotusFin, SeverelyInfectedScourge, Merman, PoisonMerman

    private class QstLayoutFlag
    {
        public const uint Flag0 = 8572; // Group 40, 41
        public const uint Flag1 = 8645; // Group 42, 43
        public const uint Flag2 = 8686; // Group 154, 155
        public const uint Flag3 = 8687; // Group 156
        public const uint Flag4 = 8689; // Group 157
    }

    private class EnemyGroupId
    {
        public const uint Set8672 = 8672;
        public const uint Set8673 = 8673;
        public const uint Set8674 = 8674;
        public const uint Set8686 = 8686;
        public const uint Set8688 = 8688;
    }

    private class NamedParamId
    {
        public const uint Corrupted2752 = 2752;
        public const uint Corrupted3158 = 3158;
    }

    protected override void InitializeState()
    {
        MissionParams.Group = ExtremeMissionUtils.Group.Travers; //Travers0
        MissionParams.MinimumMembers = 1;
        MissionParams.MaximumMembers = 4;
        MissionParams.IsSolo = false;
        MissionParams.PlaytimeInSeconds = 1500; // "IsNoTimeup": true
        MissionParams.ArmorAllowed = true;
        MissionParams.JewelryAllowed = true;
        MissionParams.MaxPawns = 3;
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 0); // Missing rewards
        AddWalletReward(WalletType.Gold, 0); // Missing rewards
        AddWalletReward(WalletType.RiftPoints, 0); // Missing rewards

        AddFixedItemReward(ItemId.KeystoneToRuin, 2);
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(0, Stage.DarknessShroudedShadoleanGreatTemple0, 10, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.HighPixiePow, 100, 4461, 2)
                .SetRepopNum(1)
                .SetRepopCount(2)
                .SetNamedEnemyParams(NamedParamId.Corrupted2752)
                .SetEnemyTargetTypesId(1),
            LibDdon.Enemy.Create(EnemyId.HighPixiePow, 100, 4461, 3)
                .SetRepopNum(0)
                .SetRepopCount(2)
                .SetNamedEnemyParams(NamedParamId.Corrupted2752)
                .SetEnemyTargetTypesId(1),
            LibDdon.Enemy.Create(EnemyId.InfectedHobgoblin, 100, 4461, 5)
                .SetRepopNum(0)
                .SetRepopCount(2)
                .SetNamedEnemyParams(NamedParamId.Corrupted2752)
                .SetEnemyTargetTypesId(1)
                .SetInfectionType(3),
            LibDdon.Enemy.Create(EnemyId.InfectedHobgoblin, 100, 4461, 6)
                .SetRepopNum(1)
                .SetRepopCount(2)
                .SetNamedEnemyParams(NamedParamId.Corrupted2752)
                .SetEnemyTargetTypesId(1)
                .SetInfectionType(3),
            LibDdon.Enemy.Create(EnemyId.LegionFighter, 100, 4461, 8)
                .SetRepopNum(0)
                .SetRepopCount(2)
                .SetNamedEnemyParams(NamedParamId.Corrupted2752)
                .SetEnemyTargetTypesId(1),
            LibDdon.Enemy.Create(EnemyId.LegionFighter, 100, 4461, 9)
                .SetRepopNum(1)
                .SetRepopCount(2)
                .SetNamedEnemyParams(NamedParamId.Corrupted2752)
                .SetEnemyTargetTypesId(1),
        });

        AddEnemies(EnemyGroupId.Set8672, Stage.DarknessShroudedShadoleanGreatTemple0, 10, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.AlteredZuhl, 100, 111525, 0)
                //.SetRepopNum(0)
                //.SetRepopCount(1)
                .SetNamedEnemyParams(NamedParamId.Corrupted3158)
                .SetEnemyTargetTypesId(4)
                .SetInfectionType(2)
                .SetIsBoss(true),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCommands([
                QuestManager.CheckCommand.IsGatherPartyInStage(3110)
            ]);
        process0.AddRawBlock(QuestAnnounceType.None)
            //process0.AddSpawnGroupsBlock(QuestAnnounceType.None, [EnemyGroupId.Set8688, EnemyGroupId.Set8686])
            .AddCheckCommands([
                QuestManager.CheckCommand.MyQstFlagOn(5219)
            ])
            .AddResultCommands([
                QuestManager.ResultCommand.StartMissionAnnounce(),
                QuestManager.ResultCommand.SetDiePlayerReturnPos(3110, 0, 0),
                QuestManager.ResultCommand.Unknown(127, 0, 0, 0, 0),
                QuestManager.ResultCommand.StartContentsTimer(0),
                QuestManager.ResultCommand.QstLayoutFlagOn(8686),
                QuestManager.ResultCommand.QstLayoutFlagOn(8688),
                QuestManager.ResultCommand.MyQstFlagOn(5312)
            ]);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCommands([
                QuestManager.CheckCommand.Prt(3110, 0, 980, -1550)
            ])
            .AddResultCommands([
                QuestManager.ResultCommand.Prt(3110, 0, 980, -1550),
                QuestManager.ResultCommand.QstLayoutFlagOn(8645)
            ]);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCommands([
                QuestManager.CheckCommand.StageNoWithoutMarker(3110)
            ])
            .AddResultCommands([
                QuestManager.ResultCommand.StageJump(3110, 3, 1, 0),
                QuestManager.ResultCommand.Unknown(130, 0, 0, 0, 0), // Sends S2C_63_5_16_NTC - Actually C2S
                QuestManager.ResultCommand.ResetDiePlayerReturnPos(0, 0, 0, 0),
                QuestManager.ResultCommand.SetDiePlayerReturnPos(3110, 3, 0, 0)
            ]);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCommands([
                QuestManager.CheckCommand.Prt(3110, 0, -445, -14154)
            ])
            .AddResultCommands([
                QuestManager.ResultCommand.Prt(3110, 0, -445, -14154)
            ]);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCommands([
                QuestManager.CheckCommand.MyQstFlagOn(5335)
            ])
            .AddResultCommands([
                QuestManager.ResultCommand.MyQstFlagOn(5334)
            ]);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddResultCommands([
                QuestManager.ResultCommand.SetAnnounce((QuestAnnounceType)1),
                QuestManager.ResultCommand.EndEndQuest(),
                QuestManager.ResultCommand.ResetDiePlayerReturnPos(0, 0, 0, 0),
                QuestManager.ResultCommand.StopCycleTimer(),
                QuestManager.ResultCommand.WorldManageQuestFlagOn (5313, 70034001)
            ]);

        var process1 = AddNewProcess(1);
        //process1.AddRawBlock(QuestAnnounceType.None)
            //.AddCheckCommands([
                //QuestManager.CheckCommand.ChainNotLess(0) // Not working for 0, but according to ingame footage should be actually 5?
            //]);
        process1.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCommands([
                QuestManager.CheckCommand.IsQuestLayoutHpNotGreater(3110, 40, 0, 20)
            ])
            .AddCheckCommands([
                QuestManager.CheckCommand.StageNo(3110),
                QuestManager.CheckCommand.DummyNotProgress()
            ])
            .AddResultCommands([
                QuestManager.ResultCommand.QstLayoutFlagOn(8572),
                QuestManager.ResultCommand.UpdateAnnounceDirect(0, 0)
            ]);
        process1.AddSpawnGroupBlock(QuestAnnounceType.None, EnemyGroupId.Set8672)
        //process1.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCommands([
                QuestManager.CheckCommand.IsKilledTargetEmSetGrpNoMarker(8672)
            ])
            .AddCheckCommands([
                QuestManager.CheckCommand.IsKilledTargetEmSetGrpNoMarker(8673)
            ])
            .AddCheckCommands([
                QuestManager.CheckCommand.IsKilledTargetEmSetGrpNoMarker(8674)
            ])
            .AddCheckCommands([
                QuestManager.CheckCommand.StageNo(3110),
                QuestManager.CheckCommand.DummyNotProgress()
            ])
            .AddResultCommands([
                QuestManager.ResultCommand.MyQstFlagOn(5281),
                QuestManager.ResultCommand.SetQuestLayoutEnemyBodyPose(3110, 40, 0, 0),
                QuestManager.ResultCommand.LayoutFlagRandomOn(8672, 8673, 8674, 8672)
            ]);
        process1.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCommands([
                QuestManager.CheckCommand.IsOmBrokenLayout(3110, 40, 0, 0)
            ])
            .AddCheckCommands([
                QuestManager.CheckCommand.StageNo(3110),
                QuestManager.CheckCommand.DummyNotProgress()
            ])
            .AddResultCommands([
                QuestManager.ResultCommand.SetQuestLayoutEnemyBodyPose(3110, 40, 0, 1)
            ]);
        process1.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCommands([
                QuestManager.CheckCommand.DummyNotProgress()
            ])
            .AddResultCommands([
                QuestManager.ResultCommand.MyQstFlagOn(5219),
                QuestManager.ResultCommand.Unknown(128, 0, 0, 0, 0),
                QuestManager.ResultCommand.UpdateAnnounceDirect(1, 0),
                QuestManager.ResultCommand.QstLayoutFlagOff(8572)
            ]);

        var process2 = AddNewProcess(2);
        process2.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCommands([
                QuestManager.CheckCommand.IsMyquestLayoutFlagOn(8572),
                QuestManager.CheckCommand.SceHitInWithoutMarker(3110, 0)
            ]);
        process2.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCommands([
                QuestManager.CheckCommand.MyQstFlagOn(5281)
            ])
            .AddResultCommands([
                QuestManager.ResultCommand.MyQstFlagOn(5280)
            ]);
        process2.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCommands([
                QuestManager.CheckCommand.DummyNotProgress()
            ])
            .AddResultCommands([
                QuestManager.ResultCommand.MyQstFlagOff(5280)
            ]);

        var process3 = AddNewProcess(3);
        process3.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCommands([
                QuestManager.CheckCommand.MyQstFlagOn(5312)
            ]);
        process3.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCommands([
                QuestManager.CheckCommand.ChainNotLess(6)
            ]);
        process3.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCommands([
                QuestManager.CheckCommand.ChainNotLess(11)
            ])
            .AddResultCommands([
                QuestManager.ResultCommand.CallGeneralAnnounce (0, 100770),
                QuestManager.ResultCommand.SetOmState(3110, 160, 0, 2),
                QuestManager.ResultCommand.QstLayoutFlagOff (8686),
                QuestManager.ResultCommand.QstLayoutFlagOn (8687)
            ]);
        process3.AddRawBlock(QuestAnnounceType.None)
            .AddResultCommands([
                QuestManager.ResultCommand.CallGeneralAnnounce (0, 100770),
                QuestManager.ResultCommand.SetOmState(3110, 161, 0, 2),
                QuestManager.ResultCommand.QstLayoutFlagOff (8688),
                QuestManager.ResultCommand.QstLayoutFlagOn (8689)
            ])
            .AddCheckCommands([
                QuestManager.CheckCommand.DummyNotProgress() // ChainNotLess 15?
            ]);
        // Missing next block?

        var process4 = AddNewProcess(4);
        process4.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCommands([
                QuestManager.CheckCommand.MyQstFlagOn(5334),
                QuestManager.CheckCommand.WorldManageQuestFlagOff(5313, 70034001)
            ]);
        process4.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCommands([
                QuestManager.CheckCommand.DummyNotProgress()
            ])
            .AddResultCommands([
                QuestManager.ResultCommand.MyQstFlagOn(5335),
                QuestManager.ResultCommand.CallGeneralAnnounce (0, 100771, 0, 300)
            ]);

        var process5 = AddNewProcess(5);
        process5.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCommands([
                QuestManager.CheckCommand.MyQstFlagOn(5334),
                QuestManager.CheckCommand.WorldManageQuestFlagOn(5313, 70034001)
            ]);
    }
}

return new ScriptedQuest();
