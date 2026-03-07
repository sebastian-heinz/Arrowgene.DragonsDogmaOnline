using Arrowgene.Ddon.Shared.Entity.PacketStructure;

namespace Arrowgene.Ddon.LoginServer.Handler;

public sealed class ClientLogoutHandler(DdonLoginServer server) : LoginRequestPacketHandler<C2LLogoutReq, L2CLogoutRes>(server)
{
    private static readonly L2CLogoutRes Response = new();

    public override L2CLogoutRes Handle(LoginClient client, C2LLogoutReq request)
    {
        return Response;
    }
}
