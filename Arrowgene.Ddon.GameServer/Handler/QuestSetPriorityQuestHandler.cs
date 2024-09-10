using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections.Generic;

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
            QuestId questId = (QuestId)packet.Structure.QuestScheduleId;

            S2CQuestSetPriorityQuestNtc ntc = new S2CQuestSetPriorityQuestNtc()
            {
                CharacterId = client.Character.CharacterId
            };

            Server.Database.InsertPriorityQuest(client.Character.CommonId, questId);

            var prioirtyQuests = Server.Database.GetPriorityQuests(client.Character.CommonId);
            foreach (var priorityQuestId in prioirtyQuests)
            {
                var quest = client.Party.QuestState.GetQuest(priorityQuestId);
                var questState = client.Party.QuestState.GetQuestState(questId);
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
