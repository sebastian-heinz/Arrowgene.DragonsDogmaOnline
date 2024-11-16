using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Asset
{
    public class CostExpScalingAsset
    {
        public CostExpScalingAsset()
        {
            CostExpScalingInfo = new Dictionary<uint, CostExpInfo>();
        }

        private uint Max { get
            {
                return CostExpScalingInfo.Keys.Max();
            } 
        }

        private uint Min
        {
            get
            {
                return CostExpScalingInfo.Keys.Min();
            }
        }

        public Dictionary<uint, CostExpInfo> CostExpScalingInfo {  get; set; }

        public CostExpInfo GetScalingInfo(uint id)
        {
            if (CostExpScalingInfo.ContainsKey(id))
            {
                return CostExpScalingInfo[id];
            }
            else if (id < Min)
            { 
                return CostExpScalingInfo[Min]; 
            }
            else if (id > Max)
            {
                return CostExpScalingInfo[Max];
            }
            else
            {
                throw new KeyNotFoundException($"IR {id} is missing from the CostExpScalingInfo.");
            }
        }
    }
}
