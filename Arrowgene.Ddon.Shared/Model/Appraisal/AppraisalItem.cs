using Arrowgene.Ddon.Shared.Entity.Structure;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Net;

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

        public CDataDispelBaseItem AsCDataDispelBaseItem(JobId jobId)
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
                var item = new CDataDispelLotData()
                {
                    ItemId = lotItem.ItemId,
                    ItemLot = new CDataDispelLotItem()
                    {
                        ItemId = lotItem.ItemId,
                        Amount = (ushort)lotItem.Amount,
                    }
                };

                foreach (var crest in lotItem.Crests)
                {
                    switch (crest.CrestType)
                    {
                        case AppraisalCrestType.Imbued:
                            item.CrestLot.Add(new CDataDispelLotCrest()
                            {
                                CrestItemId = crest.CrestId,
                            });
                            break;
                        case AppraisalCrestType.DragonTrinketAlpha:
                            foreach (var roll in DragonTrinketAlphaRewards.Rolls[crest.JobId])
                            {
                                item.CrestLot.Add(new CDataDispelLotCrest()
                                {
                                    CrestItemId = roll
                                });
                            }
                            break;
                        case AppraisalCrestType.DragonTrinketBeta:
                            foreach (var roll in DragonTrinketBetaRewards.Rolls[crest.JobId])
                            {
                                item.CrestLot.Add(new CDataDispelLotCrest()
                                {
                                    CrestItemId = roll
                                });
                            }
                            break;
                        case AppraisalCrestType.CrestLottery:
                            foreach (var roll in crest.CrestLottery)
                            {
                                item.CrestLot.Add(new CDataDispelLotCrest()
                                {
                                    CrestItemId = roll
                                });
                            }
                            break;
                        case AppraisalCrestType.BitterBlackEarring:
                            foreach (var roll in BitterBlackMazeRewards.EarringRolls[jobId])
                            {
                                item.CrestLot.Add(new CDataDispelLotCrest()
                                {
                                    CrestItemId = roll
                                });
                            }
                            break;
                        default:
                            break;
                    }
                }

                obj.LotItemList.Add(item);
            }

            return obj;
        }
    }
}
