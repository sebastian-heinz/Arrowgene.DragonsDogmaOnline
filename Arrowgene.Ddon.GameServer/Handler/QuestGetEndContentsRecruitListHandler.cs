using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
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

            foreach (var (questId, quest) in QuestManager.GetQuestsByType(QuestType.ExtremeMission))
            {
                results.Unk1List.Add(new CDataQuestRecruitListItem()
                {
                    QuestScheduleId = (uint) quest.QuestScheduleId,
                    QuestId = (uint) quest.QuestId,
                    GroupsRecruiting = (uint) Server.BoardManager.GetGroupsForBoardId(QuestManager.QuestIdToBoardId(quest.QuestId)).Where(x => !x.ContentInProgress).ToList().Count
                });
            }

            return results;
        }
    }
}
