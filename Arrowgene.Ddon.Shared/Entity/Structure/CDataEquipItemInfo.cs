using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataEquipItemInfo
    {
        public CDataEquipItemInfo()
        {
            U0 = 0;
            EquipType = 0;
            EquipSlot = 0;
            ItemID = 0;
            Color = 0;
            PlusValue = 0;
            U6 = new List<CDataEquipElementUnkType2>();
            U7 = new List<CDataEquipElementUnkType>();
            EquipElementParamList = new List<CDataEquipElementParam>();
        }

        public uint U0 { get; set; } // Probably the preset? 0 being normal equipment, 1 alt, 2+ presets
        public byte EquipType { get; set; }
        public byte EquipSlot { get; set; }
        public ushort ItemID { get; set; }
        public byte Color { get; set; }
        public byte PlusValue { get; set; }
        public List<CDataEquipElementUnkType2> U6 { get; set; } // Maybe weapon crests
        public List<CDataEquipElementUnkType> U7 { get; set; } // Maybe armor crests
        public List<CDataEquipElementParam> EquipElementParamList { get; set; } // Not 100% sure about this one
        
        public class Serializer : EntitySerializer<CDataEquipItemInfo>
        {
            public override void Write(IBuffer buffer, CDataEquipItemInfo obj)
            {
                WriteUInt32(buffer, obj.U0);
                WriteByte(buffer, obj.EquipType);
                WriteByte(buffer, obj.EquipSlot);
                WriteUInt16(buffer, obj.ItemID);
                WriteByte(buffer, obj.Color);
                WriteByte(buffer, obj.PlusValue);
                WriteEntityList(buffer, obj.U6);
                WriteEntityList(buffer, obj.U7);
                WriteEntityList(buffer, obj.EquipElementParamList);
            }

            public override CDataEquipItemInfo Read(IBuffer buffer)
            {
                CDataEquipItemInfo obj = new CDataEquipItemInfo();
                obj.U0 = ReadUInt32(buffer);
                obj.EquipType = ReadByte(buffer);
                obj.EquipSlot = ReadByte(buffer);
                obj.ItemID = ReadUInt16(buffer);
                obj.Color = ReadByte(buffer);
                obj.PlusValue = ReadByte(buffer);
                obj.U6 = ReadEntityList<CDataEquipElementUnkType2>(buffer);
                obj.U7 = ReadEntityList<CDataEquipElementUnkType>(buffer);
                obj.EquipElementParamList = ReadEntityList<CDataEquipElementParam>(buffer);
                return obj;
            }
        }
    }

}
