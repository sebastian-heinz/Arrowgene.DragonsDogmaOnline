using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Asset
{
    public class LimitBreakAsset
    {
        public LimitBreakAsset()
        {
            ListTitle = string.Empty;
            Categories = new List<LimitBreakCategory>();
        }

        public string ListTitle { get; set; }
        public List<LimitBreakCategory> Categories { get; set; }
    }

    public class LimitStatLottery
    {
        public LimitStatLottery()
        {
            Rolls = new List<ushort>();
        }
        public uint MinGreatSuccessIndex { get; set; }
        public List<ushort> Rolls { get; set; }
    }

    public class LimitBreakCategory
    {
        public LimitBreakCategory()
        {
            ShopListings = new List<byte>();
            PaymentOptions = new List<(WalletType WalletType, string Label, uint Cost)>();
            StatLottery = new List<LimitStatLottery>();
        }

        public ushort Key { get; set; }
        public byte Index { get; set; }
        public List<byte> ShopListings { get; set; }
        public HashSet<WalletType> PremiumCurrencies { get; set; } = new();
        public List<(WalletType WalletType, string Label, uint Cost)> PaymentOptions { get; set; }
        public List<LimitStatLottery> StatLottery { get; set; }
    }
}
