using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataJewelryEquipLimit
    {
        public byte JewelryType { get; set; }
        public byte MaxNum { get; set; }

        public class Serializer : EntitySerializer<CDataJewelryEquipLimit>
        {
            public override void Write(IBuffer buffer, CDataJewelryEquipLimit obj)
            {
                WriteByte(buffer, obj.JewelryType);
                WriteByte(buffer, obj.MaxNum);
            }

            public override CDataJewelryEquipLimit Read(IBuffer buffer)
            {
                CDataJewelryEquipLimit obj = new CDataJewelryEquipLimit();
                obj.JewelryType = ReadByte(buffer);
                obj.MaxNum = ReadByte(buffer);
                return obj;
            }
        }
    }
}
