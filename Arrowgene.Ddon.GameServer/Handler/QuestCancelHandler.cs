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
    public class QuestCancelHandler : GameRequestPacketHandler<C2SQuestQuestCancelReq, S2CQuestQuestCancelRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestCancelHandler));

        public QuestCancelHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestQuestCancelRes Handle(GameClient client, C2SQuestQuestCancelReq packet)
        {
            QuestId questId = (QuestId)packet.QuestScheduleId;

            var quest = QuestManager.GetQuest(questId);
            Server.Database.RemoveQuestProgress(client.Character.CommonId, quest.QuestId, quest.QuestType);
            client.Party.QuestState.CancelQuest(quest.QuestId);

            S2CQuestQuestCancelNtc cancelNtc = new S2CQuestQuestCancelNtc()
            {
                QuestId = (uint) quest.QuestId,
                QuestScheduleId = (uint) quest.QuestScheduleId
            };
            client.Party.SendToAll(cancelNtc);

            if (Server.Database.DeletePriorityQuest(client.Character.CommonId, questId))
            {
                client.Party.QuestState.UpdatePriorityQuestList(Server, client.Party);
            }

            return new S2CQuestQuestCancelRes()
            {
                QuestScheduleId = packet.QuestScheduleId
            };
        }
    }
}

