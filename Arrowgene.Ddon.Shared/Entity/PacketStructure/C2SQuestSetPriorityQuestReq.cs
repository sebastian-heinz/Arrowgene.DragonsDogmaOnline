using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestSetPriorityQuestReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_SET_PRIORITY_QUEST_REQ;

        public uint QuestScheduleId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SQuestSetPriorityQuestReq>
        {
            public override void Write(IBuffer buffer, C2SQuestSetPriorityQuestReq obj)
            {
                WriteUInt32(buffer, obj.QuestScheduleId);
            }

            public override C2SQuestSetPriorityQuestReq Read(IBuffer buffer)
            {
                C2SQuestSetPriorityQuestReq obj = new C2SQuestSetPriorityQuestReq();
                obj.QuestScheduleId = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}