using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ConnectionLogoutHandler : GameRequestPacketHandler<C2SConnectionLogoutReq, S2CConnectionLogoutRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ConnectionLogoutHandler));

        public ConnectionLogoutHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CConnectionLogoutRes Handle(GameClient client, C2SConnectionLogoutReq request)
        {
            S2CConnectionLogoutRes res = new S2CConnectionLogoutRes();
            client.Character.OnlineStatus = OnlineStatus.Offline;
            return res;
        }
    }
}
