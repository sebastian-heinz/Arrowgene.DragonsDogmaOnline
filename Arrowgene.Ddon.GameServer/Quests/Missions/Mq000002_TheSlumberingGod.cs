using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.GameServer.Quests.Missions
{
    public class Mq000002_TheSlumberingGod
    {
        public static CDataQuestList Create(QuestAsset questAssets)
        {
            var refquest = QuestManager.CloneMainQuest(questAssets, MainQuestId.TheGreatAlchemist);
            var quest = new CDataQuestList();
            quest.QuestId = (int)MainQuestId.TheSlumberingGod;
            quest.QuestScheduleId = 287350;
            quest.KeyId = 1337;
            quest.BaseLevel = 1;
            quest.Unk0 = 1;
            quest.Unk1 = 1;
            quest.Unk2 = 1;
            quest.BaseLevel = 1;
            quest.DistributionStartDate = 1440993600;
            quest.DistributionEndDate = 4103413199;

            quest.QuestLayoutFlagList = refquest.QuestLayoutFlagList;

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

        public static List<CDataQuestProcessState> StateMachineExecute(uint keyId, uint questScheduleId, uint processNo)
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
                    break;
            }

            return result;
        }
    }
}
