using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure;

public class S2CAchievementGetRewardListRes : ServerResponse
{
    public override PacketId Id => PacketId.S2C_ACHIEVEMENT_ACHIEVEMENT_GET_REWARD_LIST_RES;

    public List<CDataAchievementRewardProgress> BackgroundProgressList { get; set; } = new();

    public class Serializer : PacketEntitySerializer<S2CAchievementGetRewardListRes>
    {
        public override void Write(IBuffer buffer, S2CAchievementGetRewardListRes obj)
        {
            WriteServerResponse(buffer, obj);

            WriteEntityList(buffer, obj.BackgroundProgressList);
        }

        public override S2CAchievementGetRewardListRes Read(IBuffer buffer)
        {
            S2CAchievementGetRewardListRes obj = new S2CAchievementGetRewardListRes();

            ReadServerResponse(buffer, obj);

            obj.BackgroundProgressList = ReadEntityList<CDataAchievementRewardProgress>(buffer);

            return obj;
        }
    }
}
