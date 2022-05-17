using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataJobBaseInfo
    {
        public JobId Job;
        public byte Level;

        public class Serializer : EntitySerializer<CDataJobBaseInfo>
        {
            public override void Write(IBuffer buffer, CDataJobBaseInfo obj)
            {
                WriteByte(buffer, (byte) obj.Job);
                WriteByte(buffer, obj.Level);
            }

            public override CDataJobBaseInfo Read(IBuffer buffer)
            {
                CDataJobBaseInfo obj = new CDataJobBaseInfo();
                obj.Job = (JobId) ReadByte(buffer);
                obj.Level = ReadByte(buffer);
                return obj;
            }
        }
    }

}
