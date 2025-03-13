using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class LobbyLobbyLeaveHandler : GameStructurePacketHandler<C2SLobbyLeaveReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(LobbyLobbyLeaveHandler));

        public LobbyLobbyLeaveHandler(DdonGameServer server) : base(server)
        {
            server.ClientConnectionChangeEvent += OnClientConnectionChangeEvent;
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
            if(client.Character != null && client.Character.Stage.Id != 0) {
                // Notice all other users
                S2CUserListLeaveNtc ntc = new S2CUserListLeaveNtc();
                ntc.CharacterList.Add(new CDataCommonU32(client.Character.CharacterId));
                foreach (Client otherClient in Server.ClientLookup.GetAll())
                {
                    if (otherClient != client)
                    {
                        otherClient.Send(ntc);
                    }
                }

                Server.HubManager.LeaveAllHubs(client);
                Server.CharacterManager.CleanupOnExit(client);
                Server.PartyManager.CleanupOnExit(client);
                Server.RpcManager.AnnouncePlayerList(client.Character);
            }
        }
    }
}
