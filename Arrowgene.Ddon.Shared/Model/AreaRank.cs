using Arrowgene.Ddon.Shared.Model.Quest;

namespace Arrowgene.Ddon.Shared.Model
{
    public class AreaRank
    {
        public QuestAreaId AreaId { get; set; }
        public uint Rank { get; set; }
        public uint Point { get; set; }
        public uint WeekPoint { get; set; }
        public uint LastWeekPoint { get; set; }
        public bool CanReceiveSupply { get; set; }
    }
}
