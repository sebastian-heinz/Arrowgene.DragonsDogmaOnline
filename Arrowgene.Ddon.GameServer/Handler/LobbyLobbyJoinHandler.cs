using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class LobbyLobbyJoinHandler : StructurePacketHandler<GameClient, C2SLobbyJoinReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(LobbyLobbyJoinHandler));


        public LobbyLobbyJoinHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SLobbyJoinReq> packet)
        {
            var resp = new S2CLobbyJoinRes()
            {
                CharacterID = client.Character.Id,
                LobbyMemberInfoList = new List<CDataLobbyMemberInfo>()
                {
                   new CDataLobbyMemberInfo()
                   {
                       CharacterID = client.Character.Id,
                       FirstName = client.Character.FirstName,
                       LastName =  client.Character.LastName,
                   },

                }
            };
            client.Send(resp);
            
            // NTC
            client.Send(GameFull.Dump_14);
            client.Send(InGameDump.Dump_15);
            client.Send(InGameDump.Dump_16);
        }
    }
}
