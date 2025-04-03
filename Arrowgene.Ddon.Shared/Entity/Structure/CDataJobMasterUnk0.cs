using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataJobMasterUnk0
    {
        public JobId Job { get; set; }
        public uint Unk0 { get; set; }
        public byte Unk1 { get; set; }
        public byte Unk2 { get; set; }
        public uint Unk3 { get; set; }
        public uint Unk4 { get; set; }

        public class Serializer : EntitySerializer<CDataJobMasterUnk0>
        {
            public override void Write(IBuffer buffer, CDataJobMasterUnk0 obj)
            {
                WriteByte(buffer, (byte)obj.Job);
                WriteUInt32(buffer, obj.Unk0);
                WriteByte(buffer, obj.Unk1);
                WriteByte(buffer, obj.Unk2);
                WriteUInt32(buffer, obj.Unk3);
                WriteUInt32(buffer, obj.Unk4);
            }

            public override CDataJobMasterUnk0 Read(IBuffer buffer)
            {
                CDataJobMasterUnk0 obj = new CDataJobMasterUnk0();
                obj.Job = (JobId)ReadByte(buffer);
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadByte(buffer);
                obj.Unk2 = ReadByte(buffer);
                obj.Unk3 = ReadUInt32(buffer);
                obj.Unk4 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
