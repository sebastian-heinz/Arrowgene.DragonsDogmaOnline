using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model.Appraisal
{
    public class AppraisalLotteryItem
    {
        public uint ItemId { get; set; }
        public string Name { get; set; }
        public uint Amount { get; set; }
        public uint Slots { get; set; }

        public AppraisalLotteryItem()
        {
            Name = "";
        }
    }
}
