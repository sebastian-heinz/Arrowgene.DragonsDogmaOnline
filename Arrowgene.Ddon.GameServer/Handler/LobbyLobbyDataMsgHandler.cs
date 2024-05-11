using System.Collections.Generic;
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

        // List of lobby areas, where you're supposed to see all other players.
        // TODO: Complete with all the safe areas.
        private static readonly HashSet<uint> LobbyStageIds = new HashSet<uint>(){
            2, // White Dragon Temple
            5, // Cave Harbor
            340, // Morfaul Centrum
            341, // Dana Centrum
            377, // Glyndwr Centrum
            467, // Fort Thines
            341, // Dana Centrum
            487, // Fortress City Megado: Residential Level
        };

        private readonly PartyManager _PartyManager;

        public LobbyLobbyDataMsgHandler(DdonGameServer server) : base(server)
        {
            _PartyManager = server.PartyManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SLobbyLobbyDataMsgReq> packet)
        {
            // In the pcaps ive only seen 3.4.16 packets whose RpcPacket length is either of these
            S2CLobbyLobbyDataMsgNotice res = new S2CLobbyLobbyDataMsgNotice();
            res.Type = packet.Structure.Type; // I haven't seen any values other than 0x02
            res.CharacterId = client.Character.CharacterId; // Has to be overwritten since the request has the id set to 0
            res.RpcPacket = packet.Structure.RpcPacket;
            res.OnlineStatus = client.Character.OnlineStatus;

            if(!LobbyStageIds.Contains(client.Character.Stage.Id))
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
                    if (otherClient == null || otherClient == client)
                    {
                        // No point to send to oneself.
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
