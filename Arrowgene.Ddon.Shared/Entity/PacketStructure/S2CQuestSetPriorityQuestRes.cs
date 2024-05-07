using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestSetPriorityQuestRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_SET_PRIORITY_QUEST_RES;

        public uint QuestScheduleId { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestSetPriorityQuestRes>
        {
            public override void Write(IBuffer buffer, S2CQuestSetPriorityQuestRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.QuestScheduleId);
            }

            public override S2CQuestSetPriorityQuestRes Read(IBuffer buffer)
            {
                S2CQuestSetPriorityQuestRes obj = new S2CQuestSetPriorityQuestRes();
                ReadServerResponse(buffer, obj);
                obj.QuestScheduleId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}