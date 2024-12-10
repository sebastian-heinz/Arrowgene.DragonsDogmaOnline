using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestGetQuestScheduleInfoRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_GET_QUEST_SCHEDULE_INFO_RES;

        public uint QuestId { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestGetQuestScheduleInfoRes>
        {
            public override void Write(IBuffer buffer, S2CQuestGetQuestScheduleInfoRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.QuestId);
            }

            public override S2CQuestGetQuestScheduleInfoRes Read(IBuffer buffer)
            {
                S2CQuestGetQuestScheduleInfoRes obj = new S2CQuestGetQuestScheduleInfoRes();
                ReadServerResponse(buffer, obj);
                obj.QuestId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
