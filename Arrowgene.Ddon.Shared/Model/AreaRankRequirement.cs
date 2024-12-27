using Arrowgene.Ddon.Shared.Model.Quest;

namespace Arrowgene.Ddon.Shared.Model
{
    public class AreaRankRequirement
    {
        public QuestAreaId AreaId { get; set; }
        public uint Rank { get; set; }
        public uint MinPoint { get; set; }
        public uint AreaTrial { get; set; }
        public uint ExternalQuest { get; set; }
    }
}
