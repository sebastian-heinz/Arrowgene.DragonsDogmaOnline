using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public struct CDataEquipElementParam
    {
        public byte u0;
        public ushort u2;
        
        public class Serializer : EntitySerializer<CDataEquipElementParam>
        {
            public override void Write(IBuffer buffer, CDataEquipElementParam obj)
            {
                WriteByte(buffer, obj.u0);
                WriteUInt16(buffer, obj.u2);
            }

            public override CDataEquipElementParam Read(IBuffer buffer)
            {
                CDataEquipElementParam obj = new CDataEquipElementParam();
                obj.u0 = ReadByte(buffer);
                obj.u2 = ReadUInt16(buffer);
                return obj;
            }
        }
    }

}
