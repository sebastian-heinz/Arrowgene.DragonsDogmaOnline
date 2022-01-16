using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Shared.Csv
{
    public abstract class CsvReader<T>
    {
        private const int BufferSize = 128;
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CsvReader<T>));

        protected abstract int NumExpectedItems { get; }

        public List<T> Read(string path)
        {
            Logger.Info($"Reading {path}");
            List<T> items = new List<T>();
            FileInfo file = new FileInfo(path);
            if (!file.Exists)
            {
                return items;
            }

            using FileStream fileStream = File.OpenRead(file.FullName);
            using StreamReader streamReader = new StreamReader(
                fileStream,
                Encoding.UTF8,
                true,
                BufferSize
            );
            string line;
            while ((line = streamReader.ReadLine()) != null)
            {
                ProcessLine(line, items);
            }

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
    }
}
