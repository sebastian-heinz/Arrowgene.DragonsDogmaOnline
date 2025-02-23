using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnGetPawnTotalScoreHandler : GameRequestPacketHandler<C2SPawnGetPawnTotalScoreReq, S2CPawnGetPawnTotalScoreRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnGetPawnTotalScoreHandler));

        public PawnGetPawnTotalScoreHandler(DdonGameServer server) : base(server)
        {
        }
        
        public override S2CPawnGetPawnTotalScoreRes Handle(GameClient client, C2SPawnGetPawnTotalScoreReq request)
        {
            // TODO: Implement.
            return new();
        }
    }
}
