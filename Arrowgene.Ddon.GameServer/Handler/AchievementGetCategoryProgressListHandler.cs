using System;
using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler;

public class AchievementGetCategoryProgressListHandler : GameRequestPacketHandler<C2SAchievementGetCategoryProgressListReq, S2CAchievementGetCategoryProgressListRes>
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(AchievementGetCategoryProgressListHandler));

    public AchievementGetCategoryProgressListHandler(DdonGameServer server) : base(server)
    {
    }

    public override S2CAchievementGetCategoryProgressListRes Handle(GameClient client, C2SAchievementGetCategoryProgressListReq request)
    {
        S2CAchievementGetCategoryProgressListRes res = new S2CAchievementGetCategoryProgressListRes();

        var achievements = Server.AssetRepository.AchievementAsset.SelectMany(x => x.Value).Where(x => (byte)x.Category == request.Category);
        var progress = Server.AchievementManager.CheckProgress(client, achievements);

        res.AchievementProgressList = achievements.Zip(progress, (Achievement, Progress) => (Achievement, Progress))
            .Select(z => 
                new CDataAchievementProgress()
                {
                    AchieveIdentifier = new()
                    {
                        UId = z.Achievement.Id,
                        Index = z.Achievement.SortId
                    },
                    CurrentNum = z.Progress,
                    CompleteDate = client.Character.AchievementStatus.GetValueOrDefault(z.Achievement.Id, DateTimeOffset.MaxValue)
                }
            )
            .ToList();

        return res;
    }
}
