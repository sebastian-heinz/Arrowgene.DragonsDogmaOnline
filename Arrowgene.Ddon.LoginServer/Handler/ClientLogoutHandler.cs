using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class ClientLogoutHandler : LoginRequestPacketHandler<C2LLogoutReq, L2CLogoutRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClientLogoutHandler));

        public ClientLogoutHandler(DdonLoginServer server) : base(server)
        {
        }

        public override L2CLogoutRes Handle(LoginClient client, C2LLogoutReq request)
        {
            return new();
        }
    }
}
