using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public struct CDataEquipElementUnkType2
    {
        public byte u0;
        public uint u1;
        public ushort u2;
    }

    public class CDataEquipElementUnkType2Serializer : EntitySerializer<CDataEquipElementUnkType2>
    {
        public override void Write(IBuffer buffer, CDataEquipElementUnkType2 obj)
        {
            WriteByte(buffer, obj.u0);
            WriteUInt32(buffer, obj.u1);
            WriteUInt16(buffer, obj.u2);
        }

        public override CDataEquipElementUnkType2 Read(IBuffer buffer)
        {
            CDataEquipElementUnkType2 obj = new CDataEquipElementUnkType2();
            obj.u0 = ReadByte(buffer);
            obj.u1 = ReadUInt32(buffer);
            obj.u2 = ReadUInt16(buffer);
            return obj;
        }
    }
}
