using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;
using System;

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
            S2CRankingRankListRes res = new()
            {
                Rank = request.Rank,
            };

            var rankResults = Server.Database.SelectRankingData(request.BoardId, limit: request.Num);

            res.RankingData = rankResults;

            return res;
        }
    }
}
