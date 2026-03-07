using Arrowgene.Ddon.Shared.Entity.PacketStructure;

namespace Arrowgene.Ddon.LoginServer.Handler;

public sealed class ClientDecideCancelCharacterHandler(DdonLoginServer server) : LoginRequestPacketHandler<C2LDecideCancelCharacterReq, L2CDecideCancelCharacterRes>(server)
{
    private static readonly L2CDecideCancelCharacterRes Response = new();

    public override L2CDecideCancelCharacterRes Handle(LoginClient client, C2LDecideCancelCharacterReq request)
    {
        Server.LoginQueueManager.Remove(client.Account.Id);
        return Response;
    }
}
