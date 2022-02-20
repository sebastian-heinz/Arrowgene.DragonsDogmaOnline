using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CData_35_14_16
    {
        public CData_35_14_16()
        {
            UniqueId=0;
            Unk0=0;
        }

        public ulong UniqueId { get; set; }
        public byte Unk0 { get; set; }

        public class Serializer : EntitySerializer<CData_35_14_16>
        {
            public override void Write(IBuffer buffer, CData_35_14_16 obj)
            {
                WriteUInt64(buffer, obj.UniqueId);
                WriteByte(buffer, obj.Unk0);
            }

            public override CData_35_14_16 Read(IBuffer buffer)
            {
                CData_35_14_16 obj = new CData_35_14_16();
                obj.UniqueId = ReadUInt64(buffer);
                obj.Unk0 = ReadByte(buffer);
                return obj;
            }
        }
    }
}
