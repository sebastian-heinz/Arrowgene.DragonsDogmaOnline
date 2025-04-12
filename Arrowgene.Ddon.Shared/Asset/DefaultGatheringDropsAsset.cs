using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Asset
{
    public class DefaultGatheringDrop
    {
        public DefaultGatheringDrop()
        {
            LocationName = string.Empty;
            PresentDuringWeather = new HashSet<Weather>();
        }

        public string LocationName { get; set; }
        public bool IsSpot { get; set; }
        public ItemId ItemId { get; set; }
        public QuestAreaId AreaId { get; set; }
        public DropCategory DropCategory { get; set; }
        public uint StageId { get; set; }
        public uint MinAreaRank { get; set; }
        public HashSet<Weather> PresentDuringWeather { get; set; }
        public (long Start, long End) ValidPeriod { get; set; }
        public uint Quality { get; set; }
        public uint ItemLevel { get; set; }
        public uint MinAmount { get; set; }
        public uint MaxAmount { get; set; }
        public uint SpotPosId { get; set; }
        public StageLayoutId SpotStageLayoutId { get; set; }
    }

    public class DefaultGatheringDropsAsset
    {
        public DefaultGatheringDropsAsset()
        {
            AreaDefaultDrops = new();
            SpotDefaultDrops = new();
        }
        public Dictionary<QuestAreaId, Dictionary<DropCategory, List<DefaultGatheringDrop>>> AreaDefaultDrops { get; set; }
        public Dictionary<(StageLayoutId StageLayoutId, uint PosId), List<DefaultGatheringDrop>> SpotDefaultDrops { get; set; }
    }
}
