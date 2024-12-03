using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestSetPriorityQuestHandler : GameRequestPacketHandler<C2SQuestSetPriorityQuestReq, S2CQuestSetPriorityQuestRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestSetPriorityQuestHandler));
        
        public QuestSetPriorityQuestHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestSetPriorityQuestRes Handle(GameClient client, C2SQuestSetPriorityQuestReq request)
        {
            S2CQuestSetPriorityQuestNtc ntc = new S2CQuestSetPriorityQuestNtc()
            {
                CharacterId = client.Character.CharacterId
            };

            Server.Database.InsertPriorityQuest(client.Character.CommonId, request.QuestScheduleId);

            var priorityQuests = Server.Database.GetPriorityQuestScheduleIds(client.Character.CommonId);
            foreach (var questScheduleId in priorityQuests)
            {
                if (!QuestManager.IsQuestEnabled(questScheduleId))
                {
                    Logger.Error(client, $"Priority quest for quest state which doesn't exist or is not enabled, schedule {questScheduleId}");
                    Server.Database.DeletePriorityQuest(client.Character.CommonId, questScheduleId);
                    continue;
                }
                var quest = QuestManager.GetQuestByScheduleId(questScheduleId);
                var questStateManager = QuestManager.GetQuestStateManager(client, quest);
                var questState = questStateManager.GetQuestState(questScheduleId);
                ntc.PriorityQuestList.Add(quest.ToCDataPriorityQuest(questState?.Step ?? 0));
            }

            client.Party.SendToAll(ntc);

            return new S2CQuestSetPriorityQuestRes()
            {
                QuestScheduleId = request.QuestScheduleId
            };
        }
    }
}
