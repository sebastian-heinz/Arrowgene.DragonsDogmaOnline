using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestQuestCancelRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_QUEST_CANCEL_RES;

        public S2CQuestQuestCancelRes()
        {
        }

        public UInt32 QuestScheduleId { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestQuestCancelRes>
        {
            public override void Write(IBuffer buffer, S2CQuestQuestCancelRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.QuestScheduleId);
            }

            public override S2CQuestQuestCancelRes Read(IBuffer buffer)
            {
                S2CQuestQuestCancelRes obj = new S2CQuestQuestCancelRes();
                ReadServerResponse(buffer, obj);
                obj.QuestScheduleId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

