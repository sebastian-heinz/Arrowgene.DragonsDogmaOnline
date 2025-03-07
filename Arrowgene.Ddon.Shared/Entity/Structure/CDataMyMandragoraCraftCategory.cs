using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMyMandragoraCraftCategory
    {
        public byte CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        public class Serializer : EntitySerializer<CDataMyMandragoraCraftCategory>
        {
            public override void Write(IBuffer buffer, CDataMyMandragoraCraftCategory obj)
            {
                WriteByte(buffer, obj.CategoryId);
                WriteMtString(buffer, obj.CategoryName);
            }

            public override CDataMyMandragoraCraftCategory Read(IBuffer buffer)
            {
                CDataMyMandragoraCraftCategory obj = new CDataMyMandragoraCraftCategory();
                obj.CategoryId = ReadByte(buffer);
                obj.CategoryName = ReadMtString(buffer);
                return obj;
            }
        }
    }
}
