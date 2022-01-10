using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class WarpGetFavoriteWarpPointListHandler : PacketHandler<GameClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(WarpGetFavoriteWarpPointListHandler));


        public WarpGetFavoriteWarpPointListHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_WARP_GET_FAVORITE_WARP_POINT_LIST_REQ;

        public override void Handle(GameClient client, Packet packet)
        {
            // TODO
        }
    }
}
