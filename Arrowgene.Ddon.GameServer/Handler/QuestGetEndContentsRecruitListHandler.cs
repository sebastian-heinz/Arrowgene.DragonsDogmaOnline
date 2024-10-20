using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetEndContentsRecruitListHandler : GameRequestPacketHandler<C2SQuestGetEndContentsRecruitListReq, S2CQuestGetEndContentsRecruitListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetEndContentsRecruitListHandler));

        public QuestGetEndContentsRecruitListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestGetEndContentsRecruitListRes Handle(GameClient client, C2SQuestGetEndContentsRecruitListReq request)
        {
            var results = new S2CQuestGetEndContentsRecruitListRes();

            foreach (var questScheduleId in QuestManager.GetQuestsByType(QuestType.ExtremeMission))
            {
                var quest = QuestManager.GetQuestByScheduleId(questScheduleId);
                results.Unk1List.Add(new CDataQuestRecruitListItem()
                {
                    QuestScheduleId = quest.QuestScheduleId,
                    QuestId = (uint) quest.QuestId,
                    GroupsRecruiting = (uint) Server.BoardManager.GetGroupsForBoardId(BoardManager.QuestScheduleIdToExmBoardId(quest.QuestScheduleId)).Where(x => x.ContentStatus == ContentStatus.Recruiting).ToList().Count
                });
            }

            return results;
        }
    }
}
