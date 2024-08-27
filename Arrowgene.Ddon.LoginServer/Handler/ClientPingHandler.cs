using System;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    // These requests are sent periodically by the client (every ~10 seconds)
    // after successfully connecting to the server (client._challengeCompleted is true)
    public class ClientPingHandler : PingRequestPacketHandler<LoginClient, C2LPingReq, L2CPingRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClientPingHandler));

        public ClientPingHandler(DdonLoginServer server) : base(server)
        {
        }

        public override L2CPingRes BuildPingResponse(LoginClient client, DateTime now)
        {
            return new L2CPingRes();
        }
    }
}
