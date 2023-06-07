using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataWeaponCrestData
    {
        public byte SlotNo { get; set; } // Index starts at 1
        public uint CrestId { get; set; }
        public ushort Add { get; set; }
        
        public class Serializer : EntitySerializer<CDataWeaponCrestData>
        {
            public override void Write(IBuffer buffer, CDataWeaponCrestData obj)
            {
                WriteByte(buffer, obj.SlotNo);
                WriteUInt32(buffer, obj.CrestId);
                WriteUInt16(buffer, obj.Add);
            }

            public override CDataWeaponCrestData Read(IBuffer buffer)
            {
                CDataWeaponCrestData obj = new CDataWeaponCrestData();
                obj.SlotNo = ReadByte(buffer);
                obj.CrestId = ReadUInt32(buffer);
                obj.Add = ReadUInt16(buffer);
                return obj;
            }
        }
    }

}
