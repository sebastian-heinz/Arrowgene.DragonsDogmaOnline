using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class ClientLogoutHandler : PacketHandler<LoginClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClientLogoutHandler));


        public ClientLogoutHandler(DdonLoginServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2L_LOGOUT_REQ;

        public override void Handle(LoginClient client, IPacket packet)
        {
            ServerRes res = new ServerRes(PacketId.L2C_LOGOUT_RES);
            client.Send(res);
        }
    }
}
