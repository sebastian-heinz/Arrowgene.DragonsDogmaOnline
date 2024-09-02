using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Appraisal;

namespace Arrowgene.Ddon.Shared.Asset
{
    public class SpecialShopAsset
    {
        public SpecialShopAsset()
        {
            SpecialShops = new Dictionary<ShopType, List<ShopCategory>>();
            ShopCategories = new Dictionary<uint, ShopCategory>();
            AppraisalItems = new Dictionary<uint, AppraisalItem>();
        }

        public Dictionary<ShopType, List<ShopCategory>> SpecialShops { get; set; }
        public Dictionary<uint, ShopCategory> ShopCategories { get; set; }
        public Dictionary<uint, AppraisalItem> AppraisalItems { get; set; }
    }
}
