using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataWeaponCrestData
    {
        public byte u0;
        public uint u1;
        public ushort u2;
        public class Serializer : EntitySerializer<CDataWeaponCrestData>
        {
            public override void Write(IBuffer buffer, CDataWeaponCrestData obj)
            {
                WriteByte(buffer, obj.u0);
                WriteUInt32(buffer, obj.u1);
                WriteUInt16(buffer, obj.u2);
            }

            public override CDataWeaponCrestData Read(IBuffer buffer)
            {
                CDataWeaponCrestData obj = new CDataWeaponCrestData();
                obj.u0 = ReadByte(buffer);
                obj.u1 = ReadUInt32(buffer);
                obj.u2 = ReadUInt16(buffer);
                return obj;
            }
        }
    }

}
