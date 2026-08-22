using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnUpdatePawnShareRangeHandler : GameRequestPacketHandler<C2SPawnUpdatePawnShareRangeReq, S2CPawnUpdatePawnShareRangeRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnUpdatePawnShareRangeHandler));

        public PawnUpdatePawnShareRangeHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnUpdatePawnShareRangeRes Handle(GameClient client, C2SPawnUpdatePawnShareRangeReq request)
        {
            Pawn pawn = client.Character.Pawns.Find(x => x.PawnId == request.PawnId)
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_INVALID);

            //TODO: Actually update the pawn's share range in the database
            pawn.ShareRange = request.ShareRange;

            var res = new S2CPawnUpdatePawnShareRangeRes()
            {
                PawnId = request.PawnId,
                ShareRange = request.ShareRange
            };

            return res;
        }
    }
}