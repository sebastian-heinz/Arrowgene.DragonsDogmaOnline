using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataFurnitureLayout
    {
        public ItemId ItemID { get; set; }
        public uint OmID { get; set; }
        public byte LayoutID { get; set; }

        public class Serializer : EntitySerializer<CDataFurnitureLayout>
        {
            public override void Write(IBuffer buffer, CDataFurnitureLayout obj)
            {
                WriteUInt32(buffer, (uint)obj.ItemID);
                WriteUInt32(buffer, obj.OmID);
                WriteByte(buffer, obj.LayoutID);
            }

            public override CDataFurnitureLayout Read(IBuffer buffer)
            {
                CDataFurnitureLayout obj = new CDataFurnitureLayout();
                obj.ItemID = (ItemId)ReadUInt32(buffer);
                obj.OmID = ReadUInt32(buffer);
                obj.LayoutID = ReadByte(buffer);
                return obj;
            }
        }
    }
}
