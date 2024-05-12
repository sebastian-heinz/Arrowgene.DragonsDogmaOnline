using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataItemEquipElementParam
    {    
        public byte SlotNo { get; set; }
        public uint ItemID { get; set; }
        public ushort Unk0 { get; set; }
    
        public class Serializer : EntitySerializer<CDataItemEquipElementParam>
        {
            public override void Write(IBuffer buffer, CDataItemEquipElementParam obj)
            {
                WriteByte(buffer, obj.SlotNo);
                WriteUInt32(buffer, obj.ItemID);
                WriteUInt16(buffer, obj.Unk0);
            }
        
            public override CDataItemEquipElementParam Read(IBuffer buffer)
            {
                CDataItemEquipElementParam obj = new CDataItemEquipElementParam();
                obj.SlotNo = ReadByte(buffer);
                obj.ItemID = ReadUInt32(buffer);
                obj.Unk0 = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}