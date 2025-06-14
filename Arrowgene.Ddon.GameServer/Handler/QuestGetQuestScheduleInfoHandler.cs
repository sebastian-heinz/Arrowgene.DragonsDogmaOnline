using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetQuestScheduleInfoHandler : GameRequestPacketHandler<C2SQuestGetQuestScheduleInfoReq, S2CQuestGetQuestScheduleInfoRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetQuestScheduleInfoHandler));

        public QuestGetQuestScheduleInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestGetQuestScheduleInfoRes Handle(GameClient client, C2SQuestGetQuestScheduleInfoReq request)
        {
            var quest = QuestManager.GetQuestByScheduleId(request.QuestScheduleId) ??
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_QUEST_INTERNAL_ERROR, $"QuestScheduleId={request.QuestScheduleId} doesn't exist");
            return new S2CQuestGetQuestScheduleInfoRes()
            {
                QuestId = quest.QuestId
            };
        }
    }
}
