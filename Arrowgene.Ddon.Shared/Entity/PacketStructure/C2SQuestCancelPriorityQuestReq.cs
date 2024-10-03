using System;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestCancelPriorityQuestReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_CANCEL_PRIORITY_QUEST_REQ;

        public C2SQuestCancelPriorityQuestReq()
        {
        }

        public UInt32 QuestScheduleId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SQuestCancelPriorityQuestReq>
        {
            public override void Write(IBuffer buffer, C2SQuestCancelPriorityQuestReq obj)
            {
                WriteUInt32(buffer, obj.QuestScheduleId);
            }

            public override C2SQuestCancelPriorityQuestReq Read(IBuffer buffer)
            {
                C2SQuestCancelPriorityQuestReq obj = new C2SQuestCancelPriorityQuestReq();
                obj.QuestScheduleId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

