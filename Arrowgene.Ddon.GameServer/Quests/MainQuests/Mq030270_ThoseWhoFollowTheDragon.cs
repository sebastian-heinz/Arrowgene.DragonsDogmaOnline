using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.GameServer.Quests.MainQuests
{
    internal class Mq030270_ThoseWhoFollowTheDragon
    {
    }
}

#if false
            switch (processNo)
            {
                case 0:
                    uint reqNo = _ProcessTracker[processNo];
                    switch (reqNo)
                    {
                        case 0:
                            result = new List<CDataQuestProcessState>()
                            {
                                new CDataQuestProcessState() {
                                    ProcessNo = 1, SequenceNo = 0, BlockNo = 0,
                                    ResultCommandList = new List<CDataQuestCommand>()
                                    {
                                        QuestManager.ResultCommand.ReturnCheckPoint(0),
                                        QuestManager.ResultCommand.ReturnCheckPoint(1),
                                    }
                                },
                                new CDataQuestProcessState() {
                                    ProcessNo = 0, SequenceNo = 0, BlockNo = 1,
                                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                                    {
                                        QuestManager.CheckCommand.NpcTalkAndOrderUi(StageNo.AudienceChamber, NpcId.TheWhiteDragon, 0)
                                    }),
                                    ResultCommandList = new List<CDataQuestCommand>()
                                    {
                                        QuestManager.ResultCommand.QstTalkChg(NpcId.TheWhiteDragon, 22450)
                                    }
                                },
                            };
                            break;
                        case 1:
                            result = new List<CDataQuestProcessState>()
                            {
                                new CDataQuestProcessState() {
                                    ProcessNo = 0, SequenceNo = 0, BlockNo = 2,
                                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                                    {
                                        QuestManager.CheckCommand.EventEnd(StageNo.AudienceChamber, 230)
                                    }),
                                    ResultCommandList = new List<CDataQuestCommand>()
                                    {
                                        QuestManager.ResultCommand.EventExec(StageNo.AudienceChamber, 230, 0, 0),
                                        QuestManager.ResultCommand.SetCheckPoint()
                                    }
                                },
                            };
                            break;
                        case 2:
                            result = new List<CDataQuestProcessState>()
                            {
                                new CDataQuestProcessState() {
                                    ProcessNo = 0, SequenceNo = 0, BlockNo = 3,
                                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                                    {
                                        QuestManager.CheckCommand.EventEnd(StageNo.MarquiseKurtsResidence, 0)
                                    }),
                                    ResultCommandList = new List<CDataQuestCommand>()
                                    {
                                        QuestManager.ResultCommand.ExeEventAfterStageJumpContinue(StageNo.MarquiseKurtsResidence, 0, 0)
                                    }
                                },
                            };
                            break;
                    }
                    break;
                case 1:
                    result = new List<CDataQuestProcessState>()
                    {
                        new CDataQuestProcessState() {
                            ProcessNo = 1, SequenceNo = 0, BlockNo = 1,
                            CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                            {
                                QuestManager.CheckCommand.IsLogin()
                            }),
                            ResultCommandList = new List<CDataQuestCommand>()
                            {
                                QuestManager.ResultCommand.SetCheckPoint()
                            }
                        }
                    };
                    break;
                default:
                    result = new List<CDataQuestProcessState>()
                    {
                        new CDataQuestProcessState() {
                            ProcessNo = (ushort)(processNo + 1), SequenceNo = 0, BlockNo = 0,
                        }
                    };
                    break;
            }
#endif
