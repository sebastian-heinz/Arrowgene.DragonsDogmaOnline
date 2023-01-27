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

        protected override int NumExpectedItems => _multiLanguage ? 13 : 12;

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
                string path = properties[8];
                string name = properties[9];
                if (!uint.TryParse(properties[10], out uint keyReadIndex)) return null;
                if (!uint.TryParse(properties[11], out uint msgReadIndex)) return null;
                string str = properties[12];
                
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
                    Path = path,
                    Name = name,
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
                string path = properties[7];
                string name = properties[8];
                if (!uint.TryParse(properties[9], out uint keyReadIndex)) return null;
                if (!uint.TryParse(properties[10], out uint msgReadIndex)) return null;
                string str = properties[11];
                return new GmdIntermediateContainer
                {
                    Index = index,
                    Key = key,
                    MsgOrg = msg,
                    a2 = a2,
                    a3 = a3,
                    a4 = a4,
                    a5 = a5,
                    Path = path,
                    Name = name,
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
                rowWriter.WriteHeader(8, "Path");
                rowWriter.WriteHeader(9, "Name");
                rowWriter.WriteHeader(10, "KeyReadIndex");
                rowWriter.WriteHeader(11, "MsgReadIndex");
                rowWriter.WriteHeader(12, "Str");
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
                rowWriter.WriteHeader(7, "Path");
                rowWriter.WriteHeader(8, "Name");
                rowWriter.WriteHeader(9, "KeyReadIndex");
                rowWriter.WriteHeader(10, "MsgReadIndex");
                rowWriter.WriteHeader(11, "Str");
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
                rowWriter.Write(8, entry.Path);
                rowWriter.Write(9, entry.Name);
                rowWriter.Write(10, entry.KeyReadIndex);
                rowWriter.Write(11, entry.MsgReadIndex);
                rowWriter.Write(12, entry.Str);
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
                rowWriter.Write(7, entry.Path);
                rowWriter.Write(8, entry.Name);
                rowWriter.Write(9, entry.KeyReadIndex);
                rowWriter.Write(10, entry.MsgReadIndex);
                rowWriter.Write(11, entry.Str);
            }
        }
    }
}
