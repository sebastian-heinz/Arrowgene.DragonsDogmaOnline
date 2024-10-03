using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetQuestCompletedListHandler : GameRequestPacketHandler<C2SQuestGetQuestCompleteListReq, S2CQuestGetQuestCompleteListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetQuestCompletedListHandler));


        public QuestGetQuestCompletedListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestGetQuestCompleteListRes Handle(GameClient client, C2SQuestGetQuestCompleteListReq packet)
        {
            // client.Send(GameFull.Dump_126);
            var result = new S2CQuestGetQuestCompleteListRes()
            {
                QuestType = packet.QuestType
            };

            var completedQuests = client.Character.CompletedQuests.Values.Where(x => x.QuestType == (QuestType)packet.QuestType);
            foreach (var completedQuest in completedQuests)
            {
                result.QuestIdList.Add(new CDataQuestId()
                {
                    QuestId = (uint) completedQuest.QuestId
                });
            }

            return result;
        }
    }
}
