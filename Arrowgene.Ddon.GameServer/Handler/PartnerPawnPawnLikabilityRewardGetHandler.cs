using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartnerPawnPawnLikabilityRewardGetHandler : GameRequestPacketHandler<C2SPartnerPawnPawnLikabilityRewardGetReq, S2CPartnerPawnPawnLikabilityRewardGetRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartnerPawnPawnLikabilityRewardGetHandler));

        public PartnerPawnPawnLikabilityRewardGetHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPartnerPawnPawnLikabilityRewardGetRes Handle(GameClient client, C2SPartnerPawnPawnLikabilityRewardGetReq request)
        {
            S2CPartnerPawnPawnLikabilityRewardGetRes res = new S2CPartnerPawnPawnLikabilityRewardGetRes();
            
            // TODO: probably a list only out of convenience? update DB to remove this from reward list and add to released rewards list
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
