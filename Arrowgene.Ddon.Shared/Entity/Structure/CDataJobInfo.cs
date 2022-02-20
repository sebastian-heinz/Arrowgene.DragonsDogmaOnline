using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataJobInfo
    {
        public byte job;
        public byte level;

        public class Serializer : EntitySerializer<CDataJobInfo>
        {
            public override void Write(IBuffer buffer, CDataJobInfo obj)
            {
                WriteByte(buffer, obj.job);
                WriteByte(buffer, obj.level);
            }

            public override CDataJobInfo Read(IBuffer buffer)
            {
                CDataJobInfo obj = new CDataJobInfo();
                obj.job = ReadByte(buffer);
                obj.level = ReadByte(buffer);
                return obj;
            }
        }
    }

}
