using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestQuestCancelNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_QUEST_QUEST_CANCEL_NTC; 


        public S2CQuestQuestCancelNtc()
        {
        }

        public UInt32 QuestScheduleId { get; set; }
        public UInt32 QuestId { get ; set; }
 
        public class Serializer : PacketEntitySerializer<S2CQuestQuestCancelNtc>
        {
            public override void Write(IBuffer buffer, S2CQuestQuestCancelNtc obj)
            {
                WriteUInt32(buffer, obj.QuestScheduleId);
                WriteUInt32(buffer, obj.QuestId);
            }

            public override S2CQuestQuestCancelNtc Read(IBuffer buffer)
            {
                S2CQuestQuestCancelNtc obj = new S2CQuestQuestCancelNtc();
                obj.QuestScheduleId = ReadUInt32(buffer);
                obj.QuestId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
