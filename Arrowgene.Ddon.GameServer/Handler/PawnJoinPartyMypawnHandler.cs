using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnJoinPartyMypawnHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnJoinPartyMypawnHandler));


        public PawnJoinPartyMypawnHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_PAWN_JOIN_PARTY_MYPAWN_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            client.Send(SelectedDump.Pawn8_37_16);
            client.Send(SelectedDump.Pawn35_3_16);
            client.Send(SelectedDump.Pawn_Res);
            
        }
    }
}
