using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
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

        // Should be the list of remaining rewards the user has
        res.RewardList = new List<CDataAchieveRewardCommon>();
        // Should simply be filled with whatever the user chose based on the request packet
        res.ReceivedRewardList = new List<CDataAchieveRewardCommon>();

        // TODO: look up reward/item ID for a reward ID => then look up potential item's item recipe item ID
        // e.g. RewardId 63 => FurnitureItemId 16126 => Item's Item Recipe Id 16227 => CraftingRecipe.json Recipe ID 270001
        // A user can never receive more than one reward at a time due to how the UI works, even if we are working with lists here
        S2CItemAchievementRewardReceiveNtc unlockNtc = new S2CItemAchievementRewardReceiveNtc();
        unlockNtc.Unk0 = 2; // packet dump
        unlockNtc.Unk1 = 1; // packet dump
        unlockNtc.Unk2 = 7; // packet dump
        unlockNtc.ItemId = 16227; // packet dump
        client.Send(unlockNtc);
        
        // TODO: document in some table which recipes have been unlocked

        return res;
    }
}
