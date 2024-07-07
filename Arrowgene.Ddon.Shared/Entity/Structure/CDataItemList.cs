using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataItemList
    {
        public CDataItemList()
        {
            ItemUId = string.Empty;
            EquipElementParamList = new List<CDataEquipElementParam>();
            Unk1 = new List<CDataEquipItemInfoUnk1>();
            Unk2 = new List<CDataEquipItemInfoUnk2>();
        }

        public string ItemUId { get; set; }
        public uint ItemId { get; set; }
        public uint ItemNum { get; set; }
        public byte Unk3 { get; set; }
        public StorageType StorageType { get; set; }
        public ushort SlotNo { get; set; }
        public byte Color { get; set; }
        public byte PlusValue { get; set; }
        public bool Bind { get; set; }
        public uint EquipPoint { get; set; }
        public uint EquipCharacterID { get; set; }
        public uint EquipPawnID { get; set; }
        public List<CDataEquipElementParam> EquipElementParamList { get; set; }
        public List<CDataEquipItemInfoUnk1> Unk1 { get; set; }
        public List<CDataEquipItemInfoUnk2> Unk2 { get; set; }
        
        public class Serializer : EntitySerializer<CDataItemList>
        {
            public override void Write(IBuffer buffer, CDataItemList obj)
            {
                WriteMtString(buffer, obj.ItemUId);
                WriteUInt32(buffer, obj.ItemId);
                WriteUInt32(buffer, obj.ItemNum);
                WriteByte(buffer, obj.Unk3);
                WriteByte(buffer, (byte) obj.StorageType);
                WriteUInt16(buffer, obj.SlotNo);
                WriteByte(buffer, obj.Color);
                WriteByte(buffer, obj.PlusValue);
                WriteBool(buffer, obj.Bind);
                WriteUInt32(buffer, obj.EquipPoint);
                WriteUInt32(buffer, obj.EquipCharacterID);
                WriteUInt32(buffer, obj.EquipPawnID);
                WriteEntityList<CDataEquipElementParam>(buffer, obj.EquipElementParamList);
                WriteEntityList<CDataEquipItemInfoUnk1>(buffer, obj.Unk1);
                WriteEntityList<CDataEquipItemInfoUnk2>(buffer, obj.Unk2);
            }

            public override CDataItemList Read(IBuffer buffer)
            {
                CDataItemList obj = new CDataItemList();
                obj.ItemUId = ReadMtString(buffer);
                obj.ItemId = ReadUInt32(buffer);
                obj.ItemNum = ReadUInt32(buffer);
                obj.Unk3 = ReadByte(buffer);
                obj.StorageType = (StorageType) ReadByte(buffer);
                obj.SlotNo = ReadUInt16(buffer);
                obj.Color = ReadByte(buffer);
                obj.PlusValue = ReadByte(buffer);
                obj.Bind = ReadBool(buffer);
                obj.EquipPoint = ReadUInt32(buffer);
                obj.EquipCharacterID = ReadUInt32(buffer);
                obj.EquipPawnID = ReadUInt32(buffer);
                obj.EquipElementParamList = ReadEntityList<CDataEquipElementParam>(buffer);
                obj.Unk1 = ReadEntityList<CDataEquipItemInfoUnk1>(buffer);
                obj.Unk2 = ReadEntityList<CDataEquipItemInfoUnk2>(buffer);
                return obj;
            }
        }
    }
}
