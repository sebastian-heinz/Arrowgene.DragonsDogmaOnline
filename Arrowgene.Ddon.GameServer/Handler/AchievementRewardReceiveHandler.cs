using System.Collections.Generic;
using System.Linq;
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
        res.ReceivedRewardList = request.RewardList;

        if (request.RewardList.Any())
        {
            var requestReward = request.RewardList.First();
            UnlockableItemCategory rewardType = rewardType = UnlockableItemCategory.None;
            AchievementAsset sourceAchievement = null;
            if (requestReward.Type == 1)
            {
                rewardType = UnlockableItemCategory.ArisenCardBackground;
                client.Character.UnlockableItems.Add((rewardType, requestReward.RewardId));
                Server.Database.InsertUnlockedItem(client.Character.CharacterId, rewardType, requestReward.RewardId);

                // TODO: Send notice for this?
            }
            else
            {
                sourceAchievement = Server.AssetRepository.AchievementAsset.SelectMany(x => x.Value).Where(x => x.Id == requestReward.RewardId).FirstOrDefault();
                if (sourceAchievement != null)
                {
                    var itemInfo = ClientItemInfo.GetInfoForItemId(Server.AssetRepository.ClientItemInfos, sourceAchievement.RewardId);

                    if (itemInfo?.Category == 6)
                    {
                        rewardType = UnlockableItemCategory.FurnitureItem;
                    }
                    else if (itemInfo?.Category == 7)
                    {
                        rewardType = UnlockableItemCategory.CraftingRecipe;
                    }

                    // TODO: Investigate Unks
                    client.Send(new S2CItemAchievementRewardReceiveNtc()
                    {
                        Unk0 = 2, 
                        Unk1 = 1,
                        Unk2 = 7,
                        ItemId = sourceAchievement.RewardId
                    });

                    client.Character.UnlockableItems.Add((rewardType, sourceAchievement.RewardId));
                    Server.Database.InsertUnlockedItem(client.Character.CharacterId, rewardType, sourceAchievement.RewardId);
                }
            }
        }

        res.RewardList = Server.AchievementManager.GetRewards(client);

        return res;
    }
}
