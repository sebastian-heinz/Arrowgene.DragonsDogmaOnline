using System;
using System.Collections.Generic;
using System.Text;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Client.Resource
{
    public class Gmd : ResourceFile
    {
        public class Entry
        {
            public string Key { get; set; }
            public string Msg { get; set; }
            public uint Index { get; set; }
            public uint a2 { get; set; }
            public uint a3 { get; set; }
            public uint a4 { get; set; }
            public uint a5 { get; set; }
        }

        public List<Entry> Entries { get; }

        public Gmd()
        {
            Entries = new List<Entry>();
        }

        protected override MagicIdWidth IdWidth => MagicIdWidth.UInt;

        protected override void ReadResource(IBuffer buffer)
        {
            uint a = ReadUInt32(buffer);
            uint b = ReadUInt32(buffer);
            uint c = ReadUInt32(buffer);
            uint keyCount = ReadUInt32(buffer);
            uint stringCount = ReadUInt32(buffer);
            uint keySize = ReadUInt32(buffer);
            uint stringSize = ReadUInt32(buffer);
            uint h = ReadUInt32(buffer);
            string str = buffer.ReadCString(Encoding.UTF8);

            uint maxEntries = Math.Max(keyCount, stringCount);
            Entry[] entries = new Entry[maxEntries];
            for (int i = 0; i < maxEntries; i++)
            {
                entries[i] = new Entry();
            }

            for (int i = 0; i < keyCount; i++)
            {
                Entry entry = entries[i];
                entry.Index = ReadUInt32(buffer);
                entry.a2 = ReadUInt32(buffer);
                entry.a3 = ReadUInt32(buffer);
                entry.a4 = ReadUInt32(buffer);
                entry.a5 = ReadUInt32(buffer);
            }

            byte[] unk = buffer.ReadBytes(1024);

            for (int i = 0; i < keyCount; i++)
            {
                Entry entry = entries[i];
                entry.Key = buffer.ReadCString(Encoding.UTF8);
            }

            for (int i = 0; i < stringCount; i++)
            {
                Entry entry = entries[i];
                entry.Msg = buffer.ReadCString(Encoding.UTF8);
            }

            Entries.Clear();
            Entries.AddRange(entries);
        }
    }
}
