using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Csv
{
    public class GmdCsv : CsvReaderWriter<GmdIntermediateContainer>
    {
        private readonly bool _multiLanguage;
        
        public GmdCsv(bool multiLanguage) : base()
        {
            _multiLanguage = multiLanguage;
        }

        protected override int NumExpectedItems => _multiLanguage ? 14 : 13;

        protected override GmdIntermediateContainer CreateInstance(string[] properties)
        {
            if (_multiLanguage)
            {
                if (!uint.TryParse(properties[0], out uint index)) return null;
                string key = properties[1];
                string msgOrg = properties[2];
                string msgEn = properties[3];
                if (!uint.TryParse(properties[4], out uint a2)) return null;
                if (!uint.TryParse(properties[5], out uint a3)) return null;
                if (!uint.TryParse(properties[6], out uint a4)) return null;
                if (!uint.TryParse(properties[7], out uint a5)) return null;
                string gmdPath = properties[8];
                string arcPath = properties[9];
                string arcName = properties[10];
                if (!uint.TryParse(properties[11], out uint keyReadIndex)) return null;
                if (!uint.TryParse(properties[12], out uint msgReadIndex)) return null;
                string str = properties[13];
                
                return new GmdIntermediateContainer
                {
                    Index = index,
                    Key = key,
                    MsgOrg = msgOrg,
                    MsgEn = msgEn,
                    a2 = a2,
                    a3 = a3,
                    a4 = a4,
                    a5 = a5,
                    GmdPath = gmdPath,
                    ArcPath = arcPath,
                    ArcName = arcName,
                    KeyReadIndex = keyReadIndex,
                    MsgReadIndex = msgReadIndex,
                    Str = str
                };
            }
            else
            {                
                if (!uint.TryParse(properties[0], out uint index)) return null;
                string key = properties[1];
                string msg = properties[2];
                if (!uint.TryParse(properties[3], out uint a2)) return null;
                if (!uint.TryParse(properties[4], out uint a3)) return null;
                if (!uint.TryParse(properties[5], out uint a4)) return null;
                if (!uint.TryParse(properties[6], out uint a5)) return null;
                string gmdPath = properties[7];
                string arcPath = properties[8];
                string arcName = properties[9];
                if (!uint.TryParse(properties[10], out uint keyReadIndex)) return null;
                if (!uint.TryParse(properties[11], out uint msgReadIndex)) return null;
                string str = properties[12];
                return new GmdIntermediateContainer
                {
                    Index = index,
                    Key = key,
                    MsgOrg = msg,
                    a2 = a2,
                    a3 = a3,
                    a4 = a4,
                    a5 = a5,
                    GmdPath = gmdPath,
                    ArcPath = arcPath,
                    ArcName = arcName,
                    KeyReadIndex = keyReadIndex,
                    MsgReadIndex = msgReadIndex,
                    Str = str
                };
            }
        }

        protected override void CreateHeader(RowWriter rowWriter)
        {
            if (_multiLanguage)
            {
                rowWriter.WriteHeader(0, "Index");
                rowWriter.WriteHeader(1, "Key");
                rowWriter.WriteHeader(2, "MsgOrg");
                rowWriter.WriteHeader(3, "MsgEn");
                rowWriter.WriteHeader(4, "a2");
                rowWriter.WriteHeader(5, "a3");
                rowWriter.WriteHeader(6, "a4");
                rowWriter.WriteHeader(7, "a5");
                rowWriter.WriteHeader(8, "GmdPath");
                rowWriter.WriteHeader(9, "ArcPath");
                rowWriter.WriteHeader(10, "ArcName");
                rowWriter.WriteHeader(11, "KeyReadIndex");
                rowWriter.WriteHeader(12, "MsgReadIndex");
                rowWriter.WriteHeader(13, "Str");
            }
            else
            {
                rowWriter.WriteHeader(0, "Index");
                rowWriter.WriteHeader(1, "Key");
                rowWriter.WriteHeader(2, "Msg");
                rowWriter.WriteHeader(3, "a2");
                rowWriter.WriteHeader(4, "a3");
                rowWriter.WriteHeader(5, "a4");
                rowWriter.WriteHeader(6, "a5");
                rowWriter.WriteHeader(7, "GmdPath");
                rowWriter.WriteHeader(8, "ArcPath");
                rowWriter.WriteHeader(9, "ArcName");
                rowWriter.WriteHeader(10, "KeyReadIndex");
                rowWriter.WriteHeader(11, "MsgReadIndex");
                rowWriter.WriteHeader(12, "Str");
            }
        }

        protected override void CreateRow(GmdIntermediateContainer entry, RowWriter rowWriter)
        {
            if (_multiLanguage)
            {
                rowWriter.Write(0, entry.Index);
                rowWriter.Write(1, entry.Key);
                rowWriter.Write(2, entry.MsgOrg);
                rowWriter.Write(3, entry.MsgEn);
                rowWriter.Write(4, entry.a2);
                rowWriter.Write(5, entry.a3);
                rowWriter.Write(6, entry.a4);
                rowWriter.Write(7, entry.a5);
                rowWriter.Write(8, entry.GmdPath);
                rowWriter.Write(9, entry.ArcPath);
                rowWriter.Write(10, entry.ArcName);
                rowWriter.Write(11, entry.KeyReadIndex);
                rowWriter.Write(12, entry.MsgReadIndex);
                rowWriter.Write(13, entry.Str);
            }
            else
            {
                rowWriter.Write(0, entry.Index);
                rowWriter.Write(1, entry.Key);
                rowWriter.Write(2, entry.MsgOrg);
                rowWriter.Write(3, entry.a2);
                rowWriter.Write(4, entry.a3);
                rowWriter.Write(5, entry.a4);
                rowWriter.Write(6, entry.a5);
                rowWriter.Write(7, entry.GmdPath);
                rowWriter.Write(8, entry.ArcPath);
                rowWriter.Write(9, entry.ArcName);
                rowWriter.Write(10, entry.KeyReadIndex);
                rowWriter.Write(11, entry.MsgReadIndex);
                rowWriter.Write(12, entry.Str);
            }
        }
    }
}
