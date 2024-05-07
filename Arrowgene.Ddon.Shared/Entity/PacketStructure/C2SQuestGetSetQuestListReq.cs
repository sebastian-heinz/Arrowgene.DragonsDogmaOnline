using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestGetSetQuestListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_GET_SET_QUEST_LIST_REQ;

        public uint DistributeId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SQuestGetSetQuestListReq>
        {
            public override void Write(IBuffer buffer, C2SQuestGetSetQuestListReq obj)
            {
                WriteUInt32(buffer, obj.DistributeId);
            }

            public override C2SQuestGetSetQuestListReq Read(IBuffer buffer)
            {
                C2SQuestGetSetQuestListReq obj = new C2SQuestGetSetQuestListReq();
                obj.DistributeId = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}
