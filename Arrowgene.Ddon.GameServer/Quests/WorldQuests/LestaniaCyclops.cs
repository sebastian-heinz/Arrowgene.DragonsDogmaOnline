using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Handler;
using Arrowgene.Ddon.GameServer.Quests.MainQuests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
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
            private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(LestaniaCyclops));
            public LestaniaCyclops() : base(QuestType.World, true)
            {
            }

            public override QuestRewardParams RewardParams => new QuestRewardParams()
            {
                RandomRewardNum = 1
            };

            public override List<CDataWalletPoint> WalletRewards => new List<CDataWalletPoint>()
            {
                new CDataWalletPoint() { Type = WalletType.Gold, Value = 390 },
                new CDataWalletPoint() { Type = WalletType.RiftPoints, Value = 70 }
            };

            public override List<CDataRewardItem> FixedItemRewards => new List<CDataRewardItem>()
            {
                new CDataRewardItem() {ItemId = 21281, Num = 2},
            };

            public override List<CDataQuestExp> ExpRewards => new List<CDataQuestExp>()
            {
                new CDataQuestExp() { ExpMode = 1, Reward = 590 }
            };

            public override bool HasEnemiesInCurrentStageGroup(uint stageNo, uint groupId, uint subGroupId)
            {
                return (stageNo == (uint)StageNo.Lestania) && (groupId == 26);
            }

            public override List<S2CQuestQuestProgressWorkSaveNtc> GetProgressWorkNotices(uint stageNo, uint groupId, uint subGroupId)
            {
                return new List<S2CQuestQuestProgressWorkSaveNtc>();
            }

            public override CDataQuestList ToCDataQuestList()
            {
                // http://ddon.wikidot.com/wq:theknightsbitterenemy
                var quest = new CDataQuestList()
                {
                    QuestId = (uint)QuestId.LestaniaCyclops,
                    QuestScheduleId = (uint)QuestId.LestaniaCyclops,
                    BaseLevel = 12,
                    IsClientOrder = false,
                    IsEnable = true,
                    BaseExp = ExpRewards,
                    BaseWalletPoints = WalletRewards,
                    FixedRewardItemList = FixedItemRewards,
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

            public override List<CDataQuestProcessState> StateMachineExecute(uint processNo, uint reqNo, out QuestState questState)
            {
                List<CDataQuestProcessState> result = new List<CDataQuestProcessState>();
                questState = QuestState.InProgress;
                switch (processNo)
                {
                    case 0:
                        switch (reqNo)
                        {
                            case 0:
                                result = new List<CDataQuestProcessState>()
                                {
                                    new CDataQuestProcessState()
                                    {
                                        ProcessNo = 0x0, SequenceNo = 0x0, BlockNo = 0x2,
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
                                break;
                            case 1:
                                result = new List<CDataQuestProcessState>()
                                {
                                    new CDataQuestProcessState()
                                    {
                                        ProcessNo = 0x0, SequenceNo = 0x1, BlockNo = 0x3,
                                        ResultCommandList = new List<CDataQuestCommand>()
                                        {
                                            QuestManager.ResultCommand.SetAnnounce(QuestAnnounceType.Clear),
                                        }
                                    }
                                };
                                questState = QuestState.Complete;
                                break;
                        }
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
