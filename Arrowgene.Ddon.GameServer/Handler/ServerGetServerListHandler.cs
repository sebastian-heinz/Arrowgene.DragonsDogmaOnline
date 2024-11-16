using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ServerGetServerListHandler : GameRequestPacketHandler<C2SServerGetServerListReq, S2CServerGetServerListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ServerGetServerListHandler));

        private AssetRepository _assets;

        public ServerGetServerListHandler(DdonGameServer server) : base(server)
        {
            _assets = server.AssetRepository;
        }

        public override PacketId Id => PacketId.C2S_SERVER_GET_SERVER_LIST_REQ;

        public override S2CServerGetServerListRes Handle(GameClient client, C2SServerGetServerListReq request)
        {
            return new S2CServerGetServerListRes()
            {
                GameServerListInfo = Server.RpcManager.ServerListInfo()
            };
        }
    }
}
