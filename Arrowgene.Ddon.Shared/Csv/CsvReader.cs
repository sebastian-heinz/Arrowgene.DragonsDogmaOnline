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


        protected abstract int NumExpectedItems { get; }

        public CsvReader()
        {

        }
        
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

        /// <summary>
        /// https://www.rfc-editor.org/rfc/rfc4180
        /// </summary>
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
            int line = 1;
            List<string> fields = new List<string>();
            StringBuilder fieldBuilder = new StringBuilder();
            StringBuilder lineBuilder = new StringBuilder();
            bool isFieldFirstCharacter = true;
            bool isLineFirstCharacter = true;
            bool isFieldQuoted = false;
            bool insideComment = false;
            int previousChar = -1;

            while ((c = streamReader.Read()) > 0)
            {
                if (isLineFirstCharacter)
                {
                    isLineFirstCharacter = false;
                    if (c == '#')
                    {             
                        // comment start
                        insideComment = true;
                        previousChar = c;
                        lineBuilder.Append((char)c);
                        continue;
                    }
                    
                    int nextChar = streamReader.Peek();
                    if (c == '\r' && nextChar == '\n')
                    {
                        // empty line
                        lineBuilder.Append((char)c);
                        c = streamReader.Read();
                        lineBuilder.Append((char)c);
                        previousChar = c;
                        isLineFirstCharacter = true;
                        isFieldFirstCharacter = true;
                        continue;
                    }
                }

                if (insideComment)
                {
                    if (previousChar == '\r' && c == '\n')
                    {
                        // comment end
                        isLineFirstCharacter = true;
                        isFieldFirstCharacter = true;
                        insideComment = false;
                    }

                    previousChar = c;
                    lineBuilder.Append((char)c);
                    continue;
                }

                if (isFieldFirstCharacter)
                {
                    isFieldFirstCharacter = false;
                    if (c == '"')
                    {
                        isFieldQuoted = true;
                        fieldBuilder.Append((char)c);
                        previousChar = c;
                        lineBuilder.Append((char)c);
                        continue;
                    }
                }

                if (isFieldQuoted)
                {
                    int nextChar = streamReader.Peek();
                    bool isQuoteEscaped = c == '"' && nextChar == '"';
                    if (isQuoteEscaped)
                    {
                        fieldBuilder.Append((char)c);
                        lineBuilder.Append((char)c);
                        c = streamReader.Read();
                        fieldBuilder.Append((char)c);
                        lineBuilder.Append((char)c);
                        previousChar = c;
                        continue;
                    }

                    if (c == '"' && nextChar != '"')
                    {
                        // detect the end
                        isFieldQuoted = false;
                        if (nextChar != ',')
                        {
                            // error unescaped quote
                        }
                    }

                    fieldBuilder.Append((char)c);
                    previousChar = c;
                    lineBuilder.Append((char)c);
                    continue;
                }

                if (c == ',')
                {
                    isFieldFirstCharacter = true;
                    fields.Add(fieldBuilder.ToString());
                    fieldBuilder.Clear();
                    previousChar = c;
                    lineBuilder.Append((char)c);
                    continue;
                }

                if (previousChar == '\r' && c == '\n')
                {
                    //carriage return (The Carriage Return (CR) character (0x0D, \r))
                    //line feed (The Line Feed (LF) character (0x0A, \n))

                    string field = fieldBuilder.ToString();
                    field = field.Remove(field.Length - 1);
                    fields.Add(field);
                    fieldBuilder.Clear();

                    ProcessLine(fields, items, lineBuilder.ToString(), line++);
                    fields.Clear();
                    lineBuilder.Clear();
                    isLineFirstCharacter = true;
                    previousChar = c;
                    continue;
                }

                fieldBuilder.Append((char)c);
                lineBuilder.Append((char)c);
                previousChar = c;
            }

            if (fields.Count > 0)
            {
                // process last item if missing CRLF
                string field = fieldBuilder.ToString();
                if (field.EndsWith('\n'))
                {
                    field = field.Remove(field.Length - 1);
                }

                fields.Add(field);
                fieldBuilder.Clear();
                ProcessLine(fields, items, lineBuilder.ToString(), line);
            }

            return items;
        }

        protected abstract T CreateInstance(string[] properties);

        private void ProcessLine(List<string> fields, ICollection<T> items, string line, int lineNumber)
        {
            if (fields.Count <= 0)
            {
                return;
            }

            if (fields.Count < NumExpectedItems)
            {
                Logger.Error(
                    $"Skipping Line {lineNumber}: '{line}' expected {NumExpectedItems} values but got {fields.Count}");
                return;
            }

            T item = default;
            try
            {
                item = CreateInstance(fields.ToArray());
            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
            }


            if (item == null)
            {
                Logger.Error($"Skipping Line {lineNumber}: '{line}' could not be converted");
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
