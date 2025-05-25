using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataJobEmblemPoints
    {
        public JobId JobId { get; set; }
        public ushort Amount { get; set; }
        public ushort MaxAmount { get; set; }

        public class Serializer : EntitySerializer<CDataJobEmblemPoints>
        {
            public override void Write(IBuffer buffer, CDataJobEmblemPoints obj)
            {
                WriteByte(buffer, (byte) obj.JobId);
                WriteUInt16(buffer, obj.Amount);
                WriteUInt16(buffer, obj.MaxAmount);
            }

            public override CDataJobEmblemPoints Read(IBuffer buffer)
            {
                CDataJobEmblemPoints obj = new CDataJobEmblemPoints();
                obj.JobId = (JobId) ReadByte(buffer);
                obj.Amount = ReadUInt16(buffer);
                obj.MaxAmount = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}
