using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure;

public class C2SAchievementGetRewardListReq : IPacketStructure
{
    public PacketId Id => PacketId.C2S_ACHIEVEMENT_ACHIEVEMENT_GET_REWARD_LIST_REQ;

    public class Serializer : PacketEntitySerializer<C2SAchievementGetRewardListReq>
    {
        public override void Write(IBuffer buffer, C2SAchievementGetRewardListReq obj)
        {
        }

        public override C2SAchievementGetRewardListReq Read(IBuffer buffer)
        {
            C2SAchievementGetRewardListReq obj = new C2SAchievementGetRewardListReq();
            return obj;
        }
    }
}
