using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CInnStayInnRes : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_INN_STAY_INN_RES;

        public S2CInnStayInnRes()
        {
            Point = 0;
        }

        public C2SInnStayInnReq ReqData { get; set; }
        public uint Point { get; set; }
        public class Serializer : PacketEntitySerializer<S2CInnStayInnRes>
        {
            public override void Write(IBuffer buffer, S2CInnStayInnRes obj)
            {
                C2SInnStayInnReq req = obj.ReqData;
                WriteUInt64(buffer, 0);
                WriteByte(buffer, 1);
                WriteUInt32(buffer, obj.Point);
            }

            public override S2CInnStayInnRes Read(IBuffer buffer)
            {
                S2CInnStayInnRes obj = new S2CInnStayInnRes();
                return obj;
            }
        }

    }
}
