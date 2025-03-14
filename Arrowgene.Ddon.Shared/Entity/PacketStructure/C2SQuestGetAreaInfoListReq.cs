using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestGetAreaInfoListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_GET_AREA_INFO_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SQuestGetAreaInfoListReq>
        {
            public override void Write(IBuffer buffer, C2SQuestGetAreaInfoListReq obj)
            {
            }

            public override C2SQuestGetAreaInfoListReq Read(IBuffer buffer)
            {
                C2SQuestGetAreaInfoListReq obj = new C2SQuestGetAreaInfoListReq();

                return obj;
            }
        }
    }
}
