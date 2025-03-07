using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestCancelNavigationQuestReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_CANCEL_NAVIGATION_QUEST_REQ;

        public C2SQuestCancelNavigationQuestReq()
        {
        }

        public uint QuestScheduleId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SQuestCancelNavigationQuestReq>
        {
            public override void Write(IBuffer buffer, C2SQuestCancelNavigationQuestReq obj)
            {
                WriteUInt32(buffer, obj.QuestScheduleId);
            }

            public override C2SQuestCancelNavigationQuestReq Read(IBuffer buffer)
            {
                C2SQuestCancelNavigationQuestReq obj = new C2SQuestCancelNavigationQuestReq();
                obj.QuestScheduleId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

