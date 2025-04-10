using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler;

public class AchievementGetReceivableRewardListHandler : GameRequestPacketHandler<C2SAchievementGetReceivableRewardListReq, S2CAchievementGetReceivableRewardListRes>
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(AchievementGetReceivableRewardListHandler));

    private static readonly List<CDataAchieveRewardCommon> RewardList = new List<CDataAchieveRewardCommon>();

    public AchievementGetReceivableRewardListHandler(DdonGameServer server) : base(server)
    {
    }

    public override S2CAchievementGetReceivableRewardListRes Handle(GameClient client, C2SAchievementGetReceivableRewardListReq request)
    {
        S2CAchievementGetReceivableRewardListRes res = new S2CAchievementGetReceivableRewardListRes();

        res.RewardList = Server.AchievementManager.GetRewards(client);

        return res;
    }
}
