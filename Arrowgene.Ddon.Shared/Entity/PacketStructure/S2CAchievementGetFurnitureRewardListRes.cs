using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure;

public class S2CAchievementGetFurnitureRewardListRes : ServerResponse
{
    public override PacketId Id => PacketId.S2C_ACHIEVEMENT_ACHIEVEMENT_GET_FURNITURE_REWARD_LIST_RES;

    public List<CDataAchievementFurnitureReward> RewardList { get; set; } = new();

    public class Serializer : PacketEntitySerializer<S2CAchievementGetFurnitureRewardListRes>
    {
        public override void Write(IBuffer buffer, S2CAchievementGetFurnitureRewardListRes obj)
        {
            WriteServerResponse(buffer, obj);

            WriteEntityList(buffer, obj.RewardList);
        }

        public override S2CAchievementGetFurnitureRewardListRes Read(IBuffer buffer)
        {
            S2CAchievementGetFurnitureRewardListRes obj = new S2CAchievementGetFurnitureRewardListRes();

            ReadServerResponse(buffer, obj);

            obj.RewardList = ReadEntityList<CDataAchievementFurnitureReward>(buffer);

            return obj;
        }
    }
}
