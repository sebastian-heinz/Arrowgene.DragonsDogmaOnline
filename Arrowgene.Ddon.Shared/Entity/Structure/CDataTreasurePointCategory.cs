using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataTreasurePointCategory
    {
        public uint CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public byte EntryCount { get; set; }

        public class Serializer : EntitySerializer<CDataTreasurePointCategory>
        {
            public override void Write(IBuffer buffer, CDataTreasurePointCategory obj)
            {
                WriteUInt32(buffer, obj.CategoryId);
                WriteMtString(buffer, obj.CategoryName);
                WriteByte(buffer, obj.EntryCount);
            }

            public override CDataTreasurePointCategory Read(IBuffer buffer)
            {
                CDataTreasurePointCategory obj = new CDataTreasurePointCategory();
                obj.CategoryId = ReadUInt32(buffer);
                obj.CategoryName = ReadMtString(buffer);
                obj.EntryCount = ReadByte(buffer);
                return obj;
            }
        }
    }
}
