using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnPawnLostHandler : GameRequestPacketHandler<C2SPawnPawnLostReq, S2CPawnPawnLostRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnPawnLostHandler));
        
        public PawnPawnLostHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnPawnLostRes Handle(GameClient client, C2SPawnPawnLostReq request)
        {
            Pawn pawn = client.Character.Pawns.Where(pawn => pawn.PawnId == request.PawnId).Single();
            pawn.PawnState = PawnState.Lost;

            S2CPawnPawnLostNtc ntc = new S2CPawnPawnLostNtc()
            {
                PawnId = pawn.PawnId,
                PawnName = pawn.Name,
                IsLost = pawn.PawnState == PawnState.Lost
            };
            client.Party.SendToAll(ntc);

            return new S2CPawnPawnLostRes()
            {
                PawnId = pawn.PawnId,
                PawnName = pawn.Name,
                IsLost = pawn.PawnState == PawnState.Lost
            };
        }
    }
}