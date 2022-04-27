using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnGetLostPawnListRes : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_PAWN_GET_LOST_PAWN_LIST_RES;

        public S2CPawnGetLostPawnListRes()
        {
        }

        public C2SPawnGetLostPawnListReq ReqData { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPawnGetLostPawnListRes>
        {
            public override void Write(IBuffer buffer, S2CPawnGetLostPawnListRes obj)
            {
                WriteByteArray(buffer, Data);
            }

            public override S2CPawnGetLostPawnListRes Read(IBuffer buffer)
            {
                S2CPawnGetLostPawnListRes obj = new S2CPawnGetLostPawnListRes();
                return obj;
            }


            private readonly byte[] Data =
            {
                0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x1,
                0x0, 0x0, 0x43, 0xDA, 0x0, 0x0, 0x0
            };
        }

    }
}
