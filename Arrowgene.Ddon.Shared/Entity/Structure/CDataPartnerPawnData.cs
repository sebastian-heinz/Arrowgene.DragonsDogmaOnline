using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataPartnerPawnData
    {
        public uint PawnId { get; set; }
        public uint Likability { get; set; }
        public PawnPersonality Personality { get; set; }

        public class Serializer : EntitySerializer<CDataPartnerPawnData>
        {
            public override void Write(IBuffer buffer, CDataPartnerPawnData obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteUInt32(buffer, obj.Likability);
                WriteByte(buffer, (byte) obj.Personality);
            }

            public override CDataPartnerPawnData Read(IBuffer buffer)
            {
                CDataPartnerPawnData obj = new CDataPartnerPawnData();
                obj.PawnId = ReadUInt32(buffer);
                obj.Likability = ReadUInt32(buffer);
                obj.Personality = (PawnPersonality) ReadByte(buffer);
                return obj;
            }
        }
    }
}
