using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestDecideDeliveryItemRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_DECIDE_DELIVERY_ITEM_RES;

        public S2CQuestDecideDeliveryItemRes()
        {
        }

        public UInt32 QuestScheduleId { get; set; }
        public UInt16 ProcessNo { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestDecideDeliveryItemRes>
        {
            public override void Write(IBuffer buffer, S2CQuestDecideDeliveryItemRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.QuestScheduleId);
                WriteUInt16(buffer, obj.ProcessNo);
            }

            public override S2CQuestDecideDeliveryItemRes Read(IBuffer buffer)
            {
                S2CQuestDecideDeliveryItemRes obj = new S2CQuestDecideDeliveryItemRes();
                ReadServerResponse(buffer, obj);
                obj.QuestScheduleId = ReadUInt32(buffer);
                obj.ProcessNo = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}

