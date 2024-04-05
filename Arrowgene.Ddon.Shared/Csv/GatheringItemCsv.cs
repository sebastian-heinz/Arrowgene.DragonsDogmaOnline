using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Csv
{
    public class GatheringItemCsv : IAssetDeserializer<Dictionary<(StageId, uint), List<GatheringItem>>>
    {
        private GatheringItemRowCsv rowReader = new GatheringItemRowCsv();

        public GatheringItemCsv() : base()
        {
        }

        public Dictionary<(StageId, uint), List<GatheringItem>> ReadPath(string path)
        {
            Dictionary<(StageId, uint), List<GatheringItem>> dict = new Dictionary<(StageId, uint), List<GatheringItem>>();
            foreach (GatheringItemRow row in rowReader.ReadPath(path))
            {
                List<GatheringItem> itemsInSpot = dict.GetValueOrDefault((row.StageId, row.SubGroupId)) ?? new List<GatheringItem>();
                itemsInSpot.Add(new GatheringItem()
                {
                    ItemId = row.ItemId,
                    ItemNum = row.ItemNum,
                    MaxItemNum = row.MaxItemNum,
                    Quality = row.Quality,
                    IsHidden = row.IsHidden
                });
                dict[(row.StageId, row.SubGroupId)] = itemsInSpot;
            }
            return dict;
        }

        private class GatheringItemRowCsv : CsvReaderWriter<GatheringItemRow>
        {
            protected override int NumExpectedItems => 8;

            protected override GatheringItemRow CreateInstance(string[] properties)
            {
                int idx = 0;
                if (!uint.TryParse(properties[idx++], out uint stageId)) return null;
                if (!byte.TryParse(properties[idx++], out byte layerNo)) return null;
                if (!uint.TryParse(properties[idx++], out uint groupNo)) return null;
                if (!uint.TryParse(properties[idx++], out uint posId)) return null;
                if (!uint.TryParse(properties[idx++], out uint itemId)) return null;
                if (!uint.TryParse(properties[idx++], out uint itemNum)) return null;
                uint maxItemNum;
                if(properties.Length >= 9)
                {
                    if (!uint.TryParse(properties[idx++], out maxItemNum)) return null;
                }
                else
                {
                    // For compatibility with the older CSV format, skip this column if the length is less than 9
                    maxItemNum = itemNum;
                }
                if (!uint.TryParse(properties[idx++], out uint quality)) return null;
                if (!bool.TryParse(properties[idx++], out bool isHidden)) return null;

                return new GatheringItemRow
                {
                    StageId = new StageId(stageId, layerNo, groupNo),
                    SubGroupId = posId,
                    ItemId = itemId,
                    ItemNum = itemNum,
                    MaxItemNum = maxItemNum,
                    Quality = quality,
                    IsHidden = isHidden
                };
            }
        }

        private class GatheringItemRow
        {
            public StageId StageId { get; set; }
            public uint SubGroupId { get; set; }
            public uint ItemId { get; set; }
            public uint ItemNum { get; set; }
            public uint MaxItemNum { get; set; }
            public uint Quality { get; set; }
            public bool IsHidden { get; set; }
        }
    }

}
