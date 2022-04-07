using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestGetLotQuestListRes : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_QUEST_GET_LOT_QUEST_LIST_RES;

        public S2CQuestGetLotQuestListRes()
        {
        }

        public C2SQuestGetLotQuestListReq ReqData { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestGetLotQuestListRes>
        {
            public override void Write(IBuffer buffer, S2CQuestGetLotQuestListRes obj)
            {
                WriteByteArray(buffer, Data);
            }

            public override S2CQuestGetLotQuestListRes Read(IBuffer buffer)
            {
                S2CQuestGetLotQuestListRes obj = new S2CQuestGetLotQuestListRes();
                return obj;
            }


            private readonly byte[] Data =
            {
                0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x1, 0x0, 0x0, 0x0, 0x0,
                0x0, 0x1, 0x31, 0x7A, 0x40, 0x1, 0x0
            };
        }

    }
}
