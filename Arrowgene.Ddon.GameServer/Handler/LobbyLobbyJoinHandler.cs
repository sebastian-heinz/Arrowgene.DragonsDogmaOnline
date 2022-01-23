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
                CharacterID = 0x20FD8,
                LobbyMemberInfoList = new List<CDataLobbyMemberInfo>()
                {
                   new CDataLobbyMemberInfo()
                   {
                       CharacterID = 0x20FD8,
                       FirstName = "Rumi",
                       LastName = "Amestris",
                       ClanName = "S;R",
                       PawnID = 0,
                       Unk0 = 1,
                       Unk1 = 0,
                       Unk2 = 1,
                   },

                   new CDataLobbyMemberInfo()
                   {
                       CharacterID = 0xDEADBEEF,
                       FirstName = "Example",
                       LastName = "SecondLobbyMember",
                       ClanName = "S;R",
                       PawnID = 0,
                       Unk0 = 1,
                       Unk1 = 0,
                       Unk2 = 1,
                   },
                }
            };
            //client.Send(InGameDump.Dump_13);
            
            // NTC
            client.Send(GameFull.Dump_14);
            client.Send(InGameDump.Dump_15);
            client.Send(InGameDump.Dump_16);
        }
    }
}
