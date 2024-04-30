using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class LobbyLobbyLeaveHandler : StructurePacketHandler<GameClient, C2SLobbyLeaveReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(LobbyLobbyLeaveHandler));

        private CharacterManager _CharacterManager;

        public LobbyLobbyLeaveHandler(DdonGameServer server) : base(server)
        {
            server.ClientConnectionChangeEvent += OnClientConnectionChangeEvent;
            _CharacterManager = server.CharacterManager;
        }

        // I have no idea on when this gets called, not when exiting the game, thats for sure
        // - Return to title makes this happen
        public override void Handle(GameClient client, StructurePacket<C2SLobbyLeaveReq> packet)
        {
            client.Send(new S2CLobbyLeaveRes());
            NotifyDisconnect(client);
        }

        private void OnClientConnectionChangeEvent(object sender, ClientConnectionChangeArgs e)
        {
            if (e.Action == ClientConnectionChangeArgs.EventType.DISCONNECT)
            {
                NotifyDisconnect(e.Client);
            }
        }

        private void NotifyDisconnect(GameClient client)
        {
            if(client.Character != null) {
                // Notice all other users
                S2CUserListLeaveNtc ntc = new S2CUserListLeaveNtc();
                ntc.CharacterList.Add(new CDataCommonU32(client.Character.CharacterId));
                foreach (Client otherClient in Server.Clients)
                {
                    if (otherClient != client)
                    {
                        otherClient.Send(ntc);
                    }
                }

                _CharacterManager.UpdateDatabaseOnExit(client.Character);
            }
        }
    }
}
