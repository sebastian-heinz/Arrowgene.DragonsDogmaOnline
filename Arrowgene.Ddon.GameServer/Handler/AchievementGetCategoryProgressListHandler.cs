using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler;

public class AchievementGetCategoryProgressListHandler : GameRequestPacketHandler<C2SAchievementGetCategoryProgressListReq, S2CAchievementGetCategoryProgressListRes>
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(AchievementGetCategoryProgressListHandler));

    private static readonly List<CDataAchievementProgress> AchievementProgressListCategory6 = new()
    {
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2856,
                Index = 1725
            },
            CurrentNum = 0,
            Sequence = 0
        }
    };

    public AchievementGetCategoryProgressListHandler(DdonGameServer server) : base(server)
    {
    }

    public override S2CAchievementGetCategoryProgressListRes Handle(GameClient client, C2SAchievementGetCategoryProgressListReq request)
    {
        S2CAchievementGetCategoryProgressListRes res = new S2CAchievementGetCategoryProgressListRes();

        if (request.Category == 6) res.AchievementProgressList = AchievementProgressListCategory6;

        return res;
    }
}
