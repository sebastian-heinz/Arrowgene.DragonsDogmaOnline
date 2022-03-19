using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Client.Resource
{
    public class WarpLocationList : ResourceFile
    {
        public class Entry
        {
            public uint Id { get; set; }
            public uint SortNo { get; set; }
            public uint AreaId { get; set; }
            public uint SpotId { get; set; }
            public uint StageNo { get; set; }
            public uint PosNo { get; set; }
            public ushort MapPosX { get; set; }
            public ushort MapPosY { get; set; }
            public byte IconType { get; set; }
        }

        public List<Entry> Entries { get; }

        public WarpLocationList()
        {
            Entries = new List<Entry>();
        }
        
        protected override void ReadResource(IBuffer buffer)
        {
            Entries.Clear();
            uint count = ReadUInt32(buffer);

            for (int i = 0; i < count; i++)
            {
                Entry entry = new Entry();
                entry.Id = ReadUInt32(buffer);
                entry.SortNo = ReadUInt32(buffer);
                entry.AreaId = ReadUInt32(buffer);
                entry.SpotId = ReadUInt32(buffer);
                entry.StageNo = ReadUInt32(buffer);
                entry.PosNo = ReadUInt32(buffer);
                entry.MapPosX = ReadUInt16(buffer);
                entry.MapPosY = ReadUInt16(buffer);
                entry.IconType = ReadByte(buffer);
                ReadUInt32(buffer);
                Entries.Add(entry);
            }
        }
    }
}
