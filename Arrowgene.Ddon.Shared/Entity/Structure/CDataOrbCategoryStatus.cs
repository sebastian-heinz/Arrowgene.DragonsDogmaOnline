using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataOrbCategoryStatus
    {
        public byte CategoryId;
        public byte ReleaseNum;

        public class Serializer : EntitySerializer<CDataOrbCategoryStatus>
        {
            public override void Write(IBuffer buffer, CDataOrbCategoryStatus obj)
            {
                WriteByte(buffer, obj.CategoryId);
                WriteByte(buffer, obj.ReleaseNum);
            }

            public override CDataOrbCategoryStatus Read(IBuffer buffer)
            {
                CDataOrbCategoryStatus obj = new CDataOrbCategoryStatus();
                obj.CategoryId = ReadByte(buffer);
                obj.ReleaseNum = ReadByte(buffer);
                return obj;
            }
        }
    }
}
