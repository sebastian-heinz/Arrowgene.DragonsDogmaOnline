using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestQuestOrderNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_QUEST_QUEST_ORDER_NTC;

        public S2CQuestQuestOrderNtc()
        {
        }

        public uint QuestScheduleId { get; set; }
        public uint QuestId { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestQuestOrderNtc>
        {
            public override void Write(IBuffer buffer, S2CQuestQuestOrderNtc obj)
            {
                WriteUInt32(buffer, obj.QuestScheduleId);
                WriteUInt32(buffer, obj.QuestId);
            }

            public override S2CQuestQuestOrderNtc Read(IBuffer buffer)
            {
                S2CQuestQuestOrderNtc obj = new S2CQuestQuestOrderNtc();
                obj.QuestScheduleId = ReadUInt32(buffer);
                obj.QuestId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
