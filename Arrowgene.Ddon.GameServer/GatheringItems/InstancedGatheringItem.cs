using System;
using Arrowgene.Ddon.Shared.Model;

public class InstancedGatheringItem
{
    public InstancedGatheringItem()
    {
    }

    public InstancedGatheringItem(GatheringItem gatheringItem)
    {
        ItemId = gatheringItem.ItemId;
        ItemNum = (uint) Random.Shared.Next((int) gatheringItem.ItemNum, (int) gatheringItem.MaxItemNum+1);
        Quality = gatheringItem.Quality;
        IsHidden = gatheringItem.IsHidden;
    }

    public uint ItemId { get; set; }
    public uint ItemNum { get; set; }
    public uint Quality { get; set; }
    public bool IsHidden { get; set; }
}