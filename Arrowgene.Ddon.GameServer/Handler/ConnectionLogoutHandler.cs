using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ConnectionLogoutHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ConnectionLogoutHandler));


        public ConnectionLogoutHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_CONNECTION_LOGOUT_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            S2CConnectionLogoutRes res = new S2CConnectionLogoutRes();
            client.Send(res);
            client.Character.OnlineStatus = OnlineStatus.Offline;
        }
    }
}
