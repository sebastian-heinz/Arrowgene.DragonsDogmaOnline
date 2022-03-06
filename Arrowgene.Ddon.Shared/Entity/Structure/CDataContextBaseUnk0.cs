using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataContextBaseUnk0
    {
        public byte Unk0;
        public uint Unk1;

        public class Serializer : EntitySerializer<CDataContextBaseUnk0>
        {
            public override void Write(IBuffer buffer, CDataContextBaseUnk0 obj)
            {
                WriteByte(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
            }

            public override CDataContextBaseUnk0 Read(IBuffer buffer)
            {
                CDataContextBaseUnk0 obj = new CDataContextBaseUnk0();
                obj.Unk0 = ReadByte(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}