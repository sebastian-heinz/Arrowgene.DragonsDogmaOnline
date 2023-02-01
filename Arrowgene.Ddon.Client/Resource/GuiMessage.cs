using System;
using System.Collections.Generic;
using System.Text;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Crypto;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Client.Resource
{
    public class GuiMessage : ResourceFile
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(GuiMessage));

        public class Entry
        {
            private string _key;

            public uint Index { get; set; }

            public string Key
            {
                get => _key;
                set
                {
                    _key = value;
                    if (value == null)
                    {
                        KeyHash2X = 0;
                        KeyHash3X = 0;
                    }
                    else
                    {
                        KeyHash2X = Crc32.GetHash(value+value);
                        KeyHash3X = Crc32.GetHash(value+value+value);
                    }
                }
            }

            public string Msg { get; set; }
            public uint KeyHash2X { get; set; }
            public uint KeyHash3X { get; set; }
            public uint KeyOffset { get; set; }
            public uint a5 { get; set; }
            public uint KeyReadIndex { get; set; }
            public uint MsgReadIndex { get; set; }
        }

        public List<Entry> Entries { get; }
        public byte[] Unknown { get; set; }
        public string Str { get; set; }
        public uint Version { get; set; }
        public uint A { get; set; }
        public uint B { get; set; }
        public uint C { get; set; }

        public GuiMessage()
        {
            Entries = new List<Entry>();
        }

        protected override void ReadResource(IBuffer buffer)
        {
            Version = ReadUInt32(buffer);
            A = ReadUInt32(buffer); // Language ? 0 Japanese, 1 English ?
            B = ReadUInt32(buffer);
            C = ReadUInt32(buffer);
            uint keyCount = ReadUInt32(buffer);
            uint stringCount = ReadUInt32(buffer);
            uint keySize = ReadUInt32(buffer);
            uint stringSize = ReadUInt32(buffer);
            uint strLen = ReadUInt32(buffer);
            Str = buffer.ReadString((int)strLen);
            buffer.ReadByte(); // str null-termination

            if (keyCount > 0 && keyCount != stringCount)
            {
                // TODO it seems to work for this case as well
                // This case exists for a few files, one is 
                // /Volumes/data/game/Dragon's Dogma Online/nativePC/rom/quest/pqi_01.arc
                // ui\00_message\package_quest\package_quest_info1
            }

            uint maxEntries = Math.Max(keyCount, stringCount);
            Entry[] entries = new Entry[maxEntries];
            for (int i = 0; i < maxEntries; i++)
            {
                entries[i] = new Entry();
            }

            if (keyCount > 0)
            {
                for (int i = 0; i < keyCount; i++)
                {
                    Entry entry = entries[i];
                    entry.Index = ReadUInt32(buffer);
                    entry.KeyHash2X = ReadUInt32(buffer);
                    entry.KeyHash3X = ReadUInt32(buffer);
                    entry.KeyOffset = ReadUInt32(buffer);
                    entry.a5 = ReadUInt32(buffer); // Key Offset ?
                }

                Unknown = buffer.ReadBytes(1024);

                for (uint i = 0; i < keyCount; i++)
                {
                    Entry entry = entries[i];
                    entry.Key = buffer.ReadCString(Encoding.UTF8);
                    entry.KeyReadIndex = i;
                }
            }

            for (uint i = 0; i < stringCount; i++)
            {
                Entry entry = entries[i];
                entry.Msg = buffer.ReadCString(Encoding.UTF8);
                entry.MsgReadIndex = i;
            }

            Entries.Clear();
            Entries.AddRange(entries);
        }

        protected override void WriteResource(IBuffer buffer)
        {
            uint keyCount = 0;
            uint stringCount = 0;
            foreach (Entry entry in Entries)
            {
                if (!string.IsNullOrEmpty(entry.Key))
                {
                    keyCount++;
                }

                stringCount++;
            }

            Entries.Sort((x, y) => x.MsgReadIndex.CompareTo(y.MsgReadIndex));

            uint keySize = 0;
            uint stringSize = 0;
            uint strLen = (uint)Str.Length;

            buffer.WriteUInt32(Version);
            buffer.WriteUInt32(A);
            buffer.WriteUInt32(B);
            buffer.WriteUInt32(C);
            buffer.WriteUInt32(keyCount);
            buffer.WriteUInt32(stringCount);
            //
            int sizePosition = buffer.Position;
            buffer.WriteUInt32(keySize);
            buffer.WriteUInt32(stringSize);
            //
            buffer.WriteUInt32(strLen);
            buffer.WriteString(Str);
            buffer.WriteByte(0); // str null-termination

            keySize = (uint)buffer.Position;
            if (keyCount > 0)
            {
                uint keyOffset = 0;
                for (int i = 0; i < keyCount; i++)
                {
                    Entries[i].KeyOffset = keyOffset;
                        
                    buffer.WriteUInt32(Entries[i].Index);
                    buffer.WriteUInt32(Entries[i].KeyHash2X);
                    buffer.WriteUInt32(Entries[i].KeyHash3X);
                    buffer.WriteUInt32(Entries[i].KeyOffset);
                    buffer.WriteUInt32(Entries[i].a5);

                    keyOffset += (uint)Entries[i].Key.Length + 1;
                }

                buffer.WriteBytes(Unknown);
                for (int i = 0; i < keyCount; i++)
                {
                    buffer.WriteCString(Entries[i].Key, Encoding.UTF8);
                }
            }

            keySize = (uint)buffer.Position - keySize;

            stringSize = (uint)buffer.Position;
            for (int i = 0; i < stringCount; i++)
            {
                buffer.WriteCString(Entries[i].Msg, Encoding.UTF8);
            }

            stringSize = (uint)buffer.Position - stringSize;

            int tmpPosition = buffer.Position;
            buffer.Position = sizePosition;
            buffer.WriteUInt32(keySize);
            buffer.WriteUInt32(stringSize);
            buffer.Position = tmpPosition;
        }
    }
}
