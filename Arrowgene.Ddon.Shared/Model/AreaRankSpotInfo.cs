using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model.Quest;

namespace Arrowgene.Ddon.Shared.Model
{
    public class AreaRankSpotInfo
    {
        public QuestAreaId AreaId { get; set; }
        public uint TextIndex { get; set; }
        public uint SpotId { get; set; }
        public uint UnlockRank { get; set; }
        public uint UnlockQuest { get; set; }
    }
}
