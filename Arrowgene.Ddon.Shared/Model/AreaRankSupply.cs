using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model.Quest;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Arrowgene.Ddon.Shared.Model
{
    public class AreaRankSupply
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public QuestAreaId AreaId { get; set; }
        public uint MinRank { get; set; }
        public List<CDataBorderSupplyItem> SupplyItemInfoList { get; set; }
    }
}
