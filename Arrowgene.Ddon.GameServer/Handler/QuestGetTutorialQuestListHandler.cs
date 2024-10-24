using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.IO;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetTutorialQuestListHandler : GameRequestPacketHandler<C2SQuestGetTutorialQuestListReq, S2CQuestGetTutorialQuestListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetTutorialQuestListHandler));

        public QuestGetTutorialQuestListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestGetTutorialQuestListRes Handle(GameClient client, C2SQuestGetTutorialQuestListReq request)
        {
            var result = new S2CQuestGetTutorialQuestListRes()
            {
                StageNo = request.StageNo,
            };

            var completedQuests = Server.Database.GetCompletedQuestsByType(client.Character.CommonId, QuestType.Tutorial);

            // This handler should return personal quests which have not been started
            // yet when the player enters the StageNo
            foreach (var questScheduleId in QuestManager.GetTutorialQuestsByStageNo(request.StageNo).Where(x => QuestManager.IsQuestEnabled(x)).ToList())
            {
                var quest = QuestManager.GetQuestByScheduleId(questScheduleId);

                uint stageNo = (uint) StageManager.ConvertIdToStageNo(quest.StageId);
                if (stageNo != request.StageNo)
                {
                    continue;
                }

                if (completedQuests.Exists(x => x.QuestId == quest.QuestId))
                {
                    continue;
                }

                var questStateManager = quest.IsPersonal ? client.QuestState : client.Party.QuestState;
                var questState = questStateManager.GetQuestState(questScheduleId);
                if (questState == null || questState.Step == 0)
                {
                    var tutorialQuest = quest.ToCDataTutorialQuestList(0);
                    result.TutorialQuestList.Add(tutorialQuest);
                }

                if (questState == null)
                {
                    questStateManager.AddNewQuest(quest, 0);
                }
            }

            return result;
        }
    }
}
