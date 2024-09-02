using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartnerPawnSetHandler : GameRequestPacketHandler<C2SPartnerPawnSetReq, S2CPartnerPawnSetRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartnerPawnSetHandler));

        public PartnerPawnSetHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPartnerPawnSetRes Handle(GameClient client, C2SPartnerPawnSetReq request)
        {
            S2CPartnerPawnSetRes res = new S2CPartnerPawnSetRes();

            Pawn pawn = client.Character.Pawns.Find(p => p.PawnId == request.PawnId);
            res.PartnerInfo = new CDataPartnerPawnData
            {
                PawnId = pawn.PawnId,
                // TODO: Likability and other attributes are not stored in the pawn memory entity yet
                Likability = 1,
                Personality = 1
            };

            return res;
        }
    }
}
