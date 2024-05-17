using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Quests.MainQuests
{
    public class Mq000025_TheGreatAlchemist : Quest
    {
        public Mq000025_TheGreatAlchemist() : base(QuestId.TheGreatAlchemist, QuestType.Main)
        {
        }

        public override bool HasEnemiesInCurrentStageGroup(uint stageNo, uint groupId, uint subGroupId)
        {
            return (stageNo == (uint) StageNo.SmallCaveTombs) && (groupId == 3);
        }

        public override List<S2CQuestQuestProgressWorkSaveNtc> GetProgressWorkNotices(uint stageNo, uint groupId, uint subGroupId)
        {
            return new List<S2CQuestQuestProgressWorkSaveNtc>();
        }

        public override CDataQuestList ToCDataQuestList()
        {
            // var refquest = QuestManager.CloneMainQuest(questAssets, MainQuestId.TheGreatAlchemist);
            var quest = new CDataQuestList();
            quest.QuestId = (uint)QuestId;
            quest.QuestScheduleId = (uint)QuestId;
            quest.BaseLevel = 25;
            quest.OrderNpcId = (uint) NpcId.Leo0;
            quest.NameMsgId = 1;
            quest.DetailMsgId = 1;
            quest.BaseLevel = 1;
            quest.DistributionStartDate = 1440993600;
            quest.DistributionEndDate = 4103413199;

            quest.BaseExp = new List<CDataQuestExp>() {
                new CDataQuestExp() {ExpMode = 1, Reward = 15000}
            };

            quest.BaseWalletPoints = new List<CDataWalletPoint>() {
                new CDataWalletPoint() { Type = WalletType.Gold, Value = 20000 },
                new CDataWalletPoint() { Type = WalletType.RiftPoints, Value = 500}
            };

            quest.FixedRewardItemList = new List<CDataRewardItem> {
                new CDataRewardItem() {ItemId = 8813, Num = 1}
            };

            quest.FixedRewardSelectItemList = new List<CDataRewardItem>
            {
                new CDataRewardItem() {ItemId = 8323, Num = 1},
                new CDataRewardItem() {ItemId = 8348, Num = 1},
                new CDataRewardItem() {ItemId = 8373, Num = 1},
                new CDataRewardItem() {ItemId = 8398, Num = 1},
            };

            quest.ContentsReleaseList = new List<CDataCharacterReleaseElement>()
            {
                new CDataCharacterReleaseElement(ContentsRelease.OrbEnemy),
                new CDataCharacterReleaseElement(ContentsRelease.DragonForceAugmentation),
            };

            quest.QuestOrderConditionParamList = new List<CDataQuestOrderConditionParam>()
            {
                QuestManager.AcceptConditions.ClearPersonalQuestRestriction(QuestId.CraftedTokenOfTheHeart) // Dependency on "tutorial (personal?)" quest 60000016
            };

            quest.QuestTalkInfoList.Add(new CDataQuestTalkInfo(NpcId.Leo0, 10853));

            /**
             * Each ProcessNo is a step in the quest
             * - It appears the ResultCommandList will be executed first
             * - Check command list will then be evaluated.
             */
            quest.QuestProcessStateList = new List<CDataQuestProcessState>()
            {
                new CDataQuestProcessState()
                {
                    ProcessNo = 0, SequenceNo = 0, BlockNo = 1,
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.QstTalkChg(NpcId.Leo0, 10853)
                    },
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.NpcTalkAndOrderUi(StageNo.AudienceChamber, NpcId.Leo0, 11910)
                    }),
                },
                new CDataQuestProcessState()
                {
                    ProcessNo = 1, SequenceNo = 0, BlockNo = 0,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.MyQstFlagOn(143)
                    }),
                },
                new CDataQuestProcessState()
                {
                    ProcessNo = 2, SequenceNo = 0, BlockNo = 0,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.MyQstFlagOn(141)
                    }),
                },
                new CDataQuestProcessState()
                {
                    ProcessNo = 3, SequenceNo = 0, BlockNo = 0,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.MyQstFlagOn(142),
                        QuestManager.CheckCommand.MyQstFlagOn(143),
                    }),
                },
                new CDataQuestProcessState()
                {
                    ProcessNo = 4, SequenceNo = 0, BlockNo = 0,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.MyQstFlagOn(701),
                    }),
                },
            };

            quest.QuestEnemyInfoList.Add(new CDataQuestEnemyInfo() { GroupId = 70, Lv = 10, IsPartyRecommend = true });

            quest.QuestLayoutFlagSetInfoList = new List<CDataQuestLayoutFlagSetInfo>() {
                QuestManager.LayoutFlag.Create(141, StageNo.SmallCaveTombs, 3),
            };

            return quest;
        }
        public override List<CDataQuestProcessState> StateMachineExecute(uint processNo, uint reqNo, out QuestState questState)
        {
            List<CDataQuestProcessState> result = new List<CDataQuestProcessState>();

            switch (processNo)
            {
                case 0:
                    result = new List<CDataQuestProcessState>()
                    {
                        new CDataQuestProcessState() {
                            ProcessNo = 1, SequenceNo = 0, BlockNo = 0,
                        }
                    };
                    questState = QuestState.InProgress;
                    break;
                case 1:
                    result = new List<CDataQuestProcessState>()
                    {
                        new CDataQuestProcessState() {
                            ProcessNo = 2, SequenceNo = 0, BlockNo = 0,
                        }
                    };
                    questState = QuestState.InProgress;
                    break;
                case 2:
                    result = new List<CDataQuestProcessState>()
                    {
                        new CDataQuestProcessState() {
                            ProcessNo = 3, SequenceNo = 0, BlockNo = 0,
                        }
                    };
                    questState = QuestState.InProgress;
                    break;
                case 3:
                    result = new List<CDataQuestProcessState>()
                    {
                        new CDataQuestProcessState() {
                            ProcessNo = 4, SequenceNo = 0, BlockNo = 0,
                        }
                    };
                    questState = QuestState.InProgress;
                    break;
                default:
                    questState = QuestState.Unknown;
                    break;
            }

            return result;
        }
    }
}
