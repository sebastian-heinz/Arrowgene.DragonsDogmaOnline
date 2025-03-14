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

            // If request.CharacterIdList is always just the player character, then we can optimize this into an alternative SQL query,
            // but I leave it here as post-processing for safety.
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
