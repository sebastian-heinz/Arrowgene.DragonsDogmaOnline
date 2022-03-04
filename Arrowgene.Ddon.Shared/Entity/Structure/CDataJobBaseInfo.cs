using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataJobBaseInfo
    {
        public byte Job;
        public byte Level;

        public class Serializer : EntitySerializer<CDataJobBaseInfo>
        {
            public override void Write(IBuffer buffer, CDataJobBaseInfo obj)
            {
                WriteByte(buffer, obj.Job);
                WriteByte(buffer, obj.Level);
            }

            public override CDataJobBaseInfo Read(IBuffer buffer)
            {
                CDataJobBaseInfo obj = new CDataJobBaseInfo();
                obj.Job = ReadByte(buffer);
                obj.Level = ReadByte(buffer);
                return obj;
            }
        }
    }

}
