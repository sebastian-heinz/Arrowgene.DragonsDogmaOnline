using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Asset
{
    public class PawnCostReductionAsset
    {
        public PawnCostReductionAsset()
        {
            PawnCostReductionInfo = new Dictionary<uint, PawnCostReductionInfo>();
        }

        public Dictionary<uint, PawnCostReductionInfo> PawnCostReductionInfo { get; set; }
    }
}
