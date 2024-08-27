using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler;

public class AchievementGetRewardListHandler : GameRequestPacketHandler<C2SAchievementGetRewardListReq, S2CAchievementGetRewardListRes>
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(AchievementGetRewardListHandler));

    private static readonly List<CDataAchievementRewardProgress> BackgroundProgressList = new List<CDataAchievementRewardProgress>();

    public AchievementGetRewardListHandler(DdonGameServer server) : base(server)
    {
    }

    public override S2CAchievementGetRewardListRes Handle(GameClient client, C2SAchievementGetRewardListReq request)
    {
        S2CAchievementGetRewardListRes res = new S2CAchievementGetRewardListRes();

        // TODO: retrieve the amount of completed achievements here & check already claimed background rewards
        res.BackgroundProgressList = BackgroundProgressList;

        return res;
    }
}
