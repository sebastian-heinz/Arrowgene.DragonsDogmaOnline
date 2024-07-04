using Arrowgene.Ddon.Shared.Entity.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model.BattleContent
{
    public class BitterblackMazeAppraisalItem
    {
        public BitterblackMazeAppraisalItem()
        {
            Name = "";
            LootPool = new List<BitterblackAppraisalLotteryItem>();
        }

        public uint ItemId { get; set; }
        public string Name { get; set; }
        public List<BitterblackAppraisalLotteryItem> LootPool { get; set; }

        public CDataDispelBaseItem AsCDataDispelBaseItem()
        {
            var obj = new CDataDispelBaseItem()
            {
                Id = ItemId,
                Label = Name,
                BaseItemId = new List<CDataDispelBaseItemData>()
                {
                    new CDataDispelBaseItemData()
                    {
                        ItemId = ItemId,
                        Num = 1,
                    }
                }
            };

            foreach (var lotItem in LootPool)
            {
                obj.LotItemList.Add(new CDataDispelLotData()
                {
                    ItemId = lotItem.ItemId,
                    ItemLot = new CDataDispelLotItem()
                    {
                        ItemId = lotItem.ItemId,
                        Amount = (ushort) lotItem.Amount,
                    }
                });
            }

            return obj;
        }
    }
}
