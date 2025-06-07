using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestQuestOrderHandler : GameRequestPacketHandler<C2SQuestQuestOrderReq, S2CQuestQuestOrderRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestQuestOrderHandler));

        public QuestQuestOrderHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestQuestOrderRes Handle(GameClient client, C2SQuestQuestOrderReq request)
        {
            var res = new S2CQuestQuestOrderRes();
            var quest = QuestManager.GetQuestByScheduleId(request.QuestScheduleId)
                ?? throw new ResponseErrorException(Shared.Model.ErrorCode.ERROR_CODE_QUEST_INTERNAL_ERROR);

            var questStateManager = QuestManager.GetQuestStateManager(client, quest);
            if (questStateManager.GetActiveQuestScheduleIds().Contains(quest.QuestScheduleId))
            {
                return res;
            }

            var process = quest.ToCDataQuestList(0).QuestProcessStateList;

            if (QuestManager.IsBoardQuest(quest.QuestId))
            {
                // Force an accept announce on the first step to make the UI happy.
                process.First().ResultCommandList.Add(QuestManager.ResultCommand.SetAnnounce(QuestAnnounceType.Accept, 1));
                Server.Database.InsertQuestProgress(client.Character.CommonId, quest.QuestScheduleId, quest.QuestType, 1);
                questStateManager.AddNewQuest(quest, 1);
            }
            else
            {
                questStateManager.AddNewQuest(quest, 0);
            }

            res.QuestProcessStateList.AddRange(process);

            return res;
        }
    }
}
