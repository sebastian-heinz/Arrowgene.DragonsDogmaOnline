using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    internal class RankingRankListHandler : GameRequestPacketHandler<C2SRankingRankListReq, S2CRankingRankListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(RankingRankListHandler));

        public RankingRankListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CRankingRankListRes Handle(GameClient client, C2SRankingRankListReq request)
        {
            return new()
            {
                Rank = request.Rank,
                RankingData = Server.Database.SelectRankingData(request.BoardId, limit: request.Num)
            };
        }
    }
}
