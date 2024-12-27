using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Asset
{
    public class GachaAsset
    {
        public GachaAsset()
        {
            GachaInfoList = new Dictionary<uint, CDataGachaInfo>();
        }
        public Dictionary<uint, CDataGachaInfo> GachaInfoList { get; set; }
    }
}
