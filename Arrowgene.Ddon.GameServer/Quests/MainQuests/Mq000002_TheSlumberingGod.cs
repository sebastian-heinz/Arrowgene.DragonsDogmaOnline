using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Quests.MainQuests
{
    public class Mq000002_TheSlumberingGod : Quest
    {
        public Mq000002_TheSlumberingGod() : base(QuestId.TheSlumberingGod, QuestType.Main)
        {
        }

        public override bool HasEnemiesInCurrentStageGroup(uint stageNo, uint groupId, uint subGroupId)
        {
            return false;
        }

        public override List<S2CQuestQuestProgressWorkSaveNtc> GetProgressWorkNotices(uint stageNo, uint groupId, uint subGroupId)
        {
            return new List<S2CQuestQuestProgressWorkSaveNtc>();
        }

        public override CDataQuestList ToCDataQuestList()
        {
            // var refquest = QuestManager.CloneMainQuest(questAssets, MainQuestId.TheGreatAlchemist);
            var quest = new CDataQuestList();
            quest.QuestId = (uint) QuestId;
            quest.QuestScheduleId = (uint) QuestId;
            quest.BaseLevel = 1;
            quest.OrderNpcId = (uint) NpcId.Leo0;
            quest.NameMsgId = 1;
            quest.DetailMsgId = 1;
            quest.BaseLevel = 1;
            quest.DistributionStartDate = 1440993600;
            quest.DistributionEndDate = 4103413199;

            // quest.QuestLayoutFlagList = refquest.QuestLayoutFlagList;

            /**
             * Each ProcessNo is a step in the quest
             * - It appears the ResultCommandList will be executed first
             * - Check command list will then be evaluated.
             */
            quest.QuestProcessStateList = new List<CDataQuestProcessState>()
            {
                new CDataQuestProcessState()
                {
                    ProcessNo = 0x0, SequenceNo = 0x0, BlockNo = 0x1,
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.StageNo(StageNo.WhiteDragonTemple)
                    }),
                }
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
                            ResultCommandList = new List<CDataQuestCommand>()
                            {
                                QuestManager.ResultCommand.EventExec(StageNo.WhiteDragonTemple, 0, StageNo.WhiteDragonTemple, 0x1),
                                QuestManager.ResultCommand.SetAnnounce(QuestAnnounceType.Accept),
                            },
                            CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                            {
                                QuestManager.CheckCommand.NpcTalkAndOrderUi(StageNo.AudienceChamber, NpcId.Leo0, 0)
                            }),
                        }
                    };
                    questState = QuestState.InProgress;
                    break;
                case 1:
                    result = new List<CDataQuestProcessState>()
                    {
                        new CDataQuestProcessState() {
                            ProcessNo = 2, SequenceNo = 0, BlockNo = 0,
                            CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                            {
                                QuestManager.CheckCommand.MyQstFlagOn(0x129a)
                            }),
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
