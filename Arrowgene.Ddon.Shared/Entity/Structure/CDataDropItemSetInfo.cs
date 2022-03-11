using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataDropItemSetInfo
    {
        public CDataDropItemSetInfo()
        {
            Id = 0;
            MdlType = 0;
            X = 0;
            Y = 0;
            Z = 0;
        }

        public byte Id { get; set; }
        public byte MdlType { get; set; } // Model Type?
        public double X { get; set; }
        public float Y { get; set; }
        public double Z { get; set; }

        public class Serializer : EntitySerializer<CDataDropItemSetInfo>
        {
            public override void Write(IBuffer buffer, CDataDropItemSetInfo obj)
            {
                WriteByte(buffer, obj.Id);
                WriteByte(buffer, obj.MdlType);
                WriteDouble(buffer, obj.X);
                WriteFloat(buffer, obj.Y);
                WriteDouble(buffer, obj.Z);
            }

            public override CDataDropItemSetInfo Read(IBuffer buffer)
            {
                CDataDropItemSetInfo obj = new CDataDropItemSetInfo();
                obj.Id = ReadByte(buffer);
                obj.MdlType = ReadByte(buffer);
                obj.X = ReadDouble(buffer);
                obj.Y = ReadFloat(buffer);
                obj.Z = ReadDouble(buffer);
                return obj;
            }
        }
    }
}
