using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.Shared.Csv
{
    public class QuestScheduleIdCsv : IAssetDeserializer<Dictionary<QuestId, uint>>
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(QuestScheduleIdCsv));

        private readonly QuestScheduleIdRowCsv rowReader = new();

        public Dictionary<QuestId, uint> ReadPath(string path)
        {
            var asset = rowReader.ReadPath(path).ToDictionary(
                row => row.QuestId,
                row => QuestScheduleId.GenerateScheduleId(row.Type, row.Group, row.Subgroup, row.Index, 0)
            );

            return asset;
        }

        private class QuestScheduleIdRowCsv : CsvReaderWriter<QuestScheduleIdRow>
        {
            protected override int NumExpectedItems => 6;

            protected override QuestScheduleIdRow CreateInstance(string[] properties)
            {
                int idx = 0;
                if (!uint.TryParse(properties[idx++], out uint questId)) return null;
                if (!byte.TryParse(properties[idx++], out byte type)) return null;
                if (!byte.TryParse(properties[idx++], out byte group)) return null;
                if (!byte.TryParse(properties[idx++], out byte subgroup)) return null;
                if (!byte.TryParse(properties[idx++], out byte index)) return null;
                
                return new QuestScheduleIdRow
                {
                    QuestId = (QuestId)questId,
                    Type = type,
                    Group = group,
                    Subgroup = subgroup,
                    Index = index
                };
            }
        }

        private class QuestScheduleIdRow
        {
            public QuestId QuestId { get; set; }
            public byte Type { get; set; }
            public byte Group { get; set; }
            public byte Subgroup { get; set; }
            public byte Index { get; set; }
        }
    }
}
