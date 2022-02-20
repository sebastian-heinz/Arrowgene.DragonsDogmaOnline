using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCharacterEquipInfo
    {
        public CDataCharacterEquipInfo(CDataEquipItemInfo equipItemInfo)
        {
            EquipItemUID=equipItemInfo.ItemID.ToString(); // Perhaps?
            EquipCategory=equipItemInfo.EquipSlot; // Maybe?
            EquipType=equipItemInfo.EquipType;
        }

        public CDataCharacterEquipInfo()
        {
            EquipItemUID=string.Empty;
            EquipCategory=0;
            EquipType=0;
        }

        public string EquipItemUID { get; set; }
        public byte EquipCategory { get; set; }
        public byte EquipType { get; set; }

        public class Serializer : EntitySerializer<CDataCharacterEquipInfo>
        {
            public override void Write(IBuffer buffer, CDataCharacterEquipInfo obj)
            {
                WriteMtString(buffer, obj.EquipItemUID);
                WriteByte(buffer, obj.EquipCategory);
                WriteByte(buffer, obj.EquipType);
            }

            public override CDataCharacterEquipInfo Read(IBuffer buffer)
            {
                CDataCharacterEquipInfo obj = new CDataCharacterEquipInfo();
                obj.EquipItemUID = ReadMtString(buffer);
                obj.EquipCategory = ReadByte(buffer);
                obj.EquipType = ReadByte(buffer);
                return obj;
            }
        }
    }
}
