using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler;

public class AchievementRewardReceiveHandler : GameRequestPacketHandler<C2SAchievementRewardReceiveReq, S2CAchievementRewardReceiveRes>
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(AchievementGetRewardListHandler));

    public AchievementRewardReceiveHandler(DdonGameServer server) : base(server)
    {
    }

    public override S2CAchievementRewardReceiveRes Handle(GameClient client, C2SAchievementRewardReceiveReq request)
    {
        S2CAchievementRewardReceiveRes res = new S2CAchievementRewardReceiveRes();

        res.RewardList = new List<CDataAchieveRewardCommon>
        {
            new()
            {
                Type = 1,
                RewardId = 53
            },
            new()
            {
                Type = 2,
                RewardId = 27
            }
        };
        res.ReceivedRewardList = new List<CDataAchieveRewardCommon>
        {
            new()
            {
                Type = 2,
                RewardId = 63
            }
        };

        // TODO: look up reward/item ID for a reward ID => then look up potential item's item recipe item ID
        // e.g. RewardId 63 => FurnitureItemId 16126 => Item's Item Recipe Id 16227 => CraftingRecipe.json Recipe ID 270001
        foreach (CDataAchieveRewardCommon rewardCommon in request.RewardList)
        {
            S2CItemAchievementRewardReceiveNtc unlockNtc = new S2CItemAchievementRewardReceiveNtc();
            unlockNtc.UpdateType = ItemNoticeType.Drop;
            unlockNtc.ItemNum = 1;
            unlockNtc.StorageType = StorageType.StorageBoxExpansion;
            unlockNtc.ItemId = 16227;
            // TODO: document in some table which recipes have been unlocked
            client.Send(unlockNtc);
        }

        return res;
    }
}
