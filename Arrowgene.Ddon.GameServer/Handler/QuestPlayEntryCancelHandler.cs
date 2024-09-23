using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestPlayEntryCancelHandler : GameRequestPacketHandler<C2SQuestPlayEntryCancelReq, S2CQuestPlayEntryCancelRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestPlayEntryCancelHandler));

        public QuestPlayEntryCancelHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestPlayEntryCancelRes Handle(GameClient client, C2SQuestPlayEntryCancelReq request)
        {
            var ntc = new S2CQuestPlayEntryCancelNtc()
            {
                CharacterId = client.Character.CharacterId
            };
            client.Party.SendToAll(ntc);

            return new S2CQuestPlayEntryCancelRes();
        }
    }
}
