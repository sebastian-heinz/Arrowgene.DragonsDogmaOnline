using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataOcdImmuneLv
    {
        public byte Index { get; set; }
        public byte ImmuneLv { get; set; }

        public class Serializer : EntitySerializer<CDataOcdImmuneLv>
        {
            public override void Write(IBuffer buffer, CDataOcdImmuneLv obj)
            {
                WriteByte(buffer, obj.Index);
                WriteByte(buffer, obj.ImmuneLv);
            }
            public override CDataOcdImmuneLv Read(IBuffer buffer)
            {
                CDataOcdImmuneLv obj = new CDataOcdImmuneLv();
                obj.Index = ReadByte(buffer);
                obj.ImmuneLv = ReadByte(buffer);
                return obj;
            }
        }
    }
}