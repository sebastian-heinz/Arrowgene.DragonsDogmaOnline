using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestGetEndContentsRecruitListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_GET_END_CONTENTS_RECRUIT_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SQuestGetEndContentsRecruitListReq>
        {
            public override void Write(IBuffer buffer, C2SQuestGetEndContentsRecruitListReq obj)
            {
            }

            public override C2SQuestGetEndContentsRecruitListReq Read(IBuffer buffer)
            {
                C2SQuestGetEndContentsRecruitListReq obj = new C2SQuestGetEndContentsRecruitListReq();
                return obj;
            }
        }
    }
}
