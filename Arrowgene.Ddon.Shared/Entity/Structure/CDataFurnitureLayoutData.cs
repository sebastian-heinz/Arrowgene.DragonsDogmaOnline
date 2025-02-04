using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataFurnitureLayoutData
    {
        public uint ItemID { get; set; }
        public ClanBaseCustomizationType LayoutId { get; set; }

        public class Serializer : EntitySerializer<CDataFurnitureLayoutData>
        {
            public override void Write(IBuffer buffer, CDataFurnitureLayoutData obj)
            {
                WriteUInt32(buffer, obj.ItemID);
                WriteByte(buffer, (byte)obj.LayoutId);
            }

            public override CDataFurnitureLayoutData Read(IBuffer buffer)
            {
                CDataFurnitureLayoutData obj = new CDataFurnitureLayoutData();
                obj.ItemID = ReadUInt32(buffer);
                obj.LayoutId = (ClanBaseCustomizationType)ReadByte(buffer);
                return obj;
            }
        }
    }
}
