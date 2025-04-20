using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Asset
{
    public class JobMasterAsset
    {
        public JobMasterAsset()
        {
            JobOrders = new Dictionary<JobId, Dictionary<ReleaseType, Dictionary<uint, List<CDataActiveJobOrder>>>>();
        }
        public Dictionary<JobId, Dictionary<ReleaseType, Dictionary<uint, List<CDataActiveJobOrder>>>> JobOrders { get; set; }
    }
}
