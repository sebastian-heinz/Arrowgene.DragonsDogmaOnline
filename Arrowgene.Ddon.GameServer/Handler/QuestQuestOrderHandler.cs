using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

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
            var questScheduleId = request.QuestScheduleId;
            var quest = QuestManager.GetQuestByScheduleId(questScheduleId);
            var questStateManager = QuestManager.GetQuestStateManager(client, quest);

            if (questStateManager.GetActiveQuestScheduleIds().Contains(questScheduleId))
            {
                return res;
            }

            questStateManager.AddNewQuest(QuestManager.GetQuestByScheduleId(questScheduleId), 0);
            res.QuestProcessStateList.AddRange(quest.ToCDataQuestList(0).QuestProcessStateList);

            return res;

            // TODO: Investigate why the quest board UI fails to update promptly.
        }
    }
}
