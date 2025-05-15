using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ServerGetServerListHandler : GameRequestPacketHandler<C2SServerGetServerListReq, S2CServerGetServerListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ServerGetServerListHandler));

        public ServerGetServerListHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_SERVER_GET_SERVER_LIST_REQ;

        public override S2CServerGetServerListRes Handle(GameClient client, C2SServerGetServerListReq request)
        {
            var serverList = Server.RpcManager.ServerListInfo();

            // Special handling to make the channel list at log-in make more sense.
            if (client.Character.Stage.Id == 0)
            {
                var thisServer = serverList.Find(x => x.Id == Server.Id);
                thisServer.TrafficName = RpcManager.GetTrafficName(--thisServer.LoginNum, thisServer.MaxLoginNum);
            }

            return new S2CServerGetServerListRes()
            {
                GameServerListInfo = serverList
            };
        }
    }
}
