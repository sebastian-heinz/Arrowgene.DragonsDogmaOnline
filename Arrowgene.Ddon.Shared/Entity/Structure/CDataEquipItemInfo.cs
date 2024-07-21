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
            WeaponCrestDataList = new List<CDataEquipElementParam>();
            ArmorCrestDataList = new List<CDataEquipItemInfoUnk1>();
            Unk2List = new List<CDataEquipItemInfoUnk2>();
        }

        public uint ItemId { get; set; }
        public byte Unk0 { get; set; } // Not stored in DB cause i dont know what its for
        public EquipType EquipType { get; set; } // 1 = Equipment, 2 = Visual
        public ushort EquipSlot { get; set; }
        public byte Color { get; set; }
        public byte PlusValue { get; set; }
        public List<CDataEquipElementParam> WeaponCrestDataList { get; set; } // Used for storing crest properties
        public List<CDataEquipItemInfoUnk1> ArmorCrestDataList { get; set; }
        public List<CDataEquipItemInfoUnk2> Unk2List { get; set; }
        
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
                WriteEntityList(buffer, obj.ArmorCrestDataList);
                WriteEntityList(buffer, obj.Unk2List);
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
                obj.WeaponCrestDataList = ReadEntityList<CDataEquipElementParam>(buffer);
                obj.ArmorCrestDataList = ReadEntityList<CDataEquipItemInfoUnk1>(buffer);
                obj.Unk2List = ReadEntityList<CDataEquipItemInfoUnk2>(buffer);
                return obj;
            }
        }
    }

}
