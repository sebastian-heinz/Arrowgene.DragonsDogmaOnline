using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestGetAdventureGuideQuestListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_GET_ADVENTURE_GUIDE_QUEST_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SQuestGetAdventureGuideQuestListReq>
        {
            public override void Write(IBuffer buffer, C2SQuestGetAdventureGuideQuestListReq obj)
            {
            }

            public override C2SQuestGetAdventureGuideQuestListReq Read(IBuffer buffer)
            {
                C2SQuestGetAdventureGuideQuestListReq obj = new C2SQuestGetAdventureGuideQuestListReq();
                return obj;
            }
        }
    }
}
