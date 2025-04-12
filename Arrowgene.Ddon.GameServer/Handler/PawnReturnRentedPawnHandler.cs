using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnReturnRentedPawnHandler : GameRequestPacketHandler<C2SPawnReturnRentedPawnReq, S2CPawnReturnRentedPawnRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnReturnRentedPawnHandler));

        public PawnReturnRentedPawnHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnReturnRentedPawnRes Handle(GameClient client, C2SPawnReturnRentedPawnReq request)
        {
            // TODO: Remove pawn from rented pawn list for character
            // TODO: Remove snapshot from DB
            // TODO: Save Feedback in the database
            client.Character.RemoveRentedPawnBySlotNo(request.SlotNo);

            return new S2CPawnReturnRentedPawnRes();
        }
    }
}
