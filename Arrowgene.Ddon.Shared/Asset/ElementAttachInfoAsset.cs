using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Asset
{
    public class CostExpScalingAsset
    {
        public CostExpScalingAsset()
        {
            CostExpScalingInfo = new Dictionary<uint, ElementAttachInfo>();
        }

        public Dictionary<uint, ElementAttachInfo> CostExpScalingInfo {  get; set; }
    }
}
