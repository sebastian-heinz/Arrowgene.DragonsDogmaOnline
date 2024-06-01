using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestQuestLogInfoHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestQuestLogInfoHandler));


        public QuestQuestLogInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_QUEST_QUEST_LOG_INFO_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            S2CQuestQuestLogInfoRes obj = new S2CQuestQuestLogInfoRes();
            client.Send(obj);
        }
    }
}
