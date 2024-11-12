using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestQuestLogInfoHandler : GameRequestPacketHandler<C2SQuestQuestLogInfoReq, S2CQuestQuestLogInfoRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestQuestLogInfoHandler));

        public QuestQuestLogInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestQuestLogInfoRes Handle(GameClient client, C2SQuestQuestLogInfoReq request)
        {
            return new S2CQuestQuestLogInfoRes()
            {
                ClanQuestClearNumList = Server.ClanManager.ClanQuestCompletionStatistics(client.Character.CharacterId)
            };
        }
    }
}
