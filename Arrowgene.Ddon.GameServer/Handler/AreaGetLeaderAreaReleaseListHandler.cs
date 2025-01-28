using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class AreaGetLeaderAreaReleaseListHandler : GameRequestPacketHandler<C2SAreaGetLeaderAreaReleaseListReq, S2CAreaGetLeaderAreaReleaseListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(AreaGetLeaderAreaReleaseListHandler));

        public AreaGetLeaderAreaReleaseListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CAreaGetLeaderAreaReleaseListRes Handle(GameClient client, C2SAreaGetLeaderAreaReleaseListReq request)
        {
            var result = new S2CAreaGetLeaderAreaReleaseListRes();
            var clientRank = client.Character.AreaRanks;
            var completedQuests = client.Character.CompletedQuests;
            foreach ((var area, var rank) in clientRank)
            {
                // The client gets very angry if the unlocks are desynced from their actual ranks,
                // so spoof the rank in AreaGetAreaBaseInfoListHandler too.
                uint effectiveRank = Server.AreaRankManager.GetEffectiveRank(client.Character, area);

                var releaseList = Server.AssetRepository.AreaRankSpotInfoAsset[area]
                .Where(x =>
                    x.UnlockRank <= effectiveRank
                    && (x.UnlockQuest == 0 || completedQuests.ContainsKey((QuestId)x.UnlockQuest))
                )
                .DistinctBy(x => x.SpotId)
                .OrderBy(x => x.SpotId)
                .Select(x => new CDataCommonU32(x.SpotId))
                .ToList();

                result.ReleaseAreaInfoSetList.Add(new()
                {
                    AreaId = area,
                    ReleaseList = releaseList
                });
            }

            return result;
        }
    }
}
