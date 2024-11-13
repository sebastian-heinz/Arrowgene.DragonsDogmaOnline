using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataFurnitureLayout
    {
        public uint ItemID { get; set; }
        public uint OmID { get; set; }
        public byte LayoutId { get; set; }

        public class Serializer : EntitySerializer<CDataFurnitureLayout>
        {
            public override void Write(IBuffer buffer, CDataFurnitureLayout obj)
            {
                WriteUInt32(buffer, obj.ItemID);
                WriteUInt32(buffer, obj.OmID);
                WriteByte(buffer, obj.LayoutId);
            }

            public override CDataFurnitureLayout Read(IBuffer buffer)
            {
                CDataFurnitureLayout obj = new CDataFurnitureLayout();
                obj.ItemID = ReadUInt32(buffer);
                obj.OmID = ReadUInt32(buffer);
                obj.LayoutId = ReadByte(buffer);
                return obj;
            }
        }
    }
}
