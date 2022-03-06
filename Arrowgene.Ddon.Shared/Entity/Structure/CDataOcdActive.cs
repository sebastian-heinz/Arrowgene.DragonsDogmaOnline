using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataOcdActive
    {
        public CDataOcdActive()
        {
            OcdUID=0;
            OcdActiveLv=0;
        }

        public byte OcdUID { get; set; }
        public byte OcdActiveLv { get; set; }

        public class Serializer : EntitySerializer<CDataOcdActive>
        {
            public override void Write(IBuffer buffer, CDataOcdActive obj)
            {
                WriteByte(buffer, obj.OcdUID);
                WriteByte(buffer, obj.OcdActiveLv);
            }

            public override CDataOcdActive Read(IBuffer buffer)
            {
                CDataOcdActive obj = new CDataOcdActive();
                obj.OcdUID = ReadByte(buffer);
                obj.OcdActiveLv = ReadByte(buffer);
                return obj;
            }
        }
    }
}