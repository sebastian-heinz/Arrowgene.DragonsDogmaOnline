using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataAreaRankSeason3
    {
        public uint SpotId { get; set; }
        public bool DeadlineReached { get; set; }
        public ulong Deadline { get; set; } // Seemingly a DateTime?

        public class Serializer : EntitySerializer<CDataAreaRankSeason3>
        {
            public override void Write(IBuffer buffer, CDataAreaRankSeason3 obj)
            {
                WriteUInt32(buffer, obj.SpotId);
                WriteBool(buffer, obj.DeadlineReached);
                WriteUInt64(buffer, obj.Deadline);
            }

            public override CDataAreaRankSeason3 Read(IBuffer buffer)
            {
                CDataAreaRankSeason3 obj = new CDataAreaRankSeason3();
                obj.SpotId = ReadUInt32(buffer);
                obj.DeadlineReached = ReadBool(buffer);
                obj.Deadline = ReadUInt64(buffer);
                return obj;
            }
        }
    }
}
