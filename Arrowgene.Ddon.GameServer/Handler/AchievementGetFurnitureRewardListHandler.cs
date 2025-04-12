using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler;

public class AchievementGetFurnitureRewardListHandler : GameRequestPacketHandler<C2SAchievementGetFurnitureRewardListReq, S2CAchievementGetFurnitureRewardListRes>
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(AchievementGetFurnitureRewardListHandler));

    public AchievementGetFurnitureRewardListHandler(DdonGameServer server) : base(server)
    {
    }

    public override S2CAchievementGetFurnitureRewardListRes Handle(GameClient client, C2SAchievementGetFurnitureRewardListReq request)
    {
        S2CAchievementGetFurnitureRewardListRes res = new S2CAchievementGetFurnitureRewardListRes();

        res.RewardList = Server.AssetRepository.AchievementAsset
            .SelectMany(x => x.Value)
            .Where(x => x.RewardId > 0)
            .Select((x, i) => new CDataAchievementFurnitureReward()
            {
                RewardId = x.Id,
                SortId = x.SortId,
                AchieveIdentifier = new()
                {
                    UId = x.Id,
                    Index = x.SortId,
                },
                FurnitureItemId = x.RewardId,
                IsReceived = client.Character.AchievementStatus.ContainsKey(x.Id)
            })
            .ToList();

        return res;
    }
}
