using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestCancelPriorityQuestRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_CANCEL_PRIORITY_QUEST_RES;

        public S2CQuestCancelPriorityQuestRes()
        {
        }

        public uint QuestScheduleId { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestCancelPriorityQuestRes>
        {
            public override void Write(IBuffer buffer, S2CQuestCancelPriorityQuestRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.QuestScheduleId);
            }

            public override S2CQuestCancelPriorityQuestRes Read(IBuffer buffer)
            {
                S2CQuestCancelPriorityQuestRes obj = new S2CQuestCancelPriorityQuestRes();
                ReadServerResponse(buffer, obj);
                obj.QuestScheduleId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

