using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataPartnerPawnInfo
    {
        public uint PawnId { get; set; }
        public uint Likability { get; set; }
        public byte Personality { get; set; }

        public class Serializer : EntitySerializer<CDataPartnerPawnInfo>
        {
            public override void Write(IBuffer buffer, CDataPartnerPawnInfo obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteUInt32(buffer, obj.Likability);
                WriteByte(buffer, obj.Personality);
            }

            public override CDataPartnerPawnInfo Read(IBuffer buffer)
            {
                CDataPartnerPawnInfo obj = new CDataPartnerPawnInfo();
                obj.PawnId = ReadUInt32(buffer);
                obj.Likability = ReadUInt32(buffer);
                obj.Personality = ReadByte(buffer);
                return obj;
            }
        }
    }
}