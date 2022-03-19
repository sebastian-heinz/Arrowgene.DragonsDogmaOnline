using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataOcdActive
    {
        public CDataOcdActive()
        {
            OcdUId=0;
            OcdActiveLv=0;
        }

        public byte OcdUId { get; set; }
        public byte OcdActiveLv { get; set; }

        public class Serializer : EntitySerializer<CDataOcdActive>
        {
            public override void Write(IBuffer buffer, CDataOcdActive obj)
            {
                WriteByte(buffer, obj.OcdUId);
                WriteByte(buffer, obj.OcdActiveLv);
            }

            public override CDataOcdActive Read(IBuffer buffer)
            {
                CDataOcdActive obj = new CDataOcdActive();
                obj.OcdUId = ReadByte(buffer);
                obj.OcdActiveLv = ReadByte(buffer);
                return obj;
            }
        }
    }
}
