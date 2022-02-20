using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataItemList
    {
        public string Unk0 { get; set; }
        public uint Unk1 { get; set; }
        public uint Unk2 { get; set; }
        public byte Unk3 { get; set; } // EquipType?
        public byte Unk4 { get; set; } // EquipSlot?
        public ushort Unk5 { get; set; } // ItemID?
        public byte Unk6 { get; set; } // Color?
        public byte Unk7 { get; set; } // PlusValue?
        public bool Unk8 { get; set; }
        public uint Unk9 { get; set; }
        public uint Unk10 { get; set; }
        public uint Unk11 { get; set; }
        public List<CDataEquipElementUnkType2> Unk12 { get; set; } // Maybe weapon crests?
        public List<CDataEquipElementUnkType> Unk13 { get; set; } // Maybe armor crests?
        public List<CDataEquipElementParam> Unk14 { get; set; } // EquipElementParamList?
        
        public class Serializer : EntitySerializer<CDataItemList>
        {
            public override void Write(IBuffer buffer, CDataItemList obj)
            {
                WriteMtString(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
                WriteUInt32(buffer, obj.Unk2);
                WriteByte(buffer, obj.Unk3);
                WriteByte(buffer, obj.Unk4);
                WriteUInt16(buffer, obj.Unk5);
                WriteByte(buffer, obj.Unk6);
                WriteByte(buffer, obj.Unk7);
                WriteBool(buffer, obj.Unk8);
                WriteUInt32(buffer, obj.Unk9);
                WriteUInt32(buffer, obj.Unk10);
                WriteUInt32(buffer, obj.Unk11);
                WriteEntityList<CDataEquipElementUnkType2>(buffer, obj.Unk12);
                WriteEntityList<CDataEquipElementUnkType>(buffer, obj.Unk13);
                WriteEntityList<CDataEquipElementParam>(buffer, obj.Unk14);
            }

            public override CDataItemList Read(IBuffer buffer)
            {
                CDataItemList obj = new CDataItemList();
                obj.Unk0 = ReadMtString(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                obj.Unk2 = ReadUInt32(buffer);
                obj.Unk3 = ReadByte(buffer);
                obj.Unk4 = ReadByte(buffer);
                obj.Unk5 = ReadUInt16(buffer);
                obj.Unk6 = ReadByte(buffer);
                obj.Unk7 = ReadByte(buffer);
                obj.Unk8 = ReadBool(buffer);
                obj.Unk9 = ReadUInt32(buffer);
                obj.Unk10 = ReadUInt32(buffer);
                obj.Unk11 = ReadUInt32(buffer);
                obj.Unk12 = ReadEntityList<CDataEquipElementUnkType2>(buffer);
                obj.Unk13 = ReadEntityList<CDataEquipElementUnkType>(buffer);
                obj.Unk14 = ReadEntityList<CDataEquipElementParam>(buffer);
                return obj;
            }
        }
    }
}
