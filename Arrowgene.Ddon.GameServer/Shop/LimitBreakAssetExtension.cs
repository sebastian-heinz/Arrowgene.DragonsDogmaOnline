using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Shop
{
    public static class LimitBreakAssetExtension
    {
        public static List<CDataEquipEnhanceLotteryOption> ToLotteryExampleList(this LimitBreakAsset asset)
        {
            var results = new List<CDataEquipEnhanceLotteryOption>();

            byte listingIndex = 1;
            foreach (var category in asset.Categories)
            {
                foreach (var paymentType in category.PaymentOptions)
                {
                    var option = new CDataEquipEnhanceLotteryOption()
                    {
                        RowTitle = $"{asset.ListTitle} {paymentType.Label}",
                        Index = listingIndex,
                        Category = listingIndex,
                        WalletPointCost = new List<CDataWalletPoint>()
                        {
                            new CDataWalletPoint()
                            {
                                Type = paymentType.WalletType,
                                Value = paymentType.Cost
                            }
                        },
                        ShopTypeListings = category.ShopListings.Select(x => new CDataCommonU8(x)).ToList(),
                    };

                    var lotteryCandidates = new CDataS2CEquipEnhancedGetPacksResUnk0Unk10()
                    {
                        EffectParamList = new List<CDataS2CEquipEnhancedGetPacksResUnk0Unk10Unk1>()
                    };

                    foreach (var stat in category.StatLottery)
                    {
                        lotteryCandidates.EffectParamList.Add(new CDataS2CEquipEnhancedGetPacksResUnk0Unk10Unk1()
                        {
                            BuffId = stat.Rolls.Last(),
                            Unk1 = 1,
                            Unk2 = 2
                        });
                    }
                    option.MainSuccessExample.Add(lotteryCandidates);

                    listingIndex++;
                    results.Add(option);
                }
            }

            return results;
        }

        public static LimitBreakCategory GetCategoryForIndex(this LimitBreakAsset asset, ushort index)
        {
            ushort listingIndex = 1;
            foreach (var category in asset.Categories)
            {
                foreach (var paymentType in category.PaymentOptions)
                {
                    if (listingIndex == index)
                    {
                        return category;
                    }
                    listingIndex++;
                }
            }
            return null;
        }
    }
}
