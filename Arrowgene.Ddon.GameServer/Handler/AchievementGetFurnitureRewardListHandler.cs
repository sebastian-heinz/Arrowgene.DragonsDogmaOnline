using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler;

public class AchievementGetFurnitureRewardListHandler : GameRequestPacketHandler<C2SAchievementGetFurnitureRewardListReq, S2CAchievementGetFurnitureRewardListRes>
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(AchievementGetFurnitureRewardListHandler));

    private static readonly List<CDataAchievementFurnitureReward> RewardList = new()
    {
        new()
        {
            RewardId = 1,
            SortId = 1,
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 530,
                Index = 12
            },
            FurnitureItemId = 13225,
            IsReceived = true
        },
        new()
        {
            RewardId = 63,
            SortId = 63,
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2532,
                Index = 980
            },
            FurnitureItemId = 16126,
            IsReceived = false
        },
        new()
        {
            RewardId = 27,
            SortId = 27,
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1267,
                Index = 370
            },
            FurnitureItemId = 13236,
            IsReceived = false
        }
    };

    public AchievementGetFurnitureRewardListHandler(DdonGameServer server) : base(server)
    {
    }

    public override S2CAchievementGetFurnitureRewardListRes Handle(GameClient client, C2SAchievementGetFurnitureRewardListReq request)
    {
        S2CAchievementGetFurnitureRewardListRes res = new S2CAchievementGetFurnitureRewardListRes();

        // TODO: based on an asset with all achievements, for each character store the progress and retrieve here
        res.RewardList = RewardList;

        return res;
    }
}
