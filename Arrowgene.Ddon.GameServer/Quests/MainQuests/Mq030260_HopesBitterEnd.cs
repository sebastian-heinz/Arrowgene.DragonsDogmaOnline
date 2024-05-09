using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Handler;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
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

        public Mq030260_HopesBitterEnd() : base()
        {
            QuestType = QuestType.Main;
        }

        public override bool HasEnemiesInCurrentStageGroup(uint stageNo, uint groupId, uint subGroupId)
        {
            return false;
        }

        public override S2CItemUpdateCharacterItemNtc CreateRewardsPacket()
        {
            S2CItemUpdateCharacterItemNtc rewardNtc = new S2CItemUpdateCharacterItemNtc();
            rewardNtc.UpdateType = (ushort)ItemNoticeType.Quest;
            rewardNtc.UpdateWalletList.Add(new CDataUpdateWalletPoint() { Type = WalletType.Gold, AddPoint = 100000 });
            rewardNtc.UpdateWalletList.Add(new CDataUpdateWalletPoint() { Type = WalletType.RiftPoints, AddPoint = 10000 });
            return rewardNtc;
        }

        public override CDataQuestList ToCDataQuestList()
        {
            var quest = new CDataQuestList();
            quest.KeyId = (int)QuestId.HopesBitterEnd;
            quest.QuestScheduleId = (int)QuestId.HopesBitterEnd;
            quest.QuestId = (int)QuestId.HopesBitterEnd;
            quest.BaseLevel = 100;
            quest.ContentJoinItemRank = 0;
            quest.OrderNpcId = (uint) NpcId.TheWhiteDragon;
            quest.NameMsgId = 1;
            quest.DetailMsgId = 1;
            quest.DistributionStartDate = 1537405200;
            quest.DistributionEndDate = 2145884400;

            quest.BaseExp = new List<CDataQuestExp>() {
                new CDataQuestExp() {ExpMode = 1, Reward = 900000}
            };

            quest.BaseWalletPoints = new List<CDataWalletPoint>() {
                new CDataWalletPoint() { Type = WalletType.Gold, Value = 100000 },
                new CDataWalletPoint() { Type = WalletType.RiftPoints, Value = 10000}
            };

            quest.FixedRewardItemList = new List<CDataRewardItem> {
                new CDataRewardItem() {ItemId = 21281, Num = 2},
                new CDataRewardItem() {ItemId = 18825, Num = 50},
                new CDataRewardItem() {ItemId = 18826, Num = 5}
            };


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

            quest.QuestLayoutFlagList.Add(new CDataQuestLayoutFlag() { FlagId = 7860 }); // This makes Lise, Gurdolin and Elliot appear
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
                    ProcessNo=0xb, SequenceNo=0x0, BlockNo=0x0,
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

            Logger.Debug(JsonSerializer.Serialize<CDataQuestList>(quest));

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
                        new CDataQuestProcessState() {
                            ProcessNo = (ushort)(processNo + 1), SequenceNo = 0, BlockNo = 0,
                            ResultCommandList = new List<CDataQuestCommand>()
                            {
                                QuestManager.ResultCommand.SetAnnounce(QuestAnnounceType.Accept),
                                // QuestManager.ResultCommand.StageJump(StageNo.EvilDragonsRoost1, 2),
                                QuestManager.ResultCommand.MyQstFlagOn(4762),
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

            questState = QuestState.InProgress;

            return result;
        }
    }
}
