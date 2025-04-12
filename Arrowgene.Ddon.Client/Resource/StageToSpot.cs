using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Client.Resource
{
    public class StageToSpot : ClientFile
    {
        public class Entry
        {
            public uint StageNo { get; set; }
            public uint SpotId { get; set; }
            public byte RecommendLevel { get; set; }
        }

        public List<Entry> Entries { get; }

        public StageToSpot()
        {
            Entries = new List<Entry>();
        }

        public override void Read(IBuffer buffer)
        {
            uint data = buffer.ReadUInt32();
            uint dataNum = buffer.ReadUInt32();
            Entries.Clear();
            for (int i = 0; i < dataNum; i++)
            {
                Entries.Add(ReadEntry(buffer));
            }
        }

        public override void Write(IBuffer buffer)
        {
            throw new System.NotImplementedException();
        }

        private Entry ReadEntry(IBuffer buffer)
        {
            Entry entry = new Entry();
            entry.StageNo = ReadUInt32(buffer);
            entry.SpotId = ReadUInt32(buffer);
            entry.RecommendLevel = buffer.ReadByte();
            return entry;
        }
    }
}
