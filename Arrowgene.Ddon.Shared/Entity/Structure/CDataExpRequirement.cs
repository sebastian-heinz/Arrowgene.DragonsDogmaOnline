using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataExpRequirement
    {
        public byte Level { get; set; }
        public uint Amount { get; set; }

        public class Serializer : EntitySerializer<CDataExpRequirement>
        {
            public override void Write(IBuffer buffer, CDataExpRequirement obj)
            {
                WriteByte(buffer, obj.Level);
                WriteUInt32(buffer, obj.Amount);
            }

            public override CDataExpRequirement Read(IBuffer buffer)
            {
                CDataExpRequirement obj = new CDataExpRequirement();
                obj.Level = ReadByte(buffer);
                obj.Amount = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
