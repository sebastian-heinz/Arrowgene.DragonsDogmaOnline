using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.GameServer.Quests.MainQuests
{

    public class Mq030260_HopesBitterEnd : Quest
    {
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
            // var quest = QuestManager.CloneMainQuest(_QuestAssets, MainQuestId.HopesBitterEnd);
            var quest = new CDataQuestList();
            quest.QuestId = (int)QuestId.HopesBitterEnd;
            quest.KeyId = (int)QuestId.HopesBitterEnd;
            quest.QuestScheduleId = (int)QuestId.HopesBitterEnd;
            quest.BaseLevel = 100;
            quest.OrderNpcId = NpcId.TheWhiteDragon;
            quest.NameMsgId = 1;
            quest.DetailMsgId = 1;
            quest.DistributionStartDate = 1440993600;
            quest.DistributionEndDate = 4103413199;

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
                QuestManager.AcceptConditions.MainQuestCompletionRestriction(QuestId.TheRelicsOfTheFirstKing)
            };

            // These correspond with QstTalkChg items in CDataQuestProcessState[0].ResultCommandList
            quest.QuestTalkInfoList.Add(new CDataQuestTalkInfo(NpcId.TheWhiteDragon, 0x57b1));
            quest.QuestTalkInfoList.Add(new CDataQuestTalkInfo(NpcId.Gillian0, 0x75d4));
            quest.QuestTalkInfoList.Add(new CDataQuestTalkInfo(NpcId.LiberationArmySoldier0, 0x75a4));
            quest.QuestTalkInfoList.Add(new CDataQuestTalkInfo(NpcId.LiberationArmySoldier1, 0x75a5));
            quest.QuestTalkInfoList.Add(new CDataQuestTalkInfo(NpcId.Gurdolin3, 0x7809));
            quest.QuestTalkInfoList.Add(new CDataQuestTalkInfo(NpcId.Lise0, 0x750a));
            quest.QuestTalkInfoList.Add(new CDataQuestTalkInfo(NpcId.Elliot0, 0x780b));

            quest.QuestLayoutFlagList.Add(new CDataQuestLayoutFlag() { FlagId = 0x1eb4 }); // This makes Lise, Gurdolin and Elliot appear
            quest.QuestLayoutFlagList.Add(new CDataQuestLayoutFlag() { FlagId = 0x1f4d });

            quest.QuestProcessStateList = new List<CDataQuestProcessState>()
            {
                new CDataQuestProcessState()
                {
                    ProcessNo = 0x0, SequenceNo = 0x0, BlockNo = 0x1,
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.QstTalkChg(NpcId.TheWhiteDragon, 0x57b1),
                        QuestManager.ResultCommand.QstLayoutFlagOn(0x1eb4),
                        QuestManager.ResultCommand.QstLayoutFlagOn(0x1f4d),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Gillian0, 0x75d4),
                        QuestManager.ResultCommand.WorldManageLayoutFlagOn(0x1f12, 0x42c9e69),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.LiberationArmySoldier0, 0x75a4),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.LiberationArmySoldier1, 0x75a5),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Gurdolin3, 0x7809),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Lise0, 0x780a),
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Elliot0, 0x780b),
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
                        QuestManager.CheckCommand.MyQstFlagOn(0x129a)
                    }),
                },
                new CDataQuestProcessState()
                {
                    ProcessNo=0x2, SequenceNo=0x0, BlockNo=0x0,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.MyQstFlagOn(0x129d),
                        QuestManager.CheckCommand.StageNoWithoutMarker(StageNo.EvilDragonsRoost1),
                        QuestManager.CheckCommand.MyQstFlagOff(0x129e)
                    })
                },
                new CDataQuestProcessState()
                {
                    ProcessNo=0x3, SequenceNo=0x0, BlockNo=0x0,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.MyQstFlagOn(0x133d),
                        QuestManager.CheckCommand.MyQstFlagOff(0x1340),
                        QuestManager.CheckCommand.SceHitInWithoutMarker(StageNo.SacredFlamePath, 1)
                    })
                },
                new CDataQuestProcessState()
                {
                    ProcessNo=0x4, SequenceNo=0x0, BlockNo=0x0,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.MyQstFlagOn(0x133e),
                        QuestManager.CheckCommand.MyQstFlagOff(0x1341),
                        QuestManager.CheckCommand.SceHitInWithoutMarker(StageNo.SacredFlamePath, 2)
                    })
                },
                new CDataQuestProcessState()
                {
                    ProcessNo=0x5, SequenceNo=0x0, BlockNo=0x0,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.MyQstFlagOn(0x133f),
                        QuestManager.CheckCommand.MyQstFlagOff(0x1342),
                        QuestManager.CheckCommand.SceHitInWithoutMarker(StageNo.SacredFlamePath, 3)
                    })
                },
                new CDataQuestProcessState()
                {
                    ProcessNo=0x6, SequenceNo=0x0, BlockNo=0x0,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.MyQstFlagOn(0x129d),
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
                        QuestManager.CheckCommand.MyQstFlagOn(0x13bd),
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
                        QuestManager.CheckCommand.MyQstFlagOn(0x129d),
                        QuestManager.CheckCommand.MyQstFlagOff(0x129e),
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

            quest.QuestEnemyInfoList.Add(new CDataQuestEnemyInfo() { GroupId = 0x155, Lv = 100, IsPartyRecommend = true });
            quest.QuestEnemyInfoList.Add(new CDataQuestEnemyInfo() { GroupId = 0x16d, Lv = 100, IsPartyRecommend = true });
            quest.QuestEnemyInfoList.Add(new CDataQuestEnemyInfo() { GroupId = 0x143, Lv = 100, IsPartyRecommend = false });
            quest.QuestEnemyInfoList.Add(new CDataQuestEnemyInfo() { GroupId = 0x149, Lv = 100, IsPartyRecommend = false });
            quest.QuestEnemyInfoList.Add(new CDataQuestEnemyInfo() { GroupId = 0x169, Lv = 100, IsPartyRecommend = false });
            quest.QuestEnemyInfoList.Add(new CDataQuestEnemyInfo() { GroupId = 0x147, Lv = 100, IsPartyRecommend = false });
            quest.QuestEnemyInfoList.Add(new CDataQuestEnemyInfo() { GroupId = 0x161, Lv = 100, IsPartyRecommend = false });
            quest.QuestEnemyInfoList.Add(new CDataQuestEnemyInfo() { GroupId = 0x170, Lv = 100, IsPartyRecommend = false });

            quest.QuestLayoutFlagSetInfoList = new List<CDataQuestLayoutFlagSetInfo>() {
                QuestManager.LayoutFlag.Create(0x00001eba, StageNo.SacredFlamePath, 0x11),
                QuestManager.LayoutFlag.Create(0x00001eb8, StageNo.SacredFlamePath, 0x12),
                QuestManager.LayoutFlag.Create(0x00001eb9, StageNo.SacredFlamePath, 0x3),
                QuestManager.LayoutFlag.Create(0x00001ebb, StageNo.SacredFlamePath, 0x1),
            };

            return quest;
        }

        public override List<CDataQuestProcessState> StateMachineExecute(uint keyId, uint questScheduleId, uint processNo, out QuestState questState)
        {
            List<CDataQuestProcessState> result = new List<CDataQuestProcessState>();

            result = new List<CDataQuestProcessState>()
            {
                new CDataQuestProcessState() {
                    ProcessNo = (ushort)(processNo + 1), SequenceNo = 0, BlockNo = 0,
                }
            };

            questState = QuestState.InProgress;

            return result;
        }
    }
}
