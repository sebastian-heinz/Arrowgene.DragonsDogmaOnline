using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestGetWorldManageQuestListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_GET_WORLD_MANAGE_QUEST_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SQuestGetWorldManageQuestListReq>
        {
            public override void Write(IBuffer buffer, C2SQuestGetWorldManageQuestListReq obj)
            {
            }

            public override C2SQuestGetWorldManageQuestListReq Read(IBuffer buffer)
            {
                C2SQuestGetWorldManageQuestListReq obj = new C2SQuestGetWorldManageQuestListReq();
                return obj;
            }
        }
    }
}
