using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestSetNavigationQuestRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_SET_NAVIGATION_QUEST_RES;

        public uint QuestScheduleId { get; set; }
        public bool IsBase { get; set; } // If set to false, prints message about must set at WDT or Area Master Location

        public class Serializer : PacketEntitySerializer<S2CQuestSetNavigationQuestRes>
        {
            public override void Write(IBuffer buffer, S2CQuestSetNavigationQuestRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.QuestScheduleId);
                WriteBool(buffer, obj.IsBase);
            }

            public override S2CQuestSetNavigationQuestRes Read(IBuffer buffer)
            {
                S2CQuestSetNavigationQuestRes obj = new S2CQuestSetNavigationQuestRes();
                ReadServerResponse(buffer, obj);
                obj.QuestScheduleId = ReadUInt32(buffer);
                obj.IsBase = ReadBool(buffer);
                return obj;
            }
        }
    }
}
