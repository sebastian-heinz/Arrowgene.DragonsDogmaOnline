using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestGetAdventureGuideQuestNtcReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_GET_ADVENTURE_GUIDE_QUEST_NTC_REQ;

        public class Serializer : PacketEntitySerializer<C2SQuestGetAdventureGuideQuestNtcReq>
        {
            public override void Write(IBuffer buffer, C2SQuestGetAdventureGuideQuestNtcReq obj)
            {
            }

            public override C2SQuestGetAdventureGuideQuestNtcReq Read(IBuffer buffer)
            {
                C2SQuestGetAdventureGuideQuestNtcReq obj = new C2SQuestGetAdventureGuideQuestNtcReq();
                return obj;
            }
        }
    }
}
