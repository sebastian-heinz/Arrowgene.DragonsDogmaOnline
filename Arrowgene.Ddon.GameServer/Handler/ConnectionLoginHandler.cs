using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ConnectionLoginHandler : PacketHandler<GameClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(ConnectionLoginHandler));


        public ConnectionLoginHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_CONNECTION_LOGIN_REQ;

        public override void Handle(GameClient client, Packet packet)
        {
            client.Send(GameDump.Dump_4);
        }
    }
}
