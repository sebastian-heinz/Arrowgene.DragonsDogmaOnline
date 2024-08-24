using System;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    // These requests are sent periodically by the client (every ~10 seconds)
    // after successfully connecting to the server (client._challengeCompleted is true)
    public class ConnectionPingHandler : PingRequestPacketHandler<GameClient, C2SConnectionPingReq, S2CConnectionPingRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ConnectionPingHandler));

        public ConnectionPingHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CConnectionPingRes BuildPingResponse(GameClient client, DateTime now)
        {
            return new S2CConnectionPingRes();
        }
    }
}
