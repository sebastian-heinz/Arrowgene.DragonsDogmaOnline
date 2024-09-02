using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Shared.Csv
{
    /// <summary>
    /// https://www.rfc-editor.org/rfc/rfc4180
    /// </summary>
    public abstract class CsvReaderWriter<T> : IAssetDeserializer<List<T>>
    {
        private const int BufferSize = 128;
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CsvReaderWriter<T>));


        protected abstract int NumExpectedItems { get; }

        public CsvReaderWriter()
        {
        }

        public string WriteString(List<T> rows)
        {
            using Stream stream = new MemoryStream();
            Write(rows, stream);
            stream.Position = 0;
            using StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        public void WritePath(List<T> rows, string path)
        {
            if (File.Exists(path))
            {
                Logger.Info($"Deleting Existing: {path}");
                File.Delete(path);
            }

            Logger.Info($"Writing: {path}");
            using FileStream stream = File.OpenWrite(path);
            Write(rows, stream);
        }

        public void Write(List<T> rows, Stream stream)
        {
            RowWriter rowWriter = new RowWriter(stream);
            CreateHeader(rowWriter);
            foreach (T row in rows)
            {
                CreateRow(row, rowWriter);
                rowWriter.NextRow();
            }

            rowWriter.Flush();
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
                    if (
                        (c == '\r' && nextChar == '\n') // Line end on CRLF
                        || (c == '\n') // Line end on LF
                    )
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
                    if ((previousChar == '\r' && c == '\n') // Line end on CRLF
                    || (c == '\n') // Line end on LF
                    )
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
                        // fieldBuilder.Append((char)c); // exclude quotes on reading data
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
                        // fieldBuilder.Append((char)c); // unescape quote
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
                        if (nextChar != ',' // expect end of field, if not
                            && nextChar != '\r' && nextChar != '\n') // and it is not the line end
                        {
                            Logger.Error(
                                $"Unescaped Quote in CSV near:`{lineBuilder}{(char)c}{(char)nextChar}` (expected `{(char)nextChar}` HEX:{nextChar:X8} to be a quote)");
                        }

                        // fieldBuilder.Append((char)c);  // exclude quotes on reading data
                        previousChar = c;
                        lineBuilder.Append((char)c);
                        continue;
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

                if (
                    (previousChar == '\r' && c == '\n') // Line end on CRLF
                    || (c == '\n') // Line end on LF
                )
                {
                    //carriage return (The Carriage Return (CR) character (0x0D, \r))
                    //line feed (The Line Feed (LF) character (0x0A, \n))
                    
                    string field = fieldBuilder.ToString();
                    
                    if (field.EndsWith('\r'))
                    {
                        // For CRLF case, CR is already committed to field, need to remove it.
                        field = field.Remove(field.Length - 1);
                    }
                    
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

        protected virtual T CreateInstance(string[] properties)
        {
            throw new NotImplementedException();
        }

        protected virtual void CreateRow(T entry, RowWriter rowWriter)
        {
            throw new NotImplementedException();
        }

        protected virtual void CreateHeader(RowWriter rowWriter)
        {
            throw new NotImplementedException();
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


        public class RowWriter
        {
            private readonly StreamWriter _writer;
            private string[] _header;
            private string[] _currentRow;
            private bool _headerCompleted;

            public RowWriter(Stream stream)
            {
                _writer = new StreamWriter(stream);
                _headerCompleted = false;
            }

            public void Flush()
            {
                _writer.Flush();
            }

            public void WriteHeader(int headerIndex, string headerValue)
            {
                if (_headerCompleted)
                {
                    return;
                }

                int newHeaderSize = headerIndex + 1;
                if (_header == null)
                {
                    _header = new string[newHeaderSize];
                }


                if (newHeaderSize > _header.Length)
                {
                    string[] newHeader = new string[newHeaderSize];
                    for (int i = 0; i < _header.Length; i++)
                    {
                        newHeader[i] = _header[i];
                    }

                    _header = newHeader;
                }

                _header[headerIndex] = headerValue;
            }

            private void WriteRow(int columnIndex, string columnVal)
            {
                if (!_headerCompleted)
                {
                    WriteHeader();
                    _headerCompleted = true;
                }

                if (columnIndex > _header.Length)
                {
                    return;
                }

                if (_currentRow == null)
                {
                    _currentRow = new string[_header.Length];
                }

                _currentRow[columnIndex] = columnVal;
            }

            public void Write(int columnIndex, string columnVal)
            {
                if (columnVal == null)
                {
                    return;
                }

                if (columnVal.Contains('"'))
                {
                    columnVal = columnVal.Replace("\"", "\"\"");
                }

                if (
                    columnVal.Contains('\r')
                    || columnVal.Contains('\n')
                    || columnVal.Contains('"')
                    || columnVal.Contains(',')
                )
                {
                    columnVal = $"\"{columnVal}\"";
                }

                WriteRow(columnIndex, columnVal);
            }

            public void Write(int columnIndex, int columnVal)
            {
                WriteRow(columnIndex, $"{columnVal}");
            }

            public void Write(int columnIndex, uint columnVal)
            {
                WriteRow(columnIndex, $"{columnVal}");
            }

            public void NextRow()
            {
                for (int i = 0; i < _currentRow.Length; i++)
                {
                    _writer.Write(_currentRow[i]);
                    if (i < _currentRow.Length - 1)
                    {
                        _writer.Write(",");
                    }
                }

                _writer.Write(Environment.NewLine);
                _currentRow = null;
            }

            private void WriteHeader()
            {
                _writer.Write("#");
                for (int i = 0; i < _header.Length; i++)
                {
                    _writer.Write(_header[i]);
                    if (i < _header.Length - 1)
                    {
                        _writer.Write(",");
                    }
                }

                _writer.Write(Environment.NewLine);
            }
        }
    }
}
