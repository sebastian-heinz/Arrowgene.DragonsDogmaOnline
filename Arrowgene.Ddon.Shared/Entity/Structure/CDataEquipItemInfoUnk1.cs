using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataEquipItemInfoUnk1
    {
        public byte u0;
        public byte u1;
        public ushort u2;
        public ushort u3;

        public class Serializer : EntitySerializer<CDataEquipItemInfoUnk1>
        {
            public override void Write(IBuffer buffer, CDataEquipItemInfoUnk1 obj)
            {
                WriteByte(buffer, obj.u0);
                WriteByte(buffer, obj.u1);
                WriteUInt16(buffer, obj.u2);
                WriteUInt16(buffer, obj.u3);
            }

            public override CDataEquipItemInfoUnk1 Read(IBuffer buffer)
            {
                CDataEquipItemInfoUnk1 obj = new CDataEquipItemInfoUnk1();
                obj.u0 = ReadByte(buffer);
                obj.u1 = ReadByte(buffer);
                obj.u2 = ReadUInt16(buffer);
                obj.u3 = ReadUInt16(buffer);
                return obj;
            }
        }
    }

}
