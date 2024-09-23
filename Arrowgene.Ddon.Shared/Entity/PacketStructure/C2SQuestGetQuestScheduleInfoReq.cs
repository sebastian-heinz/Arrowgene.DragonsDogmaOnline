using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestGetQuestScheduleInfoReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_GET_QUEST_SCHEDULE_INFO_REQ;

        public C2SQuestGetQuestScheduleInfoReq()
        {
        }

        public uint QuestScheduleId {  get; set; }

        public class Serializer : PacketEntitySerializer<C2SQuestGetQuestScheduleInfoReq>
        {
            public override void Write(IBuffer buffer, C2SQuestGetQuestScheduleInfoReq obj)
            {
                WriteUInt32(buffer, obj.QuestScheduleId);
            }

            public override C2SQuestGetQuestScheduleInfoReq Read(IBuffer buffer)
            {
                C2SQuestGetQuestScheduleInfoReq obj = new C2SQuestGetQuestScheduleInfoReq();
                obj.QuestScheduleId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
