using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestQuestOrderReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_QUEST_ORDER_REQ;

        public uint QuestScheduleId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SQuestQuestOrderReq>
        {
            public override void Write(IBuffer buffer, C2SQuestQuestOrderReq obj)
            {
                WriteUInt32(buffer, obj.QuestScheduleId);
            }

            public override C2SQuestQuestOrderReq Read(IBuffer buffer)
            {
                C2SQuestQuestOrderReq obj = new C2SQuestQuestOrderReq();
                obj.QuestScheduleId = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}