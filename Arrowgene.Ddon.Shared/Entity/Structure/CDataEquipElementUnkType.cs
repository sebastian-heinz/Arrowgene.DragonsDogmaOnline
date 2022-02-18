using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataEquipElementUnkType
    {
        public byte u0;
        public byte u1;
        public ushort u2;
        public ushort u3;
    }

    public class CDataEquipElementUnkTypeSerializer : EntitySerializer<CDataEquipElementUnkType>
    {
        public override void Write(IBuffer buffer, CDataEquipElementUnkType obj)
        {
            WriteByte(buffer, obj.u0);
            WriteByte(buffer, obj.u1);
            WriteUInt16(buffer, obj.u2);
            WriteUInt16(buffer, obj.u3);
        }

        public override CDataEquipElementUnkType Read(IBuffer buffer)
        {
            CDataEquipElementUnkType obj = new CDataEquipElementUnkType();
            obj.u0 = ReadByte(buffer);
            obj.u1 = ReadByte(buffer);
            obj.u2 = ReadUInt16(buffer);
            obj.u3 = ReadUInt16(buffer);
            return obj;
        }
    }
}
