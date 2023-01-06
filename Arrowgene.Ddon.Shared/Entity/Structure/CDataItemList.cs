using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataItemList
    {
        public CDataItemList()
        {
            ItemUId = string.Empty;
            WeaponCrestDataList = new List<CDataWeaponCrestData>();
            ArmorCrestDataList = new List<CDataArmorCrestData>();
            EquipElementParamList = new List<CDataEquipElementParam>();
        }

        public string ItemUId { get; set; }
        public uint ItemId { get; set; }
        public uint ItemNum { get; set; }
        public byte Unk3 { get; set; }
        public byte StorageType { get; set; }
        public ushort SlotNo { get; set; }
        public byte Color { get; set; }
        public byte PlusValue { get; set; }
        public bool Bind { get; set; }
        public uint EquipPoint { get; set; }
        public uint EquipCharacterID { get; set; }
        public uint EquipPawnID { get; set; }
        public List<CDataWeaponCrestData> WeaponCrestDataList { get; set; }
        public List<CDataArmorCrestData> ArmorCrestDataList { get; set; }
        public List<CDataEquipElementParam> EquipElementParamList { get; set; }
        
        public class Serializer : EntitySerializer<CDataItemList>
        {
            public override void Write(IBuffer buffer, CDataItemList obj)
            {
                WriteMtString(buffer, obj.ItemUId);
                WriteUInt32(buffer, obj.ItemId);
                WriteUInt32(buffer, obj.ItemNum);
                WriteByte(buffer, obj.Unk3);
                WriteByte(buffer, obj.StorageType);
                WriteUInt16(buffer, obj.SlotNo);
                WriteByte(buffer, obj.Color);
                WriteByte(buffer, obj.PlusValue);
                WriteBool(buffer, obj.Bind);
                WriteUInt32(buffer, obj.EquipPoint);
                WriteUInt32(buffer, obj.EquipCharacterID);
                WriteUInt32(buffer, obj.EquipPawnID);
                WriteEntityList<CDataWeaponCrestData>(buffer, obj.WeaponCrestDataList);
                WriteEntityList<CDataArmorCrestData>(buffer, obj.ArmorCrestDataList);
                WriteEntityList<CDataEquipElementParam>(buffer, obj.EquipElementParamList);
            }

            public override CDataItemList Read(IBuffer buffer)
            {
                CDataItemList obj = new CDataItemList();
                obj.ItemUId = ReadMtString(buffer);
                obj.ItemId = ReadUInt32(buffer);
                obj.ItemNum = ReadUInt32(buffer);
                obj.Unk3 = ReadByte(buffer);
                obj.StorageType = ReadByte(buffer);
                obj.SlotNo = ReadUInt16(buffer);
                obj.Color = ReadByte(buffer);
                obj.PlusValue = ReadByte(buffer);
                obj.Bind = ReadBool(buffer);
                obj.EquipPoint = ReadUInt32(buffer);
                obj.EquipCharacterID = ReadUInt32(buffer);
                obj.EquipPawnID = ReadUInt32(buffer);
                obj.WeaponCrestDataList = ReadEntityList<CDataWeaponCrestData>(buffer);
                obj.ArmorCrestDataList = ReadEntityList<CDataArmorCrestData>(buffer);
                obj.EquipElementParamList = ReadEntityList<CDataEquipElementParam>(buffer);
                return obj;
            }
        }
    }
}
