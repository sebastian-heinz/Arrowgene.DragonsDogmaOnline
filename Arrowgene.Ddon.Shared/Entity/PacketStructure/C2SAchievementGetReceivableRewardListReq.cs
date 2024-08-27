using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure;

public class C2SAchievementGetReceivableRewardListReq : IPacketStructure
{
    public PacketId Id => PacketId.C2S_ACHIEVEMENT_ACHIEVEMENT_GET_RECEIVABLE_REWARD_LIST_REQ;

    public class Serializer : PacketEntitySerializer<C2SAchievementGetReceivableRewardListReq>
    {
        public override void Write(IBuffer buffer, C2SAchievementGetReceivableRewardListReq obj)
        {
        }

        public override C2SAchievementGetReceivableRewardListReq Read(IBuffer buffer)
        {
            C2SAchievementGetReceivableRewardListReq obj = new C2SAchievementGetReceivableRewardListReq();
            return obj;
        }
    }
}
