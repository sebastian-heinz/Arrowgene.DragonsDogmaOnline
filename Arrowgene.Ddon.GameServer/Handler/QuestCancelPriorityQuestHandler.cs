using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp.Consumer.BlockingQueueConsumption;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestCancelPriorityQuestHandler : GameRequestPacketHandler<C2SQuestCancelPriorityQuestReq, S2CQuestCancelPriorityQuestRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestCancelPriorityQuestHandler));

        public QuestCancelPriorityQuestHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestCancelPriorityQuestRes Handle(GameClient client, C2SQuestCancelPriorityQuestReq packet)
        {
            QuestId questId = (QuestId) packet.QuestScheduleId;

            Server.Database.DeletePriorityQuest(client.Character.CommonId, questId);

            S2CQuestSetPriorityQuestNtc ntc = new S2CQuestSetPriorityQuestNtc()
            {
                CharacterId = client.Character.CharacterId
            };

            var prioirtyQuests = Server.Database.GetPriorityQuests(client.Character.CommonId);
            foreach (var priorityQuestId in prioirtyQuests)
            {
                var quest = QuestManager.GetQuest(priorityQuestId);
                var questState = client.Party.QuestState.GetQuestState(questId);
                ntc.PriorityQuestList.Add(quest.ToCDataPriorityQuest(questState.Step));
            }

            client.Party.SendToAll(ntc);

            return new S2CQuestCancelPriorityQuestRes()
            {
                QuestScheduleId = packet.QuestScheduleId
            };
        }
    }
}
