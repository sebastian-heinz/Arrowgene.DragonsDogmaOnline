using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using System;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestCancelNavigationQuestRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_CANCEL_NAVIGATION_QUEST_RES;

        public S2CQuestCancelNavigationQuestRes()
        {
        }

        public uint QuestScheduleId { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestCancelNavigationQuestRes>
        {
            public override void Write(IBuffer buffer, S2CQuestCancelNavigationQuestRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.QuestScheduleId);
            }

            public override S2CQuestCancelNavigationQuestRes Read(IBuffer buffer)
            {
                S2CQuestCancelNavigationQuestRes obj = new S2CQuestCancelNavigationQuestRes();
                ReadServerResponse(buffer, obj);
                obj.QuestScheduleId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

