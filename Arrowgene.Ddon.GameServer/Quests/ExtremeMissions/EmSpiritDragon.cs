using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.GameServer.Quests.ExtremeMissions
{
    internal class EmSpiritDragon
    {
    }
}


#if false
            S2CQuestGetLightQuestListRes pcap = EntitySerializer.Get<S2CQuestGetLightQuestListRes>().Read(pcap_data);
            S2CQuestGetLightQuestListRes res = new S2CQuestGetLightQuestListRes();

            // TODO: Make these configurable

            // Spirit Dragon EM
            res.LightQuestList.Add(new CDataLightQuestList()
            {
                Param = new CDataQuestList() {
                    QuestScheduleId = 50300010,
                    QuestId = 50300010
                }
            });
            
------------------------------------------------
                case 50300010:
                    // Spirit Dragon EM as a Light Quest, yes
                    res.QuestProcessStateList.Add(new CDataQuestProcessState()
                    {
                        CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                        { 
                            // EmHpLess
                            QuestManager.CheckCommand.EmHpLess(StageNo.SpiritDragonsRoost2, 2, 0, 60)
                        }),
                        ResultCommandList = new List<CDataQuestCommand>()
                        {
                            QuestManager.ResultCommand.SetAnnounce(QuestAnnounceType.Accept),
                            QuestManager.ResultCommand.StageJump(StageNo.SpiritDragonsRoost2, 0),
                            QuestManager.ResultCommand.ExeEventAfterStageJump(StageNo.SpiritDragonsRoost2, 0, 0)
                        }
                    });
                    break;
            }


-----------------------------------------------

                case 50300010:
                {
                    // Spirit Dragon
                    S2CQuestQuestProgressRes res = new S2CQuestQuestProgressRes();
                    res.QuestScheduleId = packet.Structure.QuestScheduleId;
                    switch (packet.Structure.ProcessNo)
                    {
                        case 0:
                            {
                                CDataStageLayoutId layoutId = new CDataStageLayoutId(381, 0, 2);
                                List<EnemySpawn> enemySpawns = ((DdonGameServer) Server).EnemyManager.GetAssets(layoutId, 0);
                                for(byte i=1; i<enemySpawns.Count; i++)
                                {
                                    S2CInstanceEnemyRepopNtc repopNtc = new S2CInstanceEnemyRepopNtc();
                                    repopNtc.LayoutId = layoutId;
                                    repopNtc.EnemyData.PositionIndex = i;
                                    repopNtc.EnemyData.EnemyInfo = enemySpawns[i].Enemy;
                                    repopNtc.WaitSecond = 0;
                                    client.Party.SendToAll(repopNtc);
                                }
                            }

                            res.QuestProcessStateList = new List<CDataQuestProcessState>()
                            {
                                new CDataQuestProcessState()
                                {
                                    ProcessNo = 1,
                                    CheckCommandList = new List<CDataQuestProcessState.MtTypedArrayCDataQuestCommand>()
                                    {
                                        new CDataQuestProcessState.MtTypedArrayCDataQuestCommand()
                                        {
                                            ResultCommandList = new List<CDataQuestCommand>()
                                            {
                                                // CheckCommand.EmHpLess(436, 2, 0, 30)
                                                new CDataQuestCommand(48,  436, 2, 0, 30)
                                            }
                                        }
                                    }
                                }
                            };
                            break;

                        case 1:
                            {
                                CDataStageLayoutId layoutId = new CDataStageLayoutId(381, 0, 2);
                                List<EnemySpawn> enemySpawns = ((DdonGameServer) Server).EnemyManager.GetAssets(layoutId, 0);
                                for(byte i=1; i<enemySpawns.Count; i++)
                                {
                                    S2CInstanceEnemyRepopNtc repopNtc = new S2CInstanceEnemyRepopNtc();
                                    repopNtc.LayoutId = layoutId;
                                    repopNtc.EnemyData.PositionIndex = i;
                                    repopNtc.EnemyData.EnemyInfo = enemySpawns[i].Enemy;
                                    repopNtc.WaitSecond = 0;
                                    client.Party.SendToAll(repopNtc);
                                }
                            }

                            res.QuestProcessStateList = new List<CDataQuestProcessState>()
                            {
                                new CDataQuestProcessState()
                                {
                                    ProcessNo = 2,
                                    CheckCommandList = new List<CDataQuestProcessState.MtTypedArrayCDataQuestCommand>()
                                    {
                                        new CDataQuestProcessState.MtTypedArrayCDataQuestCommand()
                                        {
                                            ResultCommandList = new List<CDataQuestCommand>()
                                            {
                                                // QuestManager.CheckCommand.DieEnemy(436, 2, 0)
                                                new CDataQuestCommand(2,  436, 2, 0)
                                            }
                                        }
                                    }
                                }
                            };
                            break;
                        
                        case 2:
                            res.QuestProcessStateList = new List<CDataQuestProcessState>()
                            {
                                new CDataQuestProcessState()
                                {
                                    ProcessNo = 3,
                                    ResultCommandList = new List<CDataQuestCommand>()
                                    {
                                        // ResultCommand.EventExec(436, 1)
                                        new CDataQuestCommand(13,  436, 1),
                                        // ResultCommand.SetAnnounce(QuestAnnounceType.Clear)
                                        CDataQuestCommand.ResultSetAnnounce(CDataQuestCommand.AnnounceType.QUEST_ANNOUNCE_TYPE_CLEAR)
                                    }
                                }
                            };
                            break;
                    }

                    client.Send(res);

                    // Sent for the rest of the party members
                    S2CQuestQuestProgressNtc ntc = new S2CQuestQuestProgressNtc();
                    ntc.ProgressCharacterId = packet.Structure.ProgressCharacterId;
                    ntc.QuestScheduleId = res.QuestScheduleId;
                    ntc.QuestProcessStateList = res.QuestProcessStateList;
                    client.Party.SendToAllExcept(ntc, client);
                    break;
                }

------------------------------------------------

            private static readonly byte[] pcap_data = new byte[] {
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x1C, 0x00, 0x00, 0x00,  0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x09,
            0x1E, 0x02, 0x66, 0xEE, 0x3A, 0x00, 0x00, 0x00,  0x41, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x27,  0x10, 0x00, 0x00, 0x1F, 0xB4, 0x00, 0x00, 0x00,
            0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,  0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x5D, 0xD2, 0xF8, 0x40, 0x00, 0x00, 0x00,  0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,  0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,  0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,  0x00, 0x01, 0x00, 0x00, 0x00, 0x1C, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,  0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x03, 0x01, 0x00, 0x00, 0x00, 0xF9, 0x00,  0x00, 0x00, 0x41, 0x00, 0x00, 0x03, 0xE7, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x01,  0x00, 0x01, 0x39, 0x00, 0x00, 0x00, 0x00, 0x3F,
        };
#endif
