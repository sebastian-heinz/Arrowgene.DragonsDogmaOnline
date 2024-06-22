using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using System;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestQuestCancelReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_QUEST_CANCEL_REQ;

        public UInt32 QuestScheduleId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SQuestQuestCancelReq>
        {
            public override void Write(IBuffer buffer, C2SQuestQuestCancelReq obj)
            {
                WriteUInt32(buffer, obj.QuestScheduleId);
            }

            public override C2SQuestQuestCancelReq Read(IBuffer buffer)
            {
                C2SQuestQuestCancelReq obj = new C2SQuestQuestCancelReq();
                obj.QuestScheduleId = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}

