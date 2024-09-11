using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model.Quest;
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

            QuestId questId = (QuestId)request.QuestScheduleId;
            var quest = client.Party.QuestState.GetQuest(questId);
            if (client.Party.QuestState.GetActiveQuestIds().Contains(questId))
            {
                return res;
            }

            client.Party.QuestState.AddNewQuest(questId, 0, false);
            res.QuestProcessStateList.AddRange(quest.ToCDataQuestList(0).QuestProcessStateList);

            return res;

            // TODO: Investigate why the quest board UI fails to update promptly.
        }
    }
}
