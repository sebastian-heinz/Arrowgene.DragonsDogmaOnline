using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataPawnExpeditionInfo
    {
        public PawnExpeditionStatus SallyStatus { get; set; }
        public byte GoldenSallyPrice { get; set; }
        public byte ChargeSallyPrice { get; set; }

        public class Serializer : EntitySerializer<CDataPawnExpeditionInfo>
        {
            public override void Write(IBuffer buffer, CDataPawnExpeditionInfo obj)
            {
                WriteByte(buffer, (byte)obj.SallyStatus);
                WriteByte(buffer, obj.GoldenSallyPrice);
                WriteByte(buffer, obj.ChargeSallyPrice);

            }

            public override CDataPawnExpeditionInfo Read(IBuffer buffer)
            {
                CDataPawnExpeditionInfo obj = new CDataPawnExpeditionInfo();
                obj.SallyStatus = (PawnExpeditionStatus)ReadByte(buffer);
                obj.GoldenSallyPrice = ReadByte(buffer);
                obj.ChargeSallyPrice = ReadByte(buffer);
                return obj;
            }
        }
    }
}
