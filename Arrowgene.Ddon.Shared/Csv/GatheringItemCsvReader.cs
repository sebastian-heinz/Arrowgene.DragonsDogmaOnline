using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Csv
{
    public class GatheringItemCsvReader : CsvReader<GatheringItem>
    {
        public GatheringItemCsvReader() : base(true)
        {
            
        }
        
        protected override int NumExpectedItems => 8;

        protected override GatheringItem CreateInstance(string[] properties)
        {
            if (!uint.TryParse(properties[0], out uint stageId)) return null;
            if (!byte.TryParse(properties[1], out byte layerNo)) return null;
            if (!uint.TryParse(properties[2], out uint groupNo)) return null;
            if (!uint.TryParse(properties[3], out uint posId)) return null;
            if (!uint.TryParse(properties[4], out uint itemId)) return null;
            if (!uint.TryParse(properties[5], out uint itemNum)) return null;
            if (!uint.TryParse(properties[6], out uint unk3)) return null;
            if (!bool.TryParse(properties[7], out bool isHidden)) return null;

            return new GatheringItem
            {
                StageId = new StageId(stageId, layerNo, groupNo),
                SubGroupId = posId,
                ItemId = itemId,
                ItemNum = itemNum,
                Unk3 = unk3,
                IsHidden = isHidden
            };
        }
    }
}