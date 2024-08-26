using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure;

public class S2CAchievementRewardReceiveRes : ServerResponse
{
    public override PacketId Id => PacketId.S2C_ACHIEVEMENT_ACHIEVEMENT_REWARD_RECEIVE_RES;

    /// <summary>
    ///     Should be the list of received items, but for type 2 it can be empty.
    /// </summary>
    public List<CDataAchieveRewardCommon> ReceivedRewardList { get; set; } = new();

    /// <summary>
    ///     Remaining claimable rewards used to update the achievement UI. If wrongly filled, will leave UI in confusing state until reopened.
    /// </summary>
    public List<CDataAchieveRewardCommon> RewardList { get; set; } = new();

    public class Serializer : PacketEntitySerializer<S2CAchievementRewardReceiveRes>
    {
        public override void Write(IBuffer buffer, S2CAchievementRewardReceiveRes obj)
        {
            WriteServerResponse(buffer, obj);

            WriteEntityList(buffer, obj.ReceivedRewardList);
            WriteEntityList(buffer, obj.RewardList);
        }

        public override S2CAchievementRewardReceiveRes Read(IBuffer buffer)
        {
            S2CAchievementRewardReceiveRes obj = new S2CAchievementRewardReceiveRes();

            ReadServerResponse(buffer, obj);

            obj.ReceivedRewardList = ReadEntityList<CDataAchieveRewardCommon>(buffer);
            obj.RewardList = ReadEntityList<CDataAchieveRewardCommon>(buffer);

            return obj;
        }
    }
}
