using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model
{
    public class SpecialShop
    {
        public ShopType ShopType { get; set; }
        List<Shop> ShopList { get; set; }
    }
}
