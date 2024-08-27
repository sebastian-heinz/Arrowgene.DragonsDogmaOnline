using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestGetSetQuestInfoListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_GET_SET_QUEST_INFO_LIST_REQ;

        public C2SQuestGetSetQuestInfoListReq()
        {
        }

        public QuestAreaId DistributeId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SQuestGetSetQuestInfoListReq>
        {
            public override void Write(IBuffer buffer, C2SQuestGetSetQuestInfoListReq obj)
            {
                WriteUInt32(buffer, (uint)obj.DistributeId);
            }

            public override C2SQuestGetSetQuestInfoListReq Read(IBuffer buffer)
            {
                C2SQuestGetSetQuestInfoListReq obj = new C2SQuestGetSetQuestInfoListReq();
                obj.DistributeId = (QuestAreaId)ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
