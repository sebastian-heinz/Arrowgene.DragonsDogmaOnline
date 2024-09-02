using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Csv
{
    public class JobValueShopCsv : CsvReaderWriter<(JobId, CDataJobValueShopItem)>
    {
        protected override int NumExpectedItems => 5;

        protected override (JobId, CDataJobValueShopItem) CreateInstance(string[] properties)
        {
            if (!byte.TryParse(properties[0], out byte jobId)) return (0, null);
            if (!byte.TryParse(properties[1], out byte jobValueType)) return (0, null);
            if (!uint.TryParse(properties[2], out uint lineupId)) return (0, null);
            if (!uint.TryParse(properties[3], out uint itemId)) return (0, null);
            if (!uint.TryParse(properties[4], out uint price)) return (0, null);

            return ((JobId)jobId, new CDataJobValueShopItem()
            {
                LineupId = lineupId,
                ItemId = itemId,
                Price = price,
                IsCountLimit = false,
                CanSelectStorage = true,
                UnableReason = 0
            });
        }
    }
}
