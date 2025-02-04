using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

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

            // This is NOT required to get in game (can be commented out entirely).
            // Causes a "Server is busy, 100 people waiting message" if L2CNextConnectionServerNtc isn't sent
            client.Enqueue(new L2CLoginWaitNumNtc()
            {
                Unknown = 100
            }, packetQueue);

            // TODO: Figure out packet.Structure.RotationServerId. Always a 2?

            CDataGameServerListInfo serverListInfo;
            lock (LoadBalanceLock)
            {
                serverListInfo = Server.AssetRepository.ServerList[LoadBalanceServerIndex].ToCDataGameServerListInfo();
                LoadBalanceServerIndex = (LoadBalanceServerIndex + 1) % Server.AssetRepository.ServerList.Count;
            }

            Logger.Info(client, $"Connecting To: {serverListInfo.Addr}:{serverListInfo.Port}");

            client.Enqueue(new L2CNextConnectionServerNtc()
            {
                ServerList = serverListInfo,
                Counter = request.Counter
            }, packetQueue);

            return packetQueue;
        }
    }
}
