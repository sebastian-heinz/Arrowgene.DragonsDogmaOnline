using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestGetTutorialQuestListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_GET_TUTORIAL_QUEST_LIST_REQ;

        public uint StageNo { get; set; }

        public C2SQuestGetTutorialQuestListReq()
        {
            StageNo = 0;
        }

        public class Serializer : PacketEntitySerializer<C2SQuestGetTutorialQuestListReq>
        {
            public override void Write(IBuffer buffer, C2SQuestGetTutorialQuestListReq obj)
            {
                WriteUInt32(buffer, obj.StageNo);
            }

            public override C2SQuestGetTutorialQuestListReq Read(IBuffer buffer)
            {
                C2SQuestGetTutorialQuestListReq obj = new C2SQuestGetTutorialQuestListReq();
                obj.StageNo = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
