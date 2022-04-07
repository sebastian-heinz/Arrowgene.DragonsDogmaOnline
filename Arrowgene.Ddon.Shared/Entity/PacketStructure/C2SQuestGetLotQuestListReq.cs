using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestGetLotQuestListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_GET_LOT_QUEST_LIST_REQ;

        public uint Data0 { get; set; }

        public C2SQuestGetLotQuestListReq()
        {
            Data0 = 0;
        }

        public class Serializer : PacketEntitySerializer<C2SQuestGetLotQuestListReq>
        {
            public override void Write(IBuffer buffer, C2SQuestGetLotQuestListReq obj)
            {
                WriteUInt32(buffer, obj.Data0);
            }

            public override C2SQuestGetLotQuestListReq Read(IBuffer buffer)
            {
                C2SQuestGetLotQuestListReq obj = new C2SQuestGetLotQuestListReq();
                obj.Data0 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
