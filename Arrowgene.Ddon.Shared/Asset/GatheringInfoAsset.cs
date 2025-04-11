using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Asset
{
    public class GatheringSpotInfo
    {
        public uint GroupNo { get; set; }
        public uint PosId { get; set; }
        public GatheringType GatheringType { get; set; }
        public uint UintId { get; set; }
        public (double X, double Y, double Z) Position { get; set; }
    }

    public class GatheringInfoAsset
    {
        public GatheringInfoAsset()
        {
            GatheringInfoMap = new Dictionary<uint, Dictionary<(uint GroupNo, uint PosId), GatheringSpotInfo>>();
        }
        public Dictionary<uint, Dictionary<(uint GroupNo, uint PosId), GatheringSpotInfo>> GatheringInfoMap { get; set; }
    }
}
