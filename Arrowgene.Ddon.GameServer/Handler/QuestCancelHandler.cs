using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;

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

            var quest = client.Party.QuestState.GetQuest(questId);
            Server.Database.RemoveQuestProgress(client.Character.CommonId, quest.QuestId, quest.QuestType);
            
            bool isPriority = Server.Database.DeletePriorityQuest(client.Character.CommonId, questId);

            if (client.Party.Leader.Client == client) //Only the leader should be able to inform the party quest state.
            {
                client.Party.QuestState.CancelQuest(quest.QuestId);

                S2CQuestQuestCancelNtc cancelNtc = new S2CQuestQuestCancelNtc()
                {
                    QuestId = (uint)quest.QuestId,
                    QuestScheduleId = (uint)quest.QuestScheduleId
                };
                client.Party.SendToAll(cancelNtc);

                if (isPriority)
                {
                    client.Party.QuestState.UpdatePriorityQuestList(Server, client, client.Party);
                }
            }
            
            return new S2CQuestQuestCancelRes()
            {
                QuestScheduleId = packet.QuestScheduleId
            };
        }
    }
}

