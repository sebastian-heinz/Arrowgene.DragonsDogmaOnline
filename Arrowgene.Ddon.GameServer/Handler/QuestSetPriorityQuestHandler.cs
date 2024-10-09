using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestSetPriorityQuestHandler : GameStructurePacketHandler<C2SQuestSetPriorityQuestReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestSetPriorityQuestHandler));
        
        public QuestSetPriorityQuestHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SQuestSetPriorityQuestReq> packet)
        {
            S2CQuestSetPriorityQuestNtc ntc = new S2CQuestSetPriorityQuestNtc()
            {
                CharacterId = client.Character.CharacterId
            };

            Server.Database.InsertPriorityQuest(client.Character.CommonId, packet.Structure.QuestScheduleId);

            var prioirtyQuests = Server.Database.GetPriorityQuestScheduleIds(client.Character.CommonId);
            foreach (var questScheduleId in prioirtyQuests)
            {
                var quest = client.Party.QuestState.GetQuest(questScheduleId);
                var questState = client.Party.QuestState.GetQuestState(questScheduleId);
                ntc.PriorityQuestList.Add(quest.ToCDataPriorityQuest(questState.Step));
            }

            client.Party.SendToAll(ntc);

            client.Send(new S2CQuestSetPriorityQuestRes()
            {
                QuestScheduleId = packet.Structure.QuestScheduleId
            });
        }
    }
}
