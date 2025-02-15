using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class RankingRankListByCharacterIdHandler : GameRequestPacketHandler<C2SRankingRankListByCharacterIdReq, S2CRankingRankListByCharacterIdRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(RankingRankListByCharacterIdHandler));

        public RankingRankListByCharacterIdHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CRankingRankListByCharacterIdRes Handle(GameClient client, C2SRankingRankListByCharacterIdReq request)
        {
            S2CRankingRankListByCharacterIdRes res = new();

            var rankResults = Server.Database.SelectRankingData(request.BoardId);
            foreach(var characterId in request.CharacterIdList)
            {
                var characterRank = rankResults.Find(x => x.CommunityCharacterBaseInfo.CharacterId == characterId.Value);
                if (characterRank is null)
                {
                    continue;
                }

                res.RankingData.Add(characterRank);
            }

            return res;
        }
    }
}
