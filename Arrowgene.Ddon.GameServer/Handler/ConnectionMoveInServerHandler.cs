using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ConnectionMoveInServerHandler : PacketHandler<GameClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(ConnectionMoveInServerHandler));


        public ConnectionMoveInServerHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_CONNECTION_MOVE_IN_SERVER_REQ;

        public override void Handle(GameClient client, Packet packet)
        {
            client.Send(InGameDump.Dump_7);
        }
    }
}
