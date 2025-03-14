using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartnerPawnPawnLikabilityRewardListGetHandler : GameRequestPacketHandler<C2SPartnerPawnPawnLikabilityRewardListGetReq,
        S2CPartnerPawnPawnLikabilityRewardListGetRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartnerPawnPawnLikabilityRewardListGetHandler));

        public PartnerPawnPawnLikabilityRewardListGetHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPartnerPawnPawnLikabilityRewardListGetRes Handle(GameClient client, C2SPartnerPawnPawnLikabilityRewardListGetReq request)
        {
            // Returns unclaimed rewards when the player enters into the arisens room
            return new S2CPartnerPawnPawnLikabilityRewardListGetRes()
            {
                RewardList = Server.PartnerPawnManager.GetUnclaimedRewardsForCurrentPartnerPawn(client)
            };
        }
    }
}
