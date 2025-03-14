using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartnerPawnPawnLikabilityReleasedRewardListGetHandler : GameRequestPacketHandler<C2SPartnerPawnPawnLikabilityReleasedRewardListGetReq,
        S2CPartnerPawnPawnLikabilityReleasedRewardListGetRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartnerPawnPawnLikabilityReleasedRewardListGetHandler));


        public PartnerPawnPawnLikabilityReleasedRewardListGetHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPartnerPawnPawnLikabilityReleasedRewardListGetRes Handle(GameClient client, C2SPartnerPawnPawnLikabilityReleasedRewardListGetReq request)
        {
            return new S2CPartnerPawnPawnLikabilityReleasedRewardListGetRes()
            {
                ReleasedRewardList = Server.PartnerPawnManager.GetReleasedRewards(client)
            };
        }
    }
}
