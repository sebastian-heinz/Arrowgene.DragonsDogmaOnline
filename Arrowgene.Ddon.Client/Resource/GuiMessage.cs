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

        public enum Language : uint
        {
            Japanese = 0,
            English = 1,
            French = 2,
            Spanish = 3,
            German = 4,
            Italian = 5
        }

        public class Entry
        {
            public uint Index { get; set; }
            public string Key { get; set; }
            public string Msg { get; set; }
            public uint KeyHash2X { get; set; }
            public uint KeyHash3X { get; set; }
            public uint KeyOffset { get; set; }
            public uint LinkIndex { get; set; }
            public uint ReadIndex { get; set; }
        }

        public List<Entry> Entries { get; }
        public uint[] HashTable { get; }
        public string Str { get; set; }
        public uint Version { get; set; }
        public uint LanguageId { get; set; }
        public ulong UpdateTime { get; set; }


        public GuiMessage()
        {
            HashTable = new uint[256];
            Entries = new List<Entry>();
        }

        public uint CalculateKeyProperties()
        {
            Entries.Sort((x, y) => x.ReadIndex.CompareTo(y.ReadIndex));

            uint keyCount = 0;
            bool finished = false;
            foreach (Entry entry in Entries)
            {
                if (string.IsNullOrEmpty(entry.Key))
                {
                    finished = true;
                }
                else
                {
                    if (finished)
                    {
                        Logger.Error(
                            $"There is a gap within the keys. (keyCount: {keyCount}, Key:{entry.Key}, Msg:{entry.Msg})");
                        break;
                    }

                    keyCount++;
                }
            }

            if (keyCount > 0)
            {
                uint keyOffset = 0;
                for (int i = 0; i < keyCount; i++)
                {
                    Entries[i].Index = (uint)i;
                    Entries[i].KeyOffset = keyOffset;
                    Entries[i].LinkIndex = 0;
                    Entries[i].KeyHash2X = Crc32.GetHash(Entries[i].Key + Entries[i].Key);
                    Entries[i].KeyHash3X = Crc32.GetHash(Entries[i].Key + Entries[i].Key + Entries[i].Key);
                    keyOffset += (uint)Entries[i].Key.Length + 1;
                }

                uint bucketCounter = 0;
                Dictionary<byte, uint> buckets = new Dictionary<byte, uint>();
                for (int i = 0; i < keyCount; i++)
                {
                    Entries[i].Index = (uint)i;
                    byte bucket = (byte)(Crc32.GetHash(Entries[i].Key) & 0xFF);
                    if (buckets.ContainsKey(bucket))
                    {
                        Entries[(int)buckets[bucket]].LinkIndex = bucketCounter;
                        buckets[bucket] = bucketCounter;
                    }
                    else
                    {
                        buckets.Add(bucket, bucketCounter);
                    }

                    bucketCounter++;
                }

                uint hashTableCounter = 0;
                for (int i = 0; i < HashTable.Length; i++)
                {
                    HashTable[i] = 0;
                }

                for (int i = 0; i < keyCount; i++)
                {
                    byte bucket = (byte)(Crc32.GetHash(Entries[i].Key) & 0xFF);
                    if (HashTable[bucket] == 0)
                    {
                        HashTable[bucket] = hashTableCounter == 0 ? 0xFFFFFFFF : hashTableCounter;
                    }

                    hashTableCounter++;
                }
            }

            return keyCount;
        }

        protected override void ReadResource(IBuffer buffer)
        {
            Version = ReadUInt32(buffer);
            LanguageId = ReadUInt32(buffer);
            UpdateTime = ReadUInt64(buffer);
            uint keyCount = ReadUInt32(buffer);
            uint stringCount = ReadUInt32(buffer);
            uint keySize = ReadUInt32(buffer);
            uint stringSize = ReadUInt32(buffer);
            uint strLen = ReadUInt32(buffer);
            Str = buffer.ReadString((int)strLen);
            buffer.ReadByte(); // str null-termination

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
                    entry.LinkIndex = ReadUInt32(buffer);
                }

                for (int j = 0; j < HashTable.Length; j++)
                {
                    HashTable[j] = ReadUInt32(buffer);
                }

                for (uint i = 0; i < keyCount; i++)
                {
                    Entry entry = entries[i];
                    entry.Key = buffer.ReadCString(Encoding.UTF8);
                }
            }

            for (uint i = 0; i < stringCount; i++)
            {
                Entry entry = entries[i];
                entry.Msg = buffer.ReadCString(Encoding.UTF8);
                entry.ReadIndex = i;
            }

            Entries.Clear();
            Entries.AddRange(entries);
        }

        protected override void WriteResource(IBuffer buffer)
        {
            uint keyCount = CalculateKeyProperties();
            uint stringCount = (uint)Entries.Count;

            uint keySize = 0;
            uint stringSize = 0;
            uint strLen = (uint)Str.Length;

            buffer.WriteUInt32(Version);
            buffer.WriteUInt32(LanguageId);
            buffer.WriteUInt64(UpdateTime);
            buffer.WriteUInt32(keyCount);
            buffer.WriteUInt32(stringCount);

            int sizePosition = buffer.Position;
            buffer.WriteUInt32(keySize);
            buffer.WriteUInt32(stringSize);

            buffer.WriteUInt32(strLen);
            buffer.WriteString(Str);
            buffer.WriteByte(0); // str null-termination

            if (keyCount > 0)
            {
                for (int i = 0; i < keyCount; i++)
                {
                    buffer.WriteUInt32(Entries[i].Index);
                    buffer.WriteUInt32(Entries[i].KeyHash2X);
                    buffer.WriteUInt32(Entries[i].KeyHash3X);
                    buffer.WriteUInt32(Entries[i].KeyOffset);
                    buffer.WriteUInt32(Entries[i].LinkIndex);
                }

                for (int j = 0; j < HashTable.Length; j++)
                {
                    buffer.WriteUInt32(HashTable[j]);
                }

                keySize = (uint)buffer.Position;
                for (int i = 0; i < keyCount; i++)
                {
                    buffer.WriteCString(Entries[i].Key, Encoding.UTF8);
                }

                keySize = (uint)buffer.Position - keySize;
            }

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
