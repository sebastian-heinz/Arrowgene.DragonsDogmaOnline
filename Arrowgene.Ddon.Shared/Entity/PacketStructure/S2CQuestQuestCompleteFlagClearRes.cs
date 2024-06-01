using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestQuestCompleteFlagClearRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_QUEST_COMPLETE_FLAG_CLEAR_RES;

        public S2CQuestQuestCompleteFlagClearRes()
        {
        }

        public UInt32 QuestScheduleId { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestQuestCompleteFlagClearRes>
        {
            public override void Write(IBuffer buffer, S2CQuestQuestCompleteFlagClearRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.QuestScheduleId);
            }

            public override S2CQuestQuestCompleteFlagClearRes Read(IBuffer buffer)
            {
                S2CQuestQuestCompleteFlagClearRes obj = new S2CQuestQuestCompleteFlagClearRes();
                ReadServerResponse(buffer, obj);
                obj.QuestScheduleId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
