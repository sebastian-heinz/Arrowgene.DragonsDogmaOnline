using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

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

        uint count = (uint)client.Character.AchievementStatus.Count;

        res.BackgroundProgressList.AddRange(Server.AssetRepository.AchievementBackgroundAsset.UnlockableBackgrounds.Select(x => new CDataAchievementRewardProgress()
        {
            RewardId = x.Id,
            CurrentNum = Math.Min(count, x.Required),
            TargetNum = x.Required,
            IsReceived = client.Character.UnlockableItems.Contains((UnlockableItemCategory.ArisenCardBackground, x.Id)),
        }));

        return res;
    }
}
