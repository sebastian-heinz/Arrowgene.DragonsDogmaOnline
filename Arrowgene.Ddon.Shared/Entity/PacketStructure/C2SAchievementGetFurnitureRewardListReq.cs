using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure;

public class C2SAchievementGetFurnitureRewardListReq : IPacketStructure
{
    public PacketId Id => PacketId.C2S_ACHIEVEMENT_ACHIEVEMENT_GET_FURNITURE_REWARD_LIST_REQ;

    public class Serializer : PacketEntitySerializer<C2SAchievementGetFurnitureRewardListReq>
    {
        public override void Write(IBuffer buffer, C2SAchievementGetFurnitureRewardListReq obj)
        {
        }

        public override C2SAchievementGetFurnitureRewardListReq Read(IBuffer buffer)
        {
            C2SAchievementGetFurnitureRewardListReq obj = new C2SAchievementGetFurnitureRewardListReq();
            return obj;
        }
    }
}
