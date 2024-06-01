using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using System;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestDecideDeliveryItemNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_QUEST_DECIDE_DELIVERY_ITEM_NTC;

        public S2CQuestDecideDeliveryItemNtc()
        {
        }

        public UInt32 QuestScheduleId { get; set; }
        public UInt32 ProcessNo { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestDecideDeliveryItemNtc>
        {
            public override void Write(IBuffer buffer, S2CQuestDecideDeliveryItemNtc obj)
            {
                WriteUInt32(buffer, obj.QuestScheduleId);
                WriteUInt32(buffer, obj.ProcessNo);
            }

            public override S2CQuestDecideDeliveryItemNtc Read(IBuffer buffer)
            {
                S2CQuestDecideDeliveryItemNtc obj = new S2CQuestDecideDeliveryItemNtc();
                obj.QuestScheduleId = ReadUInt32(buffer);
                obj.ProcessNo = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

