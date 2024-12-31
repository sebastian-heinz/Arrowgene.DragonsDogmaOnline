using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetPriorityQuestHandler : GameRequestPacketHandler<C2SQuestGetPriorityQuestReq, S2CQuestGetPriorityQuestRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetPriorityQuestHandler));


        public QuestGetPriorityQuestHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestGetPriorityQuestRes Handle(GameClient client, C2SQuestGetPriorityQuestReq request)
        {
            // client.Send(GameFull.Dump_144);
            S2CQuestGetPriorityQuestRes res = new S2CQuestGetPriorityQuestRes();

            Character partyLeader = client.Party.Leader?.Client.Character ?? client.Character;
            CDataPriorityQuestSetting setting = new CDataPriorityQuestSetting();
            setting.CharacterId = partyLeader.CharacterId;

            var priorityQuestScheduleIds = Server.Database.GetPriorityQuestScheduleIds(partyLeader.CommonId);
            foreach (var questScheduleId in priorityQuestScheduleIds)
            {
                if (!QuestManager.IsQuestEnabled(questScheduleId))
                {
                    Logger.Error(client, $"Priority quest for quest state which doesn't exist or is not enabled, schedule {questScheduleId}");
                    Server.Database.DeletePriorityQuest(client.Character.CommonId, questScheduleId);
                    continue;
                }

                var quest = QuestManager.GetQuestByScheduleId(questScheduleId);
                if (quest == null)
                {
                    Logger.Error(client, $"No quest object exists for {questScheduleId}");
                    continue;
                }

                var questStateManager = QuestManager.GetQuestStateManager(client, quest);
                if (questStateManager == null)
                {
                    Logger.Error(client, $"Unable to fetch the quest state manager for {questScheduleId}");
                    continue;
                }

                var questState = questStateManager.GetQuestState(questScheduleId);
                if (questState == null)
                {
                    Logger.Error(client, $"Failed to find quest state for {questScheduleId}");
                    continue;
                }

                setting.PriorityQuestList.Add(quest.ToCDataPriorityQuest(questState?.Step ?? 0));
            }

            res.PriorityQuestSettingsList.Add(setting);

            return res;
        }
    }
}
