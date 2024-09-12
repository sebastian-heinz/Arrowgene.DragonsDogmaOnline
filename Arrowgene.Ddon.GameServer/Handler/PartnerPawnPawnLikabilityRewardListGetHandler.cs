using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
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
            S2CPartnerPawnPawnLikabilityRewardListGetRes res = new S2CPartnerPawnPawnLikabilityRewardListGetRes();

            // TODO: figure out if we have dumps for this
            res.RewardList = new List<CDataPartnerPawnReward>
            {
                // new CDataPartnerPawnReward
                // {
                //     Type = 4,
                //     Value = new CDataPartnerPawnRewardParam
                //     {
                //         ParamTypeId = 0,
                //         UID = 441
                //     }
                // }
            };

            return res;
        }
    }
}
