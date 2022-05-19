using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CData_772950
    {
        public byte Unk0 { get; set; }
        public byte Unk1 { get; set; }

        public class Serializer : EntitySerializer<CData_772950>
        {
            public override void Write(IBuffer buffer, CData_772950 obj)
            {
                WriteByte(buffer, obj.Unk0);
                WriteByte(buffer, obj.Unk1);
            }

            public override CData_772950 Read(IBuffer buffer)
            {
                CData_772950 obj = new CData_772950();
                obj.Unk0 = ReadByte(buffer);
                obj.Unk1 = ReadByte(buffer);
                return obj;
            }
        }
    }
}