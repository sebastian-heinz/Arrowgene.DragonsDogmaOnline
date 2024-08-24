using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure;

public class S2CAchievementGetReceivableRewardListRes : ServerResponse
{
    public override PacketId Id => PacketId.S2C_ACHIEVEMENT_ACHIEVEMENT_GET_RECEIVABLE_REWARD_LIST_RES;

    public List<CDataAchieveRewardCommon> RewardList { get; set; } = new();

    public class Serializer : PacketEntitySerializer<S2CAchievementGetReceivableRewardListRes>
    {
        public override void Write(IBuffer buffer, S2CAchievementGetReceivableRewardListRes obj)
        {
            WriteServerResponse(buffer, obj);

            WriteEntityList(buffer, obj.RewardList);
        }

        public override S2CAchievementGetReceivableRewardListRes Read(IBuffer buffer)
        {
            S2CAchievementGetReceivableRewardListRes obj = new S2CAchievementGetReceivableRewardListRes();

            ReadServerResponse(buffer, obj);

            obj.RewardList = ReadEntityList<CDataAchieveRewardCommon>(buffer);

            return obj;
        }
    }
}
