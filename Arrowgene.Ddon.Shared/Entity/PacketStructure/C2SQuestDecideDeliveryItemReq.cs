using System;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestDecideDeliveryItemReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_DECIDE_DELIVERY_ITEM_REQ;

        public C2SQuestDecideDeliveryItemReq()
        {
        }

        public UInt32 QuestScheduleId { get; set; }
        public UInt16 ProcessNo { get; set; }

        public class Serializer : PacketEntitySerializer<C2SQuestDecideDeliveryItemReq>
        {
            public override void Write(IBuffer buffer, C2SQuestDecideDeliveryItemReq obj)
            {
                WriteUInt32(buffer, obj.QuestScheduleId);
                WriteUInt16(buffer, obj.ProcessNo);
            }

            public override C2SQuestDecideDeliveryItemReq Read(IBuffer buffer)
            {
                C2SQuestDecideDeliveryItemReq obj = new C2SQuestDecideDeliveryItemReq();
                obj.QuestScheduleId = ReadUInt32(buffer);
                obj.ProcessNo = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}

