using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataDispelItemCategoryInfo
    {
        public CDataDispelItemCategoryInfo()
        {
            CategoryName = "";
        }

        public uint Category { get; set; }
        public string CategoryName { get; set; }

        public class Serializer : EntitySerializer<CDataDispelItemCategoryInfo>
        {
            public override void Write(IBuffer buffer, CDataDispelItemCategoryInfo obj)
            {
                WriteUInt32(buffer, obj.Category);
                WriteMtString(buffer, obj.CategoryName);
            }

            public override CDataDispelItemCategoryInfo Read(IBuffer buffer)
            {
                CDataDispelItemCategoryInfo obj = new CDataDispelItemCategoryInfo();
                obj.Category = ReadUInt32(buffer);
                obj.CategoryName = ReadMtString(buffer);
                return obj;
            }
        }
    }
}
