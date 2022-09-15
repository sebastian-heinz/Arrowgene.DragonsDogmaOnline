using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class ClientDecideCharacterIdHandler : StructurePacketHandler<LoginClient, C2LDecideCharacterIdReq>
    {
        private static readonly ServerLogger Logger =
            LogProvider.Logger<ServerLogger>(typeof(ClientDecideCharacterIdHandler));

        private AssetRepository _assets;

        public ClientDecideCharacterIdHandler(DdonLoginServer server) : base(server)
        {
            _assets = server.AssetRepository;

            foreach (CDataGameServerListInfo serverListInfo in _assets.ServerList)
            {
                Logger.Info(
                    $"Asset GameServer Entry:{serverListInfo.Id} \"{serverListInfo.Name}\" {serverListInfo.Addr}:{serverListInfo.Port}");
            }
        }

        public override void Handle(LoginClient client, StructurePacket<C2LDecideCharacterIdReq> packet)
        {
            Logger.Debug(client,
                $"C2L_DECIDE_CHARACTER_ID_REQ:\n" +
                $"    CharacterId: {packet.Structure.CharacterId}\n" +
                $"    ClientVersion: {packet.Structure.ClientVersion}\n" +
                $"    Type: {packet.Structure.Type}\n" +
                $"    RotationServerId: {packet.Structure.RotationServerId}\n" +
                $"    WaitNum: {packet.Structure.WaitNum}\n" +
                $"    Counter: {packet.Structure.Counter}\n"
            );

            client.SelectedCharacterId = packet.Structure.CharacterId;
            Logger.Debug(client, $"Decided CharacterId: {client.SelectedCharacterId}");


            L2CDecideCharacterIdRes res = new L2CDecideCharacterIdRes();
            res.CharacterId = packet.Structure.CharacterId;
            res.WaitNum = packet.Structure.WaitNum;
            client.Send(res);

            // This is NOT required to get in game (can be commented out entirely).
            L2CLoginWaitNumNtc waitNumNtc = new L2CLoginWaitNumNtc();
            waitNumNtc.Unknown = 100;
            client.Send(waitNumNtc);


            List<CDataGameServerListInfo> serverLists = new List<CDataGameServerListInfo>(_assets.ServerList);

            CDataGameServerListInfo serverList;
            // if (serverLists.Count > packet.Structure.RotationServerId)
            // {
            //     serverList = serverLists[packet.Structure.RotationServerId];
            // }
            // else
            // TODO figure out RotationServerId, at the moment always value 2?
            if (serverLists.Count > 0)
            {
                serverList = serverLists[0];
            }
            else
            {
                Logger.Error(client, "Server List not available in asset repository");
                return;
            }

            Logger.Info(client, $"Connecting To: {serverList.Addr}:{serverList.Port}");

            L2CNextConnectionServerNtc serverNtc = new L2CNextConnectionServerNtc();
            serverNtc.ServerList = serverList;
            serverNtc.Counter = packet.Structure.Counter;
            client.Send(serverNtc);
        }
    }
}
