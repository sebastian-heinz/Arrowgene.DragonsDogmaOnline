using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Handler;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.GameServer.Quests.MainQuests
{

    public class Mq030260_HopesBitterEnd : Quest
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(Mq030260_HopesBitterEnd));

        public Mq030260_HopesBitterEnd() : base(QuestId.HopesBitterEnd, QuestType.Main)
        {
        }

        public List<CDataWalletPoint> WalletRewards = new List<CDataWalletPoint>()
        {
            new CDataWalletPoint() { Type = WalletType.Gold, Value = 100000 },
            new CDataWalletPoint() { Type = WalletType.RiftPoints, Value = 10000},
        };

#if false
        public List<QuestReward> ItemRewards = new List<QuestReward>()
        {
            new QuestFixedRewardItem() {ItemId = 21281, Num = 2},
            new QuestFixedRewardItem() {ItemId = 18825, Num = 50},
            new QuestFixedRewardItem() {ItemId = 18826, Num = 5}
        };
#endif

        new public List<CDataQuestExp> ExpRewards = new List<CDataQuestExp>()
        {
            new CDataQuestExp() {Type = ExpType.ExperiencePoints, Reward = 900000}
        };

        public override bool HasEnemiesInCurrentStageGroup(QuestState questState, StageId stageId, uint subGroupId)
        {
            return (StageNo.SacredFlamePath == (StageNo)stageId.Id) && (stageId.GroupId == 3 || stageId.GroupId == 17 || stageId.GroupId == 18) ||
                   (StageNo.EvilDragonsRoost1 == (StageNo)stageId.Id && stageId.GroupId == 1);
        }

        public override List<InstancedEnemy> GetEnemiesInStageGroup(StageId stageId, uint subGroupId)
        {
            List<InstancedEnemy> enemies = new List<InstancedEnemy>();
#if false
            foreach (var target in QuestTarget.Enemies)
            {
                enemies.Add(new InstancedEnemy()
                {
                    Id = target.Id,
                    Lv = target.Level,
                    IsBossBGM = target.IsBoss,
                    IsBossGauge = target.IsBoss,
                    EnemyTargetTypesId = 4,
                    Scale = 100
                    // TODO: Get rest of properties added as optional in JSON
                });
            }
#endif
            return enemies;
        }

        public override void SendProgressWorkNotices(GameClient client, StageId stageId, uint subGroupId)
        {
            if (!gQuestProgressWork.ContainsKey((StageNo) stageId.Id))
            {
                client.Party.SendToAll(new S2CQuestQuestProgressWorkSaveNtc());
                return;
            }

            if (!gQuestProgressWork[(StageNo) stageId.Id].ContainsKey(stageId.GroupId))
            {
                client.Party.SendToAll(new S2CQuestQuestProgressWorkSaveNtc());
                return;
            }

            foreach (var work in gQuestProgressWork[(StageNo)stageId.Id][stageId.GroupId])
            {
                client.Party.SendToAll(work);
            }
        }

        public override CDataQuestOrderList ToCDataQuestOrderList()
        {
            return null;
        }

        public override CDataQuestList ToCDataQuestList()
        {
            var quest = new CDataQuestList();
            quest.KeyId = (uint) QuestId;
            quest.QuestScheduleId = (uint) QuestId;
            quest.QuestId = (int)QuestId.HopesBitterEnd;
            quest.BaseLevel = 100;
            quest.ContentJoinItemRank = 0;
            quest.OrderNpcId = (uint) NpcId.TheWhiteDragon;
            quest.NameMsgId = 1;
            quest.DetailMsgId = 1;
            quest.DistributionStartDate = 1537405200;
            quest.DistributionEndDate = 2145884400;

            quest.BaseExp = ExpRewards;
            quest.BaseWalletPoints = WalletRewards;
            quest.FixedRewardItemList = GetQuestFixedRewards();

            quest.QuestOrderConditionParamList = new List<CDataQuestOrderConditionParam>()
            {
                QuestManager.AcceptConditions.MinimumLevelRestriction(100),
                QuestManager.AcceptConditions.MainQuestCompletionRestriction(QuestId.TheRelicsOfTheFirstKing),
            };

            // These correspond with QstTalkChg items in CDataQuestProcessState[0].ResultCommandList
            quest.QuestTalkInfoList.Add(new CDataQuestTalkInfo(NpcId.TheWhiteDragon, 22449));
            quest.QuestTalkInfoList.Add(new CDataQuestTalkInfo(NpcId.Gillian0, 30164));
            quest.QuestTalkInfoList.Add(new CDataQuestTalkInfo(NpcId.LiberationArmySoldier3, 30116));
            quest.QuestTalkInfoList.Add(new CDataQuestTalkInfo(NpcId.LiberationArmySoldier4, 30117));
            quest.QuestTalkInfoList.Add(new CDataQuestTalkInfo(NpcId.Gurdolin3, 30729));
            quest.QuestTalkInfoList.Add(new CDataQuestTalkInfo(NpcId.Lise0, 30730));
            quest.QuestTalkInfoList.Add(new CDataQuestTalkInfo(NpcId.Elliot0, 30731));

            quest.QuestLayoutFlagList.Add(new CDataQuestLayoutFlag() { FlagId = 7860 }); // This makes Lise, Gurdolin and Elliot appear in the audience chamber
            quest.QuestLayoutFlagList.Add(new CDataQuestLayoutFlag() { FlagId = 8013 });

            quest.QuestProcessStateList = new List<CDataQuestProcessState>()
            {
                new CDataQuestProcessState()
                {
                    ProcessNo = 0x0, SequenceNo = 0x0, BlockNo = 0x1,
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.QstTalkChg(NpcId.TheWhiteDragon, 22449),
                        QuestManager.ResultCommand.QstLayoutFlagOn(7860),
                        QuestManager.ResultCommand.QstLayoutFlagOn(8013),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Gillian0, 30164),
                        QuestManager.ResultCommand.WorldManageLayoutFlagOn(7954, 70033001), // Quest in QuestGetCycleContentsStateListHandler WorldManageQuestOrderList ntc
                        QuestManager.ResultCommand.QstTalkChg(NpcId.LiberationArmySoldier3, 30116),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.LiberationArmySoldier4, 30117),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Gurdolin3, 30729),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Lise0, 30730),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Elliot0, 30731),
                    },
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.NpcTalkAndOrderUi(StageNo.AudienceChamber, NpcId.TheWhiteDragon, 0)
                    }),
                },
                new CDataQuestProcessState()
                {
                    ProcessNo = 0x1, SequenceNo = 0x0, BlockNo = 0x0,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.MyQstFlagOn(4762)
                    }),
                },
                new CDataQuestProcessState()
                {
                    ProcessNo=0x2, SequenceNo=0x0, BlockNo=0x0,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.MyQstFlagOn(4765),
                        QuestManager.CheckCommand.StageNoWithoutMarker(StageNo.EvilDragonsRoost1),
                        QuestManager.CheckCommand.MyQstFlagOff(4766)
                    })
                },
                new CDataQuestProcessState()
                {
                    ProcessNo=0x3, SequenceNo=0x0, BlockNo=0x0,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.MyQstFlagOn(4925),
                        QuestManager.CheckCommand.MyQstFlagOff(4928),
                        QuestManager.CheckCommand.SceHitInWithoutMarker(StageNo.SacredFlamePath, 1)
                    })
                },
                new CDataQuestProcessState()
                {
                    ProcessNo=0x4, SequenceNo=0x0, BlockNo=0x0,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.MyQstFlagOn(4926),
                        QuestManager.CheckCommand.MyQstFlagOff(4929),
                        QuestManager.CheckCommand.SceHitInWithoutMarker(StageNo.SacredFlamePath, 2)
                    })
                },
                new CDataQuestProcessState()
                {
                    ProcessNo=0x5, SequenceNo=0x0, BlockNo=0x0,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.MyQstFlagOn(4927),
                        QuestManager.CheckCommand.MyQstFlagOff(4930),
                        QuestManager.CheckCommand.SceHitInWithoutMarker(StageNo.SacredFlamePath, 3)
                    })
                },
                new CDataQuestProcessState()
                {
                    ProcessNo=0x6, SequenceNo=0x0, BlockNo=0x0,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.MyQstFlagOn(4765),
                    })
                },
                new CDataQuestProcessState()
                {
                    ProcessNo=0x7, SequenceNo=0x0, BlockNo=0x0,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.IsLinkageEnemyFlag(StageNo.EvilDragonsRoost1, 1, 0, 2)
                    })
                },
                new CDataQuestProcessState()
                {
                    ProcessNo=0x8, SequenceNo=0x0, BlockNo=0x0,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.MyQstFlagOn(5053),
                    })
                },
                new CDataQuestProcessState()
                {
                    ProcessNo=0x9, SequenceNo=0x0, BlockNo=0x0,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.IsLinkageEnemyFlag(StageNo.EvilDragonsRoost1, 1, 0, 3),
                        QuestManager.CheckCommand.DummyNotProgress()
                    })
                },
                new CDataQuestProcessState()
                {
                    ProcessNo=0xa, SequenceNo=0x0, BlockNo=0x0,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.MyQstFlagOn(4765),
                        QuestManager.CheckCommand.MyQstFlagOff(4766),
                    })
                },
                new CDataQuestProcessState()
                {
                    ProcessNo=11, SequenceNo=0x0, BlockNo=0x0,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.IsLinkageEnemyFlag(StageNo.EvilDragonsRoost1, 1, 0, 3),
                    })
                }
            };

            quest.QuestEnemyInfoList.Add(new CDataQuestEnemyInfo() { GroupId = 341, Lv = 100, IsPartyRecommend = true });
            quest.QuestEnemyInfoList.Add(new CDataQuestEnemyInfo() { GroupId = 365, Lv = 100, IsPartyRecommend = true });
            quest.QuestEnemyInfoList.Add(new CDataQuestEnemyInfo() { GroupId = 323, Lv = 100, IsPartyRecommend = false });
            quest.QuestEnemyInfoList.Add(new CDataQuestEnemyInfo() { GroupId = 329, Lv = 100, IsPartyRecommend = false });
            quest.QuestEnemyInfoList.Add(new CDataQuestEnemyInfo() { GroupId = 361, Lv = 100, IsPartyRecommend = false });
            quest.QuestEnemyInfoList.Add(new CDataQuestEnemyInfo() { GroupId = 327, Lv = 100, IsPartyRecommend = false });
            quest.QuestEnemyInfoList.Add(new CDataQuestEnemyInfo() { GroupId = 353, Lv = 100, IsPartyRecommend = false });
            quest.QuestEnemyInfoList.Add(new CDataQuestEnemyInfo() { GroupId = 368, Lv = 100, IsPartyRecommend = false });

            quest.QuestLayoutFlagSetInfoList = new List<CDataQuestLayoutFlagSetInfo>() {
                QuestManager.LayoutFlag.Create(7866, StageNo.SacredFlamePath, 17),
                QuestManager.LayoutFlag.Create(7864, StageNo.SacredFlamePath, 18),
                QuestManager.LayoutFlag.Create(7865, StageNo.SacredFlamePath, 3),
                QuestManager.LayoutFlag.Create(7867, StageNo.EvilDragonsRoost1, 1),
            };

            return quest;
        }

        public override List<CDataQuestProcessState> StateMachineExecute(QuestProcessState processState, out QuestProgressState questProgressState)
        {
            List<CDataQuestProcessState> result = new List<CDataQuestProcessState>();

            questProgressState = QuestProgressState.InProgress;
            if (!gQuestProcesses.ContainsKey(processState.ProcessNo))
            {
                Logger.Error($"The processNo={processState.ProcessNo} does not exist. Unable to fetch step");
                return result;
            }

            var processSteps = gQuestProcesses[processState.ProcessNo];
            if (processState.BlockNo >= processSteps.Count)
            {
                Logger.Error($"The processNo={processState.ProcessNo}, reqNo={processState.BlockNo} does not exist. Unable to fetch step");
                return result;
            }

            result.Add(processSteps[processState.BlockNo]);

            if (result[0].ProcessNo == 0 && result[0].SequenceNo == 1)
            {
                questProgressState = QuestProgressState.Complete;
            }
            else
            {
                questProgressState = QuestProgressState.InProgress;
            }

            return result;
        }

        // groupNo, List<CDataQuestProgressWork>
        private readonly Dictionary<StageNo, Dictionary<uint, List<S2CQuestQuestProgressWorkSaveNtc>>> gQuestProgressWork = new Dictionary<StageNo, Dictionary<uint, List<S2CQuestQuestProgressWorkSaveNtc>>>()
        {
            #region SacredFlamePath
            [StageNo.SacredFlamePath] = new Dictionary<uint, List<S2CQuestQuestProgressWorkSaveNtc>>()
            {
                [17] = new List<S2CQuestQuestProgressWorkSaveNtc>()
                {
                    new S2CQuestQuestProgressWorkSaveNtc()
                    {
                        QuestScheduleId = (uint) QuestId.HopesBitterEnd,
                        ProcessNo = 0, SequenceNo = 0, BlockNo = 8,
                        WorkList = new List<CDataQuestProgressWork>
                        {
                            QuestManager.NotifyCommand.KilledTargetEnemySetGroup(7866, StageNo.SacredFlamePath, 17)
                        }
                    },
                    new S2CQuestQuestProgressWorkSaveNtc()
                    {
                        QuestScheduleId = (uint) QuestId.HopesBitterEnd,
                        ProcessNo = 3, SequenceNo = 0, BlockNo = 1,
                        WorkList = new List<CDataQuestProgressWork>
                        {
                            QuestManager.NotifyCommand.KilledTargetEmSetGrpNoMarker(7866, StageNo.SacredFlamePath, 17)
                        }
                    },
                },
                [18] = new List<S2CQuestQuestProgressWorkSaveNtc>()
                {
                    new S2CQuestQuestProgressWorkSaveNtc()
                    {
                        QuestScheduleId = (uint) QuestId.HopesBitterEnd,
                        ProcessNo = 0, SequenceNo = 0, BlockNo = 10,
                        WorkList = new List<CDataQuestProgressWork>
                        {
                            QuestManager.NotifyCommand.KilledTargetEnemySetGroup(7864, StageNo.SacredFlamePath, 18)
                        }
                    },
                    new S2CQuestQuestProgressWorkSaveNtc()
                    {
                        QuestScheduleId = (uint) QuestId.HopesBitterEnd,
                        ProcessNo = 4, SequenceNo = 0, BlockNo = 1,
                        WorkList = new List<CDataQuestProgressWork>
                        {
                            QuestManager.NotifyCommand.KilledTargetEmSetGrpNoMarker(7864, StageNo.SacredFlamePath, 18)
                        }
                    },
                },
                [3] = new List<S2CQuestQuestProgressWorkSaveNtc>()
                {
                    new S2CQuestQuestProgressWorkSaveNtc()
                    {
                        QuestScheduleId = (uint) QuestId.HopesBitterEnd,
                        ProcessNo = 0, SequenceNo = 0, BlockNo = 12,
                        WorkList = new List<CDataQuestProgressWork>
                        {
                            QuestManager.NotifyCommand.KilledTargetEnemySetGroup(7865, StageNo.SacredFlamePath, 3)
                        }
                    },
                    new S2CQuestQuestProgressWorkSaveNtc()
                    {
                        QuestScheduleId = (uint) QuestId.HopesBitterEnd,
                        ProcessNo = 5, SequenceNo = 0, BlockNo = 1,
                        WorkList = new List<CDataQuestProgressWork>
                        {
                            QuestManager.NotifyCommand.KilledTargetEmSetGrpNoMarker(7865, StageNo.SacredFlamePath, 3)
                        }
                    },
                },
            },
            #endregion
            #region EvilDragonsRoost
            [StageNo.EvilDragonsRoost1] = new Dictionary<uint, List<S2CQuestQuestProgressWorkSaveNtc>>()
            {
                [1] = new List<S2CQuestQuestProgressWorkSaveNtc>()
                {
                    new S2CQuestQuestProgressWorkSaveNtc()
                    {
                        QuestScheduleId = (uint) QuestId.HopesBitterEnd,
                        ProcessNo = 0, SequenceNo = 0, BlockNo = 19,
                        WorkList = new List<CDataQuestProgressWork>
                        {
                            QuestManager.NotifyCommand.KilledTargetEnemySetGroup(7867, StageNo.EvilDragonsRoost1, 1)
                        }
                    }
                },
            }
            #endregion
        };

        private readonly Dictionary<uint, List<CDataQuestProcessState>> gQuestProcesses = new Dictionary<uint, List<CDataQuestProcessState>>()
        {
            #region PROCESS0
            [0] = new List<CDataQuestProcessState>()
            {

                new CDataQuestProcessState()
                {
                    ProcessNo = 0, SequenceNo = 0, BlockNo = 2,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.Prt(StageNo.LookoutCastle1, 15, 18280, -14593)
                    }),
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.SetAnnounce(QuestAnnounceType.Accept, 1),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.TheWhiteDragon, 26018),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Joseph, 26019),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Klaus0, 26020),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Elliot0, 26021),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Gurdolin3, 26022),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Lise0, 26023),
                        QuestManager.ResultCommand.QstLayoutFlagOn(7861),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Meirova0, 26024),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Gillian0, 26025),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Bertha, 26026),
                        QuestManager.ResultCommand.Prt(StageNo.LookoutCastle1, 15, 18280, -14593),
                        QuestManager.ResultCommand.QstLayoutFlagOn(7891),
                        QuestManager.ResultCommand.QstLayoutFlagOff(8013),
                        QuestManager.ResultCommand.WorldManageLayoutFlagOff(8036, 70033001),
                        QuestManager.ResultCommand.MyQstFlagOn(4762)
                    }
                },
                new CDataQuestProcessState()
                {
                    ProcessNo = 0, SequenceNo = 0, BlockNo = 3,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.EventEnd(StageNo.LookoutCastle1, 20)
                    }),
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.EventExec(StageNo.LookoutCastle1, 20, StageNo.LookoutCastle1, 0x20),
                        QuestManager.ResultCommand.SetCheckPoint()
                    }
                },
                new CDataQuestProcessState()
                {
                    ProcessNo = 0, SequenceNo = 0, BlockNo = 4,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.NewTalkNpc(StageNo.LookoutCastle1, 0, 1, 0)
                    }),
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.UpdateAnnounce(),
                        QuestManager.ResultCommand.ResetCheckPoint(),
                        QuestManager.ResultCommand.QstLayoutFlagOn(0x1eb6),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Gillian0, 0x65ac),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Gurdolin3, 0x65ad),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Lise0, 0x65ae),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Elliot0, 0x65af),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Bertha, 0x65b0),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Meirova0, 0x57f2),
                        QuestManager.ResultCommand.QstLayoutFlagOff(0x1eb4),
                    }
                },
                new CDataQuestProcessState()
                {
                    ProcessNo = 0, SequenceNo = 0, BlockNo = 5,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.NewTalkNpc(StageNo.FirefallMountainCampsite, 0, 0, 0)
                    }),
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.UpdateAnnounce(QuestAnnounceType.Accept),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Meirova0, 26034),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Cyril, 26027),
                        QuestManager.ResultCommand.QstLayoutFlagOn(7901),
                        QuestManager.ResultCommand.WorldManageLayoutFlagOff(7967, 70033001),
                        QuestManager.ResultCommand.WorldManageLayoutFlagOn(7968, 70033001)
                    }
                },
                new CDataQuestProcessState()
                {
                    ProcessNo = 0, SequenceNo = 0, BlockNo = 6,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.StageNo(StageNo.SacredFlamePath)
                    }),
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.UpdateAnnounce(),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Bacias, 26033),
                        QuestManager.ResultCommand.WorldManageLayoutFlagOn(7954, 70033001),
                        QuestManager.ResultCommand.QstLayoutFlagOn(8059)
                    }
                },
                new CDataQuestProcessState()
                {
                    ProcessNo = 0, SequenceNo = 0, BlockNo = 7,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.IsEnemyFound(StageNo.SacredFlamePath, 17, -1) // Might need to revisit this?
                    }),
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.QstLayoutFlagOn(7866),
                        QuestManager.ResultCommand.MyQstFlagOn(4925),
                        QuestManager.ResultCommand.QstLayoutFlagOff(7862),
                        QuestManager.ResultCommand.QstLayoutFlagOn(8150)
                    }
                },
                new CDataQuestProcessState()
                {
                    ProcessNo = 0, SequenceNo = 0, BlockNo = 8,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.IsKilledTargetEnemySetGroup(7866)
                    }),
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.UpdateAnnounce()
                    }
                },
                new CDataQuestProcessState()
                {
                    ProcessNo = 0, SequenceNo = 0, BlockNo = 9,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.IsEnemyFound(StageNo.SacredFlamePath, 18, -1)
                    }),
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.UpdateAnnounce(),
                        QuestManager.ResultCommand.QstLayoutFlagOff(7866),
                        QuestManager.ResultCommand.QstLayoutFlagOn(7864),
                        QuestManager.ResultCommand.QstLayoutFlagOn(8061),
                        QuestManager.ResultCommand.MyQstFlagOn(4926)
                    }
                },
                new CDataQuestProcessState()
                {
                    ProcessNo = 0, SequenceNo = 0, BlockNo = 10,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.IsKilledTargetEnemySetGroup(7864)
                    }),
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.UpdateAnnounce(),
                    }
                },
                new CDataQuestProcessState()
                {
                    ProcessNo = 0, SequenceNo = 0, BlockNo = 11,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.IsEnemyFound(StageNo.SacredFlamePath, 3, -1)
                    }),
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.UpdateAnnounce(),
                        QuestManager.ResultCommand.QstLayoutFlagOff(7864),
                        QuestManager.ResultCommand.QstLayoutFlagOn(8059),
                        QuestManager.ResultCommand.QstLayoutFlagOn(7865),
                        QuestManager.ResultCommand.MyQstFlagOn(4927)
                    }
                },
                new CDataQuestProcessState()
                {
                    ProcessNo = 0, SequenceNo = 0, BlockNo = 12,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.IsKilledTargetEnemySetGroup(7865)
                    }),
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.UpdateAnnounce(),
                    }
                },
                new CDataQuestProcessState()
                {
                    ProcessNo = 0, SequenceNo = 0, BlockNo = 13,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.StageNo(StageNo.ScaredFlamePathUpperLevel)
                    }),
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.UpdateAnnounce(),
                        QuestManager.ResultCommand.QstLayoutFlagOff(7865),
                        QuestManager.ResultCommand.WorldManageLayoutFlagOn(7955, 70033001),
                        QuestManager.ResultCommand.QstLayoutFlagOn(8060),
                        QuestManager.ResultCommand.QstLayoutFlagOn(8065),
                        QuestManager.ResultCommand.WorldManageLayoutFlagOff(8202, 70033001),
                    }
                },
                new CDataQuestProcessState()
                {
                    ProcessNo = 0, SequenceNo = 0, BlockNo = 14,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.Prt(StageNo.ScaredFlamePathUpperLevel, 21030, 2934, -24797)
                    }),
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.Prt(StageNo.ScaredFlamePathUpperLevel, 21030, 2934, -24797),
                        QuestManager.ResultCommand.UpdateAnnounce(),
                        QuestManager.ResultCommand.QstLayoutFlagOn(7904),
                        QuestManager.ResultCommand.MyQstFlagOn(4830),
                        QuestManager.ResultCommand.QstLayoutFlagOff(8063),
                        QuestManager.ResultCommand.QstLayoutFlagOff(8064),
                        QuestManager.ResultCommand.QstLayoutFlagOff(8062),
                        QuestManager.ResultCommand.QstLayoutFlagOff(7861),
                    }
                },
                new CDataQuestProcessState()
                {
                    ProcessNo = 0, SequenceNo = 0, BlockNo = 15,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.EventEnd(StageNo.EvilDragonsRoost1, 0)
                    }),
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.ExeEventAfterStageJumpContinue(StageNo.EvilDragonsRoost1, 0, 1),
                        QuestManager.ResultCommand.SetCheckPoint()
                    }
                },
                new CDataQuestProcessState()
                {
                    ProcessNo = 0, SequenceNo = 0, BlockNo = 16,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.StageNo(StageNo.EvilDragonsRoost1)
                    }),
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.StageJump(StageNo.EvilDragonsRoost1, 1)
                    }
                },
                new CDataQuestProcessState()
                {
                    ProcessNo = 0, SequenceNo = 0, BlockNo = 17,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.IsLinkageEnemyFlag(StageNo.EvilDragonsRoost1, 1, 0, 3)
                    }),
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.UpdateAnnounce(QuestAnnounceType.Accept),
                        QuestManager.ResultCommand.MyQstFlagOn(4765),
                        QuestManager.ResultCommand.QstLayoutFlagOn(7867),
                        QuestManager.ResultCommand.QstLayoutFlagOn(7868),
                        QuestManager.ResultCommand.SetDiePlayerReturnPos(StageNo.EvilDragonsRoost1, 1, 0),
                        QuestManager.ResultCommand.BgmRequestFix(1, 264),
                        QuestManager.ResultCommand.Unknown(126, 2),
                        QuestManager.ResultCommand.QstLayoutFlagOff(7904),
                    }
                },
                new CDataQuestProcessState()
                {
                    ProcessNo = 0, SequenceNo = 0, BlockNo = 18,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.EmHpNotLess(StageNo.EvilDragonsRoost1, 1, 0, 100)
                    }),
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.MyQstFlagOn(5054),
                        QuestManager.ResultCommand.MyQstFlagOff(5053),
                        QuestManager.ResultCommand.ResetDiePlayerReturnPos(0, 0),
                        QuestManager.ResultCommand.SetDiePlayerReturnPos(StageNo.EvilDragonsRoost1, 2, 0),
                        QuestManager.ResultCommand.QstLayoutFlagOff(7868),
                    }
                },
                new CDataQuestProcessState()
                {
                    ProcessNo = 0, SequenceNo = 0, BlockNo = 19,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.IsKilledTargetEnemySetGroup(7867)
                    }),
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.UpdateAnnounce(),
                        QuestManager.ResultCommand.BgmStop(),
                        QuestManager.ResultCommand.BgmRequestFix(1, 272),
                        QuestManager.ResultCommand.QstLayoutFlagOn(8545),
                    }
                },
                new CDataQuestProcessState()
                {
                    ProcessNo = 0, SequenceNo = 0, BlockNo = 20,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.EventEnd(StageNo.EvilDragonsRoost1, 5)
                    }),
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.EventExec(StageNo.EvilDragonsRoost1, 5, 0, 0),
                        QuestManager.ResultCommand.QstLayoutFlagOff(7867),
                        QuestManager.ResultCommand.QstLayoutFlagOff(7903),
                        QuestManager.ResultCommand.MyQstFlagOn(4766),
                        QuestManager.ResultCommand.StopMessage(),
                        QuestManager.ResultCommand.BgmStop(),
                        QuestManager.ResultCommand.QstLayoutFlagOff(8277),
                        QuestManager.ResultCommand.QstLayoutFlagOff(8545),
                    }
                },
                new CDataQuestProcessState()
                {
                    ProcessNo = 0, SequenceNo = 0, BlockNo = 21,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.NewTalkNpc(StageNo.EvilDragonsRoost1, 1, 0, 0)
                    }),
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.UpdateAnnounce(),
                        QuestManager.ResultCommand.QstLayoutFlagOn(7872),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Nedo0, 26035),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Meirova0, 26037),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Gillian0, 26039),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Gurdolin3, 26040),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Elliot0, 26041),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Lise0, 26042),
                        QuestManager.ResultCommand.ResetDiePlayerReturnPos(0, 0),
                        QuestManager.ResultCommand.QstLayoutFlagOff(7868),
                        QuestManager.ResultCommand.QstLayoutFlagOff(8277),
                        QuestManager.ResultCommand.SetDiePlayerReturnPos(StageNo.EvilDragonsRoost1, 2, 0),
                    }
                },
                new CDataQuestProcessState()
                {
                    ProcessNo = 0, SequenceNo = 0, BlockNo = 22,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.TalkNpc(StageNo.AudienceChamber, NpcId.TheWhiteDragon)
                    }),
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.UpdateAnnounce(),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Nedo0, 26036),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Meirova0, 26038),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Gerhard, 26043),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Bertha, 26044),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Quintus, 26045),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Bacias, 30106),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Klaus0, 26047),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Joseph, 26048),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.TheWhiteDragon, 22599),
                        QuestManager.ResultCommand.QstLayoutFlagOn(8200),
                        QuestManager.ResultCommand.ResetCheckPoint(),
                        QuestManager.ResultCommand.ResetDiePlayerReturnPos(0, 0),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Cyril, 26046),
                    }
                },
                new CDataQuestProcessState()
                {
                    ProcessNo = 0, SequenceNo = 1, BlockNo = 23,
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.SetAnnounce(QuestAnnounceType.Clear),
                    }
                }
            },
            #endregion
            #region PROCESS1
            [1] = new List<CDataQuestProcessState>()
            {
                new CDataQuestProcessState()
                {
                    ProcessNo = 1, SequenceNo = 0, BlockNo = 1,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(
                    new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.IsLogin()
                    },
                    new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.MyQstFlagOn(4765),
                        QuestManager.CheckCommand.IsMyquestLayoutFlagOff(8200),
                        QuestManager.CheckCommand.StageNoNotEq(StageNo.EvilDragonsRoost1)
                    }),
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.SetCheckPoint()
                    }
}
            },
            #endregion
            #region PROCESS2
            [2] = new List<CDataQuestProcessState>()
            {
                new CDataQuestProcessState()
                {
                    ProcessNo = 2, SequenceNo = 1, BlockNo = 1,
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.PlayMessage(22563, 8),
                        QuestManager.ResultCommand.PlayMessage(22564, 8),
                        QuestManager.ResultCommand.PlayMessage(22577, 8),
                        QuestManager.ResultCommand.PlayMessage(22591, 8),
                        QuestManager.ResultCommand.PlayMessage(22584, 8),
                        QuestManager.ResultCommand.PlayMessage(22565, 8),
                        QuestManager.ResultCommand.PlayMessage(22566, 8),
                        QuestManager.ResultCommand.PlayMessage(22578, 8),
                        QuestManager.ResultCommand.PlayMessage(22585, 8),
                        QuestManager.ResultCommand.PlayMessage(22592, 8),
                        QuestManager.ResultCommand.PlayMessage(22567, 8),
                        QuestManager.ResultCommand.PlayMessage(22568, 8),
                        QuestManager.ResultCommand.PlayMessage(22586, 8),
                        QuestManager.ResultCommand.PlayMessage(22579, 8),
                        QuestManager.ResultCommand.PlayMessage(22593, 8),
                        QuestManager.ResultCommand.PlayMessage(22571, 8),
                        QuestManager.ResultCommand.PlayMessage(22570, 8),
                        QuestManager.ResultCommand.PlayMessage(22594, 8),
                        QuestManager.ResultCommand.PlayMessage(22587, 8),
                        QuestManager.ResultCommand.PlayMessage(22580, 8),
                        QuestManager.ResultCommand.PlayMessage(22572, 8),
                        QuestManager.ResultCommand.PlayMessage(22569, 8),
                        QuestManager.ResultCommand.PlayMessage(22588, 8),
                        QuestManager.ResultCommand.PlayMessage(22581, 8),
                        QuestManager.ResultCommand.PlayMessage(22595, 8),
                        QuestManager.ResultCommand.PlayMessage(22573, 8),
                        QuestManager.ResultCommand.PlayMessage(22582, 8),
                        QuestManager.ResultCommand.PlayMessage(22589, 8),
                        QuestManager.ResultCommand.PlayMessage(22596, 8),
                        QuestManager.ResultCommand.PlayMessage(22574, 8),
                        QuestManager.ResultCommand.PlayMessage(22575, 8),
                        QuestManager.ResultCommand.PlayMessage(22590, 8),
                        QuestManager.ResultCommand.PlayMessage(22583, 8),
                        QuestManager.ResultCommand.PlayMessage(22597, 8),
                        QuestManager.ResultCommand.PlayMessage(22576, 8),
                        QuestManager.ResultCommand.SetCheckPoint(),
                    }
                }
            },
            #endregion
            #region PROCESS3
            [3] = new List<CDataQuestProcessState>()
            {
                new CDataQuestProcessState()
                {
                    ProcessNo = 3, SequenceNo = 0, BlockNo = 1,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(
                    new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.IsKilledTargetEmSetGrpNoMarker(7866)
                    }),
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.QstLayoutFlagOn(8063),
                        QuestManager.ResultCommand.SetCheckPoint()
                    }
                },
                new CDataQuestProcessState()
                {
                    ProcessNo = 3, SequenceNo = 0, BlockNo = 2,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(
                    new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.StageNoWithoutMarker(StageNo.ScaredFlamePathUpperLevel)
                    }),
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.MyQstFlagOn(4928),
                        QuestManager.ResultCommand.QstLayoutFlagOff(8063),
                        QuestManager.ResultCommand.QstLayoutFlagOn(8146),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Elliot0, 30732)
                    }
                },
                new CDataQuestProcessState()
                {
                    ProcessNo = 3, SequenceNo = 1, BlockNo = 3,
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.QstLayoutFlagOff(8146),
                        QuestManager.ResultCommand.ResetCheckPoint(),
                    }
                }
            },
            #endregion
            #region PROCESS4
            [4] = new List<CDataQuestProcessState>()
            {
                new CDataQuestProcessState()
                {
                    ProcessNo = 4, SequenceNo = 0, BlockNo = 1,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.IsKilledTargetEmSetGrpNoMarker(7864),
                    }),
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.QstLayoutFlagOn(8064),
                        QuestManager.ResultCommand.SetCheckPoint(),
                    }
                },
                new CDataQuestProcessState()
                {
                    ProcessNo = 4, SequenceNo = 0, BlockNo = 2,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.StageNoWithoutMarker(StageNo.ScaredFlamePathUpperLevel),
                    }),
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.MyQstFlagOn(4929),
                        QuestManager.ResultCommand.QstLayoutFlagOff(8064),
                        QuestManager.ResultCommand.QstLayoutFlagOn(8147),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Lise0, 30733),
                    }
                },
                new CDataQuestProcessState()
                {
                    ProcessNo = 4, SequenceNo = 1, BlockNo = 3,
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.QstLayoutFlagOff(8147),
                        QuestManager.ResultCommand.ResetCheckPoint(),
                    }
                }
            },
            #endregion
            #region PROCESS5
            [5] = new List<CDataQuestProcessState>()
            {
                new CDataQuestProcessState()
                {
                    ProcessNo = 5, SequenceNo = 0, BlockNo = 1,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(
                    new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.IsKilledTargetEmSetGrpNoMarker(7865),
                    }),
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.QstLayoutFlagOn(8062),
                        QuestManager.ResultCommand.SetCheckPoint(),
                    }
                },
                new CDataQuestProcessState()
                {
                    ProcessNo = 5, SequenceNo = 0, BlockNo = 2,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(
                    new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.StageNoWithoutMarker(StageNo.ScaredFlamePathUpperLevel),
                    }),
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.QstLayoutFlagOff(8062),
                        QuestManager.ResultCommand.MyQstFlagOn(4930),
                        QuestManager.ResultCommand.QstLayoutFlagOn(8148),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Gurdolin3, 30734),
                    }
                },
                new CDataQuestProcessState()
                {
                    ProcessNo = 5, SequenceNo = 1, BlockNo = 3,
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.QstLayoutFlagOff(8148),
                        QuestManager.ResultCommand.ResetCheckPoint()
                    }
                }
            },
            #endregion
            #region PROCESS6
            [6] = new List<CDataQuestProcessState>()
            {
                new CDataQuestProcessState()
                {
                    ProcessNo = 6, SequenceNo = 0, BlockNo = 1,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.MyQstFlagOn(4766),
                    }),
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.QstLayoutFlagOn(7867),
                        QuestManager.ResultCommand.SetCheckPoint()
                    }
                },
                new CDataQuestProcessState()
                {
                    ProcessNo = 6, SequenceNo = 0, BlockNo = 2,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.DummyNotProgress()
                    }),
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.QstLayoutFlagOff(7867),
                    }
                }
            },
            #endregion
            #region PROCESS7
            [7] = new List<CDataQuestProcessState>()
            {
                new CDataQuestProcessState()
                {
                    ProcessNo = 7, SequenceNo = 0, BlockNo = 1,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.DummyNotProgress(),
                    }),
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.MyQstFlagOn(5053),
                        QuestManager.ResultCommand.ChangeMessage()
                    }
                }
            },
            #endregion
            #region PROCESS8
            [8] = new List<CDataQuestProcessState>()
            {
                new CDataQuestProcessState()
                {
                    ProcessNo = 8, SequenceNo = 0, BlockNo = 1,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.DummyNotProgress(),
                    }),
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.LinkageEnemyFlagOn(StageNo.EvilDragonsRoost1, 1, 0, 1),
                        QuestManager.ResultCommand.SetCheckPoint()
                    }
                }
            },
            #endregion
            #region PROCESS10
            [10] = new List<CDataQuestProcessState>()
            {
                new CDataQuestProcessState()
                {
                    ProcessNo = 10, SequenceNo = 0, BlockNo = 1,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.MyQstFlagOn(5054),
                        QuestManager.CheckCommand.IsMyquestLayoutFlagOn(7867),
                    }),
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.SetCheckPoint()
                    }
                },
                new CDataQuestProcessState()
                {
                    ProcessNo = 10, SequenceNo = 0, BlockNo = 2,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.Unknown(243, 8277),
                    }),
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.QstLayoutFlagOn(8277)
                    }
                }
            },
            #endregion
            #region PROCESS11
            [11] = new List<CDataQuestProcessState>()
            {
                new CDataQuestProcessState()
                {
                    ProcessNo = 11, SequenceNo = 0, BlockNo = 1,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.IsLinkageEnemyFlag(StageNo.EvilDragonsRoost1, 1, 0, 12),
                    }),
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.SetCheckPoint(),
                        QuestManager.ResultCommand.LinkageEnemyFlagOff(StageNo.EvilDragonsRoost1, 1, 0, 12)
                    }
                },
                new CDataQuestProcessState()
                {
                    ProcessNo = 11, SequenceNo = 0, BlockNo = 2,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.DummyNotProgress()
                    }),
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.BgmStop(),
                        QuestManager.ResultCommand.BgmRequestFix(1, 265)
                    }
                }
            }
            #endregion
        };
    }
}
