using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class ClientDecideCharacterIdHandler : StructurePacketHandler<LoginClient, C2LDecideCharacterIdReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClientDecideCharacterIdHandler));

        public ClientDecideCharacterIdHandler(DdonLoginServer server) : base(server)
        {
        }

        public override void Handle(LoginClient client, StructurePacket<C2LDecideCharacterIdReq> packet)
        {
            Logger.Debug(client,
                $"C2L_DECIDE_CHARACTER_ID_REQ:\n" +
                $"    CharacterID: {packet.Structure.CharacterId}\n" +
                $"    ClientVersion: {packet.Structure.ClientVersion}\n" +
                $"    Type: {packet.Structure.Type}\n" +
                $"    RotationServerID: {packet.Structure.RotationServerId}\n" +
                $"    WaitNum: {packet.Structure.WaitNum}\n" +
                $"    Counter: {packet.Structure.Counter}\n"
            );

            L2CDecideCharacterIdRes res = new L2CDecideCharacterIdRes();
            res.CharacterId = packet.Structure.CharacterId;
            res.WaitNum = packet.Structure.WaitNum;
            client.Send(res);

            // This is NOT required to get in game (can be commented out entirely).
            L2CLoginWaitNumNtc waitNumNtc = new L2CLoginWaitNumNtc();
            waitNumNtc.Unknown = 100;
            client.Send(waitNumNtc);

            L2CNextConnectionServerNtc serverNtc = new L2CNextConnectionServerNtc();
            serverNtc.ServerList = new CDataGameServerListInfo
            {
                ID = 17,
                Name = "サーバー017",
                Brief = "",
                TrafficName = "少なめ",
                MaxLoginNum = 1000, // Player cap
                LoginNum = 0x1C, // Current players
                Addr = "127.0.0.1",
                Port = 52000,
                IsHide = false
            };
            serverNtc.Counter = 1;
            client.Send(serverNtc);
        }
    }
}
