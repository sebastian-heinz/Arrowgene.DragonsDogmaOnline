using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnGetRentedPawnListHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnGetRentedPawnListHandler));


        public PawnGetRentedPawnListHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_PAWN_GET_RENTED_PAWN_LIST_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            //client.Send(InGameDump.Dump_35);
            client.Send(new Packet(PacketId.S2C_PAWN_GET_RENTED_PAWN_LIST_RES, new byte[12]));
        }
    }
}
