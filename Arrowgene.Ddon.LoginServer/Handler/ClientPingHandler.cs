using System;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;

namespace Arrowgene.Ddon.LoginServer.Handler;

// These requests are sent periodically by the client (every ~10 seconds)
// after successfully connecting to the server (client._challengeCompleted is true)
public sealed class ClientPingHandler(DdonLoginServer server) : PingRequestPacketHandler<LoginClient, C2LPingReq, L2CPingRes>(server)
{
    private static readonly L2CPingRes Response = new();

    public override L2CPingRes BuildPingResponse(LoginClient client, DateTime now)
    {
        return Response;
    }
}
