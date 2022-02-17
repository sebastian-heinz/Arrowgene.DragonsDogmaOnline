using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class UnknownCharacterData0
    {
        public byte u0;
        public ushort u1;
    }

    public class UnkownCharacterData0Serializer : EntitySerializer<UnknownCharacterData0>
    {
        public override void Write(IBuffer buffer, UnknownCharacterData0 obj)
        {
            WriteByte(buffer, obj.u0);
            WriteUInt16(buffer, obj.u1);
        }

        public override UnknownCharacterData0 Read(IBuffer buffer)
        {
            UnknownCharacterData0 obj = new UnknownCharacterData0();
            obj.u0 = ReadByte(buffer);
            obj.u1 = ReadUInt16(buffer);
            return obj;
        }
    }
}
