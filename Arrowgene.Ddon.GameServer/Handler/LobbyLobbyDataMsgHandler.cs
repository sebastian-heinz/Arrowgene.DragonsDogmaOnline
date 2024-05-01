using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.RpcPacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class LobbyLobbyDataMsgHandler : StructurePacketHandler<GameClient, C2SLobbyLobbyDataMsgReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(LobbyLobbyDataMsgHandler));

        // List of lobby areas, where you're supposed to see all other players.
        // TODO: Complete with all the safe areas. Maybe move it to DB or config?
        private static readonly HashSet<uint> LobbyStageIds = new HashSet<uint>(){
            2, // White Dragon Temple
            341, // Dana Centrum
            486, // Fortress City Megado: Residential Level
            487, // Fortress City Megado: Residential Level
            488, // Fortress City Megado: Royal Palace Level
        };

        public LobbyLobbyDataMsgHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SLobbyLobbyDataMsgReq> packet)
        {
            // In the pcaps ive only seen 3.4.16 packets whose RpcPacket length is either of these
            S2CLobbyLobbyDataMsgNotice res = new S2CLobbyLobbyDataMsgNotice();
            res.Type = packet.Structure.Type; // I haven't seen any values other than 0x02
            res.CharacterId = client.Character.CharacterId; // Has to be overwritten since the request has the id set to 0
            res.RpcPacket = packet.Structure.RpcPacket;
            res.OnlineStatus = client.Character.OnlineStatus;

            if (client.Party != null)
            {
                if(!LobbyStageIds.Contains(client.Character.Stage.Id))
                {
                    client.Party.SendToAllExcept(res, client);
                }
                else
                {
                    foreach (GameClient otherClient in Server.ClientLookup.GetAll())
                    {
                        if (otherClient != client && (client.Character.Stage.Id == otherClient.Character.Stage.Id || client.Party.Id == otherClient.Party.Id))
                        {
                            otherClient.Send(res);
                        }
                    }
                }
            }

            // Handle additional packet contents
            RpcHandler.Handle(client, packet.Structure.Type, packet.Structure.RpcPacket);
        }
    }
}
