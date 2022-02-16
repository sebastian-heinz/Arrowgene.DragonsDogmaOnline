using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Shared.Csv
{
    public abstract class CsvReader<T>
    {
        private const int BufferSize = 128;
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CsvReader<T>));

        public bool AllowLF = false;
        protected abstract int NumExpectedItems { get; }

        public List<T> ReadString(string csv)
        {
            using Stream stream = new MemoryStream();
            using StreamWriter writer = new StreamWriter(stream);
            writer.Write(csv);
            writer.Flush();
            stream.Position = 0;
            return Read(stream);
        }

        public List<T> ReadPath(string path)
        {
            Logger.Info($"Reading {path}");

            FileInfo file = new FileInfo(path);
            if (!file.Exists)
            {
                return new List<T>();
            }

            using FileStream stream = File.OpenRead(file.FullName);
            return Read(stream);
        }

        public List<T> Read(Stream stream)
        {
            List<T> items = new List<T>();

            using StreamReader streamReader = new StreamReader(
                stream,
                Encoding.UTF8,
                true,
                BufferSize
            );

            int c;
            StringBuilder sb = new StringBuilder();
            bool previousCr = false;
            while ((c = streamReader.Read()) > 0)
            {
                // only treat \r\n as line endings
                if (c == '\r')
                {
                    //carriage return
                    previousCr = true;
                    continue;
                }

                if (c == '\n')
                {
                    //line feed 
                    if (previousCr || AllowLF)
                    {
                        ProcessLine(sb.ToString(), items);
                        sb.Clear();
                    }

                    previousCr = false;
                    continue;
                }

                sb.Append((char) c);
                previousCr = false;
            }

            ProcessLine(sb.ToString(), items);

            return items;
        }

        protected abstract T CreateInstance(string[] properties);

        private void ProcessLine(string line, ICollection<T> items)
        {
            if (string.IsNullOrEmpty(line)) return;

            if (line.StartsWith('#'))
                // Ignoring Comment
                return;
            if (line.StartsWith(','))
                // Ignoring null ID
                return;

            string[] properties = line.Split(",");
            if (properties.Length <= 0) return;

            if (properties.Length < NumExpectedItems)
            {
                Logger.Error(
                    $"Skipping Line: '{line}' expected {NumExpectedItems} values but got {properties.Length}");
                return;
            }

            T item = default;
            try
            {
                item = CreateInstance(properties);
            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
            }

            if (item == null)
            {
                Logger.Error($"Skipping Line: '{line}' could not be converted");
                return;
            }

            items.Add(item);
        }

        protected bool TryParseNullableInt(string str, out int? value)
        {
            if (string.IsNullOrEmpty(str))
            {
                value = null;
                return true;
            }

            if (int.TryParse(str, out int val))
            {
                value = val;
                return true;
            }

            value = null;
            return false;
        }

        protected bool TryParseHexUInt(string str, out uint value)
        {
            if (string.IsNullOrEmpty(str))
            {
                value = 0;
                return false;
            }
            str = str.TrimStart('0', 'x');
            return uint.TryParse(str, NumberStyles.HexNumber, null, out value);
        }
    }
}
