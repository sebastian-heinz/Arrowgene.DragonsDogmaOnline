using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCharacterEquipInfo
    {
        public CDataCharacterEquipInfo(CDataEquipItemInfo equipItemInfo)
        {
            EquipItemUId=equipItemInfo.ItemId.ToString(); // Perhaps?
            EquipCategory=equipItemInfo.Unk0; // Maybe?
            EquipType=equipItemInfo.EquipType;
        }

        public CDataCharacterEquipInfo()
        {
            EquipItemUId=string.Empty;
            EquipCategory=0;
            EquipType=0;
        }

        public string EquipItemUId { get; set; }
        public byte EquipCategory { get; set; }
        public byte EquipType { get; set; }

        public class Serializer : EntitySerializer<CDataCharacterEquipInfo>
        {
            public override void Write(IBuffer buffer, CDataCharacterEquipInfo obj)
            {
                WriteMtString(buffer, obj.EquipItemUId);
                WriteByte(buffer, obj.EquipCategory);
                WriteByte(buffer, obj.EquipType);
            }

            public override CDataCharacterEquipInfo Read(IBuffer buffer)
            {
                CDataCharacterEquipInfo obj = new CDataCharacterEquipInfo();
                obj.EquipItemUId = ReadMtString(buffer);
                obj.EquipCategory = ReadByte(buffer);
                obj.EquipType = ReadByte(buffer);
                return obj;
            }
        }
    }
}
