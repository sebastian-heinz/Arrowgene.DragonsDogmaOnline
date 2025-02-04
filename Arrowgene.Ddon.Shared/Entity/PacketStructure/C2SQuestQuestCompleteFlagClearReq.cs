using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using System;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestQuestCompleteFlagClearReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_QUEST_COMPLETE_FLAG_CLEAR_REQ;

        public UInt32 QuestScheduleId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SQuestQuestCompleteFlagClearReq>
        {
            public override void Write(IBuffer buffer, C2SQuestQuestCompleteFlagClearReq obj)
            {
                WriteUInt32(buffer, obj.QuestScheduleId);
            }

            public override C2SQuestQuestCompleteFlagClearReq Read(IBuffer buffer)
            {
                C2SQuestQuestCompleteFlagClearReq obj = new C2SQuestQuestCompleteFlagClearReq();
                obj.QuestScheduleId = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}
