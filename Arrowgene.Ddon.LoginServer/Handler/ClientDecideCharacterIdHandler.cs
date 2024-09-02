using System;
using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class ClientDecideCharacterIdHandler : LoginStructurePacketHandler<C2LDecideCharacterIdReq>
    {
        private static readonly ServerLogger Logger =
            LogProvider.Logger<ServerLogger>(typeof(ClientDecideCharacterIdHandler));

        private static int LoadBalanceServerIndex = 0;
        private static object LoadBalanceLock = new object();

        public ClientDecideCharacterIdHandler(DdonLoginServer server) : base(server)
        {
            foreach (CDataGameServerListInfo serverListInfo in Server.AssetRepository.ServerList)
            {
                Logger.Info(
                    $"Asset GameServer Entry:{serverListInfo.Id} \"{serverListInfo.Name}\" {serverListInfo.Addr}:{serverListInfo.Port}");
            }
        }

        public override void Handle(LoginClient client, StructurePacket<C2LDecideCharacterIdReq> packet)
        {
            client.SelectedCharacterId = packet.Structure.CharacterId;

            L2CDecideCharacterIdRes res = new L2CDecideCharacterIdRes();
            res.CharacterId = packet.Structure.CharacterId;
            res.WaitNum = packet.Structure.WaitNum;
            client.Send(res);

            // This is NOT required to get in game (can be commented out entirely).
            // Causes a "Server is busy, 100 people waiting message" if L2CNextConnectionServerNtc isn't sent
            L2CLoginWaitNumNtc waitNumNtc = new L2CLoginWaitNumNtc();
            waitNumNtc.Unknown = 100;
            client.Send(waitNumNtc);

            // TODO: Figure out packet.Structure.RotationServerId. Always a 2?

            CDataGameServerListInfo serverListInfo;
            lock(LoadBalanceLock)
            {   
                serverListInfo = Server.AssetRepository.ServerList[LoadBalanceServerIndex];
                LoadBalanceServerIndex = (LoadBalanceServerIndex + 1) % Server.AssetRepository.ServerList.Count;
            }

            Logger.Info(client, $"Connecting To: {serverListInfo.Addr}:{serverListInfo.Port}");

            L2CNextConnectionServerNtc serverNtc = new L2CNextConnectionServerNtc();
            serverNtc.ServerList = serverListInfo;
            serverNtc.Counter = packet.Structure.Counter;
            client.Send(serverNtc);
        }
    }
}
