using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Model.Appraisal
{
    public class AppraisalItem
    {
        public AppraisalItem()
        {
            Label = string.Empty;
            LootPool = new List<AppraisalLotteryItem>();
            BaseItems = new List<AppraisalBaseItem>();
        }

        public uint AppraisalId { get; set; }
        public string Label { get; set; }
        public List<AppraisalBaseItem> BaseItems { get; set; } // RequiredItems
        public List<AppraisalLotteryItem> LootPool { get; set; } // Potential Rewards

        public CDataDispelBaseItem AsCDataDispelBaseItem()
        {
            var obj = new CDataDispelBaseItem()
            {
                Id = AppraisalId,
                Label = Label,
            };

            foreach (var item in BaseItems)
            {
                obj.BaseItemId.Add(new CDataDispelBaseItemData()
                {
                    ItemId = item.ItemId,
                    Num = item.Amount
                });
            }

            foreach (var lotItem in LootPool)
            {
                obj.LotItemList.Add(new CDataDispelLotData()
                {
                    ItemId = lotItem.ItemId,
                    ItemLot = new CDataDispelLotItem()
                    {
                        ItemId = lotItem.ItemId,
                        Amount = (ushort)lotItem.Amount,
                    }
                });
            }

            return obj;
        }
    }
}
