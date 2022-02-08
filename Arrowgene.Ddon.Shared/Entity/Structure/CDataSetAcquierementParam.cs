using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataSetAcquierementParam
    {
        // Unidentified fields from the PS4 build
        // u8   SlotNo
        // u32  AcquirementNo
        // u8   AcquirementLv

        public CDataSetAcquierementParam()
        {
            Job=0;
            Type=0;
            Unk0=0;
            Unk1=0;
            Unk2=0;
        }

        public byte Job { get; set; }
        public byte Type { get; set; }
        public uint Unk0 { get; set; }
        public byte Unk1 { get; set; }
        public uint Unk2 { get; set; }

        public class Serializer : EntitySerializer<CDataSetAcquierementParam>
        {
            public override void Write(IBuffer buffer, CDataSetAcquierementParam obj)
            {
                WriteByte(buffer, obj.Job);
                WriteByte(buffer, obj.Type);
                WriteUInt32(buffer, obj.Unk0);
                WriteByte(buffer, obj.Unk1);
                WriteUInt32(buffer, obj.Unk2);
            }

            public override CDataSetAcquierementParam Read(IBuffer buffer)
            {
                CDataSetAcquierementParam obj = new CDataSetAcquierementParam();
                obj.Job = ReadByte(buffer);
                obj.Type = ReadByte(buffer);
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadByte(buffer);
                obj.Unk2 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}