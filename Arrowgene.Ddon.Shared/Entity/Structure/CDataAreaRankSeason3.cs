using Arrowgene.Buffers;
using System;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataAreaRankSeason3
    {
        public uint SpotId { get; set; }
        public bool DeadlineReached { get; set; }
        public DateTimeOffset Deadline { get; set; }

        public class Serializer : EntitySerializer<CDataAreaRankSeason3>
        {
            public override void Write(IBuffer buffer, CDataAreaRankSeason3 obj)
            {
                WriteUInt32(buffer, obj.SpotId);
                WriteBool(buffer, obj.DeadlineReached);
                WriteUInt64(buffer, (ulong)obj.Deadline.ToUnixTimeSeconds());
            }

            public override CDataAreaRankSeason3 Read(IBuffer buffer)
            {
                CDataAreaRankSeason3 obj = new CDataAreaRankSeason3();
                obj.SpotId = ReadUInt32(buffer);
                obj.DeadlineReached = ReadBool(buffer);
                obj.Deadline = DateTimeOffset.FromUnixTimeSeconds((long)ReadUInt64(buffer));
                return obj;
            }
        }
    }
}
