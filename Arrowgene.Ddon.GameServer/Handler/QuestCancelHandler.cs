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
            var quest = QuestManager.GetQuestByScheduleId(packet.QuestScheduleId);
            var questStateManager = QuestManager.GetQuestStateManager(client, quest);
            Server.Database.RemoveQuestProgress(client.Character.CommonId, quest.QuestScheduleId, quest.QuestType);
            
            bool isPriority = Server.Database.DeletePriorityQuest(client.Character.CommonId, quest.QuestScheduleId);

            if (quest.IsPersonal)
            {
                questStateManager.CancelQuest(quest.QuestScheduleId);

                if (isPriority && (client.Party.IsSolo || client.Party.Leader?.Client == client))
                {
                    client.Party.QuestState.UpdatePriorityQuestList(client.Party.Leader.Client).Send();
                }
            }
            else
            {
                if (client.Party.Leader?.Client == client) //Only the leader should be able to inform the party quest state.
                {
                    questStateManager.CancelQuest(quest.QuestScheduleId);

                    S2CQuestQuestCancelNtc cancelNtc = new S2CQuestQuestCancelNtc()
                    {
                        QuestId = (uint)quest.QuestId,
                        QuestScheduleId = quest.QuestScheduleId
                    };
                    client.Party.SendToAllExcept(cancelNtc, client);

                    if (isPriority)
                    {
                        client.Party.QuestState.UpdatePriorityQuestList(client).Send();
                    }
                }
            }
            
            return new S2CQuestQuestCancelRes()
            {
                QuestScheduleId = packet.QuestScheduleId
            };
        }
    }
}

