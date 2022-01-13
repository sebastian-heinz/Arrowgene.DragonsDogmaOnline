using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestQuestProgressHandler : PacketHandler<GameClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(QuestQuestProgressHandler));


        public QuestQuestProgressHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_QUEST_QUEST_PROGRESS_REQ;

        public override void Handle(GameClient client, Packet packet)
        {
            client.Send(GameFull.Dump_166);
            client.Send(GameFull.Dump_168);
            client.Send(GameFull.Dump_170);
            client.Send(GameFull.Dump_172);
            client.Send(GameFull.Dump_175);
            client.Send(GameFull.Dump_177);
            client.Send(GameFull.Dump_179);
            client.Send(GameFull.Dump_181);
            client.Send(GameFull.Dump_185);
            client.Send(GameFull.Dump_188);
            client.Send(GameFull.Dump_190);
            client.Send(GameFull.Dump_294);
            client.Send(GameFull.Dump_297);
            client.Send(GameFull.Dump_299);
            client.Send(GameFull.Dump_524);
        }
    }
}
