using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Client.Resource
{
    public class LocationData : ResourceFile
    {
        public class Entry
        {
            public MtVector3 Pos { get; set; }
            public float Radius { get; set; }
            public float Angle { get; set; }
            public float Range { get; set; }
            public ushort MessageNo { get; set; }
            public ushort Type { get; set; }
            public uint WarpPointId { get; set; }
            public bool SafeZone { get; set; }
            public uint Version { get; set; }
        }

        public List<Entry> Entries { get; }

        public LocationData()
        {
            Entries = new List<Entry>();
        }
        
        protected override void ReadResource(IBuffer buffer)
        {
            uint version = ReadUInt32(buffer);
            Entries.Clear();
            uint count = ReadUInt32(buffer);
            for (int i = 0; i < count; i++)
            {
                Entry entry = new Entry();
                entry.Pos = ReadMtVector3(buffer);
                entry.Radius = ReadFloat(buffer);
                entry.Angle = ReadFloat(buffer);
                entry.Range = ReadFloat(buffer);
                entry.MessageNo = ReadUInt16(buffer);
                entry.Type = ReadUInt16(buffer);
                entry.WarpPointId = ReadUInt32(buffer);
                entry.SafeZone = ReadBool(buffer);
                Entries.Add(entry);
            }
        }
    }
}
