using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    // This is very different from the PS4, everything could be wrong
    public class CDataEquipItemInfo
    {
        public CDataEquipItemInfo()
        {
            ItemId = 0;
            Unk0 = 0;
            EquipType = 0;
            EquipSlot = 0;
            Color = 0;
            PlusValue = 0;
            WeaponCrestDataList = new List<CDataWeaponCrestData>();
            AddStatusParamList = new List<CDataAddStatusParam>();
            EquipElementParamList = new List<CDataEquipElementParam>();
        }

        public uint ItemId { get; set; }
        public byte Unk0 { get; set; } // Not stored in DB cause i dont know what its for
        public EquipType EquipType { get; set; } // 1 = Equipment, 2 = Visual
        public ushort EquipSlot { get; set; }
        public byte Color { get; set; }
        public byte PlusValue { get; set; }
        public List<CDataWeaponCrestData> WeaponCrestDataList { get; set; }
        public List<CDataAddStatusParam> AddStatusParamList { get; set; }
        public List<CDataEquipElementParam> EquipElementParamList { get; set; }
        
        public class Serializer : EntitySerializer<CDataEquipItemInfo>
        {
            public override void Write(IBuffer buffer, CDataEquipItemInfo obj)
            {
                WriteUInt32(buffer, obj.ItemId);
                WriteByte(buffer, obj.Unk0);
                WriteByte(buffer, (byte) obj.EquipType);
                WriteUInt16(buffer, obj.EquipSlot);
                WriteByte(buffer, obj.Color);
                WriteByte(buffer, obj.PlusValue);
                WriteEntityList(buffer, obj.WeaponCrestDataList);
                WriteEntityList(buffer, obj.AddStatusParamList);
                WriteEntityList(buffer, obj.EquipElementParamList);
            }

            public override CDataEquipItemInfo Read(IBuffer buffer)
            {
                CDataEquipItemInfo obj = new CDataEquipItemInfo();
                obj.ItemId = ReadUInt32(buffer);
                obj.Unk0 = ReadByte(buffer);
                obj.EquipType = (EquipType) ReadByte(buffer);
                obj.EquipSlot = ReadUInt16(buffer);
                obj.Color = ReadByte(buffer);
                obj.PlusValue = ReadByte(buffer);
                obj.WeaponCrestDataList = ReadEntityList<CDataWeaponCrestData>(buffer);
                obj.AddStatusParamList = ReadEntityList<CDataAddStatusParam>(buffer);
                obj.EquipElementParamList = ReadEntityList<CDataEquipElementParam>(buffer);
                return obj;
            }
        }
    }

}
