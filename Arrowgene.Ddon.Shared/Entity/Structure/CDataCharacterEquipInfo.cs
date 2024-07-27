using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCharacterEquipInfo
    {
        public CDataCharacterEquipInfo()
        {
            EquipItemUId=string.Empty;
            EquipCategory=0;
            EquipType=0;
        }

        public string EquipItemUId { get; set; }
        public byte EquipCategory { get; set; } // Slot
        public EquipType EquipType { get; set; }

        public class Serializer : EntitySerializer<CDataCharacterEquipInfo>
        {
            public override void Write(IBuffer buffer, CDataCharacterEquipInfo obj)
            {
                WriteMtString(buffer, obj.EquipItemUId);
                WriteByte(buffer, obj.EquipCategory);
                WriteByte(buffer, (byte) obj.EquipType);
            }

            public override CDataCharacterEquipInfo Read(IBuffer buffer)
            {
                CDataCharacterEquipInfo obj = new CDataCharacterEquipInfo();
                obj.EquipItemUId = ReadMtString(buffer);
                obj.EquipCategory = ReadByte(buffer);
                obj.EquipType = (EquipType) ReadByte(buffer);
                return obj;
            }
        }
    }
}
