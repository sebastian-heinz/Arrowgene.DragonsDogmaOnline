using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model.BattleContent
{
    public class BitterblackAppraisalLotteryItem
    {
        public uint ItemId { get; set; }
        public string Name { get; set; }
        public uint Amount {  get; set; }

        public BitterblackAppraisalLotteryItem()
        {
            Name = "";
        }
    }
}
