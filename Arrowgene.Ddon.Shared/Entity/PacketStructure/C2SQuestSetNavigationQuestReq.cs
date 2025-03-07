using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestSetNavigationQuestReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_SET_NAVIGATION_QUEST_REQ;

        public uint QuestScheduleId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SQuestSetNavigationQuestReq>
        {
            public override void Write(IBuffer buffer, C2SQuestSetNavigationQuestReq obj)
            {
                WriteUInt32(buffer, obj.QuestScheduleId);
            }

            public override C2SQuestSetNavigationQuestReq Read(IBuffer buffer)
            {
                C2SQuestSetNavigationQuestReq obj = new C2SQuestSetNavigationQuestReq();
                obj.QuestScheduleId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
