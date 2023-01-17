using System;
using System.Collections.Generic;
using System.Text;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Client.Resource
{
    public class GuiMessage : ResourceFile
    {
        public class Entry
        {
            public uint Index { get; set; }
            public string Key { get; set; }
            public string Msg { get; set; }
            public uint a2 { get; set; }
            public uint a3 { get; set; }
            public uint a4 { get; set; }
            public uint a5 { get; set; }
        }

        public List<Entry> Entries { get; }
        public byte[] unk { get; set; }

        public GuiMessage()
        {
            Entries = new List<Entry>();
        }

        protected override void ReadResource(IBuffer buffer)
        {
            uint version = ReadUInt32(buffer);
            uint a = ReadUInt32(buffer);
            uint b = ReadUInt32(buffer);
            uint c = ReadUInt32(buffer);
            uint keyCount = ReadUInt32(buffer);
            uint stringCount = ReadUInt32(buffer);
            uint keySize = ReadUInt32(buffer);
            uint stringSize = ReadUInt32(buffer);
            uint strLen = ReadUInt32(buffer);
            string str = buffer.ReadString((int)strLen);
            buffer.ReadByte(); // str null-termination


            if (keyCount > 0 && keyCount != stringCount)
            {
                // TODO unsure how to deal if sizes are different, perhaps need to check all files if such case exists.
                throw new Exception("Please Report Me");
            }

            uint maxEntries = Math.Max(keyCount, stringCount);
            Entry[] entries = new Entry[maxEntries];
            for (int i = 0; i < maxEntries; i++)
            {
                entries[i] = new Entry();
            }

            if (keyCount > 0)
            {
                // TODO I assume this part is only parsed when "keyCount > 0"
                for (int i = 0; i < keyCount; i++)
                {
                    Entry entry = entries[i];
                    entry.Index = ReadUInt32(buffer);
                    entry.a2 = ReadUInt32(buffer);
                    entry.a3 = ReadUInt32(buffer);
                    entry.a4 = ReadUInt32(buffer);
                    entry.a5 = ReadUInt32(buffer);
                }

                unk = buffer.ReadBytes(1024); // hashTable?

                for (int i = 0; i < keyCount; i++)
                {
                    Entry entry = entries[i];
                    entry.Key = buffer.ReadCString(Encoding.UTF8);
                }
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
