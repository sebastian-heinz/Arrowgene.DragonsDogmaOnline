using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Quests.MainQuests
{
    public class Mq000002_TheSlumberingGod : Quest
    {
        public Mq000002_TheSlumberingGod() : base()
        {
            QuestType = QuestType.Main;
        }

        public override S2CItemUpdateCharacterItemNtc CreateRewardsPacket()
        {
            S2CItemUpdateCharacterItemNtc rewardNtc = new S2CItemUpdateCharacterItemNtc();
            // rewardNtc.UpdateType = (ushort)ItemNoticeType.Quest;
            // rewardNtc.UpdateWalletList.Add(new CDataUpdateWalletPoint() { Type = WalletType.Gold, AddPoint = 390 });
            // rewardNtc.UpdateWalletList.Add(new CDataUpdateWalletPoint() { Type = WalletType.RiftPoints, AddPoint = 70 });
            return rewardNtc;
        }

        public override bool HasEnemiesInCurrentStageGroup(uint stageNo, uint groupId, uint subGroupId)
        {
            return false;
        }

        public override CDataQuestList ToCDataQuestList()
        {
            // var refquest = QuestManager.CloneMainQuest(questAssets, MainQuestId.TheGreatAlchemist);
            var quest = new CDataQuestList();
            quest.QuestId = (int) QuestId.TheSlumberingGod;
            quest.QuestScheduleId = (int) QuestId.TheSlumberingGod;
            quest.BaseLevel = 1;
            quest.Unk0 = 1;
            quest.Unk1 = 1;
            quest.Unk2 = 1;
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
        public override List<CDataQuestProcessState> StateMachineExecute(uint keyId, uint questScheduleId, uint processNo, out QuestState questState)
        {
            List<CDataQuestProcessState> result = new List<CDataQuestProcessState>();

            switch (processNo)
            {
                case 0:
                    result = new List<CDataQuestProcessState>()
                    {
                        new CDataQuestProcessState() {
                            ProcessNo = 1, SequenceNo = 0x0, BlockNo = 0x0,
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
                            ProcessNo = 0x1, SequenceNo = 0x0, BlockNo = 0x0,
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
