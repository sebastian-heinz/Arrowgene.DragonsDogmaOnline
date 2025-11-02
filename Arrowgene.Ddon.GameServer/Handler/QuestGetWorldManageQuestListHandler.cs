using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetWorldManageQuestListHandler : GameRequestPacketHandler<C2SQuestGetWorldManageQuestListReq, S2CQuestGetWorldManageQuestListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetWorldManageQuestListHandler));

        public QuestGetWorldManageQuestListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestGetWorldManageQuestListRes Handle(GameClient client, C2SQuestGetWorldManageQuestListReq request)
        {
            //client.Send(GameFull.Dump_121);
            return new();
        }
    }
}
