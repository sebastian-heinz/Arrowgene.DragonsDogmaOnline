using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model
{
    public interface IStageLocationAsset<Y>
    {
        public StageLayoutId StageId { get; set; }
        public Y SubGroupId { get; set; }
    }
}