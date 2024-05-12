using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CInnStayInnRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_INN_STAY_INN_RES;

        public WalletType PointType { get; set; }
        public uint Point { get; set; }

        public class Serializer : PacketEntitySerializer<S2CInnStayInnRes>
        {
            public override void Write(IBuffer buffer, S2CInnStayInnRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteByte(buffer, (byte) obj.PointType);
                WriteUInt32(buffer, obj.Point);
            }

            public override S2CInnStayInnRes Read(IBuffer buffer)
            {
                S2CInnStayInnRes obj = new S2CInnStayInnRes();
                ReadServerResponse(buffer, obj);
                obj.PointType = (WalletType) ReadByte(buffer);
                obj.Point = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}
