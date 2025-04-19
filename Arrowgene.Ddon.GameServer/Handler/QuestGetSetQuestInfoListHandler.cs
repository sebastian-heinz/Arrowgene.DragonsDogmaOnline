using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetSetQuestInfoListHandler : GameRequestPacketHandler<C2SQuestGetSetQuestInfoListReq, S2CQuestGetSetQuestInfoListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetSetQuestInfoListHandler));

        public QuestGetSetQuestInfoListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestGetSetQuestInfoListRes Handle(GameClient client, C2SQuestGetSetQuestInfoListReq request)
        {
            var res = new S2CQuestGetSetQuestInfoListRes()
            {
                DistributeId = request.DistributeId
            };

            // Return a blank list for areas that you haven't seen the Area Master for.
            if (!QuestManager.HasWorldQuestAreaReleased(client.Character, request.DistributeId) || !client.Character.AreaRanks.TryGetValue(request.DistributeId, out var rank) || rank.Rank == 0)
            {
                return res;
            }

            foreach (var questScheduleId in client.Party.QuestState.AreaQuests(request.DistributeId))
            {
                var quest = QuestManager.GetQuestByScheduleId(questScheduleId);
                var questInfo = quest.ToCDataSetQuestInfoList();
                questInfo.CompleteNum = (ushort)(client.Party.QuestState.IsCompletedWorldQuest(quest.QuestId) ? 1 : 0); // Completed in the current instance, hides rewards.
                questInfo.IsDiscovery = quest.IsDiscoverable || client.Character.CompletedQuests.ContainsKey(quest.QuestId);
                res.SetQuestList.Add(questInfo);
            }

            return res;
        }
    }
}
