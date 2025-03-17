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
                UId = 530, // Bounty Hunter
                Index = 530
            },
            FurnitureItemId = 13225, // Mini Table
            IsReceived = true
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
