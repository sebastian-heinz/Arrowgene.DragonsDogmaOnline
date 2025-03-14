using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System.Linq;

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
            if (!client.Character.AreaRanks.TryGetValue(request.DistributeId, out var rank) || rank.Rank == 0)
            {
                return res;
            }

            var completedQuests = client.Character.CompletedQuests.Values.Where(x => x.QuestType == QuestType.World);
            foreach (var questScheduleId in client.Party.QuestState.AreaQuests(request.DistributeId))
            {
                var quest = QuestManager.GetQuestByScheduleId(questScheduleId);
                var questInfo = quest.ToCDataSetQuestInfoList();
                questInfo.CompleteNum = (ushort)(client.Party.QuestState.IsCompletedWorldQuest(quest.QuestId) ? 1 : 0); // Completed in the current instance, hides rewards.
                questInfo.IsDiscovery = quest.IsDiscoverable || (completedQuests.Where(y => y.QuestId == quest.QuestId).FirstOrDefault()?.ClearCount ?? 0) > 0;
                res.SetQuestList.Add(questInfo);
            }

            return res;
        }
    }
}
