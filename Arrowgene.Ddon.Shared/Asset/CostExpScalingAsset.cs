using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Asset
{
    public class CostExpScalingAsset
    {
        public CostExpScalingAsset()
        {
            CostExpScalingInfo = new Dictionary<uint, CostExpInfo>();
        }

        public Dictionary<uint, CostExpInfo> CostExpScalingInfo {  get; set; }
    }
}
