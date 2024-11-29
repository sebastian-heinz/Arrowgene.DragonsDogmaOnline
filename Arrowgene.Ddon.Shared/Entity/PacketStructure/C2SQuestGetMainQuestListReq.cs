using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestGetMainQuestListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_GET_MAIN_QUEST_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SQuestGetMainQuestListReq>
        {
            public override void Write(IBuffer buffer, C2SQuestGetMainQuestListReq obj)
            {
            }

            public override C2SQuestGetMainQuestListReq Read(IBuffer buffer)
            {
                C2SQuestGetMainQuestListReq obj = new C2SQuestGetMainQuestListReq();
                return obj;
            }
        }
    }
}
