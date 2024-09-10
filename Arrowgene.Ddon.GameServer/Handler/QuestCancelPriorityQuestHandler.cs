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

            // The UI responds and sends this request even if you're not the leader,
            // but we probably shouldn't let non-leaders mess with the PartyQuestState.
            if (client.Party.Leader is not null && client.Party.Leader.Client == client)
            {
                client.Party.QuestState.UpdatePriorityQuestList(Server, client.Party);
            }

            return new S2CQuestCancelPriorityQuestRes()
            {
                QuestScheduleId = packet.QuestScheduleId
            };
        }
    }
}
