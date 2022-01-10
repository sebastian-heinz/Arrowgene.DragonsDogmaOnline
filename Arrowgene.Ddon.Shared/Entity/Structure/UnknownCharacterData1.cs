using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public struct UnknownCharacterData1
    {
        public byte u0;
        public uint u1;
    }

    public class UnkownCharacterData1Serializer : EntitySerializer<UnknownCharacterData1>
    {
        public override void Write(IBuffer buffer, UnknownCharacterData1 obj)
        {
            WriteByte(buffer, obj.u0);
            WriteUInt32(buffer, obj.u1);
        }

        public override UnknownCharacterData1 Read(IBuffer buffer)
        {
            UnknownCharacterData1 obj = new UnknownCharacterData1();
            obj.u0 = ReadByte(buffer);
            obj.u1 = ReadUInt32(buffer);
            return obj;
        }
    }
}
