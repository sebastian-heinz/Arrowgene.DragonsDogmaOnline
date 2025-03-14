using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class ClientDecideCharacterIdHandler : LoginRequestPacketQueueHandler<C2LDecideCharacterIdReq, L2CDecideCharacterIdRes>
    {
        private static readonly ServerLogger Logger =
            LogProvider.Logger<ServerLogger>(typeof(ClientDecideCharacterIdHandler));

        private static int LoadBalanceServerIndex = 0;
        private static object LoadBalanceLock = new object();


        public ClientDecideCharacterIdHandler(DdonLoginServer server) : base(server)
        {
            foreach (ServerInfo serverListInfo in Server.AssetRepository.ServerList)
            {
                Logger.Info(
                    $"Asset GameServer Entry:{serverListInfo.Id} \"{serverListInfo.Name}\" {serverListInfo.Addr}:{serverListInfo.Port}");
            }
        }

        public override PacketQueue Handle(LoginClient client, C2LDecideCharacterIdReq request)
        {
            PacketQueue packetQueue = new();

            client.SelectedCharacterId = request.CharacterId;

            client.Enqueue(new L2CDecideCharacterIdRes()
            {
                CharacterId = request.CharacterId,
                WaitNum = request.WaitNum
            }, packetQueue);

            // TODO: Figure out packet.Structure.RotationServerId. Always a 2?

            CDataGameServerListInfo serverListInfo = Server.LoginQueueManager.GetBalancedServer();

            if (serverListInfo is not null)
            {
                Logger.Info(client, $"Connecting To: {serverListInfo.Addr}:{serverListInfo.Port}");

                client.Enqueue(new L2CNextConnectionServerNtc()
                {
                    ServerList = serverListInfo,
                    Counter = request.Counter
                }, packetQueue);
            }
            else
            {
                var currentQueue = Server.LoginQueueManager.Enqueue(client.Account.Id);
                client.Enqueue(new L2CLoginWaitNumNtc()
                {
                    WaitNum = (uint)currentQueue
                }, packetQueue);
            }

            return packetQueue;
        }

        
    }
}
