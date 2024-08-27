using System.Text;

namespace Arrowgene.Ddon.Shared.Csv
{
    public class GmdCsv : CsvReaderWriter<GmdCsv.Entry>
    {
        public class Entry
        {
            public uint Index { get; set; }
            public string Key { get; set; }
            public string MsgJp { get; set; }
            public string MsgEn { get; set; }
            public string GmdPath { get; set; }
            public string ArcName { get; set; }
            public string ArcPath { get; set; }
            public uint ReadIndex { get; set; }

            public string GetUniqueQualifierLanguageAgnostic()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(Index);
                sb.Append(Key);
                sb.Append(GmdPath);
                sb.Append(ArcPath);
                sb.Append(ArcName);
                sb.Append(ReadIndex);
                return sb.ToString();
            }
        }

        public GmdCsv() : base()
        {
        }

        protected override int NumExpectedItems => 8;

        protected override Entry CreateInstance(string[] properties)
        {
            if (!uint.TryParse(properties[0], out uint index)) return null;
            string key = properties[1];
            string msgJp = properties[2];
            string msgEn = properties[3];
            string gmdPath = Util.ToArcPath(properties[4]); // Normalize paths
            string arcPath = Util.ToArcPath(properties[5]); // Normalize paths
            string arcName = properties[6];
            if (!uint.TryParse(properties[7], out uint readIndex)) return null;

            return new Entry
            {
                Index = index,
                Key = key,
                MsgJp = msgJp,
                MsgEn = msgEn,
                GmdPath = gmdPath,
                ArcPath = arcPath,
                ArcName = arcName,
                ReadIndex = readIndex,
            };
        }

        protected override void CreateHeader(RowWriter rowWriter)
        {
            rowWriter.WriteHeader(0, "Index");
            rowWriter.WriteHeader(1, "Key");
            rowWriter.WriteHeader(2, "MsgJp");
            rowWriter.WriteHeader(3, "MsgEn");
            rowWriter.WriteHeader(4, "GmdPath");
            rowWriter.WriteHeader(5, "ArcPath");
            rowWriter.WriteHeader(6, "ArcName");
            rowWriter.WriteHeader(7, "ReadIndex");
        }

        protected override void CreateRow(Entry entry, RowWriter rowWriter)
        {
            rowWriter.Write(0, entry.Index);
            rowWriter.Write(1, entry.Key);
            rowWriter.Write(2, entry.MsgJp);
            rowWriter.Write(3, entry.MsgEn);
            rowWriter.Write(4, entry.GmdPath);
            rowWriter.Write(5, entry.ArcPath);
            rowWriter.Write(6, entry.ArcName);
            rowWriter.Write(7, entry.ReadIndex);
        }
    }
}
