using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class WarpWarpHandler : PacketHandler<GameClient> {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(WarpGetWarpPointListHandler));


        public WarpWarpHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_WARP_WARP_REQ;

        public override void Handle(GameClient client, Packet packet)
        {
            client.Send(GameFull.Dump_433);
        }
    }
}