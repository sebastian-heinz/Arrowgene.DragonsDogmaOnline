using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Handler;
using Arrowgene.Ddon.GameServer.Party;
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
#if false
    public class WorldQuests
    {

        public class Q20005010_TheKnightsBitterEnemy : Quest
        {
            private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(Q20005010_TheKnightsBitterEnemy));
            public Q20005010_TheKnightsBitterEnemy() : base(QuestId.TheKnightsBitterEnemy, QuestType.World, true)
            {
            }


            public override QuestRewardParams RewardParams => new QuestRewardParams()
            {
            };

            public override List<CDataWalletPoint> WalletRewards => new List<CDataWalletPoint>()
            {
                new CDataWalletPoint() { Type = WalletType.Gold, Value = 390 },
                new CDataWalletPoint() { Type = WalletType.RiftPoints, Value = 70 }
            };

            public override List<QuestReward> ItemRewards => new List<QuestReward>()
            {
                new QuestRandomReward()
                {
                    LootPool = new List<QuestRandomRewardItem>()
                    {
                        new QuestRandomRewardItem() {ItemId = 18825, Num = 50, Chance = 0.7},
                        new QuestRandomRewardItem() {ItemId = 18826, Num = 5, Chance = 0.3},
                        new QuestRandomRewardItem() {ItemId = 21281, Num = 2, Chance = 0.1},
                    }
                },
                new QuestRandomReward()
                {
                    LootPool = new List<QuestRandomRewardItem>()
                    {
                        new QuestRandomRewardItem() {ItemId = 62,   Num = 1, Chance = 0.5},
                        new QuestRandomRewardItem() {ItemId = 1674, Num = 1, Chance = 0.4},
                        new QuestRandomRewardItem() {ItemId = 2697, Num = 1, Chance = 0.3},
                        new QuestRandomRewardItem() {ItemId = 3720, Num = 1, Chance = 0.2},
                        new QuestRandomRewardItem() {ItemId = 4743, Num = 1, Chance = 0.1},
                    }
                }
            };
                        
            List<CDataQuestExp> ExpRewards => new List<CDataQuestExp>()
            {
                new CDataQuestExp() { ExpMode = 1, Reward = 590 }
            };

        public override bool HasEnemiesInCurrentStageGroup(uint stageNo, uint groupId, uint subGroupId)
            {
                return (stageNo == (uint)StageNo.Lestania) && (groupId == 26);
            }

            public override List<InstancedEnemy> GetEnemiesInCurrentStageGroup(uint stageNo, uint groupId, uint subGroupId)
            {
                return new List<InstancedEnemy>();
            }

            public override CDataQuestList ToCDataQuestList()
            {
                // http://ddon.wikidot.com/wq:theknightsbitterenemy
                var quest = new CDataQuestList()
                {
                    QuestId = (uint) QuestId,
                    QuestScheduleId = (uint) QuestId,
                    BaseLevel = 12,
                    IsClientOrder = false,
                    IsEnable = true,
                    BaseExp = ExpRewards,
                    BaseWalletPoints = WalletRewards,
                    FixedRewardItemList = GetQuestFixedRewards(),
                    FixedRewardSelectItemList = GetQuestSelectableRewards(),
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
                                            QuestManager.ResultCommand.EndEndQuest(),
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
#endif
}
