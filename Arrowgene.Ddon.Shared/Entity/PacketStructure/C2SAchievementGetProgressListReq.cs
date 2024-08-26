using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure;

public class C2SAchievementGetProgressListReq : IPacketStructure
{
    public PacketId Id => PacketId.C2S_ACHIEVEMENT_ACHIEVEMENT_GET_PROGRESS_LIST_REQ;

    public class Serializer : PacketEntitySerializer<C2SAchievementGetProgressListReq>
    {
        public override void Write(IBuffer buffer, C2SAchievementGetProgressListReq obj)
        {
        }

        public override C2SAchievementGetProgressListReq Read(IBuffer buffer)
        {
            C2SAchievementGetProgressListReq obj = new C2SAchievementGetProgressListReq();
            return obj;
        }
    }
}
