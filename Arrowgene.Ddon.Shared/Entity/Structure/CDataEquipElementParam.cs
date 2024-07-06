using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataEquipElementParam
    {
        public byte SlotNo { get; set; } // Index starts at 1
        public uint CrestId { get; set; }
        public ushort Add { get; set; }
        
        public class Serializer : EntitySerializer<CDataEquipElementParam>
        {
            public override void Write(IBuffer buffer, CDataEquipElementParam obj)
            {
                WriteByte(buffer, obj.SlotNo);
                WriteUInt32(buffer, obj.CrestId);
                WriteUInt16(buffer, obj.Add);
            }

            public override CDataEquipElementParam Read(IBuffer buffer)
            {
                CDataEquipElementParam obj = new CDataEquipElementParam();
                obj.SlotNo = ReadByte(buffer);
                obj.CrestId = ReadUInt32(buffer);
                obj.Add = ReadUInt16(buffer);
                return obj;
            }
        }
    }

}
