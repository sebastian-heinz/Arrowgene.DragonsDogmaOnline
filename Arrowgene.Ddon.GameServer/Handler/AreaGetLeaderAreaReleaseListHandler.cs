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
            foreach ( (var area, var rank) in clientRank)
            {
                var releaseList = Server.AreaRankManager.GetAreaRankSpots(client, area)
                    .Select(x => new CDataCommonU32(x.SpotId))
                    .ToList();

                result.ReleaseAreaInfoSetList.Add(new()
                {
                    AreaId = rank.AreaId,
                    ReleaseList = releaseList
                });
            }

            return result;
        }
    }
}
