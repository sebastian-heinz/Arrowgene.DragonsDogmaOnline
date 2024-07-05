using Arrowgene.Ddon.Shared.Model.Appraisal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model
{
    public class ShopCategory
    {
        public ShopCategory()
        {
            Label = String.Empty;
            Items = new List<AppraisalItem>();
        }

        public uint Id {  get; set; }
        public string Label { get; set; }
        public List<AppraisalItem> Items { get; set; }
    }
}
