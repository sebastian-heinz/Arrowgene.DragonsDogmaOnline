using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model.Appraisal
{
    public class AppraisalCrest
    {
        public AppraisalCrest()
        {
            CrestLottery = new List<uint>();
        }

        public AppraisalCrestType CrestType { get; set; }
        public uint CrestId { get; set; }
        public ushort Amount { get; set; }
        public JobId JobId { get; set; }

        public List<uint> CrestLottery {  get; set; }
    }
}
