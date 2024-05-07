using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.GameServer.Quests.WorldQuests
{
    public class WorldQuests
    {
        public class LestaniaCyclops
        {
            public static CDataQuestList Create()
            {
                // http://ddon.wikidot.com/wq:theknightsbitterenemy
                var quest = new CDataQuestList()
                {
                    QuestId = 20005010,
                    QuestScheduleId = 20005010,
                    BaseLevel = 12,
                    BaseExp = new List<CDataQuestExp>()
                    {
                        new CDataQuestExp() {ExpMode = 1, Reward = 590},
                    },
                    BaseWalletPoints = new List<CDataWalletPoint>()
                    { 
                        new CDataWalletPoint() { Type = WalletType.Gold, Value = 390},
                        new CDataWalletPoint() { Type = WalletType.RiftPoints, Value = 70}
                    },
                    QuestProcessStateList = new List<CDataQuestProcessState>()
                    {
                        new CDataQuestProcessState()
                        {
                            // ProcessNo = 0x0, SequenceNo = 0x0, BlockNo = 0x1,
                            CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                            {
                                QuestManager.CheckCommand.IsEnemyFoundForOrder(StageNo.Lestania, 26, 0)
                            })
                        }
                    }
                };

                return quest;
            }

            public static List<CDataQuestProcessState> StateMachineExecute(GameClient client, uint keyId, uint questScheduleId, uint processNo, out QuestProcess processStatus)
            {
                List<CDataQuestProcessState> result = new List<CDataQuestProcessState>();

                switch (processNo)
                {
                    case 0:
                        result = new List<CDataQuestProcessState>()
                        {
                            new CDataQuestProcessState()
                            {
                                ProcessNo = 1,
                                CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                                {
                                    QuestManager.CheckCommand.DieEnemy(StageNo.Lestania, 26, 0)
                                }),
                                ResultCommandList = new List<CDataQuestCommand>()
                                {
                                    QuestManager.ResultCommand.SetAnnounce(QuestAnnounceType.Accept),
                                }
                            }
                        };
                        processStatus = QuestProcess.ExecuteCommand;
                        break;
                    case 1:
                        result = new List<CDataQuestProcessState>()
                        {
                            new CDataQuestProcessState()
                            {
                                ProcessNo = 2,
                                ResultCommandList = new List<CDataQuestCommand>()
                                {
                                    QuestManager.ResultCommand.SetAnnounce(QuestAnnounceType.Cancel),
                                }
                            }
                        };
                        processStatus = QuestProcess.ProcessEnd;
                        break;
                    default:
                        processStatus = QuestProcess.Error;
                        break;
                }

                return result;
            }
        }
    }
}
