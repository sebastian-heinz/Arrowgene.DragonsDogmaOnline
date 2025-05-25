using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataDispelResultInfo
    {
        public CDataDispelResultInfo()
        {
            EquipElementParamList = new List<CDataEquipElementParam>();
        }

        public uint ItemId { get; set; }
        public uint ItemNum { get; set; }
        public byte Color {  get; set; }
        public byte Plus {  get; set; } // +0, +1, +2, +3
        public List<CDataEquipElementParam> EquipElementParamList { get; set; }
        public byte Unk0 { get; set; } // Safety Setting?
        public uint Unk1 { get; set; } // Equip Points?
        public uint Unk2 { get; set; } // Equip Points?

        public class Serializer : EntitySerializer<CDataDispelResultInfo>
        {
            public override void Write(IBuffer buffer, CDataDispelResultInfo obj)
            {
                WriteUInt32(buffer, obj.ItemId);
                WriteUInt32(buffer, obj.ItemNum);
                WriteByte(buffer, obj.Color);
                WriteByte(buffer, obj.Plus);
                WriteEntityList(buffer, obj.EquipElementParamList);
                WriteByte(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
                WriteUInt32(buffer, obj.Unk2);
            }

            public override CDataDispelResultInfo Read(IBuffer buffer)
            {
                CDataDispelResultInfo obj = new CDataDispelResultInfo();
                obj.ItemId = ReadUInt32(buffer);
                obj.ItemNum = ReadUInt32(buffer);
                obj.Color = ReadByte(buffer);
                obj.Plus = ReadByte(buffer);
                obj.EquipElementParamList = ReadEntityList<CDataEquipElementParam>(buffer);
                obj.Unk0 = ReadByte(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                obj.Unk2 = ReadUInt32(buffer);
                return obj;
            }
        }

        public Item ToItem()
        {
            return new Item()
            {
                ItemId = ItemId,
                Color = Color,
                PlusValue = Plus,
                EquipElementParamList = EquipElementParamList,
            };
        }
    }
}


