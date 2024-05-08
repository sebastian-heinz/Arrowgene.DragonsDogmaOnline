using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Handler;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.GameServer.Quests.WorldQuests
{
    public class WorldQuests
    {
        public class LestaniaCyclops : Quest
        {
            public LestaniaCyclops() : base()
            {
                IsDiscoverable = true;
                HasEnemy = true;
                QuestType = QuestType.World;
            }

            public override S2CItemUpdateCharacterItemNtc CreateRewardsPacket()
            {
                S2CItemUpdateCharacterItemNtc rewardNtc = new S2CItemUpdateCharacterItemNtc();
                rewardNtc.UpdateType = (ushort)ItemNoticeType.Quest;
                rewardNtc.UpdateWalletList.Add(new CDataUpdateWalletPoint() { Type = WalletType.Gold, AddPoint = 390 });
                rewardNtc.UpdateWalletList.Add(new CDataUpdateWalletPoint() { Type = WalletType.RiftPoints, AddPoint = 70 });
                return rewardNtc;
            }

            public override bool HasEnemiesInCurrentStageGroup(uint stageNo, uint groupId, uint subGroupId)
            {
                return stageNo == (uint) StageNo.Lestania && groupId == 26;
            }

            public override CDataQuestList ToCDataQuestList()
            {
                // http://ddon.wikidot.com/wq:theknightsbitterenemy
                var quest = new CDataQuestList()
                {
                    QuestId = (uint) QuestId.LestaniaCyclops,
                    QuestScheduleId = (uint) QuestId.LestaniaCyclops,
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
                            ProcessNo = 0x0, SequenceNo = 0x0, BlockNo = 0x1,
                            CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                            {
                                QuestManager.CheckCommand.IsEnemyFoundForOrder(StageNo.Lestania, 26, 0)
                            })
                        }
                    }
                };

                return quest;
            }

            public override List<CDataQuestProcessState> StateMachineExecute(uint keyId, uint questScheduleId, uint processNo, out QuestState questState)
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
                                ResultCommandList = new List<CDataQuestCommand>()
                                {
                                    QuestManager.ResultCommand.SetAnnounce(QuestAnnounceType.Accept, 1)
                                },
                                CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                                {
                                    QuestManager.CheckCommand.DieEnemy(StageNo.Lestania, 26, 0)
                                })
                            }
                        };
                        questState = QuestState.InProgress;
                        break;
                    case 1:
                        result = new List<CDataQuestProcessState>()
                        {
                            new CDataQuestProcessState()
                            {
                                ProcessNo = 1, SequenceNo = 0, BlockNo = 0,
                                ResultCommandList = new List<CDataQuestCommand>()
                                {
                                    QuestManager.ResultCommand.SetAnnounce(QuestAnnounceType.Clear),
                                    QuestManager.ResultCommand.SetAnnounce(QuestAnnounceType.End),
                                    QuestManager.ResultCommand.EndEndQuest()
                                }
                            }
                        };
                        questState = QuestState.Cleared;
                        break;
                    default:
                        questState = QuestState.Unknown;
                        break;
                }

                return result;
            }
        }
    }
}
