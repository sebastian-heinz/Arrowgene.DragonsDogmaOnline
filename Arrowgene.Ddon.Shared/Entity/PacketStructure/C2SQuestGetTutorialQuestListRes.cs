using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestGetTutorialQuestListRes : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_GET_TUTORIAL_QUEST_LIST_REQ;

        public uint StageNo { get; set; }

        public C2SQuestGetTutorialQuestListRes()
        {
            StageNo = 0;
        }

        public class Serializer : PacketEntitySerializer<C2SQuestGetTutorialQuestListRes>
        {
            public override void Write(IBuffer buffer, C2SQuestGetTutorialQuestListRes obj)
            {
                WriteUInt32(buffer, obj.StageNo);
            }

            public override C2SQuestGetTutorialQuestListRes Read(IBuffer buffer)
            {
                C2SQuestGetTutorialQuestListRes obj = new C2SQuestGetTutorialQuestListRes();
                obj.StageNo = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
