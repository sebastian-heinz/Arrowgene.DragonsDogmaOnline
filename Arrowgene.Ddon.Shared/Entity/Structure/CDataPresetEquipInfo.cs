using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataPresetEquipInfo
    {
        public CDataPresetEquipInfo()
        {
            ItemUId = string.Empty;
            EquipElementParamList = new List<CDataEquipElementParam>();
            Unk0List = new List<CDatapresetEquipUnk0>();
        }

        public string ItemUId { get; set; }
        public uint ItemId { get; set; }
        public byte EquipSlotNo { get; set; }
        public byte Color { get; set; }
        public List<CDataEquipElementParam> EquipElementParamList { get; set; }
        public List<CDatapresetEquipUnk0> Unk0List { get; set; } // Emblem?
        public byte PlusValue { get; set; }


        public class Serializer : EntitySerializer<CDataPresetEquipInfo>
        {
            public override void Write(IBuffer buffer, CDataPresetEquipInfo obj)
            {
                WriteMtString(buffer, obj.ItemUId);
                WriteUInt32(buffer, obj.ItemId);
                WriteByte(buffer, obj.EquipSlotNo);
                WriteByte(buffer, obj.Color);
                WriteEntityList(buffer, obj.EquipElementParamList);
                WriteEntityList(buffer, obj.Unk0List);
                WriteByte(buffer, obj.PlusValue);
            }

            public override CDataPresetEquipInfo Read(IBuffer buffer)
            {
                CDataPresetEquipInfo obj = new CDataPresetEquipInfo();
                obj.ItemUId = ReadMtString(buffer);
                obj.ItemId = ReadUInt32(buffer);
                obj.EquipSlotNo = ReadByte(buffer);
                obj.Color = ReadByte(buffer);
                obj.EquipElementParamList = ReadEntityList<CDataEquipElementParam>(buffer);
                obj.Unk0List = ReadEntityList<CDatapresetEquipUnk0>(buffer);
                obj.PlusValue = ReadByte(buffer);
                return obj;
            }
        }
    }
}
