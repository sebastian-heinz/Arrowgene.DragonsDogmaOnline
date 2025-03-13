using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class ClientDecideCancelCharacterHandler : LoginRequestPacketHandler<C2LDecideCancelCharacterReq, L2CDecideCancelCharacterRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClientDecideCancelCharacterHandler));


        public ClientDecideCancelCharacterHandler(DdonLoginServer server) : base(server)
        {
        }

        public override L2CDecideCancelCharacterRes Handle(LoginClient client, C2LDecideCancelCharacterReq request)
        {
            Server.LoginQueueManager.Remove(client.Account.Id);

            return new();
        }
    }
}
