using Arrowgene.Buffers;
using System;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCycleContentsRank
    {
        public byte Type { get; set; }
        public uint Rank { get; set; }
        public uint Score { get; set; }
        public DateTimeOffset UpdateDate { get; set; }

        public class Serializer : EntitySerializer<CDataCycleContentsRank>
        {
            public override void Write(IBuffer buffer, CDataCycleContentsRank obj)
            {
                WriteByte(buffer, obj.Type);
                WriteUInt32(buffer, obj.Rank);
                WriteUInt32(buffer, obj.Score);
                WriteUInt64(buffer, (ulong)obj.UpdateDate.ToUnixTimeSeconds());
            }

            public override CDataCycleContentsRank Read(IBuffer buffer)
            {
                CDataCycleContentsRank obj = new CDataCycleContentsRank();
                obj.Type = ReadByte(buffer);
                obj.Rank = ReadUInt32(buffer);
                obj.Score = ReadUInt32(buffer);
                obj.UpdateDate = DateTimeOffset.FromUnixTimeSeconds((long)ReadUInt64(buffer));
                return obj;
            }
        }
    }
}
