using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Csv
{
    public class GmdCsvReader : CsvReader<GmdIntermediateContainer>
    {
        public GmdCsvReader() : base()
        {
        }

        protected override int NumExpectedItems => 12;

        protected override GmdIntermediateContainer CreateInstance(string[] properties)
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
                Msg = msg,
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
}
