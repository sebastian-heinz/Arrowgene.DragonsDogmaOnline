using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
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
            S2CPartnerPawnPawnLikabilityReleasedRewardListGetRes res = new S2CPartnerPawnPawnLikabilityReleasedRewardListGetRes();

            res.ReleasedRewardList = new List<CDataPartnerPawnReward>
            {
                // new CDataPartnerPawnReward
                // {
                //     Type = 3,
                //     Value = new CDataPartnerPawnRewardParam
                //     {
                //         ParamTypeId = 0,
                //         UID = 45
                //     }
                // },
                // new CDataPartnerPawnReward
                // {
                //     Type = 2,
                //     Value = new CDataPartnerPawnRewardParam
                //     {
                //         ParamTypeId = 0,
                //         UID = 2
                //     }
                // },
                // new CDataPartnerPawnReward
                // {
                //     Type = 1,
                //     Value = new CDataPartnerPawnRewardParam
                //     {
                //         ParamTypeId = 2,
                //         UID = 72
                //     }
                // }
            };

            return res;
        }
    }
}
