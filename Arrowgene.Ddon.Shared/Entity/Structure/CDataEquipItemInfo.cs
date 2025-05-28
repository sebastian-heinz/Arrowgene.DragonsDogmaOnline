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
            SafetySetting = 0;
            EquipType = 0;
            EquipSlot = 0;
            Color = 0;
            PlusValue = 0;
            EquipElementParamList = new List<CDataEquipElementParam>();
            AddStatusParamList = new List<CDataAddStatusParam>();
            EquipStatParamList = new List<CDataEquipStatParam>();
        }

        public uint ItemId { get; set; }
        public byte SafetySetting { get; set; } // Not stored in DB cause i dont know what its for
        public EquipType EquipType { get; set; } // 1 = Equipment, 2 = Visual
        public ushort EquipSlot { get; set; }
        public byte Color { get; set; }
        public byte PlusValue { get; set; }
        public List<CDataEquipElementParam> EquipElementParamList { get; set; } // Used for storing crest properties
        public List<CDataAddStatusParam> AddStatusParamList { get; set; }
        public List<CDataEquipStatParam> EquipStatParamList { get; set; }
        
        public class Serializer : EntitySerializer<CDataEquipItemInfo>
        {
            public override void Write(IBuffer buffer, CDataEquipItemInfo obj)
            {
                WriteUInt32(buffer, obj.ItemId);
                WriteByte(buffer, obj.SafetySetting);
                WriteByte(buffer, (byte) obj.EquipType);
                WriteUInt16(buffer, obj.EquipSlot);
                WriteByte(buffer, obj.Color);
                WriteByte(buffer, obj.PlusValue);
                WriteEntityList(buffer, obj.EquipElementParamList);
                WriteEntityList(buffer, obj.AddStatusParamList);
                WriteEntityList(buffer, obj.EquipStatParamList);
            }

            public override CDataEquipItemInfo Read(IBuffer buffer)
            {
                CDataEquipItemInfo obj = new CDataEquipItemInfo();
                obj.ItemId = ReadUInt32(buffer);
                obj.SafetySetting = ReadByte(buffer);
                obj.EquipType = (EquipType) ReadByte(buffer);
                obj.EquipSlot = ReadUInt16(buffer);
                obj.Color = ReadByte(buffer);
                obj.PlusValue = ReadByte(buffer);
                obj.EquipElementParamList = ReadEntityList<CDataEquipElementParam>(buffer);
                obj.AddStatusParamList = ReadEntityList<CDataAddStatusParam>(buffer);
                obj.EquipStatParamList = ReadEntityList<CDataEquipStatParam>(buffer);
                return obj;
            }
        }
    }

}
