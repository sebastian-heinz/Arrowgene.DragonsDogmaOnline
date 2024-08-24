using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler;

public class AchievementGetReceivableRewardListHandler : GameRequestPacketHandler<C2SAchievementGetReceivableRewardListReq, S2CAchievementGetReceivableRewardListRes>
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(AchievementGetReceivableRewardListHandler));

    private static readonly List<CDataAchieveRewardCommon> RewardList = new()
    {
        new()
        {
            Type = 1,
            RewardId = 53
        },
        new()
        {
            Type = 2, // Craft recipe reward
            RewardId = 63
        },
        new()
        {
            Type = 2, // Craft recipe reward
            RewardId = 27
        }
    };

    public AchievementGetReceivableRewardListHandler(DdonGameServer server) : base(server)
    {
    }

    public override S2CAchievementGetReceivableRewardListRes Handle(GameClient client, C2SAchievementGetReceivableRewardListReq request)
    {
        S2CAchievementGetReceivableRewardListRes res = new S2CAchievementGetReceivableRewardListRes();

        res.RewardList = RewardList;

        return res;
    }
}
