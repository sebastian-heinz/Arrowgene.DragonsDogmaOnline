using System.Collections.Generic;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class LobbyLobbyDataMsgHandler : StructurePacketHandler<GameClient, C2SLobbyLobbyDataMsgReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(LobbyLobbyDataMsgHandler));

        private readonly PartyManager _PartyManager;

        public LobbyLobbyDataMsgHandler(DdonGameServer server) : base(server)
        {
            _PartyManager = server.PartyManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SLobbyLobbyDataMsgReq> packet)
        {
            // In the pcaps ive only seen 3.4.16 packets whose RpcPacket length is either of these
            S2CLobbyLobbyDataMsgNotice res = new S2CLobbyLobbyDataMsgNotice();
            res.Type = packet.Structure.Type;
            res.CharacterId = client.Character.CharacterId; // Has to be overwritten since the request has the id set to 0
            res.RpcPacket = packet.Structure.RpcPacket;
            res.OnlineStatus = client.Character.OnlineStatus;

            if(!StageManager.IsHubArea(client.Character.Stage))
            {
                // We stick this in a seperate if statement so if the client
                // is not in a party, we don't enter into the else condition
                if (client.Party != null)
                {
                    client.Party.SendToAllExcept(res, client);
                }
            }
            else
            {
                // We are in one of the common areas where players can see eachother
                foreach (GameClient otherClient in Server.ClientLookup.GetAll())
                {
                    if (otherClient == null || otherClient == client || otherClient.Character == null)
                    {
                        continue;
                    }

                    // Special handling for the Clan Hall
                    if (client.Character.Stage.Id == 347 && client.Character.ClanId != otherClient.Character.ClanId)
                    {
                        continue;
                    }

                    if (client.Character.Stage.Id == otherClient.Character.Stage.Id || _PartyManager.ClientsInSameParty(client, otherClient))
                    {
                        otherClient.Send(res);
                    }
                }

            }

            // Handle additional packet contents
            RpcHandler.Handle(client, packet.Structure.Type, packet.Structure.RpcPacket);
        }
    }
}
