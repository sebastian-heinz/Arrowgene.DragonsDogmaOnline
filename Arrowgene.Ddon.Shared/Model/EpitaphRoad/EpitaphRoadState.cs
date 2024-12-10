using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Model.EpitaphRoad
{
    public class EpitaphRoadState
    {
        public EpitaphRoadState()
        {
            UnlockedContent = new HashSet<uint>();
            WeeklyRewardsClaimed = new HashSet<uint>();
        }

        public HashSet<uint> UnlockedContent { get; set; }
        public HashSet<uint> WeeklyRewardsClaimed { get; set; }
    }
}
