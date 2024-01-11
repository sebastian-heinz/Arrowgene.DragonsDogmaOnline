using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetPartyQuestProgressInfoHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger =
            LogProvider.Logger<ServerLogger>(typeof(QuestGetPartyQuestProgressInfoHandler));


        public QuestGetPartyQuestProgressInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_QUEST_GET_PARTY_QUEST_PROGRESS_INFO_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            S2CQuestGetPartyQuestProgressInfoRes res = new S2CQuestGetPartyQuestProgressInfoRes();
           // client.Send(res);
             client.Send(GameFull.Dump_142);
        }
    }
}
