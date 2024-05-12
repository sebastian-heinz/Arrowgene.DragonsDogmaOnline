using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ServerGetServerListHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ServerGetServerListHandler));

        private AssetRepository _assets;

        public ServerGetServerListHandler(DdonGameServer server) : base(server)
        {
            _assets = server.AssetRepository;
        }

        public override PacketId Id => PacketId.C2S_SERVER_GET_SERVER_LIST_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            S2CServerGetServerListRes response = new S2CServerGetServerListRes();
            response.GameServerListInfo = new List<CDataGameServerListInfo>(_assets.ServerList);
            client.Send(response);
        }
    }
}
