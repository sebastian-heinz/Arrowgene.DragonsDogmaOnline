using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model.Appraisal
{
    public class AppraisalBaseItem
    {
        public AppraisalBaseItem()
        {
            Name = string.Empty;
        }

        public uint ItemId { get; set; }
        public uint Amount { get; set; }
        public string Name { get; set; }
    }
}
