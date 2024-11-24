using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SeasonDungeonGetSoulOrdealRewardListForViewHandler : GameRequestPacketHandler<C2SSeasonDungeonGetSoulOrdealRewardListForViewReq, S2CSeasonDungeonGetSoulOrdealRewardListForViewRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SeasonDungeonGetSoulOrdealRewardListForViewHandler));

        public SeasonDungeonGetSoulOrdealRewardListForViewHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CSeasonDungeonGetSoulOrdealRewardListForViewRes Handle(GameClient client, C2SSeasonDungeonGetSoulOrdealRewardListForViewReq request)
        {
            var trialInfo = Server.EpitaphRoadManager.GetTrialOptionInfo(request.EpitaphId);
            if (trialInfo == null)
            {
                return new();
            }

            var result = new S2CSeasonDungeonGetSoulOrdealRewardListForViewRes();
            foreach (var reward in trialInfo.ItemRewards)
            {
                result.RewardList.AddRange(reward.AsCDataSeasonDungeonRewardItemViewEntry());
            }
            return result;
        }
    }
}
