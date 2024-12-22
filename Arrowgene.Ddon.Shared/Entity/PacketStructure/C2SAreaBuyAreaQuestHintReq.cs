using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SAreaBuyAreaQuestHintReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_AREA_BUY_AREA_QUEST_HINT_REQ;

        public uint AreaId { get; set; }
        public uint QuestScheduleId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SAreaBuyAreaQuestHintReq>
        {
            public override void Write(IBuffer buffer, C2SAreaBuyAreaQuestHintReq obj)
            {
                WriteUInt32(buffer, obj.AreaId);
                WriteUInt32(buffer, obj.QuestScheduleId);
            }

            public override C2SAreaBuyAreaQuestHintReq Read(IBuffer buffer)
            {
                C2SAreaBuyAreaQuestHintReq obj = new C2SAreaBuyAreaQuestHintReq();
                obj.AreaId = ReadUInt32(buffer);
                obj.QuestScheduleId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
